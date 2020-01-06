using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Binary;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Cluster;
using Apache.Ignite.Core.Compute;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Discovery.Tcp.Static;
using Apache.Ignite.Core.Events;

namespace Computation
{
    class Program
    {
        static int Max = 1000000000; 
        static void Main(string[] args)
        {
            using (var ignite = Ignition.StartFromApplicationConfiguration())
            {
                Console.WriteLine();
                Console.WriteLine(">>> Fibonaci program.");

                List<int> nearestFibonacci = new List<int>();
                for (int j = 0; j < 3; j++)
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    int y = rnd.Next(Max);

                    Console.WriteLine();

                    int closest = -1;
                    var res = -1;
                    var prev = -1;

                    for (int i = 0; i < Max; i++)
                    {
                        res = ignite.GetCompute().Apply(new Fibonaci(), i);
                        if (res >= y)
                        {
                            if (res - y > prev - y)
                                closest = prev;
                            else closest = res;
                            break;
                        }
                        prev = res;

                    }

                    nearestFibonacci.Add(closest);
                    Console.WriteLine(">>> Nearest fibonaci for number {0:N} is {1:N}: ", y, closest);
                    Console.WriteLine();
                    Thread.Sleep(1000);
                }


                int minFib = nearestFibonacci.Min();
                int maxFib = nearestFibonacci.Max();
                var result = 0;

                Console.WriteLine(">>> Fibonaci between: {0:N} - {1:N}", minFib, maxFib);
                Console.WriteLine();

                Thread.Sleep(1000);
                List<int> possibleFibonaci = new List<int>();

                for (int i = 0; i < maxFib; i++)
                {
                    result = ignite.GetCompute().Apply(new FibonaciBetween(), i);
                    possibleFibonaci.Add(result);
                    if (result > maxFib)
                        break; 
                    else if (result >= minFib)
                    {
                        Console.WriteLine(">>> Fibonaci: {0:N} ", result);
                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine(">>> Example finished, press any key to exit ...");
            Console.ReadKey();

        }
    }
    public class Fibonaci : IComputeFunc<int, int>
    {
        /// <summary>
        /// Calculate character count of the given word.
        /// </summary>
        /// <param name="arg">Word.</param>
        /// <returns>Character count.</returns>
        /// 
        static readonly double sqrt5 = Math.Sqrt(5);
        static readonly double p1 = (1 + sqrt5) / 2;
        static readonly double p2 = -1 * (p1 - 1);
        public int Invoke(int arg)
        {

            double n1 = Math.Pow(p1, arg + 1);
            double n2 = Math.Pow(p2, arg + 1);
            int fib = (int)((n1 - n2) / sqrt5);
            Console.WriteLine("Fibonacci: {0:N}", fib);
            return fib;
        }
    }

    public class FibonaciBetween : IComputeFunc<int, int>
    {
        /// <summary>
        /// Calculate character count of the given word.
        /// </summary>
        /// <param name="arg">Word.</param>
        /// <returns>Character count.</returns>
        /// 
        static readonly double sqrt5 = Math.Sqrt(5);
        static readonly double p1 = (1 + sqrt5) / 2;
        static readonly double p2 = -1 * (p1 - 1);
        public int Invoke(int arg)
        {

            double n1 = Math.Pow(p1, arg + 1);
            double n2 = Math.Pow(p2, arg + 1);
            int fib = (int)((n1 - n2) / sqrt5);
            return fib;
        }
    }
}
