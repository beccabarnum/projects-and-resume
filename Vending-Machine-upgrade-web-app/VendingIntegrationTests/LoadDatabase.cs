using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingService.Database;
using VendingService.Helpers;
using VendingService.Interfaces;
using VendingService.Mock;
using VendingService.Models;

namespace VendingIntegrationTests
{
    [TestClass]
    public class LoadDatabase
    {
        //[TestMethod]
        public void PopulateDatabase()
        {
            IVendingService db = new VendingDBService();
            //IVendingService db = new MockVendingDBService();

            TestManager.PopulateDatabaseWithInventory(db);
            TestManager.PopulateDatabaseWithTransactions(db);
        }
    }
}
