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
        Thread threadDispositivo = null; // temporário
        Thread[] listaThreadsDispositivos; // lista de dispositivos para monitoramento simultâneo
        UnidadeMonitoramentoDados[] listaUnidadeMonitoramento;
        JSON_Dados_Corrente dadosRecebidosEmFormatoJSON;
        UdpClient udpServer = null;
        IPEndPoint remoteEP = null;
        string mensagemRecebida;
        byte[] bytesRecebidos;
        int contadorRecebimentoPacote = 0;
        private List<int> dadosPlotarGrafico = new List<int>();
        Boolean pararRecebimentoDados = false;
        private Stopwatch marcadorTempoCurto; 
        int correnteA = 0;
        int correnteB = 0;
        int correnteC = 0;
        int correnteMedia = 0;


        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false; // possibilita que componentes sejam chamados por threads diferentes
            InitializeComponent();

            formsPlotPacotesRecebidos.Plot.Title("Taxa de Pacotes Recebidos", true, Color.Black, 12.0f);
        }

        private void buttonIniciar_Click(object sender, EventArgs e)
        {
            // inicializações de interface
            buttonIniciar.Enabled = false;
            buttonParar.Enabled = true;
            timerPlotSinaisRecebidos.Start();
            contadorRecebimentoPacote = 0;
            dadosPlotarGrafico.Clear();
            pararRecebimentoDados = false;

            // conexão UDP
            udpServer = new UdpClient(11000);
            remoteEP = new IPEndPoint(IPAddress.Any, 11000);

            threadRecebimentosPacotes = new Thread(RecebimentoPacotesUDP);
            threadRecebimentosPacotes.Start();

            
            // GERA THREADS
            //for (int i = 0; i < quantidadeUnidadesGeradoras; i++)
            //{
            //    listaThreadsDispositivos[i] = new Thread(new ThreadStart(listaUnidadeMonitoramento[i].AnalisaDados));
            //    listaThreadsDispositivos[i].Name = "Dispositivo de Monitoramento" + i.ToString();
            //    listaThreadsDispositivos[i].Start();
            //}
        }

        private void buttonParar_Click(object sender, EventArgs e)
        {
            // parte 0: inicializações de interface
            buttonParar.Enabled = false;
            timerPlotSinaisRecebidos.Stop(); //para plotar sinais
            contadorRecebimentoPacote = 0;
            dadosPlotarGrafico.Clear();
            buttonIniciar.Enabled = true;
            pararRecebimentoDados = true;
            textBoxCorrenteAtual.Text = "";
            textBoxTimerControleCurto.Text = "";

            //// parte 1: para os objetos equivalentes a unidades geradoras            
            //for (int i = 0; i < listaUnidadesGeradorasDadosMedicao.Length; i++)
            //{
            //    listaUnidadesGeradorasDadosMedicao[i].pararEnvio = true; // vai forçar as threads pararem
            //}
            //Thread.Sleep(500);
            //listaUnidadesGeradorasDadosMedicao = null;

            // parte 2: fecha a conexão UDP
            udpServer.Close();
            threadRecebimentosPacotes.Abort();
            threadDispositivo.Abort();
        }

        private void RecebimentoPacotesUDP()
        {
            while (true)
            {
                if (pararRecebimentoDados)
                    return;

                bytesRecebidos = udpServer.Receive(ref remoteEP);
                mensagemRecebida = Encoding.ASCII.GetString(bytesRecebidos);
                dadosRecebidosEmFormatoJSON = JsonConvert.DeserializeObject<JSON_Dados_Corrente>(mensagemRecebida);

                correnteA = dadosRecebidosEmFormatoJSON.Ia;
                correnteB = dadosRecebidosEmFormatoJSON.Ib;
                correnteC = dadosRecebidosEmFormatoJSON.Ic;
                correnteMedia = (correnteA + correnteB + correnteC) / 3;

                textBoxCorrenteAtual.Text = correnteMedia.ToString() + " A";
                toolStripTextBoxConexao.Text = "UDP (" + remoteEP.Address.ToString() + ")";
                contadorRecebimentoPacote++;

                threadDispositivo = new Thread(() => AnalisaDados(correnteMedia));
                threadDispositivo.Start();
            }
        }

        private void AnalisaDados(int corrente)
        {
            double tempo_dial = 0.0;
            marcadorTempoCurto = new Stopwatch(); 

            // ALTERAR ANÁLISE DA CORRENTE
            if (corrente >= 2)
            {
                tempo_dial = 3.0;
                marcadorTempoCurto.Start(); // inicia contagem de tempo
                timerControleCurto.Start();
                textBoxTimerControleCurto.Text = marcadorTempoCurto.Elapsed.TotalSeconds.ToString();
            }

            if (marcadorTempoCurto.IsRunning)
            {
                if (Convert.ToDouble(marcadorTempoCurto.Elapsed.TotalSeconds) < tempo_dial)
                {
                    // espera
                }
                else if (marcadorTempoCurto.Elapsed.TotalSeconds >= tempo_dial)
                {
                    marcadorTempoCurto.Stop();
                    timerControleCurto.Stop();
                }
            }
        }

        private void timerControleCurto_Tick(object sender, EventArgs e)
        {
            //textBoxTimerControleCurto.Text = Convert.ToString(marcadorTempoCurto.ElapsedMilliseconds) + " ms";
        }

        private void timerPlotaModulo1SinaisEnviados_Tick(object sender, EventArgs e)
        {
            if (dadosPlotarGrafico.Count > 300) // se tiver muitas amostras, zera
            {
                dadosPlotarGrafico.Clear();
            }
            else
            {
                dadosPlotarGrafico.Add(contadorRecebimentoPacote);
            }
            contadorRecebimentoPacote = 0; // zera contagem


            // atualiza a visualização do gráfico
            double[] ys = new double[dadosPlotarGrafico.Count];
            double[] xs = DataGen.Consecutive(dadosPlotarGrafico.Count);
            for (int i = 0; i < dadosPlotarGrafico.Count; i++)
            {
                ys[i] = (double)correnteMedia;
            }
            formsPlotPacotesRecebidos.Plot.Clear();
            if (dadosPlotarGrafico.Count > 1)
            {
                formsPlotPacotesRecebidos.Plot.AddScatterLines(xs, ys, Color.Blue, 2);
                formsPlotPacotesRecebidos.Refresh();
            }
        }

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
