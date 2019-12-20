using System;
using System.IO;
using System.Xml.Serialization;
using System.Net.Sockets;


namespace final
{
    public enum Genders : int { Male, Female };

    [Serializable]
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
    
   
       private static void Serialize(Human sp)
        {

            FileStream fs = new FileStream("Person.XML", FileMode.Create);


            XmlSerializer xs = new XmlSerializer(typeof(Human));


            xs.Serialize(fs, sp);


            fs.Close();

        }


        static void Main(string[] args)
        {


            string name = null, fam = null, colour = null, a = null;
            Genders gender = 0;
            string age = null;
            int m;

            Console.Write("Your first name = ");
            name = Console.ReadLine();


            Console.Write("Your second name = ");
            fam = Console.ReadLine();
            age:
            Console.Write("Your age = ");
            age = Console.ReadLine();

             
            try
            {
                m = Convert.ToInt16(age);
              
            }
            catch
            {
                Console.WriteLine("We need integer");
                goto age;
            }

        check:
            Console.Write("Your gender (M/F) = ");
            a = Console.ReadLine();
            if ((a == "M") ^ (a == "m"))
                gender = Genders.Male;
            else if ((a == "F") ^ (a == "f"))
                gender = Genders.Female;
            else
            {
                Console.WriteLine("Wrong input! Enter one of these symbols:{ F, f, m, M }");
                goto check;
            } 
               
             

            Console.Write("Your favorite colour = ");
            colour = Console.ReadLine();

            Human Sergey = new Human(name, fam, age, gender, colour);
            Console.WriteLine(Sergey);
            Serialize(Sergey);

            socket.Connect("192.168.1.69", 904);
  
            socket.SendFile("Person.XML");
            Console.ReadLine();
        }
    }
}