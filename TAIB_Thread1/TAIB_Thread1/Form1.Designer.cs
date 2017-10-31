namespace TAIB_Thread1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelValueGerado = new System.Windows.Forms.Label();
            this.labelTextGerado = new System.Windows.Forms.Label();
            this.btnStartGeradora = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chartConsumidora = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnStartConsumidora = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartConsumidora)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelValueGerado);
            this.panel1.Controls.Add(this.labelTextGerado);
            this.panel1.Controls.Add(this.btnStartGeradora);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(117, 161);
            this.panel1.TabIndex = 0;
            // 
            // labelValueGerado
            // 
            this.labelValueGerado.AutoSize = true;
            this.labelValueGerado.Location = new System.Drawing.Point(7, 63);
            this.labelValueGerado.Name = "labelValueGerado";
            this.labelValueGerado.Size = new System.Drawing.Size(10, 13);
            this.labelValueGerado.TabIndex = 3;
            this.labelValueGerado.Text = "-";
            // 
            // labelTextGerado
            // 
            this.labelTextGerado.AutoSize = true;
            this.labelTextGerado.Location = new System.Drawing.Point(7, 50);
            this.labelTextGerado.Name = "labelTextGerado";
            this.labelTextGerado.Size = new System.Drawing.Size(72, 13);
            this.labelTextGerado.TabIndex = 2;
            this.labelTextGerado.Text = "Valor Gerado:";
            // 
            // btnStartGeradora
            // 
            this.btnStartGeradora.Location = new System.Drawing.Point(7, 20);
            this.btnStartGeradora.Name = "btnStartGeradora";
            this.btnStartGeradora.Size = new System.Drawing.Size(75, 23);
            this.btnStartGeradora.TabIndex = 1;
            this.btnStartGeradora.Text = "Start";
            this.btnStartGeradora.UseVisualStyleBackColor = true;
            this.btnStartGeradora.Click += new System.EventHandler(this.btnStartGeradora_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Th Geradora";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.chartConsumidora);
            this.panel2.Controls.Add(this.btnStartConsumidora);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(136, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(783, 357);
            this.panel2.TabIndex = 1;
            // 
            // chartConsumidora
            // 
            chartArea5.Name = "ChartArea1";
            this.chartConsumidora.ChartAreas.Add(chartArea5);
            legend5.Enabled = false;
            legend5.Name = "Legend1";
            this.chartConsumidora.Legends.Add(legend5);
            this.chartConsumidora.Location = new System.Drawing.Point(94, 4);
            this.chartConsumidora.Name = "chartConsumidora";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chartConsumidora.Series.Add(series5);
            this.chartConsumidora.Size = new System.Drawing.Size(686, 350);
            this.chartConsumidora.TabIndex = 3;
            this.chartConsumidora.Text = "chart1";
            // 
            // btnStartConsumidora
            // 
            this.btnStartConsumidora.Location = new System.Drawing.Point(6, 20);
            this.btnStartConsumidora.Name = "btnStartConsumidora";
            this.btnStartConsumidora.Size = new System.Drawing.Size(75, 23);
            this.btnStartConsumidora.TabIndex = 2;
            this.btnStartConsumidora.Text = "Start";
            this.btnStartConsumidora.UseVisualStyleBackColor = true;
            this.btnStartConsumidora.Click += new System.EventHandler(this.btnStartConsumidora_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Th Plotter";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(13, 180);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(117, 190);
            this.panel3.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Valor Gerado:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Th Aquisition";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Valor Obtido:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(10, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "-";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 395);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartConsumidora)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStartGeradora;
        private System.Windows.Forms.Button btnStartConsumidora;
        private System.Windows.Forms.Label labelValueGerado;
        private System.Windows.Forms.Label labelTextGerado;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartConsumidora;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

