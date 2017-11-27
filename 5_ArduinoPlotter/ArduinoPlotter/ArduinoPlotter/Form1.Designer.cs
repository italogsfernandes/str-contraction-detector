namespace ArduinoPlotter
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.LabelStatusConexao = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configuraçõesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajustarEscalaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eixoXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txAxisXMin = new System.Windows.Forms.ToolStripTextBox();
            this.txAxisXMax = new System.Windows.Forms.ToolStripTextBox();
            this.eixoYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txAxisYMin = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.txAxisYMax = new System.Windows.Forms.ToolStripTextBox();
            this.autoSetEixoYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.limparBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pausePlotterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialbufferlabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelStatus,
            this.LabelStatusConexao,
            this.toolStripProgressBar1,
            this.serialbufferlabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 325);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(757, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(45, 17);
            this.labelStatus.Text = "Status: ";
            // 
            // LabelStatusConexao
            // 
            this.LabelStatusConexao.Name = "LabelStatusConexao";
            this.LabelStatusConexao.Size = new System.Drawing.Size(41, 17);
            this.LabelStatusConexao.Text = "COM4";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Click += new System.EventHandler(this.toolStripProgressBar1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(757, 301);
            this.panel1.TabIndex = 2;
            // 
            // chart1
            // 
            chartArea1.AxisX.Title = "Pontos";
            chartArea1.AxisY.Title = "Volts";
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(757, 301);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title1";
            title1.Text = "STR - Arduino Plotter";
            this.chart1.Titles.Add(title1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuraçõesToolStripMenuItem,
            this.ajudaToolStripMenuItem,
            this.sobreToolStripMenuItem,
            this.pausePlotterToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(757, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configuraçõesToolStripMenuItem
            // 
            this.configuraçõesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ajustarEscalaToolStripMenuItem,
            this.limparBufferToolStripMenuItem});
            this.configuraçõesToolStripMenuItem.Name = "configuraçõesToolStripMenuItem";
            this.configuraçõesToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.configuraçõesToolStripMenuItem.Text = "Configurações";
            this.configuraçõesToolStripMenuItem.Click += new System.EventHandler(this.configuraçõesToolStripMenuItem_Click);
            // 
            // ajustarEscalaToolStripMenuItem
            // 
            this.ajustarEscalaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eixoXToolStripMenuItem,
            this.eixoYToolStripMenuItem,
            this.autoSetEixoYToolStripMenuItem});
            this.ajustarEscalaToolStripMenuItem.Name = "ajustarEscalaToolStripMenuItem";
            this.ajustarEscalaToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.ajustarEscalaToolStripMenuItem.Text = "Ajustar Escala:";
            // 
            // eixoXToolStripMenuItem
            // 
            this.eixoXToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txAxisXMin,
            this.txAxisXMax});
            this.eixoXToolStripMenuItem.Name = "eixoXToolStripMenuItem";
            this.eixoXToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.eixoXToolStripMenuItem.Text = "Eixo X:";
            // 
            // txAxisXMin
            // 
            this.txAxisXMin.Name = "txAxisXMin";
            this.txAxisXMin.Size = new System.Drawing.Size(100, 23);
            this.txAxisXMin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txAxisXMin_KeyDown);
            // 
            // txAxisXMax
            // 
            this.txAxisXMax.Name = "txAxisXMax";
            this.txAxisXMax.Size = new System.Drawing.Size(100, 23);
            this.txAxisXMax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txAxisXMax_KeyDown);
            // 
            // eixoYToolStripMenuItem
            // 
            this.eixoYToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txAxisYMin,
            this.toolStripComboBox1,
            this.txAxisYMax});
            this.eixoYToolStripMenuItem.Name = "eixoYToolStripMenuItem";
            this.eixoYToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.eixoYToolStripMenuItem.Text = "Eixo Y:";
            // 
            // txAxisYMin
            // 
            this.txAxisYMin.Name = "txAxisYMin";
            this.txAxisYMin.Size = new System.Drawing.Size(100, 23);
            this.txAxisYMin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txAxisYMin_KeyDown);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "Com Auto-Ajuste",
            "Sem Auto-Ajuste"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // txAxisYMax
            // 
            this.txAxisYMax.Name = "txAxisYMax";
            this.txAxisYMax.Size = new System.Drawing.Size(100, 23);
            this.txAxisYMax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txAxisYMax_KeyDown);
            // 
            // autoSetEixoYToolStripMenuItem
            // 
            this.autoSetEixoYToolStripMenuItem.Name = "autoSetEixoYToolStripMenuItem";
            this.autoSetEixoYToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.autoSetEixoYToolStripMenuItem.Text = "Auto-Set Eixo Y";
            this.autoSetEixoYToolStripMenuItem.Click += new System.EventHandler(this.autoSetEixoYToolStripMenuItem_Click);
            // 
            // limparBufferToolStripMenuItem
            // 
            this.limparBufferToolStripMenuItem.Name = "limparBufferToolStripMenuItem";
            this.limparBufferToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.limparBufferToolStripMenuItem.Text = "Limpar Buffer";
            this.limparBufferToolStripMenuItem.Click += new System.EventHandler(this.limparBufferToolStripMenuItem_Click);
            // 
            // ajudaToolStripMenuItem
            // 
            this.ajudaToolStripMenuItem.Name = "ajudaToolStripMenuItem";
            this.ajudaToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.ajudaToolStripMenuItem.Text = "Ajuda";
            this.ajudaToolStripMenuItem.Click += new System.EventHandler(this.ajudaToolStripMenuItem_Click);
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.sobreToolStripMenuItem.Text = "Sobre";
            this.sobreToolStripMenuItem.Click += new System.EventHandler(this.sobreToolStripMenuItem_Click);
            // 
            // pausePlotterToolStripMenuItem
            // 
            this.pausePlotterToolStripMenuItem.Name = "pausePlotterToolStripMenuItem";
            this.pausePlotterToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.pausePlotterToolStripMenuItem.Text = "Pause Plotter";
            this.pausePlotterToolStripMenuItem.Click += new System.EventHandler(this.pausePlotterToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // serialbufferlabel
            // 
            this.serialbufferlabel.Name = "serialbufferlabel";
            this.serialbufferlabel.Size = new System.Drawing.Size(73, 17);
            this.serialbufferlabel.Text = "Serial Buffer:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 347);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Arduino Plotter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolStripStatusLabel LabelStatusConexao;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configuraçõesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ajudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ajustarEscalaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eixoXToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox txAxisXMin;
        private System.Windows.Forms.ToolStripTextBox txAxisXMax;
        private System.Windows.Forms.ToolStripMenuItem eixoYToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox txAxisYMin;
        private System.Windows.Forms.ToolStripTextBox txAxisYMax;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem limparBufferToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoSetEixoYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pausePlotterToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel serialbufferlabel;
    }
}

