using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BarSimulator
{
    class Bar
    {
        public Bar()
        {
            timer.SetTimer(CloseBar);
        }

        public MyTimer timer = new MyTimer();

        private const int numberOfSeats = 10;

        private readonly Semaphore semaphore = new(numberOfSeats, numberOfSeats);
        private readonly Semaphore bar = new(2, 2);

        private List<Visitor> visitors = new(numberOfSeats);

        private List<Drink> Drinks = new()
        {
            new Drink("Vodka", 30, 300),
            new Drink("Whiskey", 50, 250),
            new Drink("Beer", 15, 400)
        };

        private List<Drink> DrinksSold = new()
        {
            new Drink("Vodka", 30, 0),
            new Drink("Whiskey", 50, 0),
            new Drink("Beer", 15, 0)
        };

        private int totalVisitors;

        public void EnterTheBar(Visitor visitor)
        {
            semaphore.WaitOne();

            lock (visitors)
            {
                Thread.Sleep(2000);
                visitors.Add(visitor);
                ++totalVisitors;
            }
        }

        public void LeaveTheBar(Visitor visitor)
        {
            lock (visitors)
            {
                if (visitors.Contains(visitor))
                {
                                    visitors.Remove(visitor);
                }
            }

            semaphore.Release();
        }

        public void CloseBar(object sender, ElapsedEventArgs args)
        {
            Console.WriteLine("SORRY, the bar is closing.");

            visitors.Clear();

            DisplaySalesResults();
        }

        public void SellDrink(Visitor visitor)
        {
            bar.WaitOne();
            lock (visitors)
            {
                var drink = GetRandomDrink();

                var checker = SellDrinkChecker(drink, visitor);

                Console.WriteLine($"{visitor.Name} wants to buy a {drink.Quantity} glasses of {drink.DrinkName}");

                if(checker == SellDrinkEnum.Success)
                {
                    Console.WriteLine($"{visitor.Name} buy a {drink.Quantity} glasses of {drink.DrinkName}");
                    DrinksSold.Find(c => c.DrinkName == drink.DrinkName).Quantity += drink.Quantity;
                    Drinks.Find(c => c.DrinkName == drink.DrinkName).Quantity -= drink.Quantity;
                    visitor.Budget -= drink.Price;
                    Thread.Sleep(1000);
                }
                else if (checker == SellDrinkEnum.MoneyIsNotEnough)
                {
                    Console.WriteLine($"Sorry {visitor.Name}, you don't have enough money to buy {drink.DrinkName}");
                }
                else if(checker == SellDrinkEnum.BeerOutOfStock)
                {
                    Console.WriteLine($"Sorry {visitor.Name}. {drink.DrinkName} is out of stock.");
                }
            }

            bar.Release();
        }

        public SellDrinkEnum SellDrinkChecker(Drink drink, Visitor visitor)
        {
            if (Drinks.Find(c => c.DrinkName == drink.DrinkName).Quantity < drink.Quantity)
            {
                return SellDrinkEnum.BeerOutOfStock;
            }
            else if (visitor.Budget < drink.Price * drink.Quantity)
            {
                return SellDrinkEnum.MoneyIsNotEnough;
            }
            else
            {
                return SellDrinkEnum.Success;
            }
        }


        private Drink GetRandomDrink()
        {
            Random r = new();

            int n = r.Next(0, Drinks.Count);

            var drink = Drinks[n].Clone();

            drink.Quantity = r.Next(1, 10);

            return drink;
        }

        public static void DisplayMessage(string message, VisitorStateEnum state)
        {
            lock (Console.Out)
            {
                switch (state)
                {
                    case VisitorStateEnum.IsGettingTheEnterLine:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(message);
                        Console.ResetColor();
                        break;
                    case VisitorStateEnum.EnterInBar:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(message);
                        Console.ResetColor();
                        break;
                    case VisitorStateEnum.ExitFromBar:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ResetColor();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public void DisplaySalesResults()
        {
            Console.WriteLine("------------------");
            Console.WriteLine($"Total visitors per night : {totalVisitors}");
            foreach (var item in DrinksSold)
            {
                Console.WriteLine($"Total sold {item.DrinkName}, {item.Quantity} worth {item.Quantity*item.Price}$");
                if (Drinks.Find(x => x.DrinkName == item.DrinkName).Quantity == DrinksSold.Find(x => x.DrinkName == item.DrinkName).Quantity)
                {
                    Console.Write($"{item.DrinkName} is out of stock.");
                }
            }
            Console.WriteLine($"Total amount of proceed is {DrinksSold.Sum(c => c.Price*c.Quantity)}$");
            Console.WriteLine("------------------");

            Environment.Exit(1);
        }


        public enum SellDrinkEnum
        {
            Success,
            BeerOutOfStock,
            MoneyIsNotEnough
        }

        public enum VisitorStateEnum
        {
            IsGettingTheEnterLine,
            EnterInBar,
            ExitFromBar
        }
    }
}
