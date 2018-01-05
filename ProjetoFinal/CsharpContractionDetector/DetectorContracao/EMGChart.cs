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
    public class EMGChart : ChartHandler {

        //Dados:
        double[] time_values;
        double[] emg_bruto_values;
        double[] hilbert_values;
        double[] hilbert_retificado_values;
        double[] envoltoria_values;
        double[] limiar_values;
        double[] detection_sites_values;

        public double limiar {
            get { return limiar_values[0]; } //retorna o limiar de detecção
            set {
                limiar_values = Enumerable.Repeat<double>(value, qnt_pontos).
                    ToArray(); //criar um array com qnt_pontos com valor igual a value
            }
        }

        //Series:
        public Series EMGBrutoSeries;
        public Series HilbertSeries;
        public Series HilbertRetificadoSeries;
        public Series EnvoltoriaSeries;
        public Series LimiarSeries;
        public Series DetectionSitesSeries;

        //Indicadores
        public bool EMGBrutoVisible;
        public bool HilbertVisible;
        public bool HilbertRetificadoVisible;
        public bool EnvoltoriaVisible;
        public bool LimiarVisible;
        public bool DetectionSitesVisible;

        public CircularBuffer<double[]> EMGPlotterBuffer; //buffer para os pontos da envoltória
        
        public EMGChart(ref Chart _chart, int _qnt_points = 20, double facq = 1000)
        {
            //Base
            this.freq_aquire = facq; //frequência de aquisição
            this.chart = _chart; //chart enviado por referência 
            this.chartArea = new ChartArea(); //instancializar uma chart area
            this.titleChart = new Title(); //instancializar um title
            this.series = new Series(); //instancializar uma série

            //Series
            EMGBrutoSeries = new Series();
            HilbertSeries = new Series();
            HilbertRetificadoSeries = new Series();
            EnvoltoriaSeries = new Series();
            LimiarSeries = new Series();
            DetectionSitesSeries = new Series();

            y_min_value = -2.5; //valor minimo do eixo y
            y_max_value = 2.5; //valor máximo do eixo y

            //Vetores
            SetQntPoints(_qnt_points); //criar os vetores e buffer com o número de pontos desejados

            //Adicionar elementos do chart
            ConfigureChart(); //configurar o chart

            this.PlotterUpdater = new Timer(); //timer para plotagem
            this.PlotterUpdater.Interval = 20; //intervalo de tempo para o timer
            this.PlotterUpdater.Tick += this.PlotterUpdater_Tick;

            //Flags de visibilidade
            EMGBrutoVisible= true;
            HilbertVisible = false;
            HilbertRetificadoVisible = false;
            EnvoltoriaVisible = true;
            LimiarVisible = false;
            DetectionSitesVisible = false;
    }

        public override void SetQntPoints(int qnt)
        {
            qnt_pontos = qnt; //quantidade de pontos

            time_values = new double[qnt_pontos];
            emg_bruto_values = new double[qnt_pontos];
            hilbert_values = new double[qnt_pontos];
            hilbert_retificado_values = new double[qnt_pontos];
            envoltoria_values = new double[qnt_pontos];
            limiar_values = new double[qnt_pontos];
            detection_sites_values = new double[qnt_pontos];

            EMGPlotterBuffer = new CircularBuffer<double[]>(qnt_pontos);
        }

        public override void ConfigureSeries(
            string xtitle = "Tempo (s)", string ytitle = "Tensão (V)") //configurar as série 
        {
            #region EMG Bruto
            EMGBrutoSeries.ChartType = SeriesChartType.FastLine; //tipo de gráfico do path
            EMGBrutoSeries.Color = Color.Blue;
            EMGBrutoSeries.ChartArea = chartArea.Name; //vincular a serie path à chartArea
            chart.Series.Add(EMGBrutoSeries); //vincular a serie path ao chart       
            #endregion

            #region Hilbert
            HilbertSeries.ChartType = SeriesChartType.FastLine; //tipo de gráfico do path
            HilbertSeries.Color = Color.Green;
            HilbertSeries.ChartArea = chartArea.Name; //vincular a serie path à chartArea
            chart.Series.Add(HilbertSeries); //vincular a serie path ao chart  
            #endregion

            #region HilbertRetificado
            HilbertRetificadoSeries.ChartType = SeriesChartType.FastLine; //tipo de gráfico do path
            HilbertRetificadoSeries.Color = Color.DarkBlue;
            HilbertRetificadoSeries.ChartArea = chartArea.Name; //vincular a serie path à chartArea
            chart.Series.Add(HilbertRetificadoSeries); //vincular a serie path ao chart  
            #endregion

            #region Envoltoria
            EnvoltoriaSeries.ChartType = SeriesChartType.FastLine; //tipo de gráfico do path
            EnvoltoriaSeries.Color = Color.DarkRed;
            EnvoltoriaSeries.ChartArea = chartArea.Name; //vincular a serie path à chartArea
            chart.Series.Add(EnvoltoriaSeries); //vincular a serie path ao chart  
            #endregion

            #region Limiar
            LimiarSeries.ChartType = SeriesChartType.FastLine;
            LimiarSeries.Color = Color.Orange;
            LimiarSeries.ChartArea = this.chartArea.Name; //vincular a serie path à chartArea
            chart.Series.Add(LimiarSeries); //vincular a serie path ao chart  
            #endregion

            #region Areas de Contracao
            DetectionSitesSeries.ChartType = SeriesChartType.Area;
            DetectionSitesSeries.Color = Color.FromArgb(50, Color.Cyan);//Cor Transparente
            DetectionSitesSeries.ChartArea = this.chartArea.Name; //vincular a serie path à chartArea
            chart.Series.Add(DetectionSitesSeries); //vincular a serie path ao chart  
            #endregion

            #region Eixos
            chartArea.AxisX.Title = xtitle;
            chartArea.AxisY.Title = ytitle;
#           endregion
        }

        public void AddPointToBuffer(
            double time,
            double EMG_Bruto,
            double Hilbert,
            double Envoltoria,
            bool IsInDetection) //adicionar valor de envoltória no buffer
        {
            EMGPlotterBuffer.SecureOverflowEnqueue(new double[]
            {
                time, EMG_Bruto, Hilbert, Envoltoria, IsInDetection?y_max_value:y_min_value
            });
        }

        public void SetNewPointArrays(
            double[] time,
            double[] EMG_Bruto,
            double[] Hilbert,
            double[] HilbertRetificado,
            double[] Envoltoria,
            bool[] IsInDetection) //adicionar valor de envoltória no buffer
        {
            time_values = time;
            emg_bruto_values = EMG_Bruto;
            hilbert_values = Hilbert;
            hilbert_retificado_values = HilbertRetificado;
            envoltoria_values = Envoltoria;
            for (int i = 0; i < IsInDetection.Length; i++) {
                detection_sites_values[i] = IsInDetection[i] ? y_max_value : y_min_value;
            }
        }


        public override void UpdateXYChartPoints()
        {
            int points_to_plot = 1;// EMGPlotterBuffer.Count;
            if(points_to_plot > 0) {
                #region Leitura dos novos pontos
                //// Desloca todos os pontos atuais para a esquerda
                //for (int i = 0; i < (qnt_pontos - points_to_plot); i++) {
                //    time_values[i] = time_values[i + points_to_plot];
                //    emg_bruto_values[i] = emg_bruto_values[i + points_to_plot];
                //    hilbert_values[i] = hilbert_values[i + points_to_plot];
                //    hilbert_retificado_values[i] = hilbert_retificado_values[i + points_to_plot];
                //    envoltoria_values[i] = envoltoria_values[i + points_to_plot];
                //    detection_sites_values[i] = detection_sites_values[i + points_to_plot];
                //}

                ////Adiciona os novos pontos no novo espaço a direita
                //for (int i = (qnt_pontos - points_to_plot); i < qnt_pontos; i++){
                //    double[] values_in_waiting = EMGPlotterBuffer.SecureDequeue();

                //    time_values[i] = values_in_waiting[0];
                //    emg_bruto_values[i] = values_in_waiting[1];
                //    hilbert_values[i] = values_in_waiting[2];
                //    hilbert_retificado_values[i] = Math.Abs(values_in_waiting[2]);
                //    envoltoria_values[i] = values_in_waiting[3];
                //    detection_sites_values[i] = values_in_waiting[4];
                //}
                #endregion
                
                #region Bindings
                if (EMGBrutoVisible) {
                    EMGBrutoSeries.Points.DataBindXY(time_values, emg_bruto_values);
                }
                if (HilbertVisible) {
                    HilbertSeries.Points.DataBindXY(time_values, hilbert_values);
                }
                if (HilbertRetificadoVisible) {
                    HilbertRetificadoSeries.Points.DataBindXY(time_values, hilbert_retificado_values);
                }
                if (EnvoltoriaVisible) {
                    EnvoltoriaSeries.Points.DataBindXY(time_values, envoltoria_values);
                }
                if (LimiarVisible) {
                    LimiarSeries.Points.DataBindXY(time_values, limiar_values);
                }
                if (DetectionSitesVisible) {
                    DetectionSitesSeries.Points.DataBindXY(time_values, detection_sites_values);
                }
                #endregion

                #region Configurar mínimo e máximo dos eixos
                chartArea.AxisX.Minimum = Convert.ToDouble(Math.Floor(
                    time_values[0] * (freq_aquire / PlotterUpdater.Interval))
                    / (freq_aquire / PlotterUpdater.Interval));
                chartArea.AxisX.Maximum = Convert.ToDouble(Math.Ceiling(
                    time_values[qnt_pontos - 1] * (freq_aquire / PlotterUpdater.Interval))
                    / (freq_aquire / PlotterUpdater.Interval));
                chartArea.AxisY.Minimum = y_min_value;
                chartArea.AxisY.Maximum = y_max_value;
                #endregion
            } 
        }  
    }
}
