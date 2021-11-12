using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BarSimulator
{
    class Visitor
    {
        public Visitor(string name, short age, decimal budget, Bar bar)
        {
            Name = name;
            Age = age;
            Budget = budget;
            Bar = bar;
        }

        public string Name { get; set; }

        public short Age { get; set; }

        public decimal Budget { get; set; }

        public bool GoHome = false;

        public Bar Bar { get; set; }

        enum NightLifeActivity { VisitBar, GoHome, Walk};

        enum BarActivity { Drink, Dance, Leave};

        private static NightLifeActivity GetRandomNightLifeActivity()
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

        private static BarActivity GetRandomBarActivity()
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
            Bar.DisplayMessage($"{Name} is getting in the line to enter the bar.", Bar.VisitorStateEnum.IsGettingTheEnterLine);
            Bar.EnterTheBar(this);
            Bar.DisplayMessage($"{Name} is entered the bar", Bar.VisitorStateEnum.EnterInBar);
            bool staysAtBar = true;
            while (staysAtBar)
            {
                var nextActivity = GetRandomBarActivity();
                switch (nextActivity)
                {
                    case BarActivity.Dance:
                        Console.WriteLine($"{Name} started dancing.");
                        Thread.Sleep(1000);
                        break;
                    case BarActivity.Drink:
                        Bar.SellDrink(this);
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

            //bool staysOut = true;

            while (!GoHome)
            {
                var nextActivity = GetRandomNightLifeActivity();

                switch (nextActivity)
                {
                    case NightLifeActivity.VisitBar:
                        if (this.Age < 18)
                        {
                            Console.WriteLine($"Sorry {this.Name}, your age is under 18, you can't enter the bar");
                            GoHome = true;
                        }
                        else
                        {
                            VisitBar();
                            GoHome = false;
                        }
                        break;
                    case NightLifeActivity.GoHome:
                        GoHome = true;
                        break;
                    case NightLifeActivity.Walk:
                        WalkOut();
                        break;
                    default:
                        break;
                }
            }

            Bar.DisplayMessage($"{Name}, is going back home", Bar.VisitorStateEnum.ExitFromBar);
        }

        public void GoToHome(object source, ElapsedEventArgs args)
        {
            GoHome = true;
        }

        public static int GetRandomNumber(int maxNum)
        {
            Random r = new();

            return r.Next(1, maxNum);
        }

        public static short GetRandomNumber(short maxNum)
        {
            Random r = new();

            return (short)r.Next(1, maxNum);
        }
    }
}
