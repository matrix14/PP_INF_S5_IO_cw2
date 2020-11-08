using System;

using System.Collections.Generic;

using System.IO;

using System.Linq;

using System.Net;

using System.Net.Sockets;

using System.Text;



namespace ServerEchoLibrary

{

    public class ServerEchoAPM : ServerEcho

    {
        private Boolean isAvailable = false;

        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerEchoAPM(IPAddress IP, int port) : base(IP, port)

        {

        }

        protected override void AcceptClient()

        {

            while (true)

            {

                TcpClient tcpClient = TcpListener.AcceptTcpClient();

                Stream = tcpClient.GetStream();

                isAvailable = true;
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);

                //callback style

                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);

                // async result style

                //IAsyncResult result = transmissionDelegate.BeginInvoke(Stream, null, null);

                ////operacje......

                //while (!result.IsCompleted) ;

                ////sprzątanie

            }

        }

        public static void printText(Stream stream, string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);
            Array.Clear(buffer, 0, buffer.Length);
        }



        private void TransmissionCallback(IAsyncResult ar)

        {
            isAvailable = false;
            Console.WriteLine("Client disconnected!");
            TcpClient tcp = (TcpClient)ar.AsyncState;
            tcp.Close();

        }

        protected override void BeginDataTransmission(NetworkStream stream)

        {

            byte[] buffer = new byte[Buffer_size];

            while (true)

            {

                try

                {

                    Console.WriteLine("Client connected!");
                    printText(stream, "Kalkulator\r\n");
                    printText(stream, "Autor: Kacper Modelski 140751\r\n");
                    printText(stream, "Uzycie: wartosc1 ENTER operacja ENTER (opcjonalnie wartosc2)\r\n");
                    printText(stream, "Operacje: ! + - / *\r\n");
                    printText(stream, "\r\n");
                    while (isAvailable)
                    {
                        stream.Read(buffer, 0, Buffer_size);
                        if (!isAvailable) break;
                        if (buffer[0] == 0x0) break;
                        if (!(buffer[0] == 13 && buffer[1] == 10))
                        {
                            Console.WriteLine("Received value: " + System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
                            calculations.doCalculation(stream, System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0'));

                            Array.Clear(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            Array.Clear(buffer, 0, buffer.Length);
                        }
                    }

                }

                catch (IOException e)

                {

                    break;

                }

            }

        }

        public override void Start()

        {

            StartListening();

            //transmission starts within the accept function

            AcceptClient();

        }



    }

}