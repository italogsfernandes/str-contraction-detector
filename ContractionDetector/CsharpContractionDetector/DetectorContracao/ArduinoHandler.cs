using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Management;
using System.Windows.Forms;

namespace DetectorContracao
{
    //
    // Summary:
    //     Represents the constants used in the Arduino Communication.
    public static class ArduinoConstants
    {
        //
        // Summary:
        //     Represents the fized size of a data packet.
        public readonly static int PACKETSIZE = 4; //tamanho do pacote de dados
        //
        // Summary:
        //     Represents the start flag used to delimiter a packet.
        public readonly static int PACKETSTART = '$'; //bandeira usado no início do pacote
        //
        // Summary:
        //     Represents the end flag used to delimiter a packet.
        public readonly static int PACKETEND = '\n'; //bandeira usada no fim do pacote
        //
        // Summary:
        //     Represents the string to be searched as the description of the serial port.
        public readonly static string ARDUINODESCRIPTION = "Arduino"; //string a ser procurada como descrição da porta serial
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
        public SerialPort serialPort; //porta serial da classe SerialPort
        //
        // Summary:
        //     Higher methods to controls a aquisition thread.
        public ThreadHandler threadAquisition; //thread para controlar aquisição dos dados
        //
        // Summary:
        //     Represents a first-in, first-out collection of objects organized in a circular way
        //     storing the data read by the aquisition thread.
        public CircularBuffer<UInt16> bufferAquisition; //buffer para receber os dados 

        //
        // Summary:
        //     To com preguiça de ficar documentando, depois e termino....
        public string PortDescription;

        public bool dataWaiting {
            get { return bufferAquisition.Count > 0; } //retorna verdadeiro se tem dados esperando 
        }

        public bool is_running;

        private string _port_name;

        private int _start_value;
        private int _msb_value;
        private int _lsb_value;
        private int _end_value;
        private bool _res_buffer;

        public ArduinoHandler(string port_name = "None", int baudrate = 115200, int readtimeout = 1000)
        {
            _port_name = port_name; //nome da porta serial
                       
            serialPort = new SerialPort(); 
            serialPort.BaudRate = baudrate; //configurar o baudrate
            serialPort.ReadTimeout = readtimeout;// obtém ou define o número de milissegundos antes de um tempo limite ocorrer quando uma operação de leitura não termina.

            threadAquisition = new ThreadHandler(AquireRoutine); //thread
            threadAquisition.setOnEndFunction(() => { serialPort.Close(); }); //configurar a onEndFunction para fechar a serial

            bufferAquisition = new CircularBuffer<ushort>(1024); //buffer do tipo ushort
        }

        public void Open() //abrir a porta serial
        {
            try //tentar abrir
            {
                serialPort.Open();
                serialPort.DiscardInBuffer(); //descarta os dados contidos no buffer de entrada da porta serial
                serialPort.DiscardOutBuffer(); //descarta os dados contidos no buffer de saída da porta serial
            }
            catch (Exception) //se der errado
            {
                MessageBox.Show("Erro ao procurar a porta serial.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Close() //fechar a porta serial
        {
            try//tentar fechar
            {
                serialPort.Close();
            }
            catch (Exception) //mostrar mensagem caso algo dê errado
            {
                MessageBox.Show("Erro ao fechar a porta serial.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StartAquisition() //começar a aquisição de dados
        {
            if (_port_name.Equals("None")) //se não foi especificado o nome da porta
            {
                _port_name = this.GetArduinoSerialPort(); //procurar a porta do Arduino
            }
            serialPort.PortName = _port_name; //configurar o nome da porta 
            this.Open(); //abrir a porta
            threadAquisition.Start(); //iniciar a thread
            is_running = true; //thread está rodando
        }

        public void StopAquisition() //parar aquisição de dados
        {
            threadAquisition.Stop(); //parar a thread de aquisição
            is_running = false; //threa está pausada
        }

        private string GetArduinoSerialPort() //procurar a porta do arduino
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
            if (string.IsNullOrWhiteSpace(portNameRes))
            {
                portNameRes = SerialPort.GetPortNames()[0];
            }
            return portNameRes;
        }

        private void AquireRoutine() //rotina de aquisição de dados
        {
            if (serialPort.IsOpen) //se a porta serial está aberta
            {
                if(serialPort.BytesToRead > ArduinoConstants.PACKETSIZE) //se tem mais elementos para ler do que o tamanho do pacote
                {
                    _start_value = serialPort.ReadByte(); //ler o primeiro valor - deve ser o valor de início do pacote
                    if(_start_value == ArduinoConstants.PACKETSTART) //Se recebeu o sinal de Start
                    {
                        _msb_value = serialPort.ReadByte();//Le o MSB
                        _lsb_value = serialPort.ReadByte();//Le o LSB
                        _end_value = serialPort.ReadByte();//Verifica o sinal de fim de pacote
                        if(_end_value == ArduinoConstants.PACKETEND) //Se o sinal foi correto, adiciona ao buffer utilizando um mutex
                        {
                            _res_buffer = bufferAquisition.SecureEnqueue((UInt16) (_msb_value << 8 | _lsb_value)); //adicionar o valor após conversão
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
