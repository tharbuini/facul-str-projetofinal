using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace STR_Receptor_Pacotes
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient udpServer = new UdpClient(11001);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (true)
                {
                    byte[] bytes = udpServer.Receive(ref endPoint);
                    string formatoPacote = Encoding.ASCII.GetString(bytes);

                    Console.WriteLine("Mensagem recebida: " + formatoPacote);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                udpServer.Close();
            }
        }
    }
}