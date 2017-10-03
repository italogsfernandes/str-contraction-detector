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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            ((System.ComponentModel.ISupportInitialize)(this.chartResultado)).BeginInit();
            this.groupBoxFormadeOnda.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartResultado
            // 
            chartArea3.Name = "ChartArea1";
            this.chartResultado.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartResultado.Legends.Add(legend3);
            this.chartResultado.Location = new System.Drawing.Point(208, 35);
            this.chartResultado.Name = "chartResultado";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartResultado.Series.Add(series3);
            this.chartResultado.Size = new System.Drawing.Size(614, 283);
            this.chartResultado.TabIndex = 1;
            this.chartResultado.Text = "Resultado:";
            // 
            // cbOnOff
            // 
            this.cbOnOff.AutoSize = true;
            this.cbOnOff.Location = new System.Drawing.Point(16, 88);
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
            this.groupBoxFormadeOnda.Location = new System.Drawing.Point(15, 177);
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
            this.label1.Location = new System.Drawing.Point(12, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Frequência:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Amplitude:";
            // 
            // tbFreq
            // 
            this.tbFreq.Location = new System.Drawing.Point(75, 111);
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
            this.tbAmplitude.Location = new System.Drawing.Point(75, 143);
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
            this.cbFreqScale.Location = new System.Drawing.Point(136, 111);
            this.cbFreqScale.Name = "cbFreqScale";
            this.cbFreqScale.Size = new System.Drawing.Size(48, 21);
            this.cbFreqScale.TabIndex = 12;
            this.cbFreqScale.SelectedIndexChanged += new System.EventHandler(this.cbFreqScale_SelectedIndexChanged);
            // 
            // timerUpdateChart
            // 
            this.timerUpdateChart.Interval = 10;
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 330);
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
    }
}

