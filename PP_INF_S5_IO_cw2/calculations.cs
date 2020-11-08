using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace ServerEchoLibrary
{
    class calculations
    {
        /// <summary>
        /// Zmienne przechowujace stan operacji
        /// value1 - pierwsza wartosc
        /// value2 - druga wartosc
        /// operation - operacja - tj. -1, 0, 1 -> jest to stan - oczekiwanie na pierwsza wartosc, znak czy na druga
        /// </summary>
        static int value1, value2, operation = -1;

        /// <summary>
        /// zmienna przechowujaca znak operacji
        /// </summary>
        static string matOper = "";

        /// <summary>
        /// Obliczanie silnia
        /// </summary>
        /// <param name="a">ile silnia obliczyc</param>
        /// <returns></returns>
        public static int calculateFact(int a)
        {
            int v = 1;
            for (int i = 1; i <= a; i++)
            {
                v = v * i;
            }
            return v;
        }

        /// <summary>
        /// Wykonywanie obliczen
        /// </summary>
        /// <param name="client">Instancja klienta</param>
        /// <param name="v">Wartosc odebrana na wejsciu</param>
        /// <returns></returns>
        public static int doCalculation(Stream stream, string v)
        {
            if (operation == -1)
            {
                operation = 0;
                value1 = Int32.Parse(v);
            }
            else if (operation == 0)
            {
                switch (v)
                {
                    case "!":
                        value1 = calculateFact(value1);
                        ServerEchoAPM.printText(stream, "Wynik: " + value1 + "\r\n");
                        operation = -1;
                        break;
                    case "+":
                        matOper = "+";
                        operation = 1;
                        break;
                    case "-":
                        matOper = "-";
                        operation = 1;
                        break;
                    case "/":
                        matOper = "/";
                        operation = 1;
                        break;
                    case "*":
                        matOper = "*";
                        operation = 1;
                        break;
                }
            }
            else if (operation == 1)
            {
                value2 = Int32.Parse(v);
                operation = -1;
                switch (matOper)
                {
                    case "+":
                        value1 = value1 + value2;
                        ServerEchoAPM.printText(stream, "Wynik: " + value1 + "\r\n");
                        break;
                    case "-":
                        value1 = value1 - value2;
                        ServerEchoAPM.printText(stream, "Wynik: " + value1 + "\r\n");
                        break;
                    case "/":
                        value1 = value1 / value2;
                        ServerEchoAPM.printText(stream, "Wynik: " + value1 + "\r\n");
                        break;
                    case "*":
                        value1 = value1 * value2;
                        ServerEchoAPM.printText(stream, "Wynik: " + value1 + "\r\n");
                        break;
                }
            }
            return operation;
        }
    }
}
