using System;
using System.Net.Sockets;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace Server
{
    public enum Genders : int { Male, Female };



    public class Human
    {
        public string FirstName;
        public string SecondName;
        public string Age;
        public Genders Gender;
        public string Colour;

        public Human(string _fn, string _sn, string _age, Genders _gn, string _clr)
        {
            FirstName = _fn;
            SecondName = _sn;
            Age = _age;
            Gender = _gn;
            Colour = _clr;
        }
        public Human()
        {
        }
        public override string ToString()
        {
            return FirstName + " " + SecondName + " " + Age + " (" + Gender + ") (favorite colour = " + Colour + ")";
        }


    }
    class Program
    {



        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {

            Human dsp = new Human();
            socket.Bind(new IPEndPoint(IPAddress.Any, 904));
            socket.Listen(2);
            Socket client = socket.Accept();
            Console.WriteLine("Successful connection");
            byte[] buffer = new byte[1024];
            client.Receive(buffer);

            Stream fs = new MemoryStream(buffer);

            XmlSerializer bf = new XmlSerializer(typeof(Human));

            dsp = (Human)bf.Deserialize(fs);




            Console.WriteLine(dsp);
            Console.ReadLine();


        }
    }
}
