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
        List<SignalGenerator> geradores;
        SignalGenerator meu_gerador;
        #region Constructor e Inicializadores
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            geradores = new List<SignalGenerator>();
            meu_gerador = new SignalGenerator();
            geradores.Add(meu_gerador);
            cbOndas.Items.Add("0");
            cbOndas.SelectedIndex = 0;

            cbFreqScale.SelectedIndex = 0;
            chartResultado.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chartResultado.Series[0].Points.Clear();
            chartResultado.ChartAreas[0].AxisX.Maximum = 100;
            chartResultado.ChartAreas[0].AxisY.Maximum = geradores[0].amplitude * 1;
            chartResultado.ChartAreas[0].AxisY.Minimum = geradores[0].amplitude * (-1);

            rbSenoidal.Checked = true;
            cbSomar.Checked = true;
            cbOnOff.Checked = true;
            
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

            double valor_para_plotar;
            double amplitudes;
            if (cbSomar.Checked)
            {
                valor_para_plotar = 0;
                amplitudes = 0;
                foreach (SignalGenerator componente in geradores)
                {
                    valor_para_plotar += componente.getValue((double)timerUpdateChart.Interval / 1000.0);
                    amplitudes += componente.amplitude;
                }
                chartResultado.ChartAreas[0].AxisY.Maximum = amplitudes;
                chartResultado.ChartAreas[0].AxisY.Minimum = -amplitudes;
            }
            else
            {
                valor_para_plotar = meu_gerador.getValue((double)timerUpdateChart.Interval / 1000.0);
            }
            chartResultado.Series[0].Points.AddY(valor_para_plotar);


            //if((double)timerUpdateChart.Interval * meu_gerador.counter / 1000.0 > chartResultado.ChartAreas[0].AxisX.Maximum)
            //{
            //    chartResultado.Series[0].Points.RemoveAt(0);
            //    chartResultado.ChartAreas[0].AxisX.Maximum += (double) timerUpdateChart.Interval / 1000.0;
            //    chartResultado.ChartAreas[0].AxisX.Minimum += (double) timerUpdateChart.Interval / 1000.0;

            //}

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
                    tbAmplitude.Focus();
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
                        chartResultado.ChartAreas[0].AxisY.Maximum = signal_amp * 1.0;
                        chartResultado.ChartAreas[0].AxisY.Minimum = signal_amp * (-1.0);
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

        private void tbSampleFreq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double aquiring_freq;
                if (Double.TryParse(tbSampleFreq.Text, out aquiring_freq))
                {
                    timerUpdateChart.Interval = Convert.ToInt32(Math.Round(1000 / aquiring_freq));
                    aquiring_freq = (double) 1000.0 / timerUpdateChart.Interval;
                    tbSampleFreq.Text = aquiring_freq.ToString("#.##");
                    tbsamplePeriod.Text = timerUpdateChart.Interval.ToString() ;
                    tbFreq.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao inserir frequência.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tbsamplePeriod_KeyDown(object sender, KeyEventArgs e)
        {
            int desired_interval;
            double aquiring_freq;
            if (Int32.TryParse(tbsamplePeriod.Text, out desired_interval))
            {
                timerUpdateChart.Interval = desired_interval;
                aquiring_freq = (double) 1000.0 / timerUpdateChart.Interval;
                tbSampleFreq.Text = aquiring_freq.ToString();
                tbFreq.Focus();
            }
            else
            {
                MessageBox.Show("Erro ao inserir frequência.", "Aviso",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbOndas_SelectedIndexChanged(object sender, EventArgs e)
        {
            meu_gerador = geradores[cbOndas.SelectedIndex];
            update_visual_elements();
        }

        void update_visual_elements()
        {
            //Frequencia de aquisição
            double aquiring_freq = (double)1000.0 / timerUpdateChart.Interval;
            tbSampleFreq.Text = aquiring_freq.ToString("#.##");
            tbsamplePeriod.Text = timerUpdateChart.Interval.ToString();
            //Frequencia e amplitude do Sinal
            tbFreq.Text = meu_gerador.frequencia.ToString("#.##");
            tbAmplitude.Text = meu_gerador.amplitude.ToString("#.##");
            //Tipo de onda
            switch (meu_gerador.forma_da_onda)
            {
                case SignalGenerator.WaveForm.Senoidal:
                    rbSenoidal.Checked = true;
                    rbQuadrada.Checked = false;
                    rbRampa.Checked = false;
                    break;
                case SignalGenerator.WaveForm.Quadrada:
                    rbSenoidal.Checked = false;
                    rbQuadrada.Checked = true;
                    rbRampa.Checked = false;
                    break;
                case SignalGenerator.WaveForm.Rampa:
                    rbSenoidal.Checked = false;
                    rbQuadrada.Checked = false;
                    rbRampa.Checked = true;
                    break;

            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            meu_gerador = new SignalGenerator();
            geradores.Add(meu_gerador);
            cbOndas.Items.Add((geradores.Count - 1).ToString());
            cbOndas.SelectedIndex = (geradores.Count - 1);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            if (cbOndas.SelectedIndex != 0)
            {
                geradores.RemoveAt(cbOndas.SelectedIndex);
                cbOndas.Items.RemoveAt(cbOndas.SelectedIndex);
                cbOndas.SelectedIndex = 0;
                meu_gerador = geradores[0];
            } else
            {
                MessageBox.Show("Não é possível remover esta componente.", "Aviso",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
