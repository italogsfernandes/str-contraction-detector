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
    public class EMGChart : ChartHandler
    {
        public Series Envoltoriaseries;
        public double[] envoltoria_values;

        public Series LimiarSeries;
        
        public double limiar {
            get { return limiar_values[0];  }
            set {
                limiar_values = Enumerable.Repeat<double>(value, qnt_pontos).
                    ToArray();
            }
        }

        public double[] limiar_values;
        public CircularBuffer<double> EnvoltoriaBuffer;

        public Series DetectionSeries;
        //Some testes:
        double[] detection_values;
        double[] x_contraction;

        double[] detections_values;
        double[] x_contractions;

        public EMGChart(ref Chart _chart, int _qnt_points = 20,
            double facq = 1000)
        {
            y_min_value = -2.5;
            y_max_value = 2.5;
            Envoltoriaseries = new Series();
            LimiarSeries = new Series();
            DetectionSeries = new Series();

            this.freq_aquire = facq;
            this.chart = _chart;
            this.chartArea = new ChartArea();
            this.titleChart = new Title();
            this.series = new Series();

            SetQntPoints(_qnt_points);

            ConfigureChart();

            this.PlotterUpdater = new Timer();
            this.PlotterUpdater.Interval = 30;
            this.PlotterUpdater.Tick += this.PlotterUpdater_Tick;
        }

        public override void SetQntPoints(int qnt = 15)
        {
            base.SetQntPoints(qnt);

            envoltoria_values = new double[qnt_pontos];
            EnvoltoriaBuffer = new CircularBuffer<double>(qnt_pontos);
            limiar_values = new double[qnt_pontos];
        }

        public override void ConfigureSeries(
            string xtitle = "Tempo (s)", string ytitle = "Tensão (V)")
        {
            base.ConfigureSeries();

            Envoltoriaseries.ChartType = SeriesChartType.FastLine;
            Envoltoriaseries.Color = Color.Green;
            Envoltoriaseries.ChartArea = this.chartArea.Name; //vincular a serie path à chartArea
            this.chart.Series.Add(Envoltoriaseries); //vincular a serie path ao chart    

            LimiarSeries.ChartType = SeriesChartType.FastLine;
            LimiarSeries.Color = Color.Orange;
            LimiarSeries.ChartArea = this.chartArea.Name; //vincular a serie path à chartArea
            this.chart.Series.Add(LimiarSeries); //vincular a serie path ao chart  

            DetectionSeries.ChartType = SeriesChartType.Area;
            DetectionSeries.Color = Color.FromArgb(50, Color.Cyan);//Cor Transparente
            DetectionSeries.ChartArea = this.chartArea.Name; //vincular a serie path à chartArea
            this.chart.Series.Add(DetectionSeries); //vincular a serie path ao chart  
        }

        public void AddEnvoloriaToBuffer(double Y)
        {
            if (!EnvoltoriaBuffer.SecureEnqueue(Y))
            {
                //MessageBox.Show("Buffer de Plottagem Cheio");
                EnvoltoriaBuffer.SecureDequeue();
                EnvoltoriaBuffer.SecureEnqueue(Y);
            }
        }

        public override void UpdateXYChartPoints()
        {
            #region EMG
            base.UpdateXYChartPoints();
            #endregion

            #region Envoltoria, Limiar e Contracao
            int points_to_add = EnvoltoriaBuffer.Count;

            if (points_to_add > 0) //Se existem pontos a sem adicionados no chart
            {
                //Desloca os Pontos atuais para a esquerda
                for (int i = 0; i < (qnt_pontos - points_to_add); i++)
                {
                    envoltoria_values[i] = envoltoria_values[i + points_to_add];
                }

                //Adiciona os novos pontos no novo espaço a direita
                for (int i = (qnt_pontos - points_to_add); i < qnt_pontos; i++)
                {
                    envoltoria_values[i] = EnvoltoriaBuffer.SecureDequeue();
                }

                //Joga todo o conjunto de pontos no chart
                Envoltoriaseries.Points.DataBindXY(this.x_values,envoltoria_values);
                LimiarSeries.Points.DataBindXY(this.x_values, limiar_values);
                

                double time_inicio = x_values[0];
                double time_end = x_values[0];
                //x_contractions = new double[];
                //detections_values = new double[];
                for (int i = 1; i < qnt_pontos; i++)
                {
                    //Subida
                    if (envoltoria_values[i] > limiar & envoltoria_values[i - 1] < limiar)
                    {
                        time_inicio = x_values[i];
                    }
                    //Descida
                    if (envoltoria_values[i] < limiar & envoltoria_values[i - 1] > limiar)
                    {
                        time_end = x_values[i];

                        var temp = Enumerable.Range((int)(time_inicio * freq_aquire),
                            (int)(Math.Ceiling((time_end - time_inicio) * freq_aquire))).ToArray();
                        x_contraction = Array.ConvertAll(temp, v => (double)v / freq_aquire);
                        detection_values = Enumerable.Repeat<double>(y_max_value, x_contraction.Length).ToArray();

                        //x_contractions.Concat(x_contraction);
                        //detections_values.Concat(detections_values);

                        DetectionSeries.Points.DataBindXY(x_contraction, detection_values);
                    }
                }
            }
            #endregion
        }

    }
}
