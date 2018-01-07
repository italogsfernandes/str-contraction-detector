using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace DetectorContracao
{
    public class ArduinoEMGPlotter : ArduinoPlotter
    {
        public CircularBuffer<double> BufferMediaMovel;

        public ArduinoEMGPlotter(ref Chart chart, ref Label _lblBufferStatus)
        {
            chartHandler = new EMGChart(ref chart, 5000);
            BufferMediaMovel = new CircularBuffer<double>(100);//Media movel de 100 pontos = 10HZ +-
            for (int i = 0; i < BufferMediaMovel.Capacity; i++)
            {
                BufferMediaMovel.SecureEnqueue(0);
            }
            chartHandler.ConfigureChart("Leituras", "EMG Plotter");


            arduinoHandler = new ArduinoHandler();

            dataconsumer = new ThreadHandler(() =>
            {
                if (arduinoHandler.dataWaiting)
                {
                    double valormedia;
                    double valor2add = arduinoHandler.bufferAquisition.
                        SecureDequeue() * 5 / 1024.0 - 2.5;
                    chartHandler.AddYToBuffer(valor2add);

                    if (BufferMediaMovel.Full)
                    {
                        BufferMediaMovel.SecureDequeue();
                        valormedia = BufferMediaMovel.ToArray().Average();
                        BufferMediaMovel.SecureEnqueue(Math.Abs(valor2add) * 3.23);
                        var temp = (EMGChart)chartHandler;
                        temp.AddEnvoloriaToBuffer(valormedia);
                    }
                }
            });
            bufferLabelUpdater = new Timer();
            lblBufferStatus = _lblBufferStatus;
            bufferLabelUpdater.Interval = 50;
            bufferLabelUpdater.Tick += bufferLabelUpdater_Tick;
        }
    }
}
