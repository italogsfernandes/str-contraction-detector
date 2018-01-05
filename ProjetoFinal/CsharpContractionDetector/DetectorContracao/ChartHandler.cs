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
        //elementos referentes ao chart e suas configurações
        public Chart chart; 
        public ChartArea chartArea; 
        public Title titleChart;
        public Series series;

        public Timer PlotterUpdater; //timer para plotagem
        public CircularBuffer<double> PlotterBuffer; //buffer de plotagem

        public int qnt_pontos; //variável para indicar o número de pontos a serem plotados

        public double[] y_values; //valores de y
        public double[] x_values; //valores de x

        public double freq_aquire; //frequência de aquisição
        public double period_aquire {
            get { return 1 / freq_aquire; } //retorna o período de aquisição 
            set { freq_aquire = 1 / value; } //configurar a frequência de aquisição de acordo com o período especificado
        }

        public double y_max_value; //maximo valor de y
        public double y_min_value; //mínimo valor de y

        public ChartHandler() //construtor
        {

        }
        public ChartHandler(ref Chart _chart, int _qnt_points = 20,
            double facq = 1000, double _ymin = -2.5, double _ymax = 5)
        {
            freq_aquire = facq; //frequência de aquisição
            chart = _chart; //chart de acordo com a referência de chart enviada
            chartArea = new ChartArea(); //chart area
            titleChart = new Title(); //título
            series = new Series(); //série

            this.SetQntPoints(_qnt_points); //configurar a quantidade de pontos
            y_min_value = _ymin; //valor mínimo
            y_max_value = _ymax; //valor máximo

            this.ConfigureChart(); //configurar o chart

            PlotterUpdater = new Timer(); //timer
            PlotterUpdater.Interval = 30; //intervalo de tempo do timer
            PlotterUpdater.Tick += PlotterUpdater_Tick;

            //series.Points.DataBindY(y_values);
            //chartArea.AxisX.Minimum = 0;
            //chartArea.AxisX.Maximum = qnt_pontos;
        }

        public virtual void SetQntPoints(int qnt = 15) //configurar a quantidade de pontos 
        {
            qnt_pontos = qnt; //quantidade de pontos
            y_values = new double[qnt_pontos]; //vetor com a quantidade de pontos solicitada 
            x_values = new double[qnt_pontos]; //vetor com a quantidade de pontos solicitada
            PlotterBuffer = new CircularBuffer<double>(qnt_pontos); //buffer de tamanho da quantidade de pontos
            for(int i = 0; i < qnt_pontos; i++)
            {
                x_values[i] = i * (1 / freq_aquire); //vetor de tempo
            }
        }

        #region Ajustando o Chart
        public void ClearChartElements() //limpar elementos do chart
        {
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.Titles.Clear();
            chart.ChartAreas.Clear();
        }

        public void ConfigureChart(
            string chartAreaName = "ChartArea1", string chartTitle = "Gráfico",
            string xtitle = "Tempo (s)", string ytitle = "Tensão (V)")
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

        public virtual void ConfigureSeries(string xtitle = "Tempo (s)", string ytitle = "Tensão (V)")
        {
            series.ChartType = SeriesChartType.FastLine; //tipo de gráfico do path
            series.Color = Color.Blue;
            series.ChartArea = chartArea.Name; //vincular a serie path à chartArea
            chart.Series.Add(series); //vincular a serie path ao chart       
            chartArea.AxisX.Title = xtitle;
            chartArea.AxisY.Title = ytitle;
        }
        #endregion

        public virtual void AddYToBuffer(double Y) //adicionar elemento ao buffer
        {
            if (!PlotterBuffer.SecureEnqueue(Y)) //se não foi possível adicionar o elemento
            {
                //MessageBox.Show("Buffer de Plottagem Cheio");
                PlotterBuffer.SecureDequeue(); //retira um elemento 
                PlotterBuffer.SecureEnqueue(Y); //adiciona o elemento desejado
            }
        }

        public virtual void UpdateYChartPoints() //atualizar os valores de y do chart 
        {
            int points_to_add = PlotterBuffer.Count; //número de elementos presentes no buffer

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
                //configurar o máximo e mínimo dos eixos
                chartArea.AxisX.Minimum = 0;
                chartArea.AxisX.Maximum = qnt_pontos;
                chartArea.AxisY.Minimum = y_min_value;
                chartArea.AxisY.Maximum = y_max_value;
            }
        }

        public virtual void UpdateXYChartPoints() //atualizar valores de x e y no gráfico
        {
            int points_to_add = PlotterBuffer.Count; //número de elementos no buffer

            if (points_to_add > 0) //Se existem pontos a sem adicionados no chart
            {
                //Desloca os Pontos atuais para a esquerda - em y e x
                for (int i = 0; i < (qnt_pontos - points_to_add); i++)
                {
                    y_values[i] = y_values[i + points_to_add];
                    x_values[i] = x_values[i + points_to_add];
                }

                //Adiciona os novos pontos no novo espaço a direita - em y e x
                for (int i = (qnt_pontos - points_to_add); i < qnt_pontos; i++)
                {
                    y_values[i] = PlotterBuffer.SecureDequeue();
                    x_values[i] = x_values[i-1] + period_aquire;
                }

                //Joga todo o conjunto de pontos no chart
                series.Points.DataBindXY(x_values,y_values);
                
                //Configurar mínimo e máximo dos eixos
                chartArea.AxisX.Minimum = Convert.ToDouble(Math.Floor(
                    x_values[0] * (freq_aquire / PlotterUpdater.Interval))
                    / (freq_aquire / PlotterUpdater.Interval)); 
                chartArea.AxisX.Maximum = Convert.ToDouble(Math.Ceiling(
                    x_values[qnt_pontos - 1] * (freq_aquire / PlotterUpdater.Interval))
                    / (freq_aquire / PlotterUpdater.Interval)); 
                chartArea.AxisY.Minimum = -2.5;
                chartArea.AxisY.Maximum = 2.5;
                chartArea.AxisY.Minimum = y_min_value;
                chartArea.AxisY.Maximum = y_max_value;

            }
        }

        public void PlotterUpdater_Tick(object sender, EventArgs e) //atualizar os pontos em x e y 
        {
            UpdateXYChartPoints();
        }

    }
}
