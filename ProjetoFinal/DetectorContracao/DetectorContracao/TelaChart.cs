using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DetectorContracao
{
    public partial class TelaChart : UserControl
    {
        ArduinoEMGPlotter emg_dacq;
        public TelaChart()
        {
            InitializeComponent();
        }

        private void bunifuVTrackbar2_ValueChanged(object sender, EventArgs e)
        {
            emg_dacq.k = 10 - (10 * bunifuVTrackbar2.Value) / 100.0;
            bunifuCustomLabel1.Text = "Limiar:\n" + emg_dacq.emgChart.limiar.ToString("0.00")
                    + "\n(k = " + emg_dacq.k.ToString("0.00") + ") ";
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            emg_dacq.Start();
            bunifuImageButton2.BackColor = bunifuImageButton1.BackColor;
            bunifuImageButton2.Enabled = true;
        }

        private void TelaChart_Load(object sender, EventArgs e)
        {
            bunifuImageButton2.Enabled = false;
            bunifuImageButton2.BackColor = Color.LightGray;
            emg_dacq = new ArduinoEMGPlotter(ref chart1, ref label1, ref bunifuProgressBar1);
            bunifuVTrackbar2.Value = 100;
            panel5.Visible = false;
            panel5.BackColor = Color.FromArgb(50, Color.LightGreen);//Cor Transparente
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
            label2.Visible = false;
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if(!emg_dacq.is_in_calibration & !emg_dacq.BufferCalibracao.Full)
            {
                emg_dacq.BufferCalibracao.Clear();
                emg_dacq.is_in_calibration = true;
                timer1.Start();
                bunifuFlatButton1.Text = "Calibrando...";
            }
            if(!emg_dacq.is_in_calibration & emg_dacq.BufferCalibracao.Full)
            {
                panel5.Visible = false;
                bunifuFlatButton1.Text = "Iniciar Calibração";
                emg_dacq.BufferCalibracao.Clear();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (emg_dacq.BufferCalibracao.Full)
            {
                label2.Text = "Limiar: " + emg_dacq.emgChart.limiar.ToString("0.00")
                    + " (k = " + emg_dacq.k.ToString("0.00") +") ";
                bunifuCustomLabel1.Text = "Limiar:\n"+ emg_dacq.emgChart.limiar.ToString("0.00")
                    + "\n(k = " + emg_dacq.k.ToString("0.00") + ") ";
                label2.Visible = true;
                timer1.Stop();
                bunifuFlatButton1.Text = "Sair";
            }
        }

        private void bunifuSwitch1_Click(object sender, EventArgs e)
        {
            emg_dacq.emgChart.EMGBrutoVisible = !emg_dacq.emgChart.EMGBrutoVisible;
        }

        private void bunifuSwitch2_Click(object sender, EventArgs e)
        {
            emg_dacq.emgChart.EnvoltoriaVisible = !emg_dacq.emgChart.EnvoltoriaVisible;
        }

        private void bunifuSwitch3_Click(object sender, EventArgs e)
        {
            emg_dacq.emgChart.LimiarVisible = !emg_dacq.emgChart.LimiarVisible;
        }

        private void bunifuSwitch4_Click(object sender, EventArgs e)
        {
            emg_dacq.emgChart.DetectionSitesVisible = !emg_dacq.emgChart.DetectionSitesVisible;
        }

        private void panel8_Resize(object sender, EventArgs e)
        {
            bunifuVTrackbar2.Size = new Size(30, panel8.Size.Height - 60);
        }

        private void bunifuSwitch5_Click(object sender, EventArgs e)
        {
            emg_dacq.emgChart.HilbertVisible = !emg_dacq.emgChart.HilbertVisible;
        }
    }
}
