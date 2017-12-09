using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibrariesExample
{
    public partial class Form1 : Form
    {

        #region Circular Buffer
        CircularBuffer<string> mycircularbuffer;
        #endregion
        #region ThreadHandler
        ThreadHandler mythread;
        int threadcounter;
        #endregion
        #region ArduinoHandler
        ArduinoHandler myarduinohandler;
        ThreadHandler aduinodataconsumer;
        Timer arduinoBufferLabel;
        #endregion
        #region ChartOptimized
        ChartHandler mychart;
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Circular Buffer
            mycircularbuffer = new CircularBuffer<string>(4);
            btnDequeue.Enabled = false;
            btnEnqueue.Enabled = true;
            #endregion
            #region ThreadHandler
            mythread = new ThreadHandler(threadFuction);
            threadcounter = 0;
            #endregion
            #region ArduinoHandler
            myarduinohandler = new ArduinoHandler();
            aduinodataconsumer = new ThreadHandler(ArduinoConsumer);
            arduinoBufferLabel = new Timer();
            arduinoBufferLabel.Interval = 50;
            arduinoBufferLabel.Tick += updateLabelBuffer;
            #endregion
            #region ChartHandler
            mychart = new ChartHandler(ref chart1);
            #endregion
        }

        #region Circular Buffer
        private void updateListView()
        {
            listView1.Clear();
            foreach (string item in mycircularbuffer.ToArray())
            {
                listView1.Items.Add(item);
            }
        }

        private void btnEnqueue_Click(object sender, EventArgs e)
        {
            mycircularbuffer.Enqueue(txtEnqueue.Text);
            updateListView();

            if (mycircularbuffer.Count == 4)
            {
                btnEnqueue.Enabled = false;
            }
            btnDequeue.Enabled = true;
        }

        private void btnDequeue_Click(object sender, EventArgs e)
        {
            txtDequeue.Text = mycircularbuffer.Dequeue();
            updateListView();
            if(mycircularbuffer.Count == 0)
            {
                btnDequeue.Enabled = false;
            }
            btnEnqueue.Enabled = true;
        }
        #endregion
        #region ThreadHandler
        public void threadFuction()
        {
            threadcounter++;
            threadcounter = threadcounter % 101;
            lblResult.Invoke(new Action(() =>
            {
                lblResult.Text = threadcounter.ToString();
            }));
            System.Threading.Thread.Sleep(500);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            mythread.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mythread.Stop();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            mythread.Pause();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            mythread.Resume();
        }
        #endregion
        #region ArduinoHandler
        void ArduinoConsumer()
        {
            if(myarduinohandler.dataWaiting)
            {
                lblResultAcq.Invoke(new Action(()=>
                {
                    lblResultAcq.Text = myarduinohandler.bufferAquisition.SecureDequeue().ToString();
                }));
            }
        }

        void updateLabelBuffer(object sender, EventArgs e)
        {
            try
            {
                lblBuffer.Text = myarduinohandler.bufferAquisition.Count.ToString() + "/" + myarduinohandler.bufferAquisition.Capacity.ToString();
                lblBufferSerial.Text = myarduinohandler.serialPort.BytesToRead.ToString() + "/" + myarduinohandler.serialPort.ReadBufferSize.ToString();
            }
            catch (Exception)
            {

            }  
        }

        private void btnStartAcq_Click(object sender, EventArgs e)
        {
            aduinodataconsumer.Start();
            arduinoBufferLabel.Start();
            myarduinohandler.StartAquisition();
            lblPortName.Text = myarduinohandler.serialPort.PortName;
            lblPortDescription.Text = myarduinohandler.PortDescription;
        }

        private void btnStopAcq_Click(object sender, EventArgs e)
        {
            aduinodataconsumer.Stop();
            myarduinohandler.StopAquisition();
        }

        private void btnPauseAcq_Click(object sender, EventArgs e)
        {
            myarduinohandler.threadAquisition.Pause();
        }

        private void btnResumeAcq_Click(object sender, EventArgs e)
        {
            myarduinohandler.threadAquisition.Resume();
        }

        private void btnPauseConsumer_Click(object sender, EventArgs e)
        {
            aduinodataconsumer.Pause();
        }

        private void btnResumeConsumer_Click(object sender, EventArgs e)
        {
            aduinodataconsumer.Resume();
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region ThreadHandler
            mythread.Stop();
            #endregion
            #region ArduinoHandler
            aduinodataconsumer.Stop();
            myarduinohandler.StopAquisition();
            arduinoBufferLabel.Stop();
            #endregion
        }

        #region ChartOptimized

        #endregion

        private void addPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mychart.series.Points.AddY(Math.Sin(DateTime.Now.Second+DateTime.Now.Millisecond/1000.0));
        }

        private void addToBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mychart.AddYToBuffer(Math.Sin(DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0));
        }

        private void updateChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mychart.UpdateYChartPoints();
        }

        private void startUpdaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mychart.PlotterUpdater.Start();
        }

        private void stopUpdaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mychart.PlotterUpdater.Stop();
        }
    }
}
