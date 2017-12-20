using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DetectorContracao
{
    public class ChartHandler
    {
        public Chart chart;
        public ChartArea chartArea;
        public Title titleChart;
        public Series series;

        public Timer PlotterUpdater;
        public CircularBuffer<double> PlotterBuffer;

        public int qnt_pontos;
        public double[] y_values;
        public double[] x_values;

        public double freq_aquire;

        public ChartHandler(ref Chart _chart, int _qnt_points = 20,
            double facq = 1000)
        {
            freq_aquire = facq;
            chart = _chart;
            chartArea = new ChartArea();
            titleChart = new Title();
            series = new Series();

            this.SetQntPoints(_qnt_points);

            this.ConfigureChart();

            PlotterUpdater = new Timer();
            PlotterUpdater.Interval = 30;
            PlotterUpdater.Tick += PlotterUpdater_Tick;

            //series.Points.DataBindY(y_values);
            //chartArea.AxisX.Minimum = 0;
            //chartArea.AxisX.Maximum = qnt_pontos;

        }

        public void SetQntPoints(int qnt = 15)
        {
            qnt_pontos = qnt;
            y_values = new double[qnt_pontos];
            x_values = new double[qnt_pontos];
            PlotterBuffer = new CircularBuffer<double>(qnt_pontos);
            for(int i = 0; i < qnt_pontos; i++)
            {
                x_values[i] = i * (1 / freq_aquire);
            }
        }

        #region Ajustando o Chart
        public void ClearChartElements()
        {
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.Titles.Clear();
            chart.ChartAreas.Clear();
        }

        public void ConfigureChart(string chartAreaName = "ChartArea1", string chartTitle = "Gráfico", string xtitle = "Tempo (s)", string ytitle = "Tensão (V)")
        {
            ClearChartElements();
            ConfigureChartArea(chartAreaName);
            ConfigureTitle(chartTitle);
            ConfigureSeries(xtitle, ytitle);
        }

        public void ConfigureChartArea(string chartAreaName = "ChartArea1")
        {
            chartArea.Name = chartAreaName;
            chartArea.AxisX.LabelStyle.Format = "{0.00}";
            chartArea.AxisX.LabelStyle.Font = new Font("Arial", 8.0F, FontStyle.Bold);
            chartArea.AxisY.LabelStyle.Format = "{0.00}";
            chartArea.AxisY.LabelStyle.Font = new Font("Arial", 8.0F, FontStyle.Bold);
            chartArea.BackColor = Color.White; //Cor de fundo da chartArea
            //Vinculando ao chart
            chart.ChartAreas.Add(chartArea);
        }

        public void ConfigureTitle(string chartTitle = "Gráfico")
        {
            //Título da ChartArea
            titleChart.Name = "TitleChart";
            titleChart.Text = chartTitle;
            titleChart.DockedToChartArea = chartArea.Name;
            titleChart.IsDockedInsideChartArea = false;
            titleChart.Docking = Docking.Top;
            titleChart.Font = new Font("Arial", 12, FontStyle.Bold);
            titleChart.ForeColor = Color.Black;
            chart.Titles.Add(titleChart);
        }

        public void ConfigureSeries(string xtitle = "Tempo (s)", string ytitle = "Tensão (V)")
        {
            series.ChartType = SeriesChartType.FastLine; //tipo de gráfico do path
            series.Color = Color.Blue;
            series.ChartArea = chartArea.Name; //vincular a serie path à chartArea
            chart.Series.Add(series); //vincular a serie path ao chart       
            chartArea.AxisX.Title = xtitle;
            chartArea.AxisY.Title = ytitle;
        }
        #endregion

        public void AddYToBuffer(double Y)
        {
            if (!PlotterBuffer.SecureEnqueue(Y))
            {
                //MessageBox.Show("Buffer de Plottagem Cheio");
                PlotterBuffer.SecureDequeue();
                PlotterBuffer.SecureEnqueue(Y);
            }
        }

        public void UpdateYChartPoints()
        {
            int points_to_add = PlotterBuffer.Count;

            if (points_to_add > 0) //Se existem pontos a sem adicionados no chart
            {
                //Desloca os Pontos atuais para a esquerda
                for (int i = 0; i < (qnt_pontos - points_to_add); i++)
                {
                    y_values[i] = y_values[i + points_to_add];
                }

                //Adiciona os novos pontos no novo espaço a direita
                for (int i = (qnt_pontos - points_to_add); i < qnt_pontos; i++)
                {
                    y_values[i] = PlotterBuffer.SecureDequeue();
                }

                //Joga todo o conjunto de pontos no chart
                series.Points.DataBindY(y_values);
                chartArea.AxisX.Minimum = 0;
                chartArea.AxisX.Maximum = qnt_pontos;
                chartArea.AxisY.Minimum = 0;
                chartArea.AxisY.Maximum = 5;
            }
        }

        public void UpdateXYChartPoints()
        {
            int points_to_add = PlotterBuffer.Count;

            if (points_to_add > 0) //Se existem pontos a sem adicionados no chart
            {
                //Desloca os Pontos atuais para a esquerda
                for (int i = 0; i < (qnt_pontos - points_to_add); i++)
                {
                    y_values[i] = y_values[i + points_to_add];
                    x_values[i] = x_values[i + points_to_add];
                }

                //Adiciona os novos pontos no novo espaço a direita
                for (int i = (qnt_pontos - points_to_add); i < qnt_pontos; i++)
                {
                    y_values[i] = PlotterBuffer.SecureDequeue();
                    x_values[i] = x_values[i-1] + (1 / freq_aquire);
                }

                //Joga todo o conjunto de pontos no chart
                series.Points.DataBindXY(x_values,y_values);
                
                chartArea.AxisX.Minimum = Convert.ToDouble(Math.Floor(
                    x_values[0] * (1000.0 / PlotterUpdater.Interval))
                    / (1000.0 / PlotterUpdater.Interval));
                chartArea.AxisX.Maximum = Convert.ToDouble(Math.Ceiling(
                    x_values[qnt_pontos - 1] * (1000.0 / PlotterUpdater.Interval))
                    / (1000.0 / PlotterUpdater.Interval)); 
                chartArea.AxisY.Minimum = 0;
                chartArea.AxisY.Maximum = 5;
            }
        }

        private void PlotterUpdater_Tick(object sender, EventArgs e)
        {
            UpdateXYChartPoints();
        }

    }
}
