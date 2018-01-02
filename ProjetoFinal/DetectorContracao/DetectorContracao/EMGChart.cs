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
        public Series Envoltoriaseries; //série para a envoltoria
        public double[] envoltoria_values;
        public double[] hilbert_values;
        public double[] envoltoria_values_borderless;
        double[] valorhbt_filtrado;
        double[] valorhbt_filtrado_borderless;

        public bool bandeiraEMG;
        public bool bandeiraEnvoltoria;
        public bool bandeiraLimiar;
        public bool bandeiraDeteccao;
        public bool bandeiraHilbert;

        public Series LimiarSeries; //série para o limiar
        
        public double limiar {
            get { return limiar_values[0];  } //retorna o limiar de detecção
            set {
                limiar_values = Enumerable.Repeat<double>(value, qnt_pontos).
                    ToArray(); //criar um array com qnt_pontos com valor igual a value
            }
        }

        public double[] limiar_values;
        public CircularBuffer<double> EnvoltoriaBuffer; //buffer para os pontos da envoltória

        public Series DetectionSeries; //série de detecção da contração

        double[] detection_values;

        public EMGChart(ref Chart _chart, int _qnt_points = 20,
            double facq = 1000)
        {
            y_min_value = -2.5; //valor minimo do eixo y
            y_max_value = 2.5; //valor máximo do eixo y
            Envoltoriaseries = new Series(); //instancializar a série para envoltória
            LimiarSeries = new Series();// instancializar a série para o limiar
            DetectionSeries = new Series(); //inicializar a série para a detecção

            this.freq_aquire = facq; //frequência de aquisição
            this.chart = _chart; //chart enviado por referência 
            this.chartArea = new ChartArea(); //instancializar uma chart area
            this.titleChart = new Title(); //instancializar um title
            this.series = new Series(); //instancializar uma série 

            SetQntPoints(_qnt_points); //criar os vetores e buffer com o número de pontos desejados

            ConfigureChart(); //configurar o chart

            this.PlotterUpdater = new Timer(); //timer para plotagem
            this.PlotterUpdater.Interval = 30; //intervalo de tempo para o timer
            this.PlotterUpdater.Tick += this.PlotterUpdater_Tick;

            bandeiraEMG = true;
            bandeiraEnvoltoria = true;
            bandeiraLimiar = true;
            bandeiraDeteccao = true;
        }

        public override void SetQntPoints(int qnt = 15)  //??
        {
            base.SetQntPoints(qnt);

            envoltoria_values = new double[qnt_pontos];
            valorhbt_filtrado = new double[qnt_pontos];
            detection_values = new double[qnt_pontos];
            hilbert_values = new double[qnt_pontos];
            envoltoria_values_borderless = new double[qnt_pontos*2];
            valorhbt_filtrado_borderless = new double[qnt_pontos*2];
            EnvoltoriaBuffer = new CircularBuffer<double>(qnt_pontos);
            limiar_values = new double[qnt_pontos];
        }

        public override void ConfigureSeries(
            string xtitle = "Tempo (s)", string ytitle = "Tensão (V)") //configurar as série 
        {
            base.ConfigureSeries();

            Envoltoriaseries.ChartType = SeriesChartType.FastLine;
            Envoltoriaseries.Color = Color.DarkRed;
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

        public void AddEnvoloriaToBuffer(double Y) //adicionar valor de envoltória no buffer
        {
            if (!EnvoltoriaBuffer.SecureEnqueue(Y)) //se não foi possível adiconar um novo valor
            {
                //MessageBox.Show("Buffer de Plottagem Cheio");
                EnvoltoriaBuffer.SecureDequeue(); //retira um valor 
                EnvoltoriaBuffer.SecureEnqueue(Y);//adiciona o novo valor
            }
        }

        public override void UpdateXYChartPoints()
        {
            #region EMG
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
                    x_values[i] = x_values[i - 1] + period_aquire;
                }

                //Joga todo o conjunto de pontos no chart
                if (bandeiraEMG)
                {
                    series.Color = Color.Blue;
                    series.Points.DataBindXY(x_values, y_values);
                }

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
                #endregion

            #region Envoltoria, Limiar e Contracao
            if (points_to_add > 0) //Se existem pontos a sem adicionados no chart
            {
                for (int i = 0; i < y_values.Length; i++)
                {
                    envoltoria_values[i] = y_values[i];
                }
                
                Accord.Math.HilbertTransform.FHT(envoltoria_values, Accord.Math.FourierTransform.Direction.Forward); //aplicar a transformada de Hilbert
                for (int i = 0; i < envoltoria_values.Length; i++)
                {
                    envoltoria_values[i] = Math.Abs(envoltoria_values[i]);
                    hilbert_values[i] = envoltoria_values[i];
                }

                valorhbt_filtrado = Butter.Butterworth(envoltoria_values, this.period_aquire, 7);
                for (int i = 0; i < valorhbt_filtrado.Length; i++)
                {
                    valorhbt_filtrado[i] = valorhbt_filtrado[i] * 3.23;
                }
                if (bandeiraHilbert)
                {
                    series.Color = Color.Green;
                    series.Points.DataBindXY(this.x_values, hilbert_values);
                }
                if (bandeiraEnvoltoria)
                {
           
                    Envoltoriaseries.Points.DataBindXY(this.x_values, valorhbt_filtrado);
                }

                #region some bigger stuff
                ////Borda falsa de inicio
                //int ii,jj;
                //ii = envoltoria_values.Length / 2;
                //jj = 0;
                //while (ii>0)
                //{
                //    envoltoria_values_borderless[jj] = envoltoria_values[ii];
                //    ii--;
                //    jj++;
                //}

                //ii = 0;
                //jj = envoltoria_values.Length / 2;
                //while ( ii < envoltoria_values.Length)
                //{
                //    envoltoria_values_borderless[jj] = envoltoria_values[ii];
                //    ii++;
                //    jj++;
                //}

                //ii = envoltoria_values.Length - 1;
                //jj = (int) 1.5* envoltoria_values.Length;
                //while (jj < envoltoria_values_borderless.Length)
                //{
                //    envoltoria_values_borderless[jj] = envoltoria_values[ii];
                //    ii--;
                //    jj++;
                //}


                //valorhbt_filtrado_borderless = Butter.Butterworth(envoltoria_values_borderless, this.period_aquire, 7);
                //valorhbt_filtrado = new double[qnt_pontos];

                //for (int i = 0; i < valorhbt_filtrado.Length; i++)
                //{
                //    valorhbt_filtrado[i] = valorhbt_filtrado_borderless[i + valorhbt_filtrado.Length / 2];
                //}
                ////for (int i = 0; i < valorhbt_filtrado.Length; i++)
                ////{
                ////    valorhbt_filtrado[i] *= 3.73;
                ////}
                ////Joga todo o conjunto de pontos no chart
                //Envoltoriaseries.Points.DataBindXY(this.x_values, valorhbt_filtrado);

                if(bandeiraLimiar)
                     LimiarSeries.Points.DataBindXY(this.x_values, limiar_values);

                #region some stuff
                double time_inicio = x_values[qnt_pontos-1];
                double time_end = x_values[0];
                //x_contractions = new double[];
                //detections_values = new double[];
                for (int i = 1; i < qnt_pontos; i++)
                {
                    //Subida
                    if (envoltoria_values[i] > limiar & envoltoria_values[i - 1] < limiar)
                    {
                        time_inicio = x_values[i];
                        time_end = x_values[qnt_pontos - 1];
                    }
                    //Descida
                    if (envoltoria_values[i] < limiar & envoltoria_values[i - 1] > limiar)
                    {
                        time_end = x_values[i];
                    }

                    if (x_values[i] > time_inicio)
                    {
                        detection_values[i] = 2.5;
                    }

                    if(x_values[i] > time_end)
                    {
                        detection_values[i] = -3;
                    }
                }
                if(bandeiraDeteccao)
                    DetectionSeries.Points.DataBindXY(x_values, detection_values);
                #endregion
                #endregion
            }
            #endregion
        }

    }
}
