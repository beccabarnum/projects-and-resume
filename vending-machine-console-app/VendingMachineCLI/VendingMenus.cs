using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Capstone;

namespace Capstone
{
    /// <summary>
    /// 
    /// </summary>
    public class VendingMenus
    {
        /// <summary>
        /// 
        /// </summary>
        private VendingMachine _vm = null;

        /// <summary>
        /// 
        /// </summary>
        double _totalMoneyAdded = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        public VendingMenus (VendingMachine vm)
        {
            _vm = vm;
        }

        /// <summary>
        /// 
        /// </summary>
        public void MainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.Clear();

                    Console.WriteLine();
                    Console.WriteLine("Please select an option below:");
                    Console.WriteLine("(1) Display Vending Machine Items");
                    Console.WriteLine("(2) Purchase");
                    Console.WriteLine("(3) Exit");
                    char input = Console.ReadKey().KeyChar;

                    if (input == '3')
                    {
                        exit = true; //leaves menu on entering 3
                    }

                    else if (input == '1')
                    {
                        Console.Clear();
                        Console.WriteLine("Location".PadRight(15, ' ') + "Name".PadRight(20, ' ') + 
                                          "Price".PadRight(20, ' ') + "Quantity".PadRight(20, ' '));
                        List<VendingItem> vendingList = _vm.GetInventory();

                        foreach (VendingItem item in vendingList)
                        {
                            Console.WriteLine($"{item.ItemLocation.PadRight(15, ' ')} {item.Name.PadRight(20, ' ')} " +
                                              $"{item.Price.ToString("C").PadRight(20, ' ')} {item.DisplayQty.PadRight(20, ' ')}");
                        }
                        Console.WriteLine("\nPress any key to return to main menu");
                        Console.ReadKey();
                    }
                    else if (input == '2')
                    {
                        PurchaseMenu();
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                        Console.ReadKey();
                    }                       
                }

                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();                    
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PurchaseMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Please select an option below:");
                    Console.WriteLine("(1) Feed Money");
                    Console.WriteLine("(2) Select Product");
                    Console.WriteLine("(3) Display Purchased Items");
                    Console.WriteLine("(4) Finish Transaction");
                    Console.WriteLine("(5) Return to Main Menu");
                    Console.WriteLine();
                    Console.WriteLine($"Current Money Available: {_vm.TotalMoney.ToString("C")}");

                char input = Console.ReadKey().KeyChar;

                string _itemSelection = "";

