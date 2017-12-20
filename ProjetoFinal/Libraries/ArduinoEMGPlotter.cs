using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace DetectorContracao
{
    public class ArduinoEMGPlotter
    {
        public ArduinoHandler arduinoHandler;
        public ThreadHandler dataconsumer;
        public Timer bufferLabelUpdater;
        public EMGChart chartHandler;
        public Label lblBufferStatus;
        public CircularBuffer<double> BufferMediaMovel;
        
        public ArduinoEMGPlotter()
        {
        }

        public ArduinoEMGPlotter(ref Chart chart, ref Label _lblBufferStatus)
        {
            chartHandler = new EMGChart(ref chart, 5000);
            BufferMediaMovel = new CircularBuffer<double>(100);
            for (int i = 0; i < BufferMediaMovel.Capacity; i++)
            {
                BufferMediaMovel.SecureEnqueue(0);
            }
            chartHandler.ConfigureChart("Leituras", "EMG Plotter");


            arduinoHandler = new ArduinoHandler();

            dataconsumer = new ThreadHandler(() => {
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
                        BufferMediaMovel.SecureEnqueue(Math.Abs(valor2add)*3.23);
                        chartHandler.AddEnvoloriaToBuffer(valormedia);
                    }
                }
            });

            bufferLabelUpdater = new Timer();
            lblBufferStatus = _lblBufferStatus;
            bufferLabelUpdater.Interval = 50;
            bufferLabelUpdater.Tick += bufferLabelUpdater_Tick;

        }

        public void bufferLabelUpdater_Tick(object sender, EventArgs e)
        {
            try
            {
                lblBufferStatus.Text =
                    "Connected to: " + arduinoHandler.PortDescription +
                    "\t Buffers: " +
                    "\t Serial Port: " +
                    arduinoHandler.serialPort.BytesToRead.ToString("D4") + "/" + arduinoHandler.serialPort.ReadBufferSize.ToString() +
                    "\t Aquisition: " +
                    arduinoHandler.bufferAquisition.Count.ToString("D4") + "/" + arduinoHandler.bufferAquisition.Capacity.ToString() +
                    "\t Plotter: " +
                    chartHandler.PlotterBuffer.Count.ToString("D4") + "/" + chartHandler.PlotterBuffer.Capacity.ToString();
            }
            catch (Exception)
            {

            }
        }

        public virtual void Finish()
        {
            dataconsumer.Stop();
            arduinoHandler.StopAquisition();
            chartHandler.PlotterUpdater.Stop();
            bufferLabelUpdater.Stop();
        }

        public virtual void Start()
        {
            chartHandler.PlotterUpdater.Start();
            dataconsumer.Start();
            arduinoHandler.StartAquisition();
            bufferLabelUpdater.Start();
        }

    }
}
