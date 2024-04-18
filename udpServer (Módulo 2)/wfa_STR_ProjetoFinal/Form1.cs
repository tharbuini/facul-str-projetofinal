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
        private List<UnidadeMonitoramentoDados> listaDispositivos = new List<UnidadeMonitoramentoDados>();
        private List<int> dadosPlotarGrafico = new List<int>();
        public Mutex mutex = new Mutex();
        JSON_Dados_Corrente dadosRecebidosJSON;
        UdpClient udpServer = null;
        IPEndPoint remoteEP = null;
        string mensagemRecebida;
        byte[] bytesRecebidos;
        int contadorRecebimentoPacote = 0;
        Boolean pararRecebimentoDados = false;
        double correnteMedia = 0;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false; 
            InitializeComponent();

            formsPlotPacotesRecebidos.Plot.Title("Dados de Corrente Recebidos", true, Color.Black, 12.0f);
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
            listViewDispositivos.Clear();

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

        List<int> listaIDDispositivos = new List<int>();  
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
                contadorRecebimentoPacote++;

                // Verifica se não é o pacote de encerramento de envios
                if (id != -1)
                {
                    if (listaDispositivos.Count <= id || listaDispositivos[id] == null)
                    {
                        // Cria uma nova instância de UnidadeMonitoramentoDados se ainda não existir
                        UnidadeMonitoramentoDados dispositivo = new UnidadeMonitoramentoDados(id, correnteMedia, udpServer, mutex);
                        listaDispositivos[id] = dispositivo;
                        Thread threadDispositivo = new Thread(() => dispositivo.AnalisaDados());
                        threadDispositivo.Start();
                    }
                    else
                    {
                        // Atualiza os dados da instância existente de UnidadeMonitoramentoDados
                        listaDispositivos[id].AtualizaCorrenteMedia(correnteMedia);
                    }

                    if (listaIDDispositivos.Contains(id))
                    {
                        // Atualiza listview com novos valores de corrente
                        foreach (ListViewItem item in listViewDispositivos.Items)
                        {
                            if (item.SubItems[0].Text == id.ToString()) // Procura pelo ID
                            {
                                item.SubItems[1].Text = correnteMedia.ToString(); // Atualiza a corrente
                            }
                        }
                    }
                    else
                    {
                        listViewDispositivos.Items.Add(new ListViewItem(new String[] { id.ToString(), correnteMedia.ToString(), "-" }));
                        listaIDDispositivos.Add(id);
                    }
                }
                else
                {
                    // Atualiza todas as correntes para 0
                    for (int i = 1; i < listaDispositivos.Count; i++)
                    {
                        if (listaDispositivos[i] != null)  
                            listaDispositivos[i].AtualizaCorrenteMedia(0);
                    }
                }
            }
        }

        public void ModificarListView(int id, double tempoAtuacao)
        {
            foreach (ListViewItem item in listViewDispositivos.Items)
            {
                if (item.SubItems[0].Text == id.ToString()) // Procura pelo ID
                {
                    item.SubItems[2].Text = tempoAtuacao.ToString(); // Atualiza o tempo de atuação
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
        private int idDispositivo;
        private double correnteNominal = 600;
        private double dial = 0.25;
        private double correnteCCMax = 10000;
        private double tempoAtuacao;
        private double correnteMedia;
        private System.Threading.Timer timer;
        private double tempoRestanteEmSegundos;
        private Mutex mutex;
        private UdpClient udpServer;
        Thread threadAlarme = null;
        Thread threadTempoAtuacao = null;
        double correnteSendoAnalisada = 0;
        Boolean timerComecou = false;
        Boolean emCurto = false;

        public UnidadeMonitoramentoDados(int p_id, double p_correnteMedia, UdpClient p_udpServer, Mutex p_mutex)
        {
            this.idDispositivo = p_id;
            this.correnteMedia = p_correnteMedia;
            this.udpServer = p_udpServer;
            this.mutex = p_mutex;
        }

        public void AnalisaDados()
        {
            Form1 meuForm = new Form1();

            while (true)
            {
                if (correnteMedia < correnteNominal)
                {
                    emCurto = false;
                    if (timerComecou)
                    {
                        timer.Dispose();
                        threadAlarme.Abort();
                        timerComecou = false;
                    }
                }

                if (correnteMedia >= correnteCCMax)
                {
                    emCurto = true;
                    Alarme();
                }

                else if (correnteMedia > correnteNominal) {
                    // Seguindo a curva muito-inversa e os valores adotados no vídeo de exemplo
                    tempoAtuacao = dial * (13.5 / ((correnteMedia / correnteNominal) - 1));
                    emCurto = true;

                    meuForm.ModificarListView(idDispositivo, tempoAtuacao);

                    if (!timerComecou)
                    {
                        threadTempoAtuacao = new Thread(new ThreadStart(MostraTempoAtuacao));
                        threadTempoAtuacao.Start();

                        tempoRestanteEmSegundos = 0;
                        timer = new System.Threading.Timer(TimerCallback, null, 0, 10);

                        timerComecou = true;
                        correnteSendoAnalisada = correnteMedia;
                    }
                    else if (correnteMedia > correnteSendoAnalisada) 
                    {
                        // Reinicializa o timer mantendo a contagem do "tempoRestanteEmSegundos" anterior
                        timer.Dispose();
                        timer = new System.Threading.Timer(TimerCallback, null, 0, 10);
                        correnteSendoAnalisada = correnteMedia;
                    }
                }
            }
        }

        private void MostraTempoAtuacao()
        {
            MessageBox.Show("Corrente: " + this.correnteMedia + " A / Tempo de Atuação: " + this.tempoAtuacao);
            threadTempoAtuacao.Abort();
        }

        private void TimerCallback(object state)
        {
            // Atualiza o tempo restante
            tempoRestanteEmSegundos += 0.01;

            if (tempoRestanteEmSegundos >= tempoAtuacao)
            {
                // Para o timer e despacha pacote 99/1 quando o tempo limite for atingido
                timer.Dispose(); 
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
            while (emCurto)
            {
                // Pacote a ser enviado para o módulo atuador
                //string formatoPacote = "{'ID': " + this.dispositivoId +
                //                       " ,'Corrente': " + this.correnteMedia + "}";

                //byte[] bytes = Encoding.ASCII.GetBytes(formatoPacote);
                //mutex.WaitOne();
                //if (udpServer != null)
                //{
                //    udpServer.Send(bytes, bytes.Length);
                //}
                //mutex.ReleaseMutex();

                threadAlarme = new Thread(new ThreadStart(EnviaPacotesAlarme));
                threadAlarme.Start();

                // Verifica a urgência de enviar pacotes
                // IMPLEMENTAR O MECANISMO QoS CONTANDO PACOTES QUE FORAM ENVIADOS
                if (correnteMedia < correnteCCMax)
                    Thread.Sleep(Convert.ToInt32(tempoAtuacao * 1000));
                else
                    Thread.Sleep(500);
            }
        }

        private void EnviaPacotesAlarme()
        {
            MessageBox.Show("Pacote de alarme enviado! ID: " + this.idDispositivo + " " + this.correnteMedia);
            //contadorPacotes++;
        }
    }
}
