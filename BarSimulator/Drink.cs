using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarSimulator
{
    class Drink
    {
        public Drink(string name, decimal price, int quantity)
        {
            DrinkName = name;
            Price = price;
            Quantity = quantity;
        }

        public string DrinkName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Drink Clone()
        {
            return (Drink)this.MemberwiseClone();
        }
    }
}
