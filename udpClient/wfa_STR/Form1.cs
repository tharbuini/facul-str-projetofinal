using ScottPlot;
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

namespace wfa_STR
{
    public partial class Form1 : Form
    {
        Thread threadRecebimentosPacotes = null;
        Thread[] listaThreadsUnidadesGeradoras;
        UnidadeGeradoraDadosMedicao[] listaUnidadesGeradorasDadosMedicao;
        UdpClient udpClient = null;
        IPEndPoint ipConexaoRecebimentoUDP = null;
        IPEndPoint ipConexaoEnvioUDP = null;
        string mensagemRecebida;
        byte[] bytesRecebidos;
        private List<int> dadosPlotarGrafico= new List<int>();
        JSON_DADOS_RECEBIDOS_CORRENTE dadosRecebidosEmFormatoJSON;
        int contadorRecebimentoPacote = 0;
        public Mutex nossoMutex = new Mutex();
        bool pararRecebimentoDados = false;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // possibilita que componentes sejam chamados por threads diferentes                                    
            // passo 1: encontra o IP e atribui a classe principal de dados e controle 
            formsPlotPacotesEnviados.Plot.Title("Taxa pacotes recebidos de subestação", true, Color.Black, 12.0f);
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
            // parte 0: inicializações de interface
            buttonIniciarEnvio.Enabled = false;
            propertyGridUnidGeradoras.Enabled = true;
            buttonPararEnvio.Enabled = true;
            timerPlotSinaisEnviados.Start(); // para plotar sinais
            contadorRecebimentoPacote = 0;
            dadosPlotarGrafico.Clear();
            pararRecebimentoDados = false;

            // parte 1: inicia a conexão UDP
            udpClient = new UdpClient();
            udpClient.Connect("127.0.0.1", 11000);

            ipConexaoRecebimentoUDP = new IPEndPoint(IPAddress.Any, 11001);

            threadRecebimentosPacotes = new Thread(RecebimentoPacotesUDP);
            threadRecebimentosPacotes.Start();

            // parte 2: gera objetos equivalentes a unidades geradoras
            int quantidadeUnidGeradoras = listViewUnidGeradora.Items.Count;
            listaUnidadesGeradorasDadosMedicao = new UnidadeGeradoraDadosMedicao[quantidadeUnidGeradoras];
            listaThreadsUnidadesGeradoras = new Thread[quantidadeUnidGeradoras];

            int codigo = 0;
            int freqEnvio = 0;
            int correnteOriginal = 0;
            int correnteCurto = 0;
            for (int i = 0; i < quantidadeUnidGeradoras; i++)
            {
                codigo = Convert.ToInt16(listViewUnidGeradora.Items[i].SubItems[0].Text);
                freqEnvio = Convert.ToInt16(listViewUnidGeradora.Items[i].SubItems[1].Text);
                correnteOriginal = Convert.ToInt16(listViewUnidGeradora.Items[i].SubItems[2].Text);
                correnteCurto = Convert.ToInt16(listViewUnidGeradora.Items[i].SubItems[3].Text);
                listaUnidadesGeradorasDadosMedicao[i] = new UnidadeGeradoraDadosMedicao(udpClient, ipConexaoEnvioUDP, nossoMutex, correnteOriginal, codigo, freqEnvio, correnteCurto, false);
            }

            // parte 3: gera threads
            for (int i = 0; i < quantidadeUnidGeradoras; i++)
            {
                listaThreadsUnidadesGeradoras[i] = new Thread(new ThreadStart(listaUnidadesGeradorasDadosMedicao[i].EnviaPacotesUDPFrequentemente));
                listaThreadsUnidadesGeradoras[i].Name = "UnidGeradora" + i.ToString();
                listaThreadsUnidadesGeradoras[i].Start();
            }
        }

        private void buttonPararEnvio_Click(object sender, EventArgs e)
        {
            // parte 0: inicializações de interface
            propertyGridUnidGeradoras.Enabled = false;
            buttonPararEnvio.Enabled = false;
            timerPlotSinaisEnviados.Stop(); //para plotar sinais
            contadorRecebimentoPacote = 0;
            dadosPlotarGrafico.Clear();
            buttonIniciarEnvio.Enabled = true;
            pararRecebimentoDados = true;

            // parte 1: para os objetos equivalentes a unidades geradoras            
            for (int i = 0; i < listaUnidadesGeradorasDadosMedicao.Length; i++)
            {
                listaUnidadesGeradorasDadosMedicao[i].pararEnvio = true; // vai forçar as threads pararem
            }
            Thread.Sleep(500);
            listaUnidadesGeradorasDadosMedicao = null;

            // parte 2: fecha a conexão UDP
            udpClient.Close();
            threadRecebimentosPacotes.Abort();
        }

