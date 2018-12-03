using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                VendingMenus menu = new VendingMenus(new VendingMachine());
                menu.MainMenu();
            }
            catch (CorruptCSVException ex)
            {
                Console.Clear();
                Console.WriteLine("CSV file is corrupt.  Please fix and try again.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);              
                Console.ReadKey();
            }
        }
    }
}
