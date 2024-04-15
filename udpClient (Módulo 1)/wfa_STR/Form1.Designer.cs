namespace wfa_STR
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonPararEnvio = new System.Windows.Forms.Button();
            this.propertyGridUnidGeradoras = new System.Windows.Forms.PropertyGrid();
            this.buttonIniciarEnvio = new System.Windows.Forms.Button();
            this.buttonLimpar = new System.Windows.Forms.Button();
            this.buttonAdicionar = new System.Windows.Forms.Button();
            this.numericUpDownValorCorrente = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.formsPlotPacotesEnviados = new ScottPlot.FormsPlot();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownFreqEnvio = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownCodUnidGen = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listViewUnidGeradora = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timerPlotSinaisEnviados = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValorCorrente)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFreqEnvio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCodUnidGen)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPararEnvio
            // 
            this.buttonPararEnvio.Enabled = false;
            this.buttonPararEnvio.Location = new System.Drawing.Point(392, 574);
            this.buttonPararEnvio.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPararEnvio.Name = "buttonPararEnvio";
            this.buttonPararEnvio.Size = new System.Drawing.Size(210, 29);
            this.buttonPararEnvio.TabIndex = 24;
            this.buttonPararEnvio.Text = "Parar";
            this.buttonPararEnvio.UseVisualStyleBackColor = true;
            this.buttonPararEnvio.Click += new System.EventHandler(this.buttonPararEnvio_Click);
            // 
            // propertyGridUnidGeradoras
            // 
            this.propertyGridUnidGeradoras.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGridUnidGeradoras.Enabled = false;
            this.propertyGridUnidGeradoras.Location = new System.Drawing.Point(2, 168);
            this.propertyGridUnidGeradoras.Margin = new System.Windows.Forms.Padding(2);
            this.propertyGridUnidGeradoras.Name = "propertyGridUnidGeradoras";
            this.propertyGridUnidGeradoras.Size = new System.Drawing.Size(256, 202);
            this.propertyGridUnidGeradoras.TabIndex = 48;
            // 
            // buttonIniciarEnvio
            // 
            this.buttonIniciarEnvio.Location = new System.Drawing.Point(23, 574);
            this.buttonIniciarEnvio.Margin = new System.Windows.Forms.Padding(2);
            this.buttonIniciarEnvio.Name = "buttonIniciarEnvio";
            this.buttonIniciarEnvio.Size = new System.Drawing.Size(211, 29);
            this.buttonIniciarEnvio.TabIndex = 23;
            this.buttonIniciarEnvio.Text = "&Iniciar";
            this.buttonIniciarEnvio.UseVisualStyleBackColor = true;
            this.buttonIniciarEnvio.Click += new System.EventHandler(this.buttonIniciarEnvio_Click);
            // 
            // buttonLimpar
            // 
            this.buttonLimpar.Location = new System.Drawing.Point(146, 137);
            this.buttonLimpar.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLimpar.Name = "buttonLimpar";
            this.buttonLimpar.Size = new System.Drawing.Size(106, 25);
            this.buttonLimpar.TabIndex = 58;
            this.buttonLimpar.Text = "Limpar";
            this.buttonLimpar.UseVisualStyleBackColor = true;
            this.buttonLimpar.Click += new System.EventHandler(this.buttonLimpar_Click);
            // 
            // buttonAdicionar
            // 
            this.buttonAdicionar.Location = new System.Drawing.Point(17, 113);
            this.buttonAdicionar.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAdicionar.Name = "buttonAdicionar";
            this.buttonAdicionar.Size = new System.Drawing.Size(235, 25);
            this.buttonAdicionar.TabIndex = 58;
            this.buttonAdicionar.Text = "Adicionar";
            this.buttonAdicionar.UseVisualStyleBackColor = true;
            this.buttonAdicionar.Click += new System.EventHandler(this.buttonAdicionar_Click);
            // 
            // numericUpDownValorCorrente
            // 
            this.numericUpDownValorCorrente.Location = new System.Drawing.Point(116, 67);
            this.numericUpDownValorCorrente.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownValorCorrente.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownValorCorrente.Name = "numericUpDownValorCorrente";
            this.numericUpDownValorCorrente.Size = new System.Drawing.Size(136, 20);
            this.numericUpDownValorCorrente.TabIndex = 47;
            this.numericUpDownValorCorrente.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.formsPlotPacotesEnviados);
            this.groupBox3.Location = new System.Drawing.Point(270, 30);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(350, 522);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Visualização";
            // 
            // formsPlotPacotesEnviados
            // 
            this.formsPlotPacotesEnviados.Location = new System.Drawing.Point(2, 15);
            this.formsPlotPacotesEnviados.Name = "formsPlotPacotesEnviados";
            this.formsPlotPacotesEnviados.Size = new System.Drawing.Size(346, 505);
            this.formsPlotPacotesEnviados.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 54;
            this.label2.Text = "Código unidade geradora:";
            // 
            // numericUpDownFreqEnvio
            // 
            this.numericUpDownFreqEnvio.Location = new System.Drawing.Point(100, 44);
            this.numericUpDownFreqEnvio.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownFreqEnvio.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownFreqEnvio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFreqEnvio.Name = "numericUpDownFreqEnvio";
            this.numericUpDownFreqEnvio.Size = new System.Drawing.Size(152, 20);
            this.numericUpDownFreqEnvio.TabIndex = 21;
            this.numericUpDownFreqEnvio.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 46);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Freq. envio (ms):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 69);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "Valor corrente RMS:";
            // 
            // numericUpDownCodUnidGen
            // 
            this.numericUpDownCodUnidGen.Location = new System.Drawing.Point(146, 21);
            this.numericUpDownCodUnidGen.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownCodUnidGen.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownCodUnidGen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCodUnidGen.Name = "numericUpDownCodUnidGen";
            this.numericUpDownCodUnidGen.Size = new System.Drawing.Size(106, 20);
            this.numericUpDownCodUnidGen.TabIndex = 53;
            this.numericUpDownCodUnidGen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.buttonAdicionar);
            this.groupBox2.Controls.Add(this.numericUpDownValorCorrente);
            this.groupBox2.Controls.Add(this.numericUpDownFreqEnvio);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numericUpDownCodUnidGen);
            this.groupBox2.Location = new System.Drawing.Point(4, 30);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(261, 147);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Adicionar nova unidade geradora";
            // 
            // listViewUnidGeradora
            // 
            this.listViewUnidGeradora.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listViewUnidGeradora.HideSelection = false;
            this.listViewUnidGeradora.Location = new System.Drawing.Point(4, 17);
            this.listViewUnidGeradora.Margin = new System.Windows.Forms.Padding(2);
            this.listViewUnidGeradora.Name = "listViewUnidGeradora";
            this.listViewUnidGeradora.Size = new System.Drawing.Size(248, 116);
            this.listViewUnidGeradora.TabIndex = 56;
            this.listViewUnidGeradora.UseCompatibleStateImageBehavior = false;
            this.listViewUnidGeradora.View = System.Windows.Forms.View.Details;
            this.listViewUnidGeradora.SelectedIndexChanged += new System.EventHandler(this.listViewUnidGeradora_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Cód. und. geradora";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Freq (ms)";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "I original";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "I curto";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox3);
            this.groupBox4.Controls.Add(this.groupBox2);
            this.groupBox4.Controls.Add(this.buttonIniciarEnvio);
            this.groupBox4.Controls.Add(this.buttonPararEnvio);
            this.groupBox4.Controls.Add(this.groupBox1);
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(648, 628);
            this.groupBox4.TabIndex = 48;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Módulo 1: Gera dados para subestação";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonLimpar);
            this.groupBox1.Controls.Add(this.propertyGridUnidGeradoras);
            this.groupBox1.Controls.Add(this.listViewUnidGeradora);
            this.groupBox1.Location = new System.Drawing.Point(4, 182);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(260, 372);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gerencia unidades geradoras";
            // 
            // timerPlotSinaisEnviados
            // 
            this.timerPlotSinaisEnviados.Interval = 500;
            this.timerPlotSinaisEnviados.Tick += new System.EventHandler(this.timerPlotSinaisEnviados_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 628);
            this.Controls.Add(this.groupBox4);
            this.Name = "Form1";
            this.Text = "Gerador Mockup";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownValorCorrente)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFreqEnvio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCodUnidGen)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPararEnvio;
        private System.Windows.Forms.PropertyGrid propertyGridUnidGeradoras;
        private System.Windows.Forms.Button buttonIniciarEnvio;
        private System.Windows.Forms.Button buttonLimpar;
        private System.Windows.Forms.Button buttonAdicionar;
        private System.Windows.Forms.NumericUpDown numericUpDownValorCorrente;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownFreqEnvio;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownCodUnidGen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listViewUnidGeradora;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private ScottPlot.FormsPlot formsPlotPacotesEnviados;
        private System.Windows.Forms.Timer timerPlotSinaisEnviados;
    }
}