                if (input == '1')
                {
                    Console.Clear();
                    FeedMoneyMenu();
                }
                else if (input == '2')
                {
                    bool itemExit = false;
                    while (!itemExit)
                    {
                        Console.Clear();
                        Console.WriteLine("Location".PadRight(15, ' ') + "Name".PadRight(20, ' ') +
                                            "Price".PadRight(20, ' ') + "Quantity".PadRight(20, ' '));
                        List<VendingItem> vendingList = _vm.GetInventory();

                        foreach (VendingItem item in vendingList)
                        {
                            Console.WriteLine($"{item.ItemLocation.PadRight(15, ' ')} {item.Name.PadRight(20, ' ')} " +
                                                $"{item.Price.ToString("C").PadRight(20, ' ')} {item.DisplayQty.PadRight(20, ' ')}");
                        }
                        Console.WriteLine();
                        Console.WriteLine($"Your money available to spend is: {_vm.TotalMoney.ToString("C")}");
                        Console.Write("Please enter the item location (or press 'q' to quit): ");

                        _itemSelection = Console.ReadLine().ToUpper();
                        if (_itemSelection == "Q")
                        {
                            itemExit = true;
                            break;
                        }

                        try
                        {
                            Console.Clear();
                            _vm.PurchaseItem(_itemSelection);
                            List<VendingItem> vendingItems = _vm.DispenseItems(_itemSelection);
                            Console.WriteLine();
                            Console.WriteLine("Congratulations, you successfully purchased the selected item.");
                            Console.WriteLine();
                            Console.WriteLine("Item Purchased: ".PadRight(20, ' ') + "Item Price: ".PadRight(20, ' '));
                            Console.WriteLine(vendingItems[0].Name.PadRight(20, ' ') + vendingItems[0].Price.ToString("C").PadRight(20, ' '));
                            Console.WriteLine();
                            Console.WriteLine("Press any key to return to the Item Selection Menu.");

                            Log.LogItem(_vm.Inventory[_itemSelection], _vm.TotalMoney);
                        }
                        catch (InvalidSelectionException)
                        {
                            Console.WriteLine("Invalid Selection - hit any key to return to Purchase Menu.");
                        }
                        catch (SoldOutException)
                        {
                            Console.WriteLine("Item is sold out, please select another item.");
                        }
                        catch (InsufficientFundsException)
                        {
                            Console.WriteLine("Insufficient funds - please select a different item or enter more " +
                                              "money.\nPress any key to continue.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                        }
                        Console.ReadKey();
                    }   
                }
                    //If product does not exist, tell customer and return to purchase menu
                    //If sold out, tell customer, return to purchase menu
                    //if valid, dispense product
                    //after dispensed, update balance and return to Purchase menu

                else if (input == '3')
                {
                    Console.Clear();
                  
                    Console.WriteLine("Item Purchased: ".PadRight(20, ' ') + "Item Price: ".PadRight(20, ' '));
                    Console.WriteLine("-".PadRight(30, '-'));
                    Console.WriteLine();

                    foreach (VendingItem item in _vm._itemsDispensed)
                    {
                        Console.WriteLine(item.Name.PadRight(20, ' ') + item.Price.ToString("C").PadRight(20, ' '));
                    }
                    Console.WriteLine("-".PadRight(30, '-'));
                    Console.WriteLine($"Total Cost: {_vm.RunningTotal.ToString("C").PadLeft(13, ' ')}");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to the Purchase Menu.");
                    Console.ReadKey();
                }
                else if (input == '4')
                {
                    Console.Clear();

                    Change change = new Change(_totalMoneyAdded, _vm.RunningTotal);
                    _vm.ResetTotalMoney();
                    Console.WriteLine($"The change you are owed is: {(change.AmountOwed / 100).ToString("C")}.  " +
                                      $"\nYou will receive {change.NumOfQuarters} quarters, {change.NumOfDimes} dimes, " +
                                      $" and {change.NumOfNickels} nickels.\nYour balance is: {_vm.TotalMoney.ToString("C")}"); 

                    Log.LogMakeChange(_totalMoneyAdded - _vm.RunningTotal);
                    _totalMoneyAdded = 0;

                    Console.WriteLine();
                    _vm._itemsDispensed.Clear();
                    _vm.ResetRunningTotal();
                    Console.WriteLine("Ready to eat? Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    foreach (VendingItem item in _vm._itemsPurchased)
                    {
                        Console.WriteLine($"You are eating {item.Name} {item.MakeSound()}");
                        Console.WriteLine();
                    }
                    Console.WriteLine("Press any key to return to the Purchase Menu.");
                    Console.ReadKey();
                    Report.GenerateSalesReport(_vm.Inventory);
                }
                else if (input == '5')
                {
                    exit = true;
                }
            }
        }

        public void FeedMoneyMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("Please select an amount to enter:");
                Console.WriteLine("(1) $1.00");
                Console.WriteLine("(2) $2.00");
                Console.WriteLine("(3) $5.00");
                Console.WriteLine("(4) $10.00");
                Console.WriteLine("(5) $20.00");
                Console.WriteLine("(6) Finish entering money and go to Purchase Menu");
                Console.WriteLine();
                Console.WriteLine($"Current Money Available: {_vm.TotalMoney.ToString("C")}");
                
                char input = Console.ReadKey().KeyChar;
                if(input == '6')
                {
                    exit = true;
                }
                else
                {
                    try //tests to see if input is a option
                    {
                        Console.Clear();
                        if (input == '1')
                        {
                            _totalMoneyAdded = _vm.AddMoney(1.00D);                          
                        }
                        else if (input == '2')
                        {
                            _totalMoneyAdded = _vm.AddMoney(2.00D);
                        }
                        else if (input == '3')
                        {
                            _totalMoneyAdded = _vm.AddMoney(5.00D);
                        }
                        else if (input == '4')
                        {
                            _totalMoneyAdded = _vm.AddMoney(10.00D);
                        }
                        else if (input == '5')
                        {
                            _totalMoneyAdded = _vm.AddMoney(20.00D);
                        }
                        Log.LogFeedMoney(_vm.MoneyInput, _vm.TotalMoney);                        
                    }
                   
                    catch
                    {
                        Console.WriteLine("Invalid menu selection.");
                    }
                }
            }           
        }        
    }
}
