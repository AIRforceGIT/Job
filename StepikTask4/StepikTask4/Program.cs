using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepikTask4
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] mass = new int[10];
            int max = 0;
            int num = 0;
            string input = null;
            string kill = null;

            input = Console.ReadLine();

            // 1* Обработка строки, парс в массив целых чисел
            for (int i = 0; i < 10; i++)
            {
                if (input.IndexOf(" ") == -1)
                {
                    kill = input.Substring(0, input.Length);
                    mass[i] = int.Parse(kill);
                    input = input.Remove(0);
                }
                else
                {
                    kill = input.Substring(0, input.IndexOf(" "));


                    mass[i] = int.Parse(kill);
                    input = input.Remove(0, input.IndexOf(" ") + 1);

                }
            }
            // 1* : DONE!
            // 2* : Реализация алгоритма нахождения максимальной суммы
            // Брутфорсим, перебираем все суммы
            // 2*.2 : двойные суммы
            for (int i = 0; i < 10; i++)
            {
                for (int j=1; j < 10; j++)
                {
                    if (i != j)
                    {
                        if ((mass[i] + mass[j] > max) && (mass[i] + mass[j] <= 100))
                            max = mass[i] + mass[j];
                        if (max == 100)

                            break;
                    }
                }

            }
            
            // 2*.3 : тройные суммы
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    for (int k = 2; k < 10; k++)
                    {
                        if ((i != j) && (j != k)&&(i!=k))
                        {
                            if ((mass[i] + mass[j] + mass[k] > max) && (mass[i] + mass[j] + mass[k] <= 100))
                                max = mass[i] + mass[j] + mass[k];
                            if (max == 100)

                                break;
                        }
                    }
                }
            }
        
            // 2*.4 : суммы из четырех
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    for (int k = 2; k < 10; k++)
                    {
                        for (int z = 3; z < 10; z++)
                        {
                            if ((i != j) && (j != k) && (k != z) && (i != k) && (i != z) && (j != z))
                            {
                                if ((mass[i] + mass[j] + mass[k] + mass[z] > max) && (mass[i] + mass[j] + mass[k] + mass[z] <= 100))
                                    max = mass[i] + mass[j] + mass[k] + mass[z];
                                if (max == 100)

                                    break;
                            }
                            
                        }
                    }
                }
            }
          
            // 2*.5 : суммы из пяти
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    for (int k = 2; k < 10; k++)
                    {
                        for (int z = 3; z < 10; z++)
                        {
                            for (int x = 4; x < 10; x++)
                            {
                                if ((i != j) && (j != k) && (k != z) && (i != k) && (i != z) && (j != z) && (i != x) && (j != x) && (k != x) && (z != x))
                                {
                                    if ((mass[i] + mass[j] + mass[k] + mass[z] + mass[x] > max) && (mass[i] + mass[j] + mass[k] + mass[z] + mass[x] <= 100))
                                        max = mass[i] + mass[j] + mass[k] + mass[z] + mass[x];
                                    if (max == 100)

                                        break;
                                }
                            }

                        }
                    }
                }
            }
            
            // 2*.6 : суммы из шести
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    for (int k = 2; k < 10; k++)
                    {
                        for (int z = 3; z < 10; z++)
                        {
                            for (int x = 4; x < 10; x++)
                            {
                                for (int c = 5; c < 10; c++)
                                {
                                    if ((i != j) && (j != k) && (k != z) && (i != k) && (i != z) && (j != z) && (i != x) && (j != x) && (k != x) && (z != x) && (i != c) && (j != c) && (k != c) && (z != c) && (x != c))
                                    {
                                        if ((mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c] > max) && (mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c] <= 100))
                                            max = mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c];
                                        if (max == 100)

                                            break;
                                    }
                                }
                            }

                        }
                    }
                }
            }
           
            // 2*.7 : суммы из семи
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    for (int k = 2; k < 10; k++)
                    {
                        for (int z = 3; z < 10; z++)
                        {
                            for (int x = 4; x < 10; x++)
                            {
                                for (int c = 5; c < 10; c++)
                                {
                                    for (int v = 6; v < 10; v++)
                                    {
                                        if ((i != j) && (j != k) && (k != z) && (i != k) && (i != z) && (j != z) && (i != x) && (j != x) && (k != x) && (z != x) && (i != c) && (j != c) && (k != c) && (z != c) && (x != c) && (i != v) && (j != v) && (k != v) && (z != v) && (x != v) && (c != v))
                                        {
                                            if ((mass[v] + mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c] > max) && (mass[v] + mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c] <= 100))
                                                max = mass[v] + mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c];
                                            if (max == 100)

                                                break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            
            // 2*.8 : суммы из восьми
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    for (int k = 2; k < 10; k++)
                    {
                        for (int z = 3; z < 10; z++)
                        {
                            for (int x = 4; x < 10; x++)
                            {
                                for (int c = 5; c < 10; c++)
                                {
                                    for (int v = 6; v < 10; v++)
                                    {
                                        for (int b = 7; b < 10; b++)
                                        {
                                            if ((i != j) && (j != k) && (k != z) && (i != k) && (i != z) && (j != z) && (i != x) && (j != x) && (k != x) && (z != x) && (i != c) && (j != c) && (k != c) && (z != c) && (x != c) && (i != v) && (j != v) && (k != v) && (z != v) && (x != v) && (c != v) && (i != b) && (j != b) && (k != b) && (z != b) && (x != b) && (c != b) && (v != b))
                                            {
                                                if ((mass[b] + mass[v] + mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c] > max) && (mass[b] + mass[v] + mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c] <= 100))
                                                    max = mass[b] + mass[v] + mass[i] + mass[j] + mass[k] + mass[z] + mass[x] + mass[c];
                                                if (max == 100)

                                                    break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
           
            // 2*.9 : суммы из девяти
            int sum = 0;
            for (int i= 0; i < 10; i++)
            {
                num = mass[i];
                mass[i] = 0;
                sum = 0;
                for(int j = 0; j < 10; j++)
                {
                    sum += mass[j];
                   
                }
                if ((sum > max) && (sum <= 100))
                {
                    max = sum;
                    if (max == 100)
                        break;
                }
                mass[i] = num;
            }
           
            // 2*.10 : суммы из десяти
            sum = 0;
            for (int j = 0; j < 10; j++)
            {
                sum += mass[j];
                
                if ((sum > max) && (sum <= 100))
                {
                    max = sum;
                    if (max == 100)
                        break;
                    
                }
            }
            // 2* Наитупейший, ресурсо/время-затратный алгоритм: DONE!
            Console.WriteLine("max = ");
            Console.WriteLine(max);
            Console.ReadKey();
        }
    }
}
/*на вход подается строка из 10 целых чисел разделенных пробелами
 * найти наибольшую сумму чисел, не превышающую 100
 * Notes:
 *  Общее кол-во вариаций = 1013
*/