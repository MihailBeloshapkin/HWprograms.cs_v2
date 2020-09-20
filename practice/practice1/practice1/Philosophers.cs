using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace practice1
{
    public class Philosophers
    {
        public Philosophers(List<Object> forks, int num)
        {
            this.forks = forks;
            this.num = num;
        }

        private const int MaxTime = 10;

        private readonly int num;
        private readonly List<Object> forks;
        private static readonly Random Random = new Random();
        private volatile bool isDead;

        public void Live()
        {
            while(!isDead)
            {
                var time = Random.Next(MaxTime);
                Console.WriteLine($"Philosopher {num} is thinking for {time} milliseconds.");
                Thread.Sleep(time);

                var firstFork = num == forks.Count - 1 ? 0 : num;
                var secondFork = num == forks.Count - 1 ? num : num + 1;

                time = Random.Next(MaxTime);
                Console.WriteLine($"Philosopher {num} is trying to take {firstFork} fork.");
                lock (forks[firstFork])
                {
                    Console.WriteLine($"Philosopher {num} is trying to take {secondFork} fork.");
                    lock (forks[secondFork])
                    {
                        Console.WriteLine($"Philosopher {num} is eating for {time} milliseconds.");
                        Thread.Sleep(time);
                    }
                }
            }
        }

        public void Kill()
        {
            this.isDead = false;
        }

    }
}
