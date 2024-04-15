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
            timerPlotPacotesRecebidos.Start();
            contadorRecebimentoPacote = 0;
            dadosPlotarGrafico.Clear();
            pararRecebimentoDados = false;

            udpServer = new UdpClient(11000);
            remoteEP = new IPEndPoint(IPAddress.Any, 11000);

            toolStripTextBoxConexao.Text = "UDP (" + remoteEP.Address.ToString() + ")";

            threadRecebimentosPacotes = new Thread(RecebimentoPacotesUDP);
            threadRecebimentosPacotes.Start();

            // RECEBE PACOTES SOMENTE DO DISPOSITIVO COM ID IGUAL
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
            timerPlotPacotesRecebidos.Stop();
            contadorRecebimentoPacote = 0;
            dadosPlotarGrafico.Clear();
            buttonIniciar.Enabled = true;
            pararRecebimentoDados = true;
            textBoxCorrenteAtual.Text = "";
            textBoxTempoEspera.Text = "";
            textBoxTimerControleCurto.Text = "";
            timer.Dispose();

            //for (int i = 0; i < listaUnidadesGeradorasDadosMedicao.Length; i++)
            //{
            //    listaUnidadesGeradorasDadosMedicao[i].pararEnvio = true; // vai forçar as threads pararem
            //}
            //Thread.Sleep(500);
            //listaUnidadesGeradorasDadosMedicao = null;

            // encerrando conexão e threads
            if (threadRecebimentosPacotes != null)
            {
                threadRecebimentosPacotes.Abort();
            }

            if (udpServer != null)
            { 
                udpServer.Close();
            }

            if (threadDispositivo != null) 
            { 
                threadDispositivo.Abort();
            }
        }

        // RECEBIMENTO DE PACOTES DELEGA PACOTES PARA CADA THREAD DE ACORDO COM ID?
        // OU CADA UMA TEM QUE RECEBER? ----> ACHO QUE É ISSO
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
                contadorRecebimentoPacote++;


                if (correnteMedia >= correnteCCMax)
                {
                    textBoxTempoEspera.Text = "0 s";
                    //Alarme();
                }
                else if (correnteMedia < correnteNominal && threadAberta)
                {
                    threadDispositivo.Abort();
                    timer.Dispose();

                    textBoxTempoEspera.Text = "";
                    textBoxTimerControleCurto.Text = "";
                }
                //else if (((correnteMedia >= correnteNominal) && !threadAberta) || ((correnteMedia > correnteSendoAnalisada) && threadAberta))
                else if ((correnteMedia > correnteNominal) && !threadAberta)
                {
                    threadDispositivo = new Thread(() => AnalisaDados(correnteMedia));
                    threadDispositivo.Start();
                    threadAberta = true;
                    correnteSendoAnalisada = correnteMedia;
                }
            }
        }

        Boolean timerComecou = false;
        Boolean estaEmCurto = false;
        double tempoAtuacao = 0;

        private System.Threading.Timer timer;
        private double tempoRestanteEmSegundos;

        private void AnalisaDados(double corrente)
        {
            while (true)
            {
                if (estaEmCurto)
                {
                    break;
                }

               
                // seguindo a curva muito-inversa e os valores adotados no vídeo de exemplo
                tempoAtuacao = dial * (13.5 / ((correnteCCMax / corrente) - 1)); 
                textBoxTempoEspera.Text = tempoAtuacao.ToString() + " s";

                if (!timerComecou)
                {
                    //marcadorTempoCurto = new Stopwatch();
                    //marcadorTempoCurto.Start(); // inicia contagem de tempo
                    //timerControleCurto.Start();
                    
                    tempoRestanteEmSegundos = 0;
                    timer = new System.Threading.Timer(TimerCallback, null, 0, 10);
                    
                    timerComecou = true;
                }
            }
        }

        private void TimerCallback(object state)
        {
            // atualiza o tempo restante
            tempoRestanteEmSegundos += 0.01;

            // atualiza o texto do textbox com o tempo restante
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => textBoxTimerControleCurto.Text = tempoRestanteEmSegundos.ToString("F2")));
            }
            else
            {
                textBoxTimerControleCurto.Text = tempoRestanteEmSegundos.ToString("F2");
            }

            if (tempoRestanteEmSegundos >= tempoAtuacao)
            {
                timer.Dispose(); // para o timer quando o tempo limite for atingido
                Alarme();
                //MessageBox.Show("Tempo limite atingido!");
            }
        }

        private void timerControleCurto_Tick(object sender, EventArgs e)
        {

        //    double tempoDecorrido = marcadorTempoCurto.ElapsedMilliseconds / 1000;
        //    textBoxTimerControleCurto.Text = Convert.ToString(tempoDecorrido) + " s";

        //    if (tempoDecorrido < tempoAtuacao)
        //    {
        //        // espera
        //    }
        //    else
        //    {
        //        marcadorTempoCurto.Stop();
        //        timerControleCurto.Stop();
        //        timerComecou = false;
        //        textBoxTimerControleCurto.Text = "Concluído em: " + Convert.ToString(marcadorTempoCurto.ElapsedMilliseconds / 1000) + " seg. Timer fechado";

        //        // estaEmCurto = true;
        //        // Alarme();
        //    }
        }

        private void timerPlotPacotesRecebidos_Tick(object sender, EventArgs e)
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

            // atualiza a visualização do gráfico
            double[] ys = new double[dadosPlotarGrafico.Count];
            double[] xs = DataGen.Consecutive(dadosPlotarGrafico.Count);
            for (int i = 0; i < dadosPlotarGrafico.Count; i++)
            {
                ys[i] = dadosPlotarGrafico[i];
            }
            formsPlotPacotesRecebidos.Plot.Clear();
            if (dadosPlotarGrafico.Count > 1)
            {
                formsPlotPacotesRecebidos.Plot.AddScatterLines(xs, ys, Color.Blue, 2);
                formsPlotPacotesRecebidos.Refresh();
            }
        }

        public void Alarme()
        {
            MessageBox.Show("Pacote enviado! ID: " + dadosRecebidosJSON.idDispositivo);
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
