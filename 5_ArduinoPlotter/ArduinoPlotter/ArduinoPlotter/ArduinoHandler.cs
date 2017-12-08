using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Management;
using System.Windows.Forms;

/* ESTOU PROGRAMANDO ESSA AINDA, AGUARDE...
 */
namespace ArduinoPlotter
{
    public static class ArduinoConstants
    {
        public readonly static int PACKETSIZE = 4;
        public readonly static char PACKETSTART = '$';
        public readonly static char PACKETEND = '\n';
        


    }

    public class ArduinoHandler
    {
        SerialPort arduinoPort;
        ThreadHandler th_aquirer;
        CircularBuffer<UInt16> data;
        double freq_aquire;
        string arduino_description;
        string _port_name;

        char valor_lido;
        UInt16 data_read;

        public ArduinoHandler(string port_name = "None", int baudrate = 115200, int readtimeout = 1000)
        {
            arduino_description = "None";

            _port_name = port_name;
            if (_port_name.Equals("None"))
            {
                _port_name = this.GetArduinoSerialPort();
            }

           
            arduinoPort = new SerialPort();
            arduinoPort.BaudRate = baudrate;
            arduinoPort.ReadTimeout = readtimeout;

            try
            {
                arduinoPort.Open();
                arduinoPort.DiscardInBuffer();
                arduinoPort.DiscardOutBuffer();
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao procurar a porta serial.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetArduinoSerialPort()
        {
            string[] serialDevices;// Auxiliar - Stores all devices that have serial capablities
            string portNameRes;

            List<string> listSerial = new List<string>();
            List<ManagementObject> listObj = new List<ManagementObject>();//using System.Management;
            try
            {
                // get only devices that are working properly."
                string query = "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0";// get only devices that are working properly."
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                listObj = searcher.Get().Cast<ManagementObject>().ToList();
                searcher.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
            //varrer lista com dispositivos encontrados
            foreach (ManagementObject obj in listObj)
            {
                object captionObj = obj["Caption"]; //This will get you the friendly name.
                if (captionObj != null)
                {
                    string caption = captionObj.ToString();
                    if (caption.Contains("(COM")) //This is where I filter on COM-ports.
                    {
                        listSerial.Add(caption);
                    }
                }
            }
            //remover duplicatas (Distinct()) e ordenar resposta em array --- COM1, COM2, ...
            serialDevices = listSerial.Distinct().OrderBy(s => s).ToArray();
            //varrer a lista de dispositivos com capacidade seril encontrada, até localizar
            //aquele com o Friendly name do Arduino.
            //Se existir, serialDevices[i] conterá algo do tipo "Arduino Due Programming Port (COMx)"
            // onde COMx é o port com localizado x = 1, 2, 3...
            portNameRes = "";
            foreach (string s in serialDevices)
            {
                if (string.IsNullOrWhiteSpace(s))
                    continue; //vá para o próximo string

                //possui o friendly name do Arduino? 
                #region Encontrando  "Arduino"
                // Due (Programming Port)"
                if (s.Contains("Arduino"))
                {
                    //Retornar o nome do port "COMx ou COMxx" AO FINAL do string.
                    //localizar a última sequencia "(COM"
                    int lastindex = -1;
                    int index = 0;
                    do
                    {
                        index = s.IndexOf("(COM", index);
                        if (index != -1)
                        {
                            lastindex = index;
                            index++;
                        }
                    } while (index != -1);
                    if (lastindex < 0)
                    {
                        MessageBox.Show("Erro ao tentar localizar COM port do Arduino", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return "";
                    }
                    //remover parte inicial com friendly name e '('
                    portNameRes = s.Substring(lastindex + 1);
                    //Remover qualquer coisa que esteja após o próximo ')'.
                    lastindex = portNameRes.IndexOf(")", 0);
                    //Deve existir ')' e deve possuir pelo menos 4 caracteres na resposta "COMx"...
                    if (lastindex <= 3)
                    {
                        MessageBox.Show("Erro ao tentar localizar COM port do Arduino", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return "";
                    }
                    //remover qualquer coisa após ')'. 
                    portNameRes = portNameRes.Substring(0, lastindex);
                }
                #endregion

            }

            return portNameRes;
        }

        private void aquireRoutine()
        {
            try
            {
                if (arduinoPort.IsOpen)
                {
                    if (arduinoPort.BytesToRead > ArduinoConstants.PACKETSIZE)
                    {
                        valor_lido = (char)arduinoPort.ReadChar();
                        if (valor_lido == ArduinoConstants.PACKETSTART)
                        {
                            data_read = (UInt16)arduinoPort.ReadByte();
                            data_read = (UInt16)((data_read << 8) | ((UInt16)arduinoPort.ReadByte()));
                            valor_lido = (char)arduinoPort.ReadChar();

                            //    if (valor_lido == ArduinoConstants.PACKETEND) {

                            //        //Adicionar no buffer de plotagem
                            //        access_control.WaitOne();
                            //        retorno_circular_buffer = data_read.Enqueue(Convert.ToUInt16(data2plot));
                            //        access_control.ReleaseMutex();
                            //        //Adicionar no buffer da fft
                            //        pacote_fft[pacote_fft_index] = Convert.ToUInt16(data2plot);
                            //        pacote_fft_index = pacote_fft_index + 1;
                            //        if (pacote_fft_index >= fftwindow)
                            //        {
                            //            fft_access_control.WaitOne();
                            //            fft_buffer.Enqueue(pacote_fft);
                            //            fft_access_control.ReleaseMutex();
                            //            pacote_fft_index = 0;
                            //        }

                            //        if (!retorno_circular_buffer)
                            //        { //Se nao foi possivel adicionar ao buffer:
                            //            #region Mostra Mensagem de Erro
                            //            statusStrip1.Invoke(new Action(() =>
                            //            {
                            //                LabelStatusConexao.Text = "Buffer Cheio - " + arduinoPort.PortName.ToString();
                            //            }));
                            //            aquirer_is_alive = false;
                            //            plotter_is_alive = false;
                            //            MessageBox.Show("O buffer de dados encheu.", "Erro na Aquisição",
                            //                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //            #endregion
                            //        }

                            //    }
                            //}
                            //statusStrip1.BeginInvoke(new Action(() =>
                            //{
                            //    serialbufferlabel.Text = "Serial Port Buffer: " + (arduinoPort.BytesToRead).ToString() + " of " + arduinoPort.ReadBufferSize.ToString();
                            //}));
                        }

                    }
                }


    }
}
