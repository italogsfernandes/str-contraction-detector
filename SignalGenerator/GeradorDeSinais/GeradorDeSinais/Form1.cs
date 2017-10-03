using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeradorDeSinais
{
    public partial class MainWindow : Form
    {
        SignalGenerator meu_gerador;

        #region Constructor e Inicializadores
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            meu_gerador = new SignalGenerator();
            cbFreqScale.SelectedIndex = 0;
            chartResultado.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chartResultado.Series[0].Points.Clear();
            chartResultado.ChartAreas[0].AxisX.Maximum = 100;
            chartResultado.ChartAreas[0].AxisY.Maximum = meu_gerador.amplitude * 1.1;
            chartResultado.ChartAreas[0].AxisY.Minimum = meu_gerador.amplitude * (-1.1);

            rbSenoidal.Checked = true;
            
        }
        #endregion

        private void radiosFormasdeOnda_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSenoidal.Checked)
            {
                meu_gerador.setSenoidal();
            }
            else if (rbQuadrada.Checked)
            {
                meu_gerador.setQuadrada();
            }
            else if(rbRampa.Checked)
            {
                meu_gerador.setRampa();
            }
        }

        private void timerUpdateChart_Tick(object sender, EventArgs e)
        {
            //chartResultado.Series[0].Points.AddXY(           
            //    (double)timerUpdateChart.Interval * meu_gerador.counter / 1000.0,
            //    meu_gerador.getValue((double)timerUpdateChart.Interval / 1000.0));

            //Qnt de pontos por segundo = 100
            //Qnt de pontos por ciclo = Fsample/Fsignal
            //Incremento do angulo = 2pi / Qnt de pontos por ciclo;


           chartResultado.Series[0].Points.AddY(meu_gerador.getValue((double)timerUpdateChart.Interval / 1000.0));
            if (chartResultado.Series[0].Points.Count >= 100)
            {
                chartResultado.Series[0].Points.RemoveAt(0);
            }
        }

        private void cbOnOff_CheckedChanged(object sender, EventArgs e)
        {
            if (timerUpdateChart.Enabled)
            {
                timerUpdateChart.Stop();
            } else
            {
                timerUpdateChart.Start();
            }
        }


        private void tbFreq_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                double signal_freq;
                if (Double.TryParse(tbFreq.Text, out signal_freq))
                {
                    meu_gerador.frequencia = signal_freq * Math.Pow(10,cbFreqScale.SelectedIndex);
                    cbFreqScale.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao inserir frequência.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbFreqScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            double signal_freq;
            if (Double.TryParse(tbFreq.Text, out signal_freq))
            {
                meu_gerador.frequencia = signal_freq * Math.Pow(10, cbFreqScale.SelectedIndex);
                tbAmplitude.Focus();
            }
            else
            {
                MessageBox.Show("Erro ao inserir frequência.", "Aviso",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbAmplitude_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double signal_amp;
                if (Double.TryParse(tbAmplitude.Text, out signal_amp))
                {
                    if (signal_amp > 0) { 
                        meu_gerador.amplitude = signal_amp;
                        chartResultado.ChartAreas[0].AxisY.Maximum = signal_amp * 1.1;
                        chartResultado.ChartAreas[0].AxisY.Minimum = signal_amp * (-1.1);
                        tbFreq.Focus();
                    } else
                    {
                        MessageBox.Show("Insira uma amplitude maior do que 0.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Erro ao inserir amplitude.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
