using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;


namespace High_CPU_Test
{
    class Program
    {

        public static void CPUKill(object cpuUsage)
        {
            Parallel.For(0, 1, new Action<int>((int i) =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (true)
                {
                    if (watch.ElapsedMilliseconds > (int)cpuUsage)
                    {
                        Thread.Sleep(100 - (int)cpuUsage);
                        watch.Reset();
                        watch.Start();
                    }
                }
            }));

        }

        public static void CPUKillPeriod(int cpuUsage, int killPeriod, int waitPeriod)
        {
            Console.WriteLine("Burning " + cpuUsage + "% of CPU for " + killPeriod + "sec ...");
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(CPUKill));
                t.Start(cpuUsage);
                threads.Add(t);
            }
            Thread.Sleep(killPeriod * 1000);
            foreach (var t in threads)
            {
                t.Abort();
            }

            // upto machine performance to aborted all thread, normally 3-5sec will do it job.
            Console.WriteLine("wait another " + waitPeriod + "sec to make sure all Thread is aborted.");
            Thread.Sleep(waitPeriod * 1000);
        }

        static void Main(string[] args)
        {
            CPUKillPeriod(30, 5, 4);
            CPUKillPeriod(50, 5, 4);
            CPUKillPeriod(70, 5, 4);
            CPUKillPeriod(90, 5, 4);
        }
    }
}
