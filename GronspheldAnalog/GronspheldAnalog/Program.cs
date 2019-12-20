using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GronspheldAnalog
{
    class Program
    {
        
        //Setting up a GB alphabet
        public void SetDic(Dictionary<int, char> abc)
        {
            
            abc.Add(0, 'a');
            abc.Add(1, 'b');
            abc.Add(2, 'c');
            abc.Add(3, 'd');
            abc.Add(4, 'e');
            abc.Add(5, 'f');
            abc.Add(6, 'g');
            abc.Add(7, 'h');
            abc.Add(8, 'i');
            abc.Add(9, 'j');
            abc.Add(10, 'k');
            abc.Add(11, 'l');
            abc.Add(12, 'm');
            abc.Add(13, 'n');
            abc.Add(14, 'o');
            abc.Add(15, 'p');
            abc.Add(16, 'q');
            abc.Add(17, 'r');
            abc.Add(18, 's');
            abc.Add(19, 't');
            abc.Add(20, 'u');
            abc.Add(21, 'v');
            abc.Add(22, 'w');
            abc.Add(23, 'x');
            abc.Add(24, 'y');
            abc.Add(25, 'z');
            abc.Add(26, 'a');
            abc.Add(27, 'b');
            abc.Add(28, 'c');
            abc.Add(29, 'd');
            abc.Add(30, 'e');
            abc.Add(31, 'f');
            abc.Add(32, 'g');
            abc.Add(33, 'h');
            abc.Add(34, 'i');


        }
        
        // Creating Gronsfeld's table
        public char[,] TableCreator(Dictionary<int, char> abc, char[,] table)
        {
            for (int i = 0; i <= 9; i++)
            {
                for(int j = 0; j <= 25; j++)
                {
                    table[i, j] = abc.ElementAt(j + i).Value;
                }
            }
            return table;
        }
        
        static void Main(string[] args)
        {
            Program cobj = new Program();
            Dictionary<int, char> dict = new Dictionary<int, char>(35);
            char[,] table = new char[10, 26];
            string DecWord = "", EncWord = "", KEY = "2019", newKEY = KEY, trash = null, result = "";
            string abc = "abcdefghijklmnopqrstuvwxyz";
            int count1 = 0, count2 = 0;
            cobj.SetDic(dict);
            cobj.TableCreator(dict, table);


            Console.WriteLine("Выберете что вы хотите сделать: ");
            Console.WriteLine("         1. Закодировать. ");
            Console.WriteLine("         2. Раскодировать. ");
            Console.WriteLine("         3. Завершить. ");
            trash = Console.ReadLine();
          
          
          
            if (trash == "1")
            {
                Console.WriteLine(" ENCODING ... ");
                Console.Write("Введите слово, которое вы хотите закодировать: ");
                
                EncWord = Console.ReadLine();
                EncWord = EncWord.ToLower();
                Console.WriteLine();
                Console.WriteLine(EncWord);
                
                while (newKEY.Length < EncWord.Length)
                {
                    newKEY += KEY;
                }
                if (newKEY.Length > EncWord.Length)
                {
                    newKEY = newKEY.Substring(0, newKEY.Length - (newKEY.Length - EncWord.Length));
                }
                
                for (int i = 0; i < EncWord.Length; i++)
                {
                    
                    count1 = abc.IndexOf(EncWord[i]);
                    count2 = newKEY[i]-48;
                    result += table[count2, count1];
                }
                Console.WriteLine("Результат : {0}",result);
                Console.WriteLine();
            }



          
            if (trash == "2")
            {
                Console.WriteLine(" DECODING ... ");
                Console.WriteLine("Введите слово, которое вы хотите раскодировать: ");

                DecWord = Console.ReadLine();
                DecWord = DecWord.ToLower();
                Console.WriteLine();
                Console.WriteLine(DecWord);
               
                while (newKEY.Length < DecWord.Length)
                {
                    newKEY += KEY;
                }
                if (newKEY.Length > DecWord.Length)
                {
                    newKEY = newKEY.Substring(0, newKEY.Length - (newKEY.Length - DecWord.Length));
                }

                for (int i = 0; i < DecWord.Length; i++)
                {
                    count1 = newKEY[i] - 48;
                    count2 = abc.IndexOf(DecWord[i])-count1;
                    if (count2 < 0)
                        count2 = 25 + count2;
                    result += table[0, count2];
                }
                Console.WriteLine("Результат : {0}", result);
                Console.WriteLine();

            }
            Console.WriteLine("Finished!");
            Console.ReadKey();

        }
    }
}
