using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Accord.Math;
using System.Numerics; 


namespace DetectorContracao
{
    public class ArduinoEMGPlotter : ArduinoPlotter //herdar as coisas da classe ArduinoPlotter
    {

        /// <summary>
        /// Objeto do Tipo emgChart que será utilizado no lugar do ChartHandler.
        /// Contendo metodos para o tratamento dos sinais caracteriticos do EMG.]
        /// Como a envoltoria e a detecção de contração.
        /// </summary>
        public EMGChart emgChart;
        #region Calibração
        /// <summary>
        /// Flag indicando que esta ocorrendo a calibração.
        /// </summary>
        public bool is_in_calibration;
        /// <summary>
        /// Este Buffer se mantem vazio até a excução da primeira calibração,
        /// Após isso ele estará cheio contendo os dados usados para a calibração.
        /// </summary>
        public CircularBuffer<double> BufferCalibracao;

        /// <summary>
        /// Dados da calibração apos a transformada de hilbert.
        /// </summary>
        private double[] valorhbt;

        /// <summary>
        /// Array de dados salvos no buffer de calibração.
        /// </summary>
        private double[] calibrar;

        private double[] calibrar_filtrado;
        private double media;
        private double std;
        private double _k;
        #endregion

        public double k {
            get { return _k; }
            set { _k = value; emgChart.limiar = media + _k * std; }
        }

        Bunifu.Framework.UI.BunifuProgressBar calib_progress_bar;

        public ArduinoEMGPlotter(ref Chart chart, ref Label _lblBufferStatus, ref Bunifu.Framework.UI.BunifuProgressBar _calib_progress_bar)
        {
            _k = 2;
            is_in_calibration = false;
            calib_progress_bar = _calib_progress_bar;

            chartHandler = new EMGChart(ref chart, 4096); //chart com 5000 pontos
            emgChart = (EMGChart) chartHandler;
            BufferCalibracao = new CircularBuffer<double>(4096);
            valorhbt = new double[2048];

            chartHandler.ConfigureChart("Leituras", "EMG Plotter");

            arduinoHandler = new ArduinoHandler();
            dataconsumer = new ThreadHandler(() =>
            {

                if (arduinoHandler.dataWaiting)
                {
                    
                    double valor2add = arduinoHandler.bufferAquisition.
                        SecureDequeue() * 5 / 1024.0 - 2.5;

                    chartHandler.AddYToBuffer(valor2add);

                    #region  Janelamento da calibração
                    if (is_in_calibration)
                    {
                        BufferCalibracao.Enqueue(valor2add);
                        if (BufferCalibracao.Count % 50 == 0)
                        {
                            calib_progress_bar.BeginInvoke(new Action(() =>
                            {
                                calib_progress_bar.Value = (int)(100.0f * BufferCalibracao.Count / (double)BufferCalibracao.Capacity);
                            }));
                        }
                        if (BufferCalibracao.Full)
                        {
                            calibrar = BufferCalibracao.ToArray();
                            Accord.Math.HilbertTransform.FHT(calibrar, Accord.Math.FourierTransform.Direction.Forward); //aplicar a transformada de Hilbert
                            calibrar = calibrar.Abs();
                            calibrar_filtrado = Butter.Butterworth(calibrar, emgChart.period_aquire, 7);
                            media = calibrar.Average();
                            std = Accord.Statistics.Measures.StandardDeviation(calibrar);
                            emgChart.limiar = media + k * std;
                            is_in_calibration = false;
                        }
                    }
                    #endregion
                    //Esta rotina deve ser Otimizada para que os dados nao sejam processados num Timer
                    //Os dados devem ir para um buffer de processamento e não para o mesmo buffer de plotagem
                }
            });

            bufferLabelUpdater = new Timer();
            lblBufferStatus = _lblBufferStatus;
            bufferLabelUpdater.Interval = 50;
            bufferLabelUpdater.Tick += bufferLabelUpdater_Tick;
        }
    }
}
