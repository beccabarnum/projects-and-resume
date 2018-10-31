using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public abstract class VendingItem
    {
        public string ItemLocation { get; set; } 
        public string Name { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; } = 5;
        public int AmountSold
        {
            get
            {
                return 5 - Qty;
            }
        }
        public bool IsAvailable
        {
            get
            {
                return Qty > 0;
            }
        }
        public string DisplayQty
        {
            get
            {
                if (Qty > 0)
                {
                    return Qty.ToString();
                }
                else
                {
                    return "SOLD OUT";
                }
            }
        }

        public abstract string MakeSound();

        public VendingItem(string itemLocation, string name, double price)
        {
            ItemLocation = itemLocation;
            Name = name;
            Price = price;
        }
    }
}
