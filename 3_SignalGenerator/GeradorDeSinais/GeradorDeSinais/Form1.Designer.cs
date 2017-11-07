namespace GeradorDeSinais
{
    partial class MainWindow
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartResultado = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbOnOff = new System.Windows.Forms.CheckBox();
            this.groupBoxFormadeOnda = new System.Windows.Forms.GroupBox();
            this.rbRampa = new System.Windows.Forms.RadioButton();
            this.rbQuadrada = new System.Windows.Forms.RadioButton();
            this.rbSenoidal = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFreq = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbAmplitude = new System.Windows.Forms.TextBox();
            this.cbFreqScale = new System.Windows.Forms.ComboBox();
            this.timerUpdateChart = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbSampleFreq = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbsamplePeriod = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbOndas = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.cbSomar = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartResultado)).BeginInit();
            this.groupBoxFormadeOnda.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartResultado
            // 
            chartArea1.Name = "ChartArea1";
            this.chartResultado.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartResultado.Legends.Add(legend1);
            this.chartResultado.Location = new System.Drawing.Point(208, 35);
            this.chartResultado.Name = "chartResultado";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartResultado.Series.Add(series1);
            this.chartResultado.Size = new System.Drawing.Size(614, 283);
            this.chartResultado.TabIndex = 1;
            this.chartResultado.Text = "Resultado:";
            // 
            // cbOnOff
            // 
            this.cbOnOff.AutoSize = true;
            this.cbOnOff.Location = new System.Drawing.Point(12, 55);
            this.cbOnOff.Name = "cbOnOff";
            this.cbOnOff.Size = new System.Drawing.Size(59, 17);
            this.cbOnOff.TabIndex = 5;
            this.cbOnOff.Text = "On/Off";
            this.cbOnOff.UseVisualStyleBackColor = true;
            this.cbOnOff.CheckedChanged += new System.EventHandler(this.cbOnOff_CheckedChanged);
            // 
            // groupBoxFormadeOnda
            // 
            this.groupBoxFormadeOnda.Controls.Add(this.rbRampa);
            this.groupBoxFormadeOnda.Controls.Add(this.rbQuadrada);
            this.groupBoxFormadeOnda.Controls.Add(this.rbSenoidal);
            this.groupBoxFormadeOnda.Location = new System.Drawing.Point(18, 217);
            this.groupBoxFormadeOnda.Name = "groupBoxFormadeOnda";
            this.groupBoxFormadeOnda.Size = new System.Drawing.Size(169, 87);
            this.groupBoxFormadeOnda.TabIndex = 6;
            this.groupBoxFormadeOnda.TabStop = false;
            this.groupBoxFormadeOnda.Text = "Forma de Onda:";
            // 
            // rbRampa
            // 
            this.rbRampa.AutoSize = true;
            this.rbRampa.Location = new System.Drawing.Point(7, 64);
            this.rbRampa.Name = "rbRampa";
            this.rbRampa.Size = new System.Drawing.Size(59, 17);
            this.rbRampa.TabIndex = 5;
            this.rbRampa.TabStop = true;
            this.rbRampa.Text = "Rampa";
            this.rbRampa.UseVisualStyleBackColor = true;
            this.rbRampa.CheckedChanged += new System.EventHandler(this.radiosFormasdeOnda_CheckedChanged);
            // 
            // rbQuadrada
            // 
            this.rbQuadrada.AutoSize = true;
            this.rbQuadrada.Location = new System.Drawing.Point(6, 43);
            this.rbQuadrada.Name = "rbQuadrada";
            this.rbQuadrada.Size = new System.Drawing.Size(72, 17);
            this.rbQuadrada.TabIndex = 4;
            this.rbQuadrada.TabStop = true;
            this.rbQuadrada.Text = "Quadrada";
            this.rbQuadrada.UseVisualStyleBackColor = true;
            this.rbQuadrada.CheckedChanged += new System.EventHandler(this.radiosFormasdeOnda_CheckedChanged);
            // 
            // rbSenoidal
            // 
            this.rbSenoidal.AutoSize = true;
            this.rbSenoidal.Location = new System.Drawing.Point(6, 19);
            this.rbSenoidal.Name = "rbSenoidal";
            this.rbSenoidal.Size = new System.Drawing.Size(66, 17);
            this.rbSenoidal.TabIndex = 3;
            this.rbSenoidal.TabStop = true;
            this.rbSenoidal.Text = "Senoidal";
            this.rbSenoidal.UseVisualStyleBackColor = true;
            this.rbSenoidal.CheckedChanged += new System.EventHandler(this.radiosFormasdeOnda_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Frequência:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Amplitude:";
            // 
            // tbFreq
            // 
            this.tbFreq.Location = new System.Drawing.Point(78, 157);
            this.tbFreq.Name = "tbFreq";
            this.tbFreq.Size = new System.Drawing.Size(54, 20);
            this.tbFreq.TabIndex = 9;
            this.tbFreq.Text = "1.0";
            this.tbFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbFreq.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFreq_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 29);
            this.label2.TabIndex = 10;
            this.label2.Text = "Opções:";
            // 
            // tbAmplitude
            // 
            this.tbAmplitude.Location = new System.Drawing.Point(78, 189);
            this.tbAmplitude.Name = "tbAmplitude";
            this.tbAmplitude.Size = new System.Drawing.Size(54, 20);
            this.tbAmplitude.TabIndex = 11;
            this.tbAmplitude.Text = "1.0";
            this.tbAmplitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbAmplitude.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbAmplitude_KeyDown);
            // 
            // cbFreqScale
            // 
            this.cbFreqScale.FormattingEnabled = true;
            this.cbFreqScale.Items.AddRange(new object[] {
            "Hz",
            "KHz",
            "MHz",
            "GHz"});
            this.cbFreqScale.Location = new System.Drawing.Point(139, 157);
            this.cbFreqScale.Name = "cbFreqScale";
            this.cbFreqScale.Size = new System.Drawing.Size(48, 21);
            this.cbFreqScale.TabIndex = 12;
            this.cbFreqScale.SelectedIndexChanged += new System.EventHandler(this.cbFreqScale_SelectedIndexChanged);
            // 
            // timerUpdateChart
            // 
            this.timerUpdateChart.Interval = 20;
            this.timerUpdateChart.Tick += new System.EventHandler(this.timerUpdateChart_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(203, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "Resultado:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(497, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Frequência Aquisição [Hz]:";
            // 
            // tbSampleFreq
            // 
            this.tbSampleFreq.Location = new System.Drawing.Point(687, 9);
            this.tbSampleFreq.Name = "tbSampleFreq";
            this.tbSampleFreq.Size = new System.Drawing.Size(39, 20);
            this.tbSampleFreq.TabIndex = 15;
            this.tbSampleFreq.Text = "50.0";
            this.tbSampleFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbSampleFreq.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSampleFreq_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(637, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Fs [Hz]: ";
            // 
            // tbsamplePeriod
            // 
            this.tbsamplePeriod.Location = new System.Drawing.Point(776, 9);
            this.tbsamplePeriod.Name = "tbsamplePeriod";
            this.tbsamplePeriod.Size = new System.Drawing.Size(39, 20);
            this.tbsamplePeriod.TabIndex = 17;
            this.tbsamplePeriod.Text = "20";
            this.tbsamplePeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbsamplePeriod.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbsamplePeriod_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(732, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Ts [ms]:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Ondas:";
            // 
            // cbOndas
            // 
            this.cbOndas.FormattingEnabled = true;
            this.cbOndas.Location = new System.Drawing.Point(62, 92);
            this.cbOndas.Name = "cbOndas";
            this.cbOndas.Size = new System.Drawing.Size(125, 21);
            this.cbOndas.TabIndex = 20;
            this.cbOndas.SelectedIndexChanged += new System.EventHandler(this.cbOndas_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(62, 118);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 23);
            this.btnAdd.TabIndex = 21;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(128, 118);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(59, 23);
            this.btnRemove.TabIndex = 22;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // cbSomar
            // 
            this.cbSomar.AutoSize = true;
            this.cbSomar.Location = new System.Drawing.Point(12, 122);
            this.cbSomar.Name = "cbSomar";
            this.cbSomar.Size = new System.Drawing.Size(47, 17);
            this.cbSomar.TabIndex = 23;
            this.cbSomar.Text = "Sum";
            this.cbSomar.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 330);
            this.Controls.Add(this.cbSomar);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cbOndas);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbsamplePeriod);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbSampleFreq);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbFreqScale);
            this.Controls.Add(this.tbAmplitude);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbFreq);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBoxFormadeOnda);
            this.Controls.Add(this.cbOnOff);
            this.Controls.Add(this.chartResultado);
            this.Name = "MainWindow";
            this.Text = "Gerador de Sinais - v0.0.1";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartResultado)).EndInit();
            this.groupBoxFormadeOnda.ResumeLayout(false);
            this.groupBoxFormadeOnda.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartResultado;
        private System.Windows.Forms.CheckBox cbOnOff;
        private System.Windows.Forms.GroupBox groupBoxFormadeOnda;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFreq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbAmplitude;
        private System.Windows.Forms.ComboBox cbFreqScale;
        private System.Windows.Forms.RadioButton rbRampa;
        private System.Windows.Forms.RadioButton rbQuadrada;
        private System.Windows.Forms.RadioButton rbSenoidal;
        private System.Windows.Forms.Timer timerUpdateChart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbSampleFreq;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbsamplePeriod;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbOndas;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.CheckBox cbSomar;
    }
}

