using Accord.Math;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DetectorContracao {
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


        int points_to_process;
        int qnt_pontos;


        double[] time_values;
        double[] emg_bruto_values;
        double[] hilbert_values;
        double[] hilbert_retificado_values;
        double[] envoltoria_values;
        bool[] contraction_sites;

        double aquire_interval;
        double limiar;

        public ArduinoEMGPlotter(ref Chart chart, ref Label _lblBufferStatus, ref Bunifu.Framework.UI.BunifuProgressBar _calib_progress_bar)
        {
            iniciarVariaveisProcessamento();

            _k = 2;
            is_in_calibration = false;
            calib_progress_bar = _calib_progress_bar;
            


            chartHandler = new EMGChart(ref chart, qnt_pontos); //chart com 5000 pontos
            emgChart = (EMGChart)chartHandler;


            BufferCalibracao = new CircularBuffer<double>(4096);
            valorhbt = new double[2048];

            chartHandler.ConfigureChart("Leituras", "EMG Plotter");

            bufferLabelUpdater = new Timer();
            lblBufferStatus = _lblBufferStatus;
            bufferLabelUpdater.Interval = 50;
            bufferLabelUpdater.Tick += bufferLabelUpdater_Tick;

            arduinoHandler = new ArduinoHandler();
            dataconsumer = new ThreadHandler(processingRoutine);
            #region Lambda from hell
            //() =>
            //{

            //    if (arduinoHandler.dataWaiting)
            //    {

            //        double valor2add = arduinoHandler.bufferAquisition.
            //            SecureDequeue() * 5 / 1024.0 - 2.5;

            //        chartHandler.AddYToBuffer(valor2add);

            //        #region  Janelamento da calibração
            //        if (is_in_calibration)
            //        {
            //            BufferCalibracao.Enqueue(valor2add);
            //            if (BufferCalibracao.Count % 50 == 0)
            //            {
            //                calib_progress_bar.BeginInvoke(new Action(() =>
            //                {
            //                    calib_progress_bar.Value = (int)(100.0f * BufferCalibracao.Count / (double)BufferCalibracao.Capacity);
            //                }));
            //            }
            //            if (BufferCalibracao.Full)
            //            {
            //                calibrar = BufferCalibracao.ToArray();
            //                Accord.Math.HilbertTransform.FHT(calibrar, Accord.Math.FourierTransform.Direction.Forward); //aplicar a transformada de Hilbert
            //                calibrar = calibrar.Abs();
            //                calibrar_filtrado = Butter.Butterworth(calibrar, emgChart.period_aquire, 7);
            //                media = calibrar.Average();
            //                std = Accord.Statistics.Measures.StandardDeviation(calibrar);
            //                emgChart.limiar = media + k * std;
            //                is_in_calibration = false;
            //            }
            //        }
            //        #endregion
            //        //Esta rotina deve ser Otimizada para que os dados nao sejam processados num Timer
            //        //Os dados devem ir para um buffer de processamento e não para o mesmo buffer de plotagem
            //    }
            //});
            #endregion
        }

        public void iniciarVariaveisProcessamento()
        {
            qnt_pontos = 4096;
            aquire_interval = 0.001;
            limiar = 0.5;

            time_values = new double[qnt_pontos];
            emg_bruto_values = new double[qnt_pontos];
            hilbert_values = new double[qnt_pontos];
            contraction_sites = new bool[qnt_pontos];
        }

        public void processingRoutine()
        {
            points_to_process = arduinoHandler.bufferAquisition.Count; //número de elementos no buffer
            if (points_to_process > 30) //Processando a 1 decimo da frequencia de aquisição = 100Hz
            {
                //Obtem vetor de tempo
                #region Time
                //Desloca os Pontos atuais para a esquerda
                for (int i = 0; i < (qnt_pontos - points_to_process); i++)
                {
                    time_values[i] = time_values[i + points_to_process];
                }
                for (int i = (qnt_pontos - points_to_process); i < qnt_pontos; i++)
                {
                    time_values[i] = time_values[i - 1] + aquire_interval;
                }
                #endregion

                //Descarrega dados do EMG bruto
                #region EMG_bruto
                //Desloca os Pontos atuais para a esquerda
                for (int i = 0; i < (qnt_pontos - points_to_process); i++)
                {
                    emg_bruto_values[i] = emg_bruto_values[i + points_to_process];
                }

                //Adiciona os novos pontos no novo espaço a direita
                for (int i = (qnt_pontos - points_to_process); i < qnt_pontos; i++)
                {
                    emg_bruto_values[i] = arduinoHandler.bufferAquisition.
                           SecureDequeue() * 5 / 1024.0 - 2.5;
                }
                #endregion

                //Aplica a transformada de Hilbert
                #region EMG filtrado por Hilbert
                hilbert_values = emg_bruto_values.Copy();
                Accord.Math.HilbertTransform.FHT(hilbert_values, Accord.Math.FourierTransform.Direction.Forward); //aplicar a transformada de Hilbert
                hilbert_retificado_values = hilbert_values.Abs();
                #endregion

                //Passa um filtro passa baixa de resposta butterworth em 7Hz
                #region Envoltoria através de um filtro passa baixa
                double[] primeira_metade = hilbert_retificado_values.Get(0, qnt_pontos / 2);
                double[] primeira_metade_invertida = primeira_metade.Reversed();
                double[] segunda_metade = hilbert_retificado_values.Get(qnt_pontos / 2, qnt_pontos);
                double[] segunda_metade_invertida = segunda_metade.Reversed();

                double[] janela_passa_baixa = primeira_metade_invertida.Concatenate(hilbert_retificado_values);
                janela_passa_baixa = janela_passa_baixa.Concatenate(segunda_metade_invertida);

                double[] grande_envoltoria = Butter.Butterworth(janela_passa_baixa, aquire_interval, 7);
                envoltoria_values = grande_envoltoria.Get(qnt_pontos/2, 3*qnt_pontos/2);
                #endregion

                //Verifica se o a envoltoria esta acima ou abaixo do limiar
                #region Detecção do Limiar
                double time_inicio = time_values[qnt_pontos - 1];
                double time_end = time_values[0];

                for (int i = 1; i < qnt_pontos; i++)
                {
                    //Borda de Subida
                    if (envoltoria_values[i] > limiar & envoltoria_values[i - 1] < limiar)
                    {
                        time_inicio = time_values[i]; //Armazena tempo de inicio da contração
                        time_end = time_values[qnt_pontos - 1]; //E reseta tempo de termino
                    }
                    //Borda de Descida
                    if (envoltoria_values[i] < limiar & envoltoria_values[i - 1] > limiar)
                    {
                        time_end = time_values[i];//Armazena tempo de termino da contração
                    }

                    if (time_values[i] > time_inicio)
                    {
                        contraction_sites[i] = true; //Se existe um tempo de inicio antes de mim eu estou em contração
                    }

                    if (time_values[i] > time_end)
                    {
                        contraction_sites[i] = false; //Se existe um tempo de termino antes de mim eu não estou em contração
                    }
                }
                #endregion

                //Envia novos dados para o chart:
                emgChart.SetNewPointArrays(time_values,
                    emg_bruto_values,
                    hilbert_values,
                    hilbert_retificado_values,
                    envoltoria_values,
                    contraction_sites);
            }

        }
    }
}
