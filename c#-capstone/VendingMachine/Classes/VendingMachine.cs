using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone
{
    public class VendingMachine
    {
        private Dictionary<string, VendingItem> _inventory = new Dictionary<string, VendingItem>();
        private double _totalMoney = 0;
        private double _runningTotal = 0;
        public List<VendingItem> _itemsDispensed = new List<VendingItem>();
        public List<VendingItem> _itemsPurchased = new List<VendingItem>();

        public double MoneyInput { get; set; }
        public Dictionary<string, VendingItem> Inventory
        {
            get { return _inventory; }
        }
        public double TotalMoney
        {
            get { return _totalMoney; }
        }
        public double RunningTotal
        {
            get { return _runningTotal; }

        }

        public VendingMachine()
        {
            try
            {
                ReadFile();
            }
            catch
            {
                throw new CorruptCSVException();
            }
        }

        public List<VendingItem> GetInventory()
        {
            return _inventory.Values.ToList();
        }

        private void ReadFile()
        {
            string fileName = "vendingmachine.csv";
            string fileDirectory = Environment.CurrentDirectory;
            //string fullPath = @"C:\Workspace\team\week-4-pair-exercises-team-3\c#-capstone\etc\vendingmachine.csv";
            string fullPath = Path.Combine(fileDirectory, fileName);
            using (StreamReader file = new StreamReader(fileName))
            {
                List<string> itemList = new List<string>();

                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    //List<string> lineList = new List<string>();

                    //lineList.AddRange(line.Split('|'));
                    var lineList = line.Split('|');

                    if (lineList[3] == "Chip")
                    {
                        Chip chip = new Chip(lineList[0], lineList[1], double.Parse(lineList[2]));
                        _inventory.Add(lineList[0], chip);
                    }
                    else if (lineList[3] == "Candy")
                    {
                        Candy candy = new Candy(lineList[0], lineList[1], double.Parse(lineList[2]));
                        _inventory.Add(lineList[0], candy);
                    }
                    else if (lineList[3] == "Drink")
                    {
                        Drink drink = new Drink(lineList[0], lineList[1], double.Parse(lineList[2]));
                        _inventory.Add(lineList[0], drink);
                    }
                    else if (lineList[3] == "Gum")
                    {
                        Gum gum = new Gum(lineList[0], lineList[1], double.Parse(lineList[2]));
                        _inventory.Add(lineList[0], gum);
                    }
                }
            }
        }

        public double AddMoney(double moneyInput)
        {
            MoneyInput = moneyInput;
            _totalMoney += moneyInput;

            return _totalMoney;
        }

        public void PurchaseItem(string itemRequest)
        {
            if(!_inventory.ContainsKey(itemRequest))
            {
                throw new InvalidSelectionException();
            }
            var item = _inventory[itemRequest];

            if(!item.IsAvailable)
            {
                throw new SoldOutException();
            }
            if (_totalMoney >= item.Price)
            {
                item.Qty -= 1;
                _totalMoney -= item.Price;
                _runningTotal += item.Price;
            }
            else
            {
                throw new InsufficientFundsException();
            }
        }

        public List<VendingItem> DispenseItems(string itemRequest)
        {
            List<VendingItem> dispensedItem = new List<VendingItem>();
            foreach (VendingItem item in _inventory.Values)
            {
                if (item.ItemLocation == itemRequest)
                {
                    dispensedItem.Add(item);
                    _itemsDispensed.Add(item);
                    _itemsPurchased.Add(item);
                }
            }
            return dispensedItem;
        }

        public void ResetTotalMoney()
        {
            _totalMoney = 0;
        }

        public void ResetRunningTotal()
        {
            _runningTotal = 0;
        }
        //public string ItemName { get; set; } 
        //public double ItemPrice { get; set; } 

        //public void DispenseItems(string itemRequest)
        //{
        //    string dispensedItem = "";

        //    foreach (VendingItem item in _inventory.Values)
        //    {

        //        if (item.ItemLocation == itemRequest)
        //        {
        //            ItemName = item.Name;
        //            ItemPrice = item.Price;
        //            //add each dispensed item to a list
        //            dispensedItem = ItemName + ItemPrice.ToString("C");
        //            _itemsDispensed.Add(dispensedItem);
        //            _itemsPurchased.Add(item);
        //        }

        //    }

        //}
    }
}
