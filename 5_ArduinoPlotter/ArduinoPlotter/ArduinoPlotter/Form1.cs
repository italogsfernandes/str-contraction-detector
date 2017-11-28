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
        Thread processer;

        bool aquirer_is_alive;
        bool plotter_is_alive;
        bool processer_is_alive;

        bool plotter_is_running;
        bool processer_is_running;

        Mutex access_control;
        Mutex fft_access_control;

        CircularBuffer<UInt16> data_read;
        CircularBuffer<UInt16[]> fft_buffer;

        int max_x_points;
        double freq_aquire;
        string arduino_description;
        bool auto_ajuste_enabled;

        FFT2 fft = new FFT2();
        uint fftN; //2^12 = 4096
        int fftwindow;
        #endregion

        #region Inicializadores, Construtores e Terminadores
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Variaveis ajustaveis:
            freq_aquire = 1000;
            //max_x_points = 1000;

            fftN = 10; //2^10 = 1024
            fftwindow = (int)Math.Pow(2, fftN);
            //fftwindow = 256; //log2(256) = 8;
            //fftN = (uint)Math.Log(fftwindow, 2);
            #endregion

            #region Buffers
            fft_buffer = new CircularBuffer<UInt16[]>();
            data_read = new CircularBuffer<UInt16>(12000);
            toolStripProgressBar1.Maximum = 12000;
            #endregion

            #region Chart
            toolStripComboBox1.SelectedIndex = 0;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            //chart1.Titles[0].Text = "Sensor Cardiaco";
            chart1.Series[0].BorderWidth = 1;
            chart1.ChartAreas[0].AxisX.Title = "Pontos";
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{0:0.0}";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0.0}";

            chart2.ChartAreas[0].AxisY.Title = "Energia [|S(f)|^2]";
            chart2.ChartAreas[0].AxisX.Title = "Frequência [Hz]";
            auto_ajuste_enabled = true;
            #endregion

            #region Porta Serial do Arduino
            arduino_description = "None";
            arduinoPort = new SerialPort();
            arduinoPort.ReadBufferSize = 20000;
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
            #endregion

            #region Threads
            plotter_is_running = true;
            processer_is_running = true;
            access_control = new Mutex();
            fft_access_control = new Mutex();
            aquirer = new Thread(doAquire);
            plotter = new Thread(doPlot);
            processer = new Thread(doFFT);
            plotter_is_alive = true;
            aquirer_is_alive = true;
            processer_is_alive = true;
            processer.Priority = ThreadPriority.Normal;
            aquirer.Priority = ThreadPriority.Highest;
            plotter.Priority = ThreadPriority.Normal;
            aquirer.Start();
            plotter.Start();
            processer.Start();
            #endregion

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
                            string resultado = portDescription.Substring(portDescription.Length - 5,4);
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
            bool retorno_circular_buffer;
            UInt16[] pacote_fft;
            pacote_fft = new UInt16[fftwindow];
            int pacote_fft_index = 0;

            while (aquirer_is_alive)
            {
                try
                {
                    if (arduinoPort.IsOpen)
                    {
                        if (arduinoPort.BytesToRead > 4)
                        {
                            valor_lido = (char)arduinoPort.ReadChar();
                            if (valor_lido == '$') {
                                data2plot = arduinoPort.ReadByte();
                                data2plot = (data2plot << 8) | arduinoPort.ReadByte();
                                valor_lido = (char)arduinoPort.ReadChar();

                                if (valor_lido == '\n') {

                                    //Adicionar no buffer de plotagem
                                    access_control.WaitOne();
                                    retorno_circular_buffer = data_read.Enqueue(Convert.ToUInt16(data2plot));
                                    access_control.ReleaseMutex();
                                    //Adicionar no buffer da fft
                                    pacote_fft[pacote_fft_index] = Convert.ToUInt16(data2plot);
                                    pacote_fft_index = pacote_fft_index + 1;
                                    if (pacote_fft_index >= fftwindow) {
                                        fft_access_control.WaitOne();
                                        fft_buffer.Enqueue(pacote_fft);
                                        fft_access_control.ReleaseMutex();
                                        pacote_fft_index = 0;
                                    }

                                    if (!retorno_circular_buffer){ //Se nao foi possivel adicionar ao buffer:
                                        #region Mostra Mensagem de Erro
                                        statusStrip1.Invoke(new Action(() =>
                                        {
                                            LabelStatusConexao.Text = "Buffer Cheio - " + arduinoPort.PortName.ToString();
                                        }));
                                        aquirer_is_alive = false;
                                        plotter_is_alive = false;
                                        MessageBox.Show("O buffer de dados encheu.", "Erro na Aquisição",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
#endregion
                                    }
                                    
                                }
                            }
                            statusStrip1.BeginInvoke(new Action(() =>
                            {
                                serialbufferlabel.Text = "Serial Port Buffer: " + (arduinoPort.BytesToRead).ToString() + " of " + arduinoPort.ReadBufferSize.ToString();
                            }));

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
            int qnt_anterior_in_buffer = 0;
            double time_increment = 1.0 / freq_aquire;
            double time_stamp = 0;

            int qnt_points_every_plot;
            int qnt_points_chart;
            double time_every_plot;
            double total_time_chart;
            time_every_plot = 0.1; //plot each 100ms
            total_time_chart = 3;//5 segundos, deve ser multiplot de time_every_plot

            qnt_points_every_plot = (int) (freq_aquire * (time_every_plot));//freq_aquire - > 1s ------------ qnt_points_every_plot - > 100ms
            qnt_points_chart = (int) ((total_time_chart / time_every_plot) * qnt_points_every_plot);

            #region Vetores do grafico
            //Vetor de tempo
            double[] time_values;
            time_values = new double[qnt_points_chart];
            for (int i = 0; i < qnt_points_chart; i++) { time_values[i] = i * time_increment; }

            //Vetor de dados para plotar
            double[] y_values;
            y_values = new double[qnt_points_chart];
            for (int i = 0; i < qnt_points_chart; i++) { y_values[i] = 0; }

            //Vetor de dados colorir
            double[] color_values;
            color_values = new double[qnt_points_chart];
            for (int i = 0; i < qnt_points_chart; i++) { color_values[i] = time_values[i] > (total_time_chart - 1)?3.0:-3.0; }
            #endregion

            while (plotter_is_alive)
            {
                qnt_in_buffer = data_read.Count;
                #region Atualiza Label
                if (qnt_in_buffer != qnt_anterior_in_buffer)
                {
                    statusStrip1.Invoke(new Action(() =>
                    {
                        labelStatus.Text = "Status: " + qnt_in_buffer.ToString() + " dados no Buffer.";
                        toolStripProgressBar1.Increment(qnt_in_buffer - qnt_anterior_in_buffer);
                    }));
                }
                qnt_anterior_in_buffer = qnt_in_buffer;
                #endregion

                if (plotter_is_running)
                {
                    if(qnt_in_buffer > qnt_points_every_plot)
                    {
                        for (int i = 0; i < (qnt_points_chart - qnt_points_every_plot); i++)
                        {
                            y_values[i] = y_values[i+ qnt_points_every_plot];
                        }

                        access_control.WaitOne();
                        for (int i = (qnt_points_chart - qnt_points_every_plot); i < qnt_points_chart; i++)
                        {
                            y_values[i] = (data_read.Dequeue() - 512)*5.0 / 1023.0;
                        }
                        access_control.ReleaseMutex();

                        //time_stamp += time_increment;
                        chart1.Invoke(new Action(() =>
                        {
                            chart1.Series[0].Points.DataBindXY(time_values, y_values);
                            //chart1.Series[1].Points.DataBindXY(time_values, color_values);

                            chart1.ChartAreas[0].AxisY.Minimum = -3;
                            chart1.ChartAreas[0].AxisX.Maximum = 3;
                            

                            //chart1.ChartAreas[0].AxisX.Minimum = 0;
                            //chart1.ChartAreas[0].AxisX.Maximum = total_time_chart;

                            //chart1.Series[0].Points.AddXY( time_stamp, (value2plot-512)*5/1023.0);

                            //if (chart1.Series[0].Points.Count > max_x_points)
                            //{
                            //    chart1.Series[0].Points.RemoveAt(0);
                            //    chart1.ChartAreas[0].AxisX.Minimum += time_increment;
                            //    chart1.ChartAreas[0].AxisX.Maximum += time_increment;
                            //}
                            //if (auto_ajuste_enabled && value2plot > chart1.ChartAreas[0].AxisY.Maximum)
                            //{
                            //    //chart1.ChartAreas[0].AxisY.Maximum = value2plot;
                            //    chart1.ChartAreas[0].AxisY.Minimum = -3;
                            //    chart1.ChartAreas[0].AxisY.Maximum = 3;
                            //    txAxisYMax.Text = value2plot.ToString("#.##");
                            //}
                        }));

                        qnt_in_buffer = data_read.Count;
                        #region Atualiza Label
                        if (qnt_in_buffer != qnt_anterior_in_buffer)
                        {
                            statusStrip1.Invoke(new Action(() =>
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
        public void doFFT()
        {
            int qnt_in_buffer;
            UInt16[] values_fft;

            double[] realFFT = new double[fftwindow];
            double[] imFFT = new double[fftwindow];
            double[] pow = new double[fftwindow / 2];
            double[] freqs = new double[fftwindow / 2];
           

            while (processer_is_alive)
            {
                qnt_in_buffer = fft_buffer.Count;
                if (processer_is_running)
                {
                    if (qnt_in_buffer > 0)
                    {
                        fft_access_control.WaitOne();
                        values_fft = fft_buffer.Dequeue();
                        fft_access_control.ReleaseMutex();


                        //FFT
                        for (int i = 0; i < fftwindow; i++)
                        {
                            realFFT[i] = values_fft[i] - 512;
                            imFFT[i] = 0;
                        }
                        fft.init(fftN);
                        fft.run(realFFT, imFFT);

                        chart2.Invoke(new Action(() =>
                        {
                            chart2.Series[0].Points.Clear();
                        }));
                        for (int i = 0; i < pow.Length; i++)
                        {
                            freqs[i] = i * ((freq_aquire / 2) / (fftwindow / 2));
                            pow[i] = Math.Sqrt(Math.Pow(realFFT[i], 2) + Math.Pow(imFFT[i], 2));
                            //chart2.Invoke(new Action(() =>
                            //{
                            //    chart2.Series[0].Points.AddXY(freqs[i], pow[i]);

                            //}));
                        }

                        chart2.Invoke(new Action(() =>
                        {
                            chart2.Series[0].Points.DataBindXY(freqs, pow);

                            chart2.ChartAreas[0].AxisX.Minimum = 0;
                            chart2.ChartAreas[0].AxisX.Maximum = 500;
                            
                            //chart2.ChartAreas[0].AxisY.Minimum = 0;
                            //chart2.ChartAreas[0].AxisY.Maximum = 2000;
                        }));

                        //
                        //chart2.ChartAreas[0].AxisY.Minimum = 0;
                        //chart2.ChartAreas[0].AxisY.Maximum = 10;
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
