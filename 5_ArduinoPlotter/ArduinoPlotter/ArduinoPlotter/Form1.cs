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
        #region Atributos e Propriedades
        SerialPort arduinoPort;
        Thread aquirer;
        Thread plotter;
        bool aquirer_is_alive;
        bool plotter_is_alive;
        Mutex access_control;
        CircularBuffer<UInt16> data_read;
        int max_x_points;
        bool plotter_is_running;
        string arduino_description;
        bool auto_ajuste_enabled;
        #endregion

        #region Inicializadores, Construtores e Terminadores
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arduino_description = "None";
            max_x_points = 1500;
            auto_ajuste_enabled = true;
            plotter_is_running = true;
            toolStripComboBox1.SelectedIndex = 0;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            //chart1.Titles[0].Text = "Sensor Cardiaco";
            chart1.Series[0].BorderWidth = 1;

            arduinoPort = new SerialPort();
            arduinoPort.PortName = GetArduinoSerialPort();
            arduinoPort.BaudRate = 115200;
            arduinoPort.ReadTimeout = 1000;
            try {
                arduinoPort.Open();
                arduinoPort.DiscardInBuffer();
                arduinoPort.DiscardOutBuffer();
                LabelStatusConexao.Text = "Conectado à " + arduino_description;
            }
            catch (Exception) {
                LabelStatusConexao.Text = "Desconectado à " + arduino_description;
                MessageBox.Show("Erro ao procurar a porta serial.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            access_control = new Mutex();
            data_read = new CircularBuffer<UInt16>(10240);
            toolStripProgressBar1.Maximum = 1023;
            aquirer = new Thread(doAquire);
            plotter = new Thread(doPlot);
            plotter_is_alive = true;
            aquirer_is_alive = true;
            aquirer.Priority = ThreadPriority.Normal;
            plotter.Priority = ThreadPriority.Normal;
            aquirer.Start();
            plotter.Start();
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.0}";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.0}";
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
                        if (obj["Caption"] == null)
                        {
                            ss = "";
                        }
                        else
                        {
                            ss = obj["Caption"].ToString();
                        }
                        if (ss.Contains("Arduino"))
                        {
                            string portDescription = obj["Caption"].ToString();
                            arduino_description = portDescription;
                            string resultado = portDescription.Substring(portDescription.Length - 6,5);
                            return resultado;
                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine(e.ToString());
                return "COM3";
            }
            return "COM25"; // Work around.... 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            aquirer_is_alive = false;
            plotter_is_alive = false;
            plotter.Abort();
        }

        #endregion

        #region Threads
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
                        while (arduinoPort.BytesToRead > 4)
                        {
                            valor_lido = (char)arduinoPort.ReadChar();
                            if (valor_lido == '$')
                            {
                                data2plot = arduinoPort.ReadByte();
                                data2plot = (data2plot << 8) | arduinoPort.ReadByte();
                                valor_lido = (char)arduinoPort.ReadChar();
                                if (valor_lido == '\n')
                                {

                                    bool retorno_circular_buffer;
                                    access_control.WaitOne();
                                    retorno_circular_buffer = data_read.Enqueue(Convert.ToUInt16(data2plot));
                                    access_control.ReleaseMutex();
                                    if (!retorno_circular_buffer)
                                    {
                                        statusStrip1.Invoke(new Action(() =>
                                        {
                                            LabelStatusConexao.Text = "Buffer Cheio - " + arduinoPort.PortName.ToString();
                                        }));
                                        
                                        MessageBox.Show("O buffer de dados encheu.", "Erro na Aquisição",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        aquirer_is_alive = false;
                                        plotter_is_alive = false;
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //LabelStatusConexao.Text = "Desconectado à Porta " + arduinoPort.PortName.ToString();
                    MessageBox.Show("Arduino Desconectado", "Erro na Aquisição",
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            arduinoPort.Close();
        }
        public void doPlot()
        {
            UInt16 value2plot;
            int qnt_in_buffer;
            double freq_aquire = 500;
            double time_increment = 1.0 / freq_aquire;
            double time_stamp = 0;
            int qnt_anterior_in_buffer = 0;
            while (plotter_is_alive)
            {
                qnt_in_buffer = data_read.Count;
                #region Atualiza Label
                if (qnt_in_buffer != qnt_anterior_in_buffer)
                {
                    statusStrip1.BeginInvoke(new Action(() =>
                    {
                        labelStatus.Text = "Status: " + qnt_in_buffer.ToString() + " dados no Buffer.";
                        toolStripProgressBar1.Increment(qnt_in_buffer - qnt_anterior_in_buffer);
                    }));
                }
                qnt_anterior_in_buffer = qnt_in_buffer;
                #endregion


                if (plotter_is_running)
                {
                    while (qnt_in_buffer > 0)
                    {
                        access_control.WaitOne();
                        value2plot = data_read.Dequeue();
                        access_control.ReleaseMutex();

                        time_stamp += time_increment;
                        chart1.Invoke(new Action(() =>
                        {
                            chart1.Series[0].Points.AddXY(time_stamp, value2plot);

                            if (chart1.Series[0].Points.Count > max_x_points)
                            {
                                chart1.Series[0].Points.RemoveAt(0);
                                chart1.ChartAreas[0].AxisX.Minimum += time_increment;
                                chart1.ChartAreas[0].AxisX.Maximum += time_increment;
                            }
                            //if (auto_ajuste_enabled && value2plot > chart1.ChartAreas[0].AxisY.Maximum)
                            //{
                            //    chart1.ChartAreas[0].AxisY.Maximum = value2plot;
                            //    txAxisYMax.Text = value2plot.ToString("#.##");
                            //}
                        }));

                        qnt_in_buffer = data_read.Count;
                        #region Atualiza Label
                        if (qnt_in_buffer != qnt_anterior_in_buffer)
                        {
                            statusStrip1.BeginInvoke(new Action(() =>
                            {
                                labelStatus.Text = "Status: " + qnt_in_buffer.ToString() + " dados no Buffer.";
                                toolStripProgressBar1.Increment(qnt_in_buffer - qnt_anterior_in_buffer);
                            }));
                        }
                        qnt_anterior_in_buffer = qnt_in_buffer;
                        #endregion
                    }
                }
            }
        }
        #endregion

        #region Interface Callbacks
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


        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            auto_ajuste_enabled = toolStripComboBox1.SelectedIndex == 0;
        }

        private void txAxisXMin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double novo_valor;
                if (Double.TryParse(txAxisXMin.Text, out novo_valor))
                {
                    chart1.ChartAreas[0].AxisX.Minimum = novo_valor;
                }
                else
                {
                    MessageBox.Show("Erro ao inserir o valor.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txAxisXMax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double novo_valor;
                if (Double.TryParse(txAxisXMax.Text, out novo_valor))
                {
                    chart1.ChartAreas[0].AxisX.Maximum = novo_valor + 1;
                    max_x_points = (int) novo_valor;
                }
                else
                {
                    MessageBox.Show("Erro ao inserir o valor.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txAxisYMin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double novo_valor;
                if (Double.TryParse(txAxisYMin.Text, out novo_valor))
                {
                    chart1.ChartAreas[0].AxisY.Minimum = novo_valor;
                }
                else
                {
                    MessageBox.Show("Erro ao inserir o valor.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txAxisYMax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double novo_valor;
                if (Double.TryParse(txAxisYMax.Text, out novo_valor))
                {
                    if (!auto_ajuste_enabled)
                    {
                        chart1.ChartAreas[0].AxisY.Maximum = novo_valor;
                    }
                }
                else
                {
                    MessageBox.Show("Erro ao inserir o valor.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txAxisXMin.Text = chart1.ChartAreas[0].AxisX.Minimum.ToString();
            txAxisXMax.Text = chart1.ChartAreas[0].AxisX.Maximum.ToString();
            txAxisYMin.Text = chart1.ChartAreas[0].AxisY.Minimum.ToString();
            txAxisYMax.Text = chart1.ChartAreas[0].AxisY.Maximum.ToString();
        }

        private void limparBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            access_control.WaitOne();
            data_read.Clear();
            access_control.ReleaseMutex();
        }

        private void autoSetEixoYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripComboBox1.SelectedIndex = 1;
            auto_ajuste_enabled = false;
            chart1.ChartAreas[0].AxisY.Minimum = 2;
            chart1.ChartAreas[0].AxisY.Maximum = 3;
        }

        private void pausePlotterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            plotter_is_running = !plotter_is_running;
        }

        private void timer1_Tick(object sender, EventArgs e) { 
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }


}
