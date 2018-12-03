using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Candy : VendingItem
    {
        public Candy(string itemLocation, string name, double price) : base(itemLocation, name, price)
        {
            
        }

        public override string MakeSound()
        {
            return "Munch Munch, Yum!";
        }
    }
}
