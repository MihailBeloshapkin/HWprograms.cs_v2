using System;
using System.Collections.Generic;
using System.Threading;

namespace practice1
{
    class Program
    {
        private const int PhilosophersCount = 2;

        static void Main(string[] args)
        {
            var philosophers = new List<Philosophers>(PhilosophersCount);
            var forks = new List<object>(PhilosophersCount);

            for (var i = 0; i < PhilosophersCount; i++)
            {
                philosophers.Add(new Philosophers(forks, i));
                forks.Add(new object());
            }

            var threads = new Thread[PhilosophersCount];
            for (var i = 0; i < PhilosophersCount; i++)
            {
                var localI = i;
                threads[i] = new Thread(() => philosophers[localI].Live());
                threads[i].Start();
            }

            Console.ReadLine();
            foreach (var philosopher in philosophers)
            {
                philosopher.Kill();
            }

            for (var i = 0; i < PhilosophersCount; i++)
            {
                threads[i].Join();
            }

        }
    }
}
