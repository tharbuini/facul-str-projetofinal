﻿using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot.Drawing.Colormaps;

namespace STR_Gerador
{
    public partial class Form1 : Form
    {
        Thread threadRecebimentosPacotes = null;
        Thread[] listaThreadsUnidadesGeradoras;
        UnidadeGeradoraDadosMedicao[] listaUnidadesGeradorasDadosMedicao;
        UdpClient udpClient = null;
        private List<int> dadosPlotarGrafico = new List<int>();
        public Mutex mutex = new Mutex();
        int quantidadeUnidGeradoras = 0;

        public Form1()
        {
            InitializeComponent();
            // Possibilitando que componentes sejam chamados por threads diferentes  
            Control.CheckForIllegalCrossThreadCalls = false;                                  
            
            formsPlotPacotesEnviados.Plot.Title("Dados de corrente referente à unidade geradora 1", true, Color.Black, 12.0f);
        }

        private void listViewUnidGeradora_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listaUnidadesGeradorasDadosMedicao == null)
                return;

            int indiceUnidGeradora = 0;
            if (listViewUnidGeradora.SelectedItems.Count > 0)
                indiceUnidGeradora = Convert.ToInt16(listViewUnidGeradora.SelectedItems[0].Text);
            else
                return;

            for (int i = 0; i < listaUnidadesGeradorasDadosMedicao.Length; i++)
            {
                if (listaUnidadesGeradorasDadosMedicao[i].codDestaUnidade == indiceUnidGeradora)
                {
                    propertyGridUnidGeradoras.SelectedObject = listaUnidadesGeradorasDadosMedicao[i];
                    return;
                }
            }
        }

        private void buttonIniciarEnvio_Click(object sender, EventArgs e)
        {
            buttonIniciarEnvio.Enabled = false;
            propertyGridUnidGeradoras.Enabled = true;
            buttonPararEnvio.Enabled = true;
            timerPlotSinaisEnviados.Start();
            dadosPlotarGrafico.Clear();

            // Iniciando conexão UDP no localhost
            udpClient = new UdpClient();
            udpClient.Connect("127.0.0.1", 11000);

            // Gerando unidades geradoras com limite de 5
            quantidadeUnidGeradoras = listViewUnidGeradora.Items.Count;
            listaUnidadesGeradorasDadosMedicao = new UnidadeGeradoraDadosMedicao[5];
            listaThreadsUnidadesGeradoras = new Thread[5];

            for (int i = 0; i < quantidadeUnidGeradoras; i++)
            {
                int codigo = Convert.ToInt16(listViewUnidGeradora.Items[i].SubItems[0].Text);
                int freqEnvio = Convert.ToInt16(listViewUnidGeradora.Items[i].SubItems[1].Text);
                int correnteOriginal = Convert.ToInt16(listViewUnidGeradora.Items[i].SubItems[2].Text);
                listaUnidadesGeradorasDadosMedicao[i] = new UnidadeGeradoraDadosMedicao(udpClient, mutex, correnteOriginal, codigo, freqEnvio);
            }

            // Gerando threads para as unidades
            for (int i = 0; i < quantidadeUnidGeradoras; i++)
            {
                listaThreadsUnidadesGeradoras[i] = new Thread(new ThreadStart(listaUnidadesGeradorasDadosMedicao[i].EnviaPacotesUDPFrequentemente));
                listaThreadsUnidadesGeradoras[i].Name = "UnidGeradora" + i.ToString();
                listaThreadsUnidadesGeradoras[i].Start();
            }
        }

        private void buttonPararEnvio_Click(object sender, EventArgs e)
        {
            propertyGridUnidGeradoras.Enabled = false;
            buttonPararEnvio.Enabled = false;
            timerPlotSinaisEnviados.Stop();
            dadosPlotarGrafico.Clear();
            formsPlotPacotesEnviados.Plot.Clear();
            buttonIniciarEnvio.Enabled = true;

            // Encerrando os objetos equivalentes a unidades geradoras            
            for (int i = 0; i < listaUnidadesGeradorasDadosMedicao.Length; i++)
            {
                listaUnidadesGeradorasDadosMedicao[i].pararEnvio = true;
            }
            listaUnidadesGeradorasDadosMedicao = null;

            // Enviando último pacote com corrente zerada após a finalização
            string formatoPacote = "{'Ia': " + '0' + " ,'Ib': " + '0' + " ,'Ic': " + '0' + " ,'idDispositivo': " + "-1" + "}";
            byte[] bytes = Encoding.ASCII.GetBytes(formatoPacote);
            mutex.WaitOne(); 
            if (udpClient != null)
                udpClient.Send(bytes, bytes.Length);
            mutex.ReleaseMutex();

            // Fechando a conexão UDP
            if (udpClient != null) 
            { 
                udpClient.Close();
            }
            if (threadRecebimentosPacotes != null)
            { 
                threadRecebimentosPacotes.Abort();
            }

        }

        private void buttonLimpar_Click(object sender, EventArgs e)
        {
            listViewUnidGeradora.Items.Clear();
            formsPlotPacotesEnviados.Plot.Clear();
            numericUpDownCodUnidGen.Value = 1;
        }

        private void buttonAdicionar_Click(object sender, EventArgs e)
        {
            if (numericUpDownCodUnidGen.Value > 5)
            {
                MessageBox.Show("Limite de dispositivos atingido!");
            }
            else 
            { 
                listViewUnidGeradora.Items.Add(new ListViewItem(new String[] { numericUpDownCodUnidGen.Value.ToString(), numericUpDownFreqEnvio.Value.ToString(), numericUpDownValorCorrente.Value.ToString() }));
                numericUpDownCodUnidGen.Value = numericUpDownCodUnidGen.Value + 1;

                // Adicionando unidade geradora
                if (buttonIniciarEnvio.Enabled == false)
                {
                    int codigo = Convert.ToInt16(listViewUnidGeradora.Items[quantidadeUnidGeradoras].SubItems[0].Text);
                    int freqEnvio = Convert.ToInt16(listViewUnidGeradora.Items[quantidadeUnidGeradoras].SubItems[1].Text);
                    int correnteOriginal = Convert.ToInt16(listViewUnidGeradora.Items[quantidadeUnidGeradoras].SubItems[2].Text);

                    listaUnidadesGeradorasDadosMedicao[quantidadeUnidGeradoras] = new UnidadeGeradoraDadosMedicao(udpClient, mutex, correnteOriginal, codigo, freqEnvio);

                    listaThreadsUnidadesGeradoras[quantidadeUnidGeradoras] = new Thread(new ThreadStart(listaUnidadesGeradorasDadosMedicao[quantidadeUnidGeradoras].EnviaPacotesUDPFrequentemente));
                    listaThreadsUnidadesGeradoras[quantidadeUnidGeradoras].Name = "UnidGeradora" + quantidadeUnidGeradoras.ToString();
                    listaThreadsUnidadesGeradoras[quantidadeUnidGeradoras].Start();

                    quantidadeUnidGeradoras++;
                }
            }
        }

        private void timerPlotSinaisEnviados_Tick(object sender, EventArgs e)
        {
            if (listaUnidadesGeradorasDadosMedicao.Length > 0)
            {
                if (dadosPlotarGrafico.Count > 300)
                {
                    dadosPlotarGrafico.Clear();
                }
                else
                {
                    dadosPlotarGrafico.Add(Convert.ToInt32(listaUnidadesGeradorasDadosMedicao[0].valorCorrente));
                }

                // Atualizando visualização do gráfico
                double[] ys = new double[dadosPlotarGrafico.Count];
                double[] xs = DataGen.Consecutive(dadosPlotarGrafico.Count);
                for (int i = 0; i < dadosPlotarGrafico.Count; i++)
                {
                    ys[i] = dadosPlotarGrafico[i];
                }
                formsPlotPacotesEnviados.Plot.Clear();
                if (dadosPlotarGrafico.Count > 1)
                {
                    formsPlotPacotesEnviados.Plot.AddScatterLines(xs, ys, Color.Blue, 2);
                    formsPlotPacotesEnviados.Refresh();
                }
            }
        }
    }

    public class JSON_DADOS_RECEBIDOS_CORRENTE
    {
        public int Ia { get; set; }
        public int Ib { get; set; }
        public int Ic { get; set; }
        public int numPacote { get; set; }
        public int idDispositivo { get; set; }
    }

    public class UnidadeGeradoraDadosMedicao
    {
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Valor de corrente emitido pelo pacote")]
        [DisplayName("Corrente normal")]
        public int valorCorrente { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Código desta unidade geradora de pacotes")]
        [DisplayName("COD")]
        public int codDestaUnidade { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Frequencia de envio de pacotes em ms")]
        [DisplayName("Freq. envio")]
        public int freqEnvioPacotesMS { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Quantidade de pacotes enviados")]
        [DisplayName("Pacotes enviados")]
        public int contadorPacotesEnviados { get; set; }

        [Browsable(false)]
        public bool pararEnvio { get; set; }

        public Mutex mutex;
        public UdpClient udpClient;
        public IPEndPoint ipConexaoEnvioUDP;

        public UnidadeGeradoraDadosMedicao(UdpClient p_udpClient, Mutex p_mutex, int p_valorCorrente, int p_codDestaUnidade, int p_freqEnvioPacotesMS)
        {
            mutex = p_mutex;
            valorCorrente = p_valorCorrente;
            codDestaUnidade = p_codDestaUnidade;
            freqEnvioPacotesMS = p_freqEnvioPacotesMS;
            udpClient = p_udpClient;
            contadorPacotesEnviados = 0;
            pararEnvio = false;
        }
        
        public void EnviaPacotesUDPFrequentemente() 
        {
            string formatoPacote;
            string corrente;
            byte[] bytes;
            int correntePrev = 0;

            // Caso a corrente não tenha alteração em relação à última, não envia pacotes
            while (true)
            {
                if (valorCorrente != correntePrev)
                {
                    if (pararEnvio)
                        return;
                        
                    corrente = Convert.ToString(valorCorrente);
                    correntePrev = valorCorrente;

                    if (contadorPacotesEnviados < 65000)
                        contadorPacotesEnviados++;
                    else
                        contadorPacotesEnviados = 0;
                    formatoPacote = "{'Ia': " + corrente +
                                    " ,'Ib': " + corrente +
                                    " ,'Ic': " + corrente +
                                    " ,'numPacote': " + contadorPacotesEnviados.ToString() +
                                    " ,'idDispositivo': " + codDestaUnidade.ToString() + "}";

                    bytes = Encoding.ASCII.GetBytes(formatoPacote);
                    mutex.WaitOne(); // Bloqueia esta região para uma simples thread acessar
                    if (udpClient != null)
                        udpClient.Send(bytes, bytes.Length);
                    mutex.ReleaseMutex(); // Desbloqueia esta região
                }
                else
                {
                    continue;
                }
            }
        }
    }
}


