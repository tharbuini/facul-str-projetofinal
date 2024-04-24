namespace STR_Identificador_Curto
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
            this.listViewDispositivos = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonParar = new System.Windows.Forms.Button();
            this.buttonIniciar = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxConexao = new System.Windows.Forms.ToolStripTextBox();
            this.timerPlotPacotesRecebidos = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(1011, 511);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Módulo 2: Identificador de Curto-Circuito IEC";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.formsPlotPacotesRecebidos);
            this.groupBox3.Location = new System.Drawing.Point(356, 21);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(640, 478);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Visualização";
            // 
            // formsPlotPacotesRecebidos
            // 
            this.formsPlotPacotesRecebidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlotPacotesRecebidos.Location = new System.Drawing.Point(3, 17);
            this.formsPlotPacotesRecebidos.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.formsPlotPacotesRecebidos.Name = "formsPlotPacotesRecebidos";
            this.formsPlotPacotesRecebidos.Size = new System.Drawing.Size(634, 459);
            this.formsPlotPacotesRecebidos.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listViewDispositivos);
            this.groupBox2.Controls.Add(this.buttonParar);
            this.groupBox2.Controls.Add(this.buttonIniciar);
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Location = new System.Drawing.Point(12, 21);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(339, 478);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controle";
            // 
            // listViewDispositivos
            // 
            this.listViewDispositivos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewDispositivos.HideSelection = false;
            this.listViewDispositivos.Location = new System.Drawing.Point(7, 22);
            this.listViewDispositivos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listViewDispositivos.Name = "listViewDispositivos";
            this.listViewDispositivos.Size = new System.Drawing.Size(324, 349);
            this.listViewDispositivos.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewDispositivos.TabIndex = 5;
            this.listViewDispositivos.UseCompatibleStateImageBehavior = false;
            this.listViewDispositivos.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Corrente (A)";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 80;
            // 
            // buttonParar
            // 
            this.buttonParar.Location = new System.Drawing.Point(220, 378);
            this.buttonParar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonParar.Name = "buttonParar";
            this.buttonParar.Size = new System.Drawing.Size(93, 27);
            this.buttonParar.TabIndex = 4;
            this.buttonParar.Text = "Parar";
            this.buttonParar.UseVisualStyleBackColor = true;
            this.buttonParar.Click += new System.EventHandler(this.buttonParar_Click);
            // 
            // buttonIniciar
            // 
            this.buttonIniciar.Location = new System.Drawing.Point(21, 378);
            this.buttonIniciar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonIniciar.Name = "buttonIniciar";
            this.buttonIniciar.Size = new System.Drawing.Size(93, 27);
            this.buttonIniciar.TabIndex = 3;
            this.buttonIniciar.Text = "Iniciar";
            this.buttonIniciar.UseVisualStyleBackColor = true;
            this.buttonIniciar.Click += new System.EventHandler(this.buttonIniciar_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripTextBoxConexao});
            this.toolStrip1.Location = new System.Drawing.Point(3, 445);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(333, 31);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(74, 24);
            this.toolStripLabel1.Text = "Conexão: ";
            // 
            // toolStripTextBoxConexao
            // 
            this.toolStripTextBoxConexao.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxConexao.Name = "toolStripTextBoxConexao";
            this.toolStripTextBoxConexao.ReadOnly = true;
            this.toolStripTextBoxConexao.Size = new System.Drawing.Size(132, 27);
            // 
            // timerPlotPacotesRecebidos
            // 
            this.timerPlotPacotesRecebidos.Interval = 500;
            this.timerPlotPacotesRecebidos.Tick += new System.EventHandler(this.timerPlotPacotesRecebidos_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 511);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Identificador de Curto";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private ScottPlot.FormsPlot formsPlotPacotesRecebidos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Button buttonParar;
        private System.Windows.Forms.Button buttonIniciar;
        private System.Windows.Forms.Timer timerPlotPacotesRecebidos;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxConexao;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ListView listViewDispositivos;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

