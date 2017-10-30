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
        public bool th_consumidora_alive;
        public bool th_consumidora_running;

        public double valor_gerado;
        public bool data_waiting;

        public Thread th_geradora_handler;
        public Thread th_consumidora_handler;


        public delegate void UpdateValueDel(int new_value);
        public UpdateValueDel updateDel;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            th_geradora_alive = false;
            th_geradora_running = false;
            th_consumidora_alive = false;
            th_consumidora_running = false;

            data_waiting = false;

            //Criando as threads
            th_geradora_handler = new Thread(geradora);
            th_consumidora_handler = new Thread(consumidora);

            th_geradora_handler.Priority = ThreadPriority.Normal;
            th_consumidora_handler.Priority = ThreadPriority.Normal;

            labelValueGerado.Text = "5";


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

                    //labelValueGerado.Text = count.ToString();
                    //labelValueGerado.Invoke(delegate(int valor) { );
                    //updateDel = new UpdateValueDel(atualiza_valor);

                    //updateDel.Invoke(count);//Invoke(updateDel(count));

                    //labelValueGerado.BeginInvoke(delegate (string s)
                    //{
                    //   labelValueGerado.Text = s;
                    //}, new object[] { count.ToString() });

                    labelValueGerado.BeginInvoke(new Action(() =>
                    {
                        labelValueGerado.Text = count.ToString();
                    }));
                        //(s) => { labelValueGerado.Text = "Ue";}, new object[] { count.ToString() });
                    //labelValueGerado.Invoke((s) => { labelValueGerado.Text = s}, new object[] { count.ToString() }));
                    //Adicinando a variavel de comunicação
                    if (!data_waiting)
                    {
                        valor_gerado = count;
                        data_waiting = true;
                    }
                    Thread.Sleep(10); //Define taxa de amostragem em 100 Hz
                }
            }
        }

        public void consumidora()
        {
            while (th_consumidora_alive)
            {
                if (th_consumidora_running)
                {
                    if (data_waiting)
                    {
                        //processa valor
                        //valor_gerado
                        chartConsumidora.BeginInvoke(new Action(() =>
                        {
                            chartConsumidora.Series[0].Points.AddY(valor_gerado);
                            if(chartConsumidora.Series[0].Points.Count >= 200)
                            {
                                chartConsumidora.Series[0].Points.RemoveAt(0);
                            }
                        }));
                        
                        data_waiting = false;
                    }
                }
            }
        }

        #endregion

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
            if (!th_consumidora_alive)
            {
                btnStartConsumidora.Text = "Stop";
                th_consumidora_alive = true;
                th_consumidora_running = true;
                th_consumidora_handler.Start();
            }
            else
            {
                if (th_consumidora_running)
                {
                    btnStartConsumidora.Text = "Start";
                    th_consumidora_running = false;
                    //th_geradora_handler.Abort();
                }
                else
                {
                    btnStartConsumidora.Text = "Stop";
                    th_consumidora_running = true;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            th_geradora_alive = false;
            th_consumidora_alive = false;


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
    }
}
