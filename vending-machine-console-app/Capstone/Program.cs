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
            //three lines of code in program.cs:

            VendingMachine vm = new VendingMachine();
            VendingMachineCLI cli = new VendingMachineCLI(vm); //pass in constructor
            cli.MainMenu(); //method to open main menu

        }
    }
}
