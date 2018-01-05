using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DetectorContracao
{
    public partial class Form1 : Form
    {
        public double limiar;
        
        TelaChart telaChart;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            telaChart = new TelaChart();
            panel1.Controls.Add(telaChart);
            telaChart.Dock = DockStyle.Fill;
        }
    }
}
