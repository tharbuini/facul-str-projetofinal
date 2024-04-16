using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        Thread[] listaThreadsDispositivos;

        private List<UnidadeMonitoramentoDados> listaDispositivos = new List<UnidadeMonitoramentoDados>();
        JSON_Dados_Corrente dadosRecebidosJSON;
        UdpClient udpServer = null;
        IPEndPoint remoteEP = null;
        string mensagemRecebida;
        byte[] bytesRecebidos;
        int contadorRecebimentoPacote = 0;
        private List<int> dadosPlotarGrafico = new List<int>();
        Boolean pararRecebimentoDados = false;
        
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

            // Inicializa a lista de dispositivos
            listaDispositivos = new List<UnidadeMonitoramentoDados>();
            for (int i = 0; i <= 5; i++)
            {
                listaDispositivos.Add(null);
            }

            // Inicia o recebimento de pacotes
            threadRecebimentosPacotes = new Thread(RecebimentoPacotesUDP);
            threadRecebimentosPacotes.Start();
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

            // Encerra a conexão UDP
            if (udpServer != null)
            {
                udpServer.Close();
            }

            // Aguarda o término da thread de recebimento de pacotes
            if (threadRecebimentosPacotes != null && threadRecebimentosPacotes.IsAlive)
            {
                threadRecebimentosPacotes.Join();
            }
        }

        private void RecebimentoPacotesUDP()
        {
            while (true)
            {
                if (pararRecebimentoDados)
                    return;

                bytesRecebidos = udpServer.Receive(ref remoteEP);
                mensagemRecebida = Encoding.ASCII.GetString(bytesRecebidos);
                dadosRecebidosJSON = JsonConvert.DeserializeObject<JSON_Dados_Corrente>(mensagemRecebida);

                int id = dadosRecebidosJSON.idDispositivo;
                correnteMedia = (dadosRecebidosJSON.Ia + dadosRecebidosJSON.Ib + dadosRecebidosJSON.Ic) / 3;

                textBoxCorrenteAtual.Text = correnteMedia.ToString() + " A" + " / Dispositivo: " + id.ToString();
                contadorRecebimentoPacote++;

                if (id != -1) 
                {
                    if (listaDispositivos.Count <= id || listaDispositivos[id] == null)
                    {
                        // Cria uma nova instância de UnidadeMonitoramentoDados se ainda não existir
                        UnidadeMonitoramentoDados dispositivo = new UnidadeMonitoramentoDados(id, correnteNominal, correnteCCMax, correnteMedia, dial);
                        listaDispositivos[id] = dispositivo;
                        Thread threadDispositivo = new Thread(() => dispositivo.AnalisaDados());
                        threadDispositivo.Start();
                    }
                    else
                    {
                        // Atualiza os dados da instância existente de UnidadeMonitoramentoDados
                        listaDispositivos[id].AtualizaCorrenteMedia(correnteMedia);
                    }
                }
            }
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
        private int dispositivoId;
        private double correnteNominal;
        private double correnteCCMax;
        private double dial;
        private double tempoAtuacao;
        private double correnteMedia;

        public UnidadeMonitoramentoDados(int id, double correnteNominal, double correnteCCMax, double correnteMedia, double dial)
        {
            this.dispositivoId = id;
            this.correnteNominal = correnteNominal;
            this.correnteCCMax = correnteCCMax;
            this.correnteMedia = correnteMedia;
            this.dial = dial;
        }

        Boolean timerComecou = false;
        private System.Threading.Timer timer;
        private double tempoRestanteEmSegundos;
        public void AnalisaDados()
        {
            while (true)
            {

                if (correnteMedia < correnteNominal && timerComecou)
                {
                    timer.Dispose();
                    timerComecou = false;
                    break;
                }

                if (correnteMedia >= correnteCCMax)
                {
                    Alarme();
                    Thread.Sleep(10000);
                }

                else if (correnteMedia > correnteNominal) {
                    // seguindo a curva muito-inversa e os valores adotados no vídeo de exemplo
                    tempoAtuacao = dial * (13.5 / ((correnteCCMax / correnteMedia) - 1));

                    if (!timerComecou)
                    {
                        MessageBox.Show("Corrente: " + this.correnteMedia + " / Tempo de Atuação: " + this.tempoAtuacao);

                        tempoRestanteEmSegundos = 0;
                        timer = new System.Threading.Timer(TimerCallback, null, 0, 10);

                        timerComecou = true;
                    }
                }
            }
        }

        private void TimerCallback(object state)
        {
            // atualiza o tempo restante
            tempoRestanteEmSegundos += 0.01;

            if (tempoRestanteEmSegundos >= tempoAtuacao)
            {
                timer.Dispose(); // para o timer quando o tempo limite for atingido
                Alarme();
            }
        }

        public void AtualizaCorrenteMedia(double novaCorrenteMedia)
        {
            // Atualiza a corrente média com o novo valor recebido
            this.correnteMedia = novaCorrenteMedia;
        }

        public void Alarme()
        {
            MessageBox.Show("Pacote de alarme enviado! ID: " + this.dispositivoId + " " + this.correnteMedia);
        }
    }
}
