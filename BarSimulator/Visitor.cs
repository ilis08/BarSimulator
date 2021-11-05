using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarSimulator
{
    class Visitor
    {
        public string Name { get; set; }

        public short Age { get; set; }

        public decimal Budget { get; set; }

        public Bar Bar { get; set; }

        enum NightLifeActivity { VisitBar, GoHome, Walk};

        enum BarActivity { Drink, Dance, Leave};

        private NightLifeActivity GetRandomNightLifeActivity()
        {
            int n = GetRandomNumber(101);

            if (n < 20)
            {
                return NightLifeActivity.Walk;
            }
            else if (n < 90)
            {
                return NightLifeActivity.VisitBar;
            }
            else
            {
                return NightLifeActivity.GoHome;
            }
        }

        private BarActivity GetRandomBarActivity()
        {
            int n = GetRandomNumber(101);

            if (n < 40)
            {
                return BarActivity.Dance;
            }
            else if (n < 90)
            {
                return BarActivity.Drink;
            }
            else
            {
                return BarActivity.Leave;
            }
        }

        private void WalkOut()
        {
            Console.WriteLine($"{Name} is walking in the streets.");
        }

        private void VisitBar()
        {
            Console.WriteLine($"{Name} is getting in the line to enter the bar.");
            Bar.EnterTheBar(this);
            Console.WriteLine($"{Name} entered the bar!");
            bool staysAtBar = true;
            while (staysAtBar)
            {
                var nextActivity = GetRandomBarActivity();
                switch (nextActivity)
                {
                    case BarActivity.Dance:
                        Console.WriteLine($"{Name} is dancing.");
                        Thread.Sleep(1000);
                        break;
                    case BarActivity.Drink:
                        Console.WriteLine($"{Name} is drinking.");
                        Thread.Sleep(100);
                        break;
                    case BarActivity.Leave:
                        Console.WriteLine($"{Name} is leaving the bar.");
                        Bar.LeaveTheBar(this);
                        staysAtBar = false;
                        break;
                    default: throw new NotImplementedException();
                }
            }
        }

        public void StartTheNightLife()
        {
            WalkOut();

            bool staysOut = true;

            while (staysOut)
            {
                var nextActivity = GetRandomNightLifeActivity();

                switch (nextActivity)
                {
                    case NightLifeActivity.VisitBar:
                        VisitBar();
                        staysOut = false;
                        break;
                    case NightLifeActivity.GoHome:
                        staysOut = false;
                        break;
                    case NightLifeActivity.Walk:
                        WalkOut();
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"{Name} is going back home.");
        }

        private static int GetRandomNumber(int maxNum)
        {
            Random r = new();

            return r.Next(1, maxNum);
        }
    }
}
