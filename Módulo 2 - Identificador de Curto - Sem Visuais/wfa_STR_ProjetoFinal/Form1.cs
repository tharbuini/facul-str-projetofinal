using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace STR_Identificador_Curto
{
    public partial class Form1 : Form
    {
        private List<UnidadeMonitoramentoDados> listaDispositivos = new List<UnidadeMonitoramentoDados>();
        private List<int> dadosPlotarGrafico = new List<int>();
        public Mutex mutex = new Mutex();
        Thread threadRecebimentosPacotes = null;
        JSON_Dados_Corrente dadosRecebidosJSON;
        UdpClient udpServer = null;
        IPEndPoint remoteEP = null;
        string mensagemRecebida;
        byte[] bytesRecebidos;
        bool pararRecebimentoDados = false;
        double correnteMedia = 0;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false; 
            InitializeComponent();
        }

        private void buttonIniciar_Click(object sender, EventArgs e)
        {
            buttonIniciar.Enabled = false;
            buttonParar.Enabled = true;
            dadosPlotarGrafico.Clear();
            pararRecebimentoDados = false;

            // Iniciando conexão UDP
            udpServer = new UdpClient(11000);
            remoteEP = new IPEndPoint(IPAddress.Any, 11000);

            toolStripTextBoxConexao.Text = "UDP (" + remoteEP.Address.ToString() + ")";

            // Inicializando a lista de dispositivos
            listaDispositivos = new List<UnidadeMonitoramentoDados>();
            for (int i = 0; i <= 5; i++)
            {
                listaDispositivos.Add(null);
            }

            // Iniciando o recebimento de pacotes
            threadRecebimentosPacotes = new Thread(RecebimentoPacotesUDP);
            threadRecebimentosPacotes.Start();
        }

        private void buttonParar_Click(object sender, EventArgs e)
        {
            buttonParar.Enabled = false;
            dadosPlotarGrafico.Clear();
            buttonIniciar.Enabled = true;
            pararRecebimentoDados = true;
            listViewDispositivos.Clear();

            // Encerrando a conexão UDP
            if (udpServer != null)
            {
                udpServer.Close();
            }

            // Aguardando o término da thread de recebimento de pacotes
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

                if (udpServer != null)
                {
                    bytesRecebidos = udpServer.Receive(ref remoteEP);

                    mensagemRecebida = Encoding.ASCII.GetString(bytesRecebidos);
                    dadosRecebidosJSON = JsonConvert.DeserializeObject<JSON_Dados_Corrente>(mensagemRecebida);

                    int id = dadosRecebidosJSON.idDispositivo;
                    correnteMedia = (dadosRecebidosJSON.Ia + dadosRecebidosJSON.Ib + dadosRecebidosJSON.Ic) / 3;

                    // Verificando se não é o pacote de encerramento de envios
                    if (id != -1)
                    {
                        if (listaDispositivos.Count <= id || listaDispositivos[id] == null)
                        {
                            // Criando uma nova instância de UnidadeMonitoramentoDados se ainda não existir
                            UnidadeMonitoramentoDados dispositivo = new UnidadeMonitoramentoDados(id, correnteMedia, mutex);
                            listaDispositivos[id] = dispositivo;
                            Thread threadDispositivo = new Thread(() => dispositivo.AnalisaDados());
                            threadDispositivo.Start();
                        }
                        else
                        {
                            // Atualizando os dados da instância existente de UnidadeMonitoramentoDados
                            listaDispositivos[id].AtualizaCorrenteMedia(correnteMedia);
                        }

                        if (listaIDDispositivos.Contains(id))
                        {
                            // Atualizando listview com novos valores de corrente
                            foreach (ListViewItem item in listViewDispositivos.Items)
                            {
                                if (item.SubItems[0].Text == id.ToString()) // Procurando pelo ID
                                {
                                    item.SubItems[1].Text = correnteMedia.ToString(); // Atualizando a corrente
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
                        // Atualizando todas as correntes para 0
                        for (int i = 1; i < listaDispositivos.Count; i++)
                        {
                            if (listaDispositivos[i] != null)
                                listaDispositivos[i].AtualizaCorrenteMedia(0);
                        }
                    }
                }
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
        private double dial = 0.25;
        private double correntePartida51 = 600; // A corrente nominal para esse caso seria 502A. A aproximação feita foi I51 = In x 1,2.
        private double correnteCCMax = 7320;    // A corrente calculada no vídeo-base https://www.youtube.com/watch?v=rGn6zAh6ce4
        private double tempoAtuacao;
        private double correnteMedia;
        private System.Threading.Timer timer;
        private double tempoRestanteEmSegundos;
        private Mutex mutex;
        private UdpClient udpClient;
        Thread threadAlarme = null;
        Thread threadTempoAtuacao = null;
        DateTime horaCurto;
        double correnteSendoAnalisada = 0;
        bool timerComecou = false;
        bool emCurto = false;
        int contadorPacotes = 0;

        public UnidadeMonitoramentoDados(int p_id, double p_correnteMedia, Mutex p_mutex)
        {
            this.idDispositivo = p_id;
            this.correnteMedia = p_correnteMedia;
            this.mutex = p_mutex;

            // Inicializando o cliente UDP e definindo o endereço de broadcast
            udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
        }

        public void AnalisaDados()
        {
            while (true)
            {
                if (correnteMedia < correntePartida51)
                {
                    emCurto = false;
                    tempoRestanteEmSegundos = 0;
                    if (timerComecou)
                    {
                        timer.Dispose();
                        timerComecou = false;
                        threadAlarme.Abort();
                        EnviaPacote(idDispositivo, emCurto, correnteSendoAnalisada, DateTime.Now.ToString("h:mm:ss.fff tt"));
                    }
                }
                else 
                {
                    emCurto = true;

                    if (!timerComecou)
                    {
                        horaCurto = DateTime.Now;
                        tempoRestanteEmSegundos = 0;
                        timer = new System.Threading.Timer(TimerCallback, null, 0, 10);

                        timerComecou = true;
                        correnteSendoAnalisada = correnteMedia;
                    }
                    else if (correnteMedia != correnteSendoAnalisada)
                    {
                        // Reinicializando o timer mantendo a contagem do "tempoRestanteEmSegundos" anterior
                        timer.Dispose();
                        if (threadAlarme != null)
                            threadAlarme.Abort();

                        timer = new System.Threading.Timer(TimerCallback, null, 0, 10);
                        timerComecou = true;
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
            // Atualizando o tempo restante
            tempoRestanteEmSegundos += 0.01;
            tempoAtuacao = dial * (13.5 / ((correnteMedia / correntePartida51) - 1));

            if (tempoRestanteEmSegundos >= tempoAtuacao)
            {
                // Parando o timer e despachando pacote 99/1 quando o tempo limite for atingido
                timer.Dispose();
                threadAlarme = new Thread(new ThreadStart(Alarme));
                threadAlarme.Start();
            }
        }

        public void AtualizaCorrenteMedia(double novaCorrenteMedia)
        {
            // Atualizando a corrente média com o novo valor recebido
            this.correnteMedia = novaCorrenteMedia;
        }

        public void Alarme()
        {
            while (emCurto)
            {
                // Suspendendo a Thread por um segundo para evitar sobrecarregamento da rede
                if (contadorPacotes > 1000)
                    Thread.Sleep(1000);

                EnviaPacote(idDispositivo, emCurto, correnteSendoAnalisada, horaCurto.ToString("h:mm:ss.fff tt"));
                contadorPacotes++;

                // Verificando a urgência de enviar pacotes
                if (correnteMedia < correnteCCMax)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    // Caso a corrente seja maior que o limite superior, envia com mais rapidez
                    Thread.Sleep(100);
                }
            }

            contadorPacotes = 0;
        }
        
        private void EnviaPacote(int idDispositivo, bool emCurto, double corrente, string hora)
        {
            // Pacote a ser enviado para o módulo atuador
            string formatoPacote = "{'ID': " + idDispositivo +
                                   ", 'Curto': " + emCurto +
                                   ", 'Corrente': " + corrente +
                                   ", 'Hora': " + hora + "}";

            byte[] bytes = Encoding.ASCII.GetBytes(formatoPacote);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 11001);

            mutex.WaitOne();
            try
            {
                udpClient.Send(bytes, bytes.Length, endPoint);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
