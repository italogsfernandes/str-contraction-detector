using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace DetectorContracao
{
    public class ArduinoPlotter
    {
        public ArduinoHandler arduinoHandler;
        public ThreadHandler dataconsumer;
        public Timer bufferLabelUpdater;
        public ChartHandler chartHandler;
        public Label lblBufferStatus;

        public ArduinoPlotter() //construtor 
        {
        }

        public ArduinoPlotter(ref Chart chart, ref Label _lblBufferStatus)
        {
            chartHandler = new ChartHandler(ref chart,5000); //para lidar com o chart

            chartHandler.ConfigureChart("Leituras", "Arduino Plotter", "Pontos", "Valores"); //configurar o chart
            
          
            arduinoHandler = new ArduinoHandler(); //para lidar com a aquisição de dados a partir do arduino

            //thread para consumir os dados 
            dataconsumer = new ThreadHandler(() => {
                if (arduinoHandler.dataWaiting) {
                    chartHandler.AddYToBuffer(arduinoHandler.bufferAquisition.SecureDequeue()*5/1024.0);
                }
            });

            bufferLabelUpdater = new Timer(); //timer para atualizar a label
            lblBufferStatus = _lblBufferStatus;
            bufferLabelUpdater.Interval = 50; //intervalo de tempo para timer da label
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

        public virtual void Finish() //parar a execução
        {
            dataconsumer.Stop(); //parar a thread que consome os dados - timer
            arduinoHandler.StopAquisition(); //parar aquisisão de dados
            chartHandler.PlotterUpdater.Stop(); //parar de atualizar o chart
            bufferLabelUpdater.Stop(); //parar de atualizar a label - timer 
        }

        public virtual void Start() //iniciar a execução
        {
            chartHandler.PlotterUpdater.Start(); //iniciar a atualização do chart
            dataconsumer.Start(); //iniciar a thread consumidora
            arduinoHandler.StartAquisition(); //começar a aquisição de dados
            bufferLabelUpdater.Start(); //iniciar atualizaçãod da label
        }

    }
}
