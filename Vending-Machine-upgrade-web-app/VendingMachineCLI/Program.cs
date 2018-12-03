using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.File;
using VendingService.Helpers;
using VendingService.Interfaces;
using VendingService.Mock;
using VendingService.Models;

namespace VendingMachineCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //IVendingService db = new VendingDBService();
            IVendingService db = new MockVendingDBService();

            var items = db.GetVendingItems();

            InventoryManager inventory = new InventoryManager(items);
            for (int rowIdx = 1; rowIdx <= inventory.RowCount; rowIdx++)
            {
                for (int colIdx = 1; colIdx <= inventory.ColCount; colIdx++)
                {
                    Console.WriteLine(inventory.GetVendingItem(rowIdx, colIdx).ToString());
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            ReportManager reportManager = new ReportManager(db);
            Report report = reportManager.GetReport(2018, db.GetProductItems());
            var reportItems = report.ReportItems;
            foreach (var item in reportItems)
            {
                Console.WriteLine($"{item.Name}|{item.Qty}");
            }
            Console.WriteLine();
            Console.WriteLine($"**Total Sales** {report.TotalSales.ToString("c")}");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            ILogService log = new LogFileService();
            Change change = TestManager.PopulateLogFileWithOperations(db, log);
            Console.WriteLine(log.GetLogData());

            Console.WriteLine();
            Console.WriteLine();

            TransactionManager trans = new TransactionManager(db, log);
            Console.WriteLine($"Change: {change.Dollars} Dollars {change.Quarters} Quarters {change.Dimes} Dimes {change.Nickels} Nickels {change.Pennies} Pennies");

            Console.ReadKey();
        }
    }
}
