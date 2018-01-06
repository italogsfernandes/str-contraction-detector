namespace LibrariesExample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.addPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startUpdaterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopUpdaterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lblBufferSerial = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPauseConsumer = new System.Windows.Forms.Button();
            this.btnResumeConsumer = new System.Windows.Forms.Button();
            this.lblBuffer = new System.Windows.Forms.Label();
            this.lblPortDescription = new System.Windows.Forms.Label();
            this.lblPortName = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblResultAcq = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPauseAcq = new System.Windows.Forms.Button();
            this.btnResumeAcq = new System.Windows.Forms.Button();
            this.btnStopAcq = new System.Windows.Forms.Button();
            this.btnStartAcq = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblResult = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lblEnqueue = new System.Windows.Forms.Label();
            this.txtDequeue = new System.Windows.Forms.TextBox();
            this.btnDequeue = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEnqueue = new System.Windows.Forms.TextBox();
            this.btnEnqueue = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(683, 361);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.Transparent;
            this.tabPage5.Controls.Add(this.chart2);
            this.tabPage5.Controls.Add(this.panel1);
            this.tabPage5.Controls.Add(this.statusStrip2);
            this.tabPage5.Controls.Add(this.menuStrip1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(675, 335);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "ArduinoPlotter";
            // 
            // chart2
            // 
            chartArea5.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea5);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.Name = "Legend1";
            this.chart2.Legends.Add(legend5);
            this.chart2.Location = new System.Drawing.Point(200, 24);
            this.chart2.Name = "chart2";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chart2.Series.Add(series5);
            this.chart2.Size = new System.Drawing.Size(475, 289);
            this.chart2.TabIndex = 3;
            this.chart2.Text = "chart2";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MintCream;
            this.panel1.Controls.Add(this.label9);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 289);
            this.panel1.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Status:";
            // 
            // statusStrip2
            // 
            this.statusStrip2.Location = new System.Drawing.Point(0, 313);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(675, 22);
            this.statusStrip2.TabIndex = 1;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(675, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.chart1);
            this.tabPage4.Controls.Add(this.menuStrip2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(675, 335);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "ChartHandler";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea6.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea6);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend6.Name = "Legend1";
            this.chart1.Legends.Add(legend6);
            this.chart1.Location = new System.Drawing.Point(0, 24);
            this.chart1.Name = "chart1";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.chart1.Series.Add(series6);
            this.chart1.Size = new System.Drawing.Size(675, 311);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPointToolStripMenuItem,
            this.addToBufferToolStripMenuItem,
            this.updateChartToolStripMenuItem,
            this.startUpdaterToolStripMenuItem,
            this.stopUpdaterToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(675, 24);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // addPointToolStripMenuItem
            // 
            this.addPointToolStripMenuItem.Name = "addPointToolStripMenuItem";
            this.addPointToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.addPointToolStripMenuItem.Text = "AddPoint";
            this.addPointToolStripMenuItem.Click += new System.EventHandler(this.addPointToolStripMenuItem_Click);
            // 
            // addToBufferToolStripMenuItem
            // 
            this.addToBufferToolStripMenuItem.Name = "addToBufferToolStripMenuItem";
            this.addToBufferToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.addToBufferToolStripMenuItem.Text = "AddToBuffer";
            this.addToBufferToolStripMenuItem.Click += new System.EventHandler(this.addToBufferToolStripMenuItem_Click);
            // 
            // updateChartToolStripMenuItem
            // 
            this.updateChartToolStripMenuItem.Name = "updateChartToolStripMenuItem";
            this.updateChartToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.updateChartToolStripMenuItem.Text = "UpdateChart";
            this.updateChartToolStripMenuItem.Click += new System.EventHandler(this.updateChartToolStripMenuItem_Click);
            // 
            // startUpdaterToolStripMenuItem
            // 
            this.startUpdaterToolStripMenuItem.Name = "startUpdaterToolStripMenuItem";
            this.startUpdaterToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.startUpdaterToolStripMenuItem.Text = "StartUpdater";
            this.startUpdaterToolStripMenuItem.Click += new System.EventHandler(this.startUpdaterToolStripMenuItem_Click);
            // 
            // stopUpdaterToolStripMenuItem
            // 
            this.stopUpdaterToolStripMenuItem.Name = "stopUpdaterToolStripMenuItem";
            this.stopUpdaterToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.stopUpdaterToolStripMenuItem.Text = "StopUpdater";
            this.stopUpdaterToolStripMenuItem.Click += new System.EventHandler(this.stopUpdaterToolStripMenuItem_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.richTextBox1);
            this.tabPage3.Controls.Add(this.lblBufferSerial);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.btnPauseConsumer);
            this.tabPage3.Controls.Add(this.btnResumeConsumer);
            this.tabPage3.Controls.Add(this.lblBuffer);
            this.tabPage3.Controls.Add(this.lblPortDescription);
            this.tabPage3.Controls.Add(this.lblPortName);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.lblResultAcq);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.btnPauseAcq);
            this.tabPage3.Controls.Add(this.btnResumeAcq);
            this.tabPage3.Controls.Add(this.btnStopAcq);
            this.tabPage3.Controls.Add(this.btnStartAcq);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(675, 335);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "ArduinoHandler";
            // 
            // lblBufferSerial
            // 
            this.lblBufferSerial.AutoSize = true;
            this.lblBufferSerial.Location = new System.Drawing.Point(162, 157);
            this.lblBufferSerial.Name = "lblBufferSerial";
            this.lblBufferSerial.Size = new System.Drawing.Size(10, 13);
            this.lblBufferSerial.TabIndex = 20;
            this.lblBufferSerial.Text = "-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(89, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Buffer Serial:";
            // 
            // btnPauseConsumer
            // 
            this.btnPauseConsumer.Location = new System.Drawing.Point(121, 205);
            this.btnPauseConsumer.Name = "btnPauseConsumer";
            this.btnPauseConsumer.Size = new System.Drawing.Size(75, 45);
            this.btnPauseConsumer.TabIndex = 18;
            this.btnPauseConsumer.Text = "Pause Consumer";
            this.btnPauseConsumer.UseVisualStyleBackColor = true;
            this.btnPauseConsumer.Click += new System.EventHandler(this.btnPauseConsumer_Click);
            // 
            // btnResumeConsumer
            // 
            this.btnResumeConsumer.Location = new System.Drawing.Point(202, 205);
            this.btnResumeConsumer.Name = "btnResumeConsumer";
            this.btnResumeConsumer.Size = new System.Drawing.Size(75, 45);
            this.btnResumeConsumer.TabIndex = 17;
            this.btnResumeConsumer.Text = "Resume Consumer";
            this.btnResumeConsumer.UseVisualStyleBackColor = true;
            this.btnResumeConsumer.Click += new System.EventHandler(this.btnResumeConsumer_Click);
            // 
            // lblBuffer
            // 
            this.lblBuffer.AutoSize = true;
            this.lblBuffer.Location = new System.Drawing.Point(162, 179);
            this.lblBuffer.Name = "lblBuffer";
            this.lblBuffer.Size = new System.Drawing.Size(10, 13);
            this.lblBuffer.TabIndex = 16;
            this.lblBuffer.Text = "-";
            // 
            // lblPortDescription
            // 
            this.lblPortDescription.AutoSize = true;
            this.lblPortDescription.Location = new System.Drawing.Point(162, 135);
            this.lblPortDescription.Name = "lblPortDescription";
            this.lblPortDescription.Size = new System.Drawing.Size(10, 13);
            this.lblPortDescription.TabIndex = 15;
            this.lblPortDescription.Text = "-";
            // 
            // lblPortName
            // 
            this.lblPortName.AutoSize = true;
            this.lblPortName.Location = new System.Drawing.Point(162, 113);
            this.lblPortName.Name = "lblPortName";
            this.lblPortName.Size = new System.Drawing.Size(10, 13);
            this.lblPortName.TabIndex = 14;
            this.lblPortName.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(80, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Buffer Circular:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(93, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Description:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(99, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "PortName:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Informações:";
            // 
            // lblResultAcq
            // 
            this.lblResultAcq.AutoSize = true;
            this.lblResultAcq.Location = new System.Drawing.Point(88, 83);
            this.lblResultAcq.Name = "lblResultAcq";
            this.lblResultAcq.Size = new System.Drawing.Size(10, 13);
            this.lblResultAcq.TabIndex = 9;
            this.lblResultAcq.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Resultado:";
            // 
            // btnPauseAcq
            // 
            this.btnPauseAcq.Location = new System.Drawing.Point(188, 33);
            this.btnPauseAcq.Name = "btnPauseAcq";
            this.btnPauseAcq.Size = new System.Drawing.Size(75, 23);
            this.btnPauseAcq.TabIndex = 7;
            this.btnPauseAcq.Text = "Pause";
            this.btnPauseAcq.UseVisualStyleBackColor = true;
            this.btnPauseAcq.Click += new System.EventHandler(this.btnPauseAcq_Click);
            // 
            // btnResumeAcq
            // 
            this.btnResumeAcq.Location = new System.Drawing.Point(269, 33);
            this.btnResumeAcq.Name = "btnResumeAcq";
            this.btnResumeAcq.Size = new System.Drawing.Size(75, 23);
            this.btnResumeAcq.TabIndex = 6;
            this.btnResumeAcq.Text = "Resume";
            this.btnResumeAcq.UseVisualStyleBackColor = true;
            this.btnResumeAcq.Click += new System.EventHandler(this.btnResumeAcq_Click);
            // 
            // btnStopAcq
            // 
            this.btnStopAcq.Location = new System.Drawing.Point(107, 34);
            this.btnStopAcq.Name = "btnStopAcq";
            this.btnStopAcq.Size = new System.Drawing.Size(75, 23);
            this.btnStopAcq.TabIndex = 5;
            this.btnStopAcq.Text = "Stop";
            this.btnStopAcq.UseVisualStyleBackColor = true;
            this.btnStopAcq.Click += new System.EventHandler(this.btnStopAcq_Click);
            // 
            // btnStartAcq
            // 
            this.btnStartAcq.Location = new System.Drawing.Point(26, 34);
            this.btnStartAcq.Name = "btnStartAcq";
            this.btnStartAcq.Size = new System.Drawing.Size(75, 23);
            this.btnStartAcq.TabIndex = 4;
            this.btnStartAcq.Text = "Start";
            this.btnStartAcq.UseVisualStyleBackColor = true;
            this.btnStartAcq.Click += new System.EventHandler(this.btnStartAcq_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.richTextBox2);
            this.tabPage2.Controls.Add(this.lblResult);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.btnPause);
            this.tabPage2.Controls.Add(this.btnResume);
            this.tabPage2.Controls.Add(this.btnStop);
            this.tabPage2.Controls.Add(this.btnStart);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(675, 335);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ThreadHandler";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(94, 76);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(10, 13);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Resultado:";
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(194, 26);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnResume
            // 
            this.btnResume.Location = new System.Drawing.Point(275, 26);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(75, 23);
            this.btnResume.TabIndex = 2;
            this.btnResume.Text = "Resume";
            this.btnResume.UseVisualStyleBackColor = true;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(113, 27);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(32, 27);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.richTextBox3);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Controls.Add(this.lblEnqueue);
            this.tabPage1.Controls.Add(this.txtDequeue);
            this.tabPage1.Controls.Add(this.btnDequeue);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtEnqueue);
            this.tabPage1.Controls.Add(this.btnEnqueue);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(675, 335);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "CircularBuffer";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(333, 39);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(324, 52);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // lblEnqueue
            // 
            this.lblEnqueue.AutoSize = true;
            this.lblEnqueue.Location = new System.Drawing.Point(19, 42);
            this.lblEnqueue.Name = "lblEnqueue";
            this.lblEnqueue.Size = new System.Drawing.Size(35, 13);
            this.lblEnqueue.TabIndex = 5;
            this.lblEnqueue.Text = "Insira:";
            // 
            // txtDequeue
            // 
            this.txtDequeue.Enabled = false;
            this.txtDequeue.Location = new System.Drawing.Point(82, 71);
            this.txtDequeue.Name = "txtDequeue";
            this.txtDequeue.Size = new System.Drawing.Size(128, 20);
            this.txtDequeue.TabIndex = 4;
            // 
            // btnDequeue
            // 
            this.btnDequeue.Location = new System.Drawing.Point(216, 68);
            this.btnDequeue.Name = "btnDequeue";
            this.btnDequeue.Size = new System.Drawing.Size(75, 23);
            this.btnDequeue.TabIndex = 3;
            this.btnDequeue.Text = "Dequeue";
            this.btnDequeue.UseVisualStyleBackColor = true;
            this.btnDequeue.Click += new System.EventHandler(this.btnDequeue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Valor Lido:";
            // 
            // txtEnqueue
            // 
            this.txtEnqueue.Location = new System.Drawing.Point(82, 39);
            this.txtEnqueue.Name = "txtEnqueue";
            this.txtEnqueue.Size = new System.Drawing.Size(128, 20);
            this.txtEnqueue.TabIndex = 1;
            // 
            // btnEnqueue
            // 
            this.btnEnqueue.Location = new System.Drawing.Point(216, 39);
            this.btnEnqueue.Name = "btnEnqueue";
            this.btnEnqueue.Size = new System.Drawing.Size(75, 23);
            this.btnEnqueue.TabIndex = 0;
            this.btnEnqueue.Text = "Enqueue";
            this.btnEnqueue.UseVisualStyleBackColor = true;
            this.btnEnqueue.Click += new System.EventHandler(this.btnEnqueue_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(359, 33);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(70, 250);
            this.richTextBox1.TabIndex = 21;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(32, 108);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(318, 199);
            this.richTextBox2.TabIndex = 6;
            this.richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(22, 110);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(269, 207);
            this.richTextBox3.TabIndex = 7;
            this.richTextBox3.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(333, 110);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 78);
            this.label10.TabIndex = 8;
            this.label10.Text = "Exemplo da Classe CircularBuffer\r\nBuffer Capacity: -\r\nBuffer Count: -\r\nBuffer Arr" +
    "ay: -\r\nBuffer Head: -\r\nBuffer Tail: -";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(372, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(170, 39);
            this.label11.TabIndex = 7;
            this.label11.Text = "Exemplo da Classe ThreadHandler\r\nA Thread conta ate 100 \r\nEm intervalos de 500ms";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(431, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(244, 52);
            this.label12.TabIndex = 22;
            this.label12.Text = "Exemplo da Calsse ArduinoHandler\r\n\r\nLê valores da Porta Serial em formatos de pac" +
    "otes\r\nO sketch para arduino é o SignalGenerator\r\n";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 361);
            this.Controls.Add(this.tabControl1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnEnqueue;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnDequeue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEnqueue;
        private System.Windows.Forms.Label lblEnqueue;
        private System.Windows.Forms.TextBox txtDequeue;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnPauseAcq;
        private System.Windows.Forms.Button btnResumeAcq;
        private System.Windows.Forms.Button btnStopAcq;
        private System.Windows.Forms.Button btnStartAcq;
        private System.Windows.Forms.Label lblResultAcq;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblBuffer;
        private System.Windows.Forms.Label lblPortDescription;
        private System.Windows.Forms.Label lblPortName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnPauseConsumer;
        private System.Windows.Forms.Button btnResumeConsumer;
        private System.Windows.Forms.Label lblBufferSerial;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem addPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToBufferToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateChartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startUpdaterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopUpdaterToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
    }
}

