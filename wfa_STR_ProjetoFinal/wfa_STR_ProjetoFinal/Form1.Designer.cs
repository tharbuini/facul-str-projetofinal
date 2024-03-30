namespace wfa_STR_ProjetoFinal
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.formsPlotPacotesRecebidos = new ScottPlot.FormsPlot();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonParar = new System.Windows.Forms.Button();
            this.buttonIniciar = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCorrenteAtual = new System.Windows.Forms.TextBox();
            this.timerControleCurto = new System.Windows.Forms.Timer(this.components);
            this.timerPlotSinaisRecebidos = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 511);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Módulo 2: Identificador de Curto-Circuito IEC";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.formsPlotPacotesRecebidos);
            this.groupBox3.Location = new System.Drawing.Point(295, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(493, 478);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Visualização";
            // 
            // formsPlotPacotesRecebidos
            // 
            this.formsPlotPacotesRecebidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlotPacotesRecebidos.Location = new System.Drawing.Point(3, 18);
            this.formsPlotPacotesRecebidos.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.formsPlotPacotesRecebidos.Name = "formsPlotPacotesRecebidos";
            this.formsPlotPacotesRecebidos.Size = new System.Drawing.Size(487, 457);
            this.formsPlotPacotesRecebidos.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonParar);
            this.groupBox2.Controls.Add(this.buttonIniciar);
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxCorrenteAtual);
            this.groupBox2.Location = new System.Drawing.Point(12, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(277, 160);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controle";
            // 
            // buttonParar
            // 
            this.buttonParar.Location = new System.Drawing.Point(152, 77);
            this.buttonParar.Name = "buttonParar";
            this.buttonParar.Size = new System.Drawing.Size(94, 27);
            this.buttonParar.TabIndex = 4;
            this.buttonParar.Text = "Parar";
            this.buttonParar.UseVisualStyleBackColor = true;
            // 
            // buttonIniciar
            // 
            this.buttonIniciar.Location = new System.Drawing.Point(32, 77);
            this.buttonIniciar.Name = "buttonIniciar";
            this.buttonIniciar.Size = new System.Drawing.Size(94, 27);
            this.buttonIniciar.TabIndex = 3;
            this.buttonIniciar.Text = "Iniciar";
            this.buttonIniciar.UseVisualStyleBackColor = true;
            this.buttonIniciar.Click += new System.EventHandler(this.buttonIniciar_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(3, 129);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(5);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(2);
            this.toolStrip1.Size = new System.Drawing.Size(271, 28);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Corrente Atual:";
            // 
            // textBoxCorrenteAtual
            // 
            this.textBoxCorrenteAtual.Location = new System.Drawing.Point(11, 43);
            this.textBoxCorrenteAtual.Name = "textBoxCorrenteAtual";
            this.textBoxCorrenteAtual.ReadOnly = true;
            this.textBoxCorrenteAtual.Size = new System.Drawing.Size(260, 22);
            this.textBoxCorrenteAtual.TabIndex = 0;
            // 
            // timerPlotSinaisRecebidos
            // 
            this.timerPlotSinaisRecebidos.Interval = 1000;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 511);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private ScottPlot.FormsPlot formsPlotPacotesRecebidos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer timerControleCurto;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCorrenteAtual;
        private System.Windows.Forms.Button buttonParar;
        private System.Windows.Forms.Button buttonIniciar;
        private System.Windows.Forms.Timer timerPlotSinaisRecebidos;
    }
}

