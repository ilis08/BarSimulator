using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace BarSimulator
{

    class Program
    {
        static volatile bool stopParty;

        static void Main(string[] args)
        {
            stopParty = false;

            CsvData names = new();

            names.LoadString("Names");

            Bar bar = new();

            List<Thread> studentThreads = new();
            List<Visitor> visitors = new();

            for (int i = 0; i < 100; i++)
            {
                var visitor = new Visitor(names.data[i], (short)Visitor.GetRandomNumber(80), Visitor.GetRandomNumber(1000), bar);
                bar.timer.ActionEvent += visitor.GoToHome;
                visitors.Add(visitor);
                var thread = new Thread(visitor.StartTheNightLife);
                thread.Start();
                studentThreads.Add(thread);
            }

            foreach (var item in studentThreads)
            {
                item.Join();
            }
           

            Console.WriteLine("------------------");
            Console.WriteLine("The party is over.");
            Console.WriteLine("------------------");

            //bar.DisplaySalesResults();
        }
    }
}
