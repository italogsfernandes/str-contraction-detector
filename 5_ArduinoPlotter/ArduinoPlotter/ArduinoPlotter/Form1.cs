using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Management;

namespace ArduinoPlotter
{
    public partial class Form1 : Form
    {
        SerialPort arduinoPort;
        Thread aquirer;
        Thread plotter;
        bool aquirer_is_alive;
        bool plotter_is_alive;
        Mutex access_control;
        Queue<double> data_read;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            arduinoPort = new SerialPort();
            arduinoPort.PortName = GetArduinoSerialPort();
            arduinoPort.BaudRate = 115200;
            arduinoPort.ReadTimeout = 1000;
            try {
                arduinoPort.Open();
                arduinoPort.DiscardInBuffer();
                arduinoPort.DiscardOutBuffer();
            }
            catch (Exception) {
                LabelStatusConexao.Text = "Desconectado à Porta " + arduinoPort.PortName.ToString();
                MessageBox.Show("Erro ao procurar a porta serial.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            access_control = new Mutex();
            data_read = new Queue<double>(1024);
            aquirer = new Thread(doAquire);
            plotter = new Thread(doPlot);
            plotter_is_alive = true;
            aquirer_is_alive = true;
            aquirer.Start();
            plotter.Start();
        }

 
        public void doAquire()
        {
            int data2plot;
            char valor_lido;
            while (aquirer_is_alive)
            {
                try
                {
                    if (arduinoPort.IsOpen)
                    {
                        if (arduinoPort.BytesToRead > 4)
                        {
                            valor_lido = (char)arduinoPort.ReadChar();
                            if (valor_lido == '$')
                            {
                                data2plot = arduinoPort.ReadByte();
                                data2plot = (data2plot << 8) | arduinoPort.ReadByte();
                                valor_lido = (char)arduinoPort.ReadChar();
                                if (valor_lido == '\n')
                                {
                                    access_control.WaitOne();
                                    data_read.Enqueue(data2plot * 5.0 / 1023.0);
                                    access_control.ReleaseMutex();
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    LabelStatusConexao.Text = "Desconectado à Porta " + arduinoPort.PortName.ToString();
                    MessageBox.Show("Erro com a porta serial.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            arduinoPort.Close();
        }
        public void doPlot()
        {
            double value2plot;
            int qnt_in_buffer;
            while (plotter_is_alive)
            {
                access_control.WaitOne();
                qnt_in_buffer = data_read.Count;
                statusStrip1.Invoke(new Action(() =>
                {
                    labelStatus.Text = "Status: " + qnt_in_buffer.ToString() + " dados no Buffer.";
                }));
                access_control.ReleaseMutex();
                if (qnt_in_buffer > 0)
                {
                    access_control.WaitOne();
                    value2plot = data_read.Dequeue();
                    access_control.ReleaseMutex();
                    chart1.Invoke(new Action(() =>
                    {
                        chart1.Series[0].Points.AddY(value2plot);
                        if(chart1.Series[0].Points.Count > 600)
                        {
                            chart1.Series[0].Points.RemoveAt(0);
                        }
                    }));
                }
            }
        }

        public string GetArduinoSerialPort()
        {
            try
            {
                var query = new ManagementObjectSearcher("root\\CIMV2",
                                                         "SELECT * FROM Win32_PnPEntity");

                query.Options.Timeout = new TimeSpan(0, 0, 1); //timeout => TimeSpan(horas, minutos, segundos);
                query.Options.ReturnImmediately = false;

                foreach (ManagementObject obj in query.Get())
                {
                    using (obj)
                    {
                        string ss;
                        if (obj["Caption"] == null){
                            ss = "";
                        }
                        else{
                            ss = obj["Caption"].ToString();
                        }
                        if (ss.Contains("Arduino")) {
                            string portDescription = obj["Caption"].ToString();
                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
            return "COM5"; // Work around.... 
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            aquirer_is_alive = false;
            plotter_is_alive = false;
        }

        private void ajudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "http://www.google.com/";
                myProcess.Start();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "https://github.com/italogfernandes/str";
                myProcess.Start();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
    

}
