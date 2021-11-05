using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarSimulator
{
    class Bar
    {
        private const int numberOfSeats = 10;

        private readonly Semaphore semaphore = new(numberOfSeats, numberOfSeats);

        List<Visitor> visitors = new List<Visitor>(numberOfSeats);

        List<Drink> Drinks = new();

        public void EnterTheBar(Visitor visitor)
        {
            semaphore.WaitOne();

            if (CheckVisitor(visitor))
            {
                lock (visitors)
                {
                    visitors.Add(visitor);
                }
            }
        }

        public void LeaveTheBar(Visitor visitor)
        {
            lock (visitors)
            {
                visitors.Remove(visitor);
            }
        }

        public void SellDrink(Visitor visitor, Drink drink)
        {
            lock (visitors)
            {
                if (drink.Quantity > 0)
                {
                    Console.WriteLine($"{visitor.Name} buy a {drink.Quantity} glasses of {drink.DrinkName}");
                    Drinks.Find(c => c.DrinkName == drink.DrinkName).Quantity -= drink.Quantity;
                    visitor.Budget -= drink.Price;
                    Thread.Sleep(1000);
                    Console.WriteLine($"{visitor.Name} ");
                }
                else
                {
                    Console.WriteLine("Drink is out of stock!");
                }
            }
        }


        private static bool CheckVisitor(Visitor visitor)
        {
            if (visitor.Age < 18)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