        private void buttonLimpar_Click(object sender, EventArgs e)
        {
            listViewUnidGeradora.Items.Clear();
        }

        private void buttonAdicionar_Click(object sender, EventArgs e)
        {
            listViewUnidGeradora.Items.Add(new ListViewItem(new String[] { numericUpDownCodUnidGen.Value.ToString(), numericUpDownFreqEnvio.Value.ToString(), numericUpDownValorCorrente.Value.ToString(), numericUpDownValorCurto.Value.ToString() }));
            numericUpDownCodUnidGen.Value = numericUpDownCodUnidGen.Value + 1;
        }

        private void RecebimentoPacotesUDP()
        {
            while (true)
            {
                if (pararRecebimentoDados) // para o recebimento caso a flag seja True
                    return;
                bytesRecebidos = udpClient.Receive(ref ipConexaoRecebimentoUDP);
                //mensagemRecebida = Encoding.ASCII.GetString(bytesRecebidos);
                //dadosRecebidosEmFormatoJSON = JsonConvert.DeserializeObject<JSON_DADOS_RECEBIDOS_CORRENTE>(menssagemRecebida);
                //textBoxRecepcaoConexao.Text = textBoxRecepcaoConexao.Text + "RECEBEU_UDP (" + ipConexaoRecebimentoUDP.Address.ToString() + ") >>" + mensagemRecebida + Environment.NewLine;
                contadorRecebimentoPacote++;
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
            public int contadoPacotesEnviados { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Description("Valor que indica se o item esta em curto")]
            [DisplayName("Valor curto")]
            public int valorCurto { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Description("Indica se está em curto")]
            [DisplayName("Em curto?")]
            public bool estaCurto { get; set; }

            [Browsable(false)]
            public bool pararEnvio { get; set; }

            public Mutex nossoMutex;
            public UdpClient usocketConexaoUDP;
            public IPEndPoint ipConexaoEnvioUDP;

            public UnidadeGeradoraDadosMedicao(UdpClient p_usocketConexaoUDP, IPEndPoint p_ipConexaoEnvioUDP, Mutex p_nossoMutex, int p_valorCorrente, int p_codDestaUnidade, int p_freqEnvioPacotesMS, int p_valorCurto, bool p_estaCurto)
            {
                nossoMutex = p_nossoMutex;
                valorCorrente = p_valorCorrente;
                codDestaUnidade = p_codDestaUnidade;
                freqEnvioPacotesMS = p_freqEnvioPacotesMS;
                valorCurto = p_valorCurto;
                estaCurto = p_estaCurto;
                usocketConexaoUDP = p_usocketConexaoUDP;
                ipConexaoEnvioUDP = p_ipConexaoEnvioUDP;
                contadoPacotesEnviados = 0;
                pararEnvio = false;
            }

            public void EnviaPacotesUDPFrequentemente() // avi ser usado como uma thread
            {
                string formatoPacote;
                string corrente;
                byte[] bytes;

                while (true)
                {
                    corrente = Convert.ToString(valorCorrente);
                    if (contadoPacotesEnviados < 65000)
                        contadoPacotesEnviados++;
                    else
                        contadoPacotesEnviados = 0;
                    formatoPacote = "{'Ia': " + corrente +
                                    " ,'Ib': " + corrente +
                                    " ,'Ic': " + corrente +
                                    " ,'numPacote': " + contadoPacotesEnviados.ToString() +
                                    " ,'idDispositivo': " + codDestaUnidade.ToString() + "}";

                    bytes = Encoding.ASCII.GetBytes(formatoPacote);
                    nossoMutex.WaitOne(); // bloqueia esta região para uma simples thread acessar
                    if (usocketConexaoUDP != null)
                        usocketConexaoUDP.Send(bytes, bytes.Length);
                    nossoMutex.ReleaseMutex(); // desbloqueia esta região 
                    if (pararEnvio)
                        return;
                    if ((valorCorrente < valorCurto) || !estaCurto) // se não estiver em curto
                        Thread.Sleep(freqEnvioPacotesMS); // aguarda até próximo envio
                    else // está em curto
                        Thread.Sleep(1); // sleep() para não sobrecarregar
                }
            }
        }

        private void timerPlotSinaisEnviados_Tick(object sender, EventArgs e)
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


            // atualiza visualização do gráfico
            double[] ys = new double[dadosPlotarGrafico.Count];
            double[] xs = DataGen.Consecutive(dadosPlotarGrafico.Count);
            for (int i = 0; i < dadosPlotarGrafico.Count; i++)
            {
                ys[i] = (double)dadosPlotarGrafico[i];
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

