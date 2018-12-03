using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone
{
    public class Log
    {
        public static string _transactionType = "";
        public static double _transactionAmount = 0;
        public static double _remainingAmount = 0;

        public static void LogFeedMoney(double moneyInput, double remainingAmount)
        {
            _transactionType = "FEED MONEY";
            _transactionAmount = moneyInput;
            _remainingAmount = remainingAmount;
            LogEntry();
        }

        public static void LogItem(VendingItem item, double remainingAmount)
        {
            _transactionType = $"{item.Name} {item.ItemLocation}";
            _transactionAmount = remainingAmount + item.Price;
            _remainingAmount = _transactionAmount - item.Price;
            LogEntry();
        }

        public static void LogMakeChange(double amountOwed)
        {
            _transactionType = "GIVE CHANGE";
            _transactionAmount = amountOwed;
            _remainingAmount = 0;
            LogEntry();
        }

        private static void LogEntry()
        {
            string fileName = "Log.txt";
            string filePath = Environment.CurrentDirectory;

            string fullPath = Path.Combine(filePath, fileName);

            using (StreamWriter log = new StreamWriter(fileName, true))
            {
                log.WriteLine($"{DateTime.Now.ToString().PadRight(30, ' ')}  {_transactionType.PadRight(25, ' ')}  " +
                              $"{_transactionAmount.ToString("C").PadRight(10, ' ')} {_remainingAmount.ToString("C")}");
            }
        }
    }
}

