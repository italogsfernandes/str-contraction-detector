using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace LibrariesExample
{
    public class ArduinoPlotter
    {
        public ArduinoHandler arduinoHandler;
        public ThreadHandler dataconsumer;
        public Timer bufferLabelUpdater;
        public ChartHandler chartHandler;
        public Label lblBufferStatus;

        public ArduinoPlotter(ref Chart chart, ref Label _lblBufferStatus)
        {
            chartHandler = new ChartHandler(ref chart,1000);

            chartHandler.ConfigureChart("Leituras", "Arduino Plotter", "Pontos", "Valores");
            
          
            arduinoHandler = new ArduinoHandler();

            dataconsumer = new ThreadHandler(() => {
                if (arduinoHandler.dataWaiting) {
                    chartHandler.AddYToBuffer(arduinoHandler.bufferAquisition.SecureDequeue()*5/1024.0);
                }
            });

            bufferLabelUpdater = new Timer();
            lblBufferStatus = _lblBufferStatus;
            bufferLabelUpdater.Interval = 50;
            bufferLabelUpdater.Tick += bufferLabelUpdater_Tick;

        }

        void bufferLabelUpdater_Tick(object sender, EventArgs e)
        {
            try
            {
                lblBufferStatus.Text =
                    "Connected to:\n" + arduinoHandler.PortDescription +
                    "\nBuffers: " +
                    "\nSerial Port: " +
                    arduinoHandler.serialPort.BytesToRead.ToString("D4") + "/" + arduinoHandler.serialPort.ReadBufferSize.ToString() +
                    "\nAquisition: " +
                    arduinoHandler.bufferAquisition.Count.ToString("D4") + "/" + arduinoHandler.bufferAquisition.Capacity.ToString() +
                    "\nPlotter: " +
                    chartHandler.PlotterBuffer.Count.ToString("D4") + "/" + chartHandler.PlotterBuffer.Capacity.ToString();
            }
            catch (Exception)
            {

            }
        }

        public void Finish()
        {
            dataconsumer.Stop();
            arduinoHandler.StopAquisition();
            chartHandler.PlotterUpdater.Stop();
            bufferLabelUpdater.Stop();
        }

        public void Start()
        {
            chartHandler.PlotterUpdater.Start();
            dataconsumer.Start();
            arduinoHandler.StartAquisition();
            bufferLabelUpdater.Start();
        }

    }
}
