using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Management;
using System.Windows.Forms;

namespace LibrariesExample
{
    //
    // Summary:
    //     Represents the constants used in the Arduino Communication.
    public static class ArduinoConstants
    {
        //
        // Summary:
        //     Represents the fized size of a data packet.
        public readonly static int PACKETSIZE = 4;
        //
        // Summary:
        //     Represents the start flag used to delimiter a packet.
        public readonly static int PACKETSTART = '$';
        //
        // Summary:
        //     Represents the end flag used to delimiter a packet.
        public readonly static int PACKETEND = '\n';
        //
        // Summary:
        //     Represents the string to be searched as the description of the serial port.
        public readonly static string ARDUINODESCRIPTION = "Arduino";
    }

    //
    // Summary:
    //     Handles the communication with the arduino microcontroller. Its mainly function is data aquisition.
    public class ArduinoHandler
    {
        //
        // Summary:
        //     Represents a serial port resource.To browse the .NET Framework source code for
        //     this type, see the Reference Source.
        public SerialPort serialPort;
        //
        // Summary:
        //     Higher methods to controls a aquisition thread.
        public ThreadHandler threadAquisition;
        //
        // Summary:
        //     Represents a first-in, first-out collection of objects organized in a circular way
        //     storing the data read by the aquisition thread.
        public CircularBuffer<UInt16> bufferAquisition;

        //
        // Summary:
        //     To com preguiça de ficar documentando, depois e termino....
        public string PortDescription;

        public bool dataWaiting {
            get { return bufferAquisition.Count > 0; }
        }

        private string _port_name;

        private int _start_value;
        private int _msb_value;
        private int _lsb_value;
        private int _end_value;
        private bool _res_buffer;

        public ArduinoHandler(string port_name = "None", int baudrate = 115200, int readtimeout = 1000)
        {
            _port_name = port_name;
                       
            serialPort = new SerialPort();
            serialPort.BaudRate = baudrate;
            serialPort.ReadTimeout = readtimeout;

            threadAquisition = new ThreadHandler(AquireRoutine);
            threadAquisition.setOnEndFunction(() => { serialPort.Close(); });

            bufferAquisition = new CircularBuffer<ushort>(1024);
        }

        public void Open()
        {
            try
            {
                serialPort.Open();
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao procurar a porta serial.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Close()
        {
            try
            {
                serialPort.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao fechar a porta serial.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StartAquisition()
        {
            if (_port_name.Equals("None"))
            {
                _port_name = this.GetArduinoSerialPort();
            }
            serialPort.PortName = _port_name;
            this.Open();
            threadAquisition.Start();
        }

        public void StopAquisition()
        {
            threadAquisition.Stop();
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
                if (s.Contains(ArduinoConstants.ARDUINODESCRIPTION))
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
                    PortDescription = s;
                }
                #endregion

            }

            return portNameRes;
        }

        private void AquireRoutine()
        {
            if (serialPort.IsOpen)
            {
                if(serialPort.BytesToRead > ArduinoConstants.PACKETSIZE)
                {
                    _start_value = serialPort.ReadByte();
                    if(_start_value == ArduinoConstants.PACKETSTART) //Se recebeu o sinal de Start
                    {
                        _msb_value = serialPort.ReadByte();//Le o MSB
                        _lsb_value = serialPort.ReadByte();//Le o LSB
                        _end_value = serialPort.ReadByte();//Verifica o sinal de fim de pacote
                        if(_end_value == ArduinoConstants.PACKETEND) //Se o sinal foi correto, adiciona ao buffer utilizando um mutex
                        {
                            _res_buffer = bufferAquisition.SecureEnqueue((UInt16) (_msb_value << 8 | _lsb_value));
                            if (!_res_buffer) //Se nao foi possivel adicionar ao buffer:
                            {
                                MessageBox.Show("O buffer de dados encheu.", "Erro na Aquisição",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

            }
        }

    }
}
