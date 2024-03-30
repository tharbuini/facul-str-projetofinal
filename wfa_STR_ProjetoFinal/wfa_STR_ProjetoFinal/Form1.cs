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

namespace wfa_STR_ProjetoFinal
{
    public partial class Form1 : Form
    {
        Thread threadRecebimentosPacotes = null;
        Thread[] listaThreadsDispositivos; // lista de dispositivos para monitoramento simultâneo
        UnidadeMonitoramentoDados[] listaUnidadeMonitoramento;
        JSON_Dados_Corrente dadosRecebidosEmFormatoJSON;
        UdpClient uSocketConexaoUDP = null;
        IPEndPoint ipConexaoRecebimentoUDP = null;
        IPEndPoint ipConexaoEnvioUDP = null;
        string mensagemRecebida;
        byte[] bytesRecebidos;
        int contadorRecebimentoPacote = 0;
        private List<int> dadosPlotarGrafico = new List<int>();
        Boolean pararRecebimentoDados = false;

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
            uSocketConexaoUDP = new UdpClient(12345);
            ipConexaoRecebimentoUDP = new IPEndPoint(IPAddress.Any, 12345);
            ipConexaoEnvioUDP = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 12345);
            threadRecebimentosPacotes = new Thread(RecebimentoPacotesUDP);
            threadRecebimentosPacotes.Start();

            // COMO DESCOBRIR A QUANTIDADE DE UNIDADES GERADORAS DO OUTRO APLICATIVO?
            // gera threads
            for (int i = 0; i < quantidadeUnidadesGeradoras; i++)
            {
                listaThreadsDispositivos[i] = new Thread(new ThreadStart(listaUnidadeMonitoramento[i].AnalisaDados));
                listaThreadsDispositivos[i].Name = "Dispositivo de Monitoramento" + i.ToString();
                listaThreadsDispositivos[i].Start();
            }

        }

        // COMO DESCOBRIR A QUANTIDADE DE UNIDADES GERADORAS DO OUTRO APLICATIVO?
        // talvez função assíncrona (em thread) que monitora os IDs das unidades geradoras que estão enviando pacotes, que manda pacotes em broadcast e vê quem responde?
        // talvez verificar ID a cada vez que receber corrente para monitorar?
        // talvez receber como pacote periodicamente do outro módulo e apenas atribuir a uma variável?

        private void RecebimentoPacotesUDP()
        {
            while (true)
            {
                if (pararRecebimentoDados)
                    return;
                bytesRecebidos = uSocketConexaoUDP.Receive(ref ipConexaoRecebimentoUDP);
                mensagemRecebida = Encoding.ASCII.GetString(bytesRecebidos);
                dadosRecebidosEmFormatoJSON = JsonConvert.DeserializeObject<JSON_Dados_Corrente>(mensagemRecebida);
                textBoxCorrenteAtual.Text = textBoxCorrenteAtual.Text + "RECEBEU_UDP (" + ipConexaoRecebimentoUDP.Address.ToString() + ") >>" + mensagemRecebida + Environment.NewLine;
                contadorRecebimentoPacote++;
            }
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
                ys[i] = (double)dadosPlotarGrafico[i];
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
