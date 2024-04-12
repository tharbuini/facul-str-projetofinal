using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using ScottPlot;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace wfa_STR_ProjetoFinal
{
    public partial class Form1 : Form
    {
        Thread threadRecebimentosPacotes = null;
        Thread threadDispositivo = null;
        //Thread[] listaThreadsDispositivos;
        //UnidadeMonitoramentoDados[] listaUnidadeMonitoramento;
        JSON_Dados_Corrente dadosRecebidosJSON;
        UdpClient udpServer = null;
        IPEndPoint remoteEP = null;
        string mensagemRecebida;
        byte[] bytesRecebidos;
        int contadorRecebimentoPacote = 0;
        private List<int> dadosPlotarGrafico = new List<int>();
        Boolean pararRecebimentoDados = false;
        private Stopwatch marcadorTempoCurto;
        
        double correnteMedia = 0;
        double correnteNominal = 600;
        double dial = 0.25;
        double correnteCCMax = 10000;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false; 
            InitializeComponent();

            formsPlotPacotesRecebidos.Plot.Title("Taxa de Pacotes Recebidos", true, Color.Black, 12.0f);
        }

        private void buttonIniciar_Click(object sender, EventArgs e)
        {
            buttonIniciar.Enabled = false;
            buttonParar.Enabled = true;
            timerPlotSinaisRecebidos.Start();
            contadorRecebimentoPacote = 0;
            dadosPlotarGrafico.Clear();
            pararRecebimentoDados = false;

            udpServer = new UdpClient(11000);
            remoteEP = new IPEndPoint(IPAddress.Any, 11000);

            threadRecebimentosPacotes = new Thread(RecebimentoPacotesUDP);
            threadRecebimentosPacotes.Start();

            // gera threads
            //for (int i = 0; i < quantidadeUnidadesGeradoras; i++)
            //{
            //    listaThreadsDispositivos[i] = new Thread(new ThreadStart(listaUnidadeMonitoramento[i].AnalisaDados));
            //    listaThreadsDispositivos[i].Name = "Dispositivo de Monitoramento" + i.ToString();
            //    listaThreadsDispositivos[i].Start();
            //}
        }

        private void buttonParar_Click(object sender, EventArgs e)
        {
            buttonParar.Enabled = false;
            timerPlotSinaisRecebidos.Stop();
            contadorRecebimentoPacote = 0;
            dadosPlotarGrafico.Clear();
            buttonIniciar.Enabled = true;
            pararRecebimentoDados = true;
            textBoxCorrenteAtual.Text = "";
            textBoxTimerControleCurto.Text = "";
           
            //for (int i = 0; i < listaUnidadesGeradorasDadosMedicao.Length; i++)
            //{
            //    listaUnidadesGeradorasDadosMedicao[i].pararEnvio = true; // vai forçar as threads pararem
            //}
            //Thread.Sleep(500);
            //listaUnidadesGeradorasDadosMedicao = null;

            udpServer.Close();
            threadRecebimentosPacotes.Abort();
            threadDispositivo.Abort();
        }

        Boolean threadAberta = false;
        double correnteSendoAnalisada = 0;
        private void RecebimentoPacotesUDP()
        {
            while (true)
            {
                if (pararRecebimentoDados)
                    return;

                bytesRecebidos = udpServer.Receive(ref remoteEP);
                mensagemRecebida = Encoding.ASCII.GetString(bytesRecebidos);
                dadosRecebidosJSON = JsonConvert.DeserializeObject<JSON_Dados_Corrente>(mensagemRecebida);

                correnteMedia = (dadosRecebidosJSON.Ia + dadosRecebidosJSON.Ib + dadosRecebidosJSON.Ic) / 3;

                textBoxCorrenteAtual.Text = correnteMedia.ToString() + " A";
                toolStripTextBoxConexao.Text = "UDP (" + remoteEP.Address.ToString() + ")";
                contadorRecebimentoPacote++;

                if (correnteMedia > correnteCCMax)
                {
                    break;
                }

                if (correnteMedia < correnteSendoAnalisada && threadAberta) 
                {
                    threadDispositivo.Abort();
                }

                if (((correnteMedia >= correnteNominal) && !threadAberta) || ((correnteMedia > correnteSendoAnalisada) && threadAberta))
                {
                    threadDispositivo = new Thread(() => AnalisaDados(correnteMedia));
                    threadDispositivo.Start();
                    threadAberta = true;
                    correnteSendoAnalisada = correnteMedia;
                }
            }
        }

        Boolean timerComecou = false;
        double tempoAtuacao = 0;
        private void AnalisaDados(double corrente)
        {
            if (!timerComecou) 
            { 
                marcadorTempoCurto = new Stopwatch();
                marcadorTempoCurto.Start(); // inicia contagem de tempo
                timerControleCurto.Start();
                timerComecou = true;
            }

            // seguindo a curva muito-inversa e os valores adotados no vídeo de exemplo
            tempoAtuacao = dial * (13.5 / ((correnteCCMax / corrente) - 1));
            textBoxTempoEspera.Text = tempoAtuacao.ToString();
        }

        private void timerControleCurto_Tick(object sender, EventArgs e)
        {
            double tempoDecorrido = marcadorTempoCurto.Elapsed.TotalMilliseconds;
            textBoxTimerControleCurto.Text = tempoDecorrido.ToString() + " ms";

            if (tempoDecorrido < tempoAtuacao)
            {
                // espera
            }
            else if (tempoDecorrido >= tempoAtuacao)
            {
                marcadorTempoCurto.Stop();
                timerControleCurto.Stop();
                timerComecou = false;
            }
        }

        private void timerPlotSinaisRecebidos_Tick(object sender, EventArgs e)
        {
            if (dadosPlotarGrafico.Count > 300) // se tiver muitas amostras, zera
            {
                dadosPlotarGrafico.Clear();
            }
            else
            {
                dadosPlotarGrafico.Add(Convert.ToInt32(correnteMedia));
            }
            contadorRecebimentoPacote = 0; // zera contagem

            textBoxTempoEspera.Text = contadorRecebimentoPacote.ToString();

            // atualiza a visualização do gráfico
            double[] ys = new double[dadosPlotarGrafico.Count];
            double[] xs = DataGen.Consecutive(dadosPlotarGrafico.Count);
            for (int i = 0; i < dadosPlotarGrafico.Count; i++)
            {
                ys[i] = (double)dadosPlotarGrafico[i];
            }
            formsPlotPacotesRecebidos.Plot.Clear();
            if (dadosPlotarGrafico.Count > 1)
            {
                formsPlotPacotesRecebidos.Plot.AddScatterLines(xs, ys, Color.Blue, 2);
                formsPlotPacotesRecebidos.Refresh();
            }
        }

        //public void Alarme()
        //{

        //}

    } // -------- FIM CLASSE ---------


    public class JSON_Dados_Corrente
    {
        public int Ia { get; set; }
        public int Ib { get; set; }
        public int Ic { get; set; }
        public int numPacote { get; set; }
        public int idDispositivo { get; set; }
    }

    public class UnidadeMonitoramentoDados
    {
        public void AnalisaDados() // vai ser usado como uma thread
        {
        }
    }
}
