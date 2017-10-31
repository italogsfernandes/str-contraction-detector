using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TAIB_Thread1
{
    public partial class Form1 : Form
    {
        public bool th_geradora_alive;
        public bool th_geradora_running;
        public bool th_aquisition_alive;
        public bool th_aquisition_running;
        public bool th_plotter_alive;
        public bool th_plotter_running;

        public double valor_gerado;
        public bool data_waiting;

        public Thread th_geradora_handler;
        public Thread th_aquisition_handler;
        public Thread th_plotter_handler;

        public CircularBuffer<double> data_buffer;

        public delegate void UpdateValueDel(int new_value);
        public UpdateValueDel updateDel;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            data_buffer = new CircularBuffer<double>(1024);

            th_geradora_alive = false;
            th_geradora_running = false;
            th_aquisition_alive = false;
            th_aquisition_running = false;
            th_plotter_alive = false;
            th_plotter_running = false;

            data_waiting = false;

            //Criando as threads
            th_geradora_handler = new Thread(geradora);
            th_aquisition_handler = new Thread(aquisition);
            th_plotter_handler = new Thread(plotter);

            th_geradora_handler.Priority = ThreadPriority.Normal;
            th_aquisition_handler.Priority = ThreadPriority.Normal;
            th_plotter_handler.Priority = ThreadPriority.Normal;

            labelValueGerado.Text = "0";


        }

        #region Funcoes das Threads


        public void atualiza_valor(int new_value)
        {
            labelValueGerado.Text = new_value.ToString();
        }

        public void geradora()
        {
            int count;
            count = -1;
            while (th_geradora_alive)
            {
                if (th_geradora_running)
                {
                    count++; // Gerando valor
                    if (count >= 100)
                    {
                        count = 0;
                    }
                    labelValueGerado.BeginInvoke(new Action(() =>
                    {
                        labelValueGerado.Text = count.ToString() + "\nData Waiting: " + data_waiting.ToString();
                    }));
                    if (!data_waiting)
                    {
                        valor_gerado = count;
                        data_waiting = true;
                    }
                    Thread.Sleep(10); //Define taxa de amostragem em 100 Hz
                }
            }
        }

        public void aquisition()
        {
            while (th_aquisition_alive)
            {
                if (th_aquisition_running)
                {
                    if (data_waiting)
                    {
                        label3.BeginInvoke(new Action(() =>
                        {
                            label3.Text = valor_gerado.ToString() + "\nBuffer: " + data_buffer.Count.ToString();
                        }));
                        data_buffer.Enqueue(valor_gerado);
                        data_waiting = false;
                    }
                    //Thread.Sleep(11);
                }
            }
        }

        public void plotter()
        {
            while (th_plotter_alive)
            {
                if (th_plotter_running)
                {
                    if (data_buffer.Count > 0)
                    {
                        double valor_para_plotar;
                        valor_para_plotar = data_buffer.Dequeue();
                        label7.BeginInvoke(new Action(() =>
                        {
                            label7.Text = valor_para_plotar.ToString() + "\nBuffer: " + data_buffer.Count.ToString();
                        }));
                        //processa valor
                        chartConsumidora.Invoke(new Action(() =>
                        {
                            chartConsumidora.Series[0].Points.AddY(valor_para_plotar);
                            if (chartConsumidora.Series[0].Points.Count >= 200)
                            {
                                chartConsumidora.Series[0].Points.RemoveAt(0);
                            }
                            if (valor_para_plotar > chartConsumidora.ChartAreas[0].AxisY.Maximum)
                            {
                                chartConsumidora.ChartAreas[0].AxisY.Maximum = valor_para_plotar;
                            }
                        }));
                    }
                    Thread.Sleep(5);
                }
            }

        }

        #endregion

        #region Eventos e Callbacks

        private void btnStartGeradora_Click(object sender, EventArgs e)
        {
            if (!th_geradora_alive)
            {
                btnStartGeradora.Text = "Stop";
                th_geradora_alive = true;
                th_geradora_running = true;
                th_geradora_handler.Start();
            }
            else
            {
                if (th_geradora_running)
                {
                    btnStartGeradora.Text = "Start";
                    th_geradora_running = false;
                    //th_geradora_handler.Abort();
                }
                else
                {
                    btnStartGeradora.Text = "Stop";
                    th_geradora_running = true;
                }
            }
        }

        private void btnStartConsumidora_Click(object sender, EventArgs e)
        {
            if (!th_plotter_alive)
            {
                btnStartConsumidora.Text = "Stop";
                th_plotter_alive = true;
                th_plotter_running = true;
                th_plotter_handler.Start();
            }
            else
            {
                if (th_plotter_running)
                {
                    btnStartConsumidora.Text = "Start";
                    th_plotter_running = false;
                    //th_geradora_handler.Abort();
                }
                else
                {
                    btnStartConsumidora.Text = "Stop";
                    th_plotter_running = true;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            th_geradora_alive = false;
            th_aquisition_alive = false;
            th_plotter_alive = false;


            // Another way but not so optimized
            /*
            if (th_consumidora_handler.IsAlive)
            {
                th_consumidora_handler.Abort();
            }
            if (th_geradora_handler.IsAlive)
            {
                th_geradora_handler.Abort();
            }*/
            
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            if (!th_aquisition_alive)
            {
                button1.Text = "Stop";
                th_aquisition_alive = true;
                th_aquisition_running = true;
                th_aquisition_handler.Start();
            }
            else
            {
                if (th_aquisition_running)
                {
                    button1.Text = "Start";
                    th_aquisition_running = false;
                    //th_geradora_handler.Abort();
                }
                else
                {
                    button1.Text = "Stop";
                    th_aquisition_running = true;
                }
            }
        }
        #endregion

    }
}
