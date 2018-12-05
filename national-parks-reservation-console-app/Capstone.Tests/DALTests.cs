using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;


namespace Capstone
{
    [TestClass]
    public class DALTests
    {
        private TransactionScope _tran;
        string _databaseConnection = @"Data Source =.\SQLEXPRESS;Initial Catalog = ParksDB; Integrated Security = True";
        

        [TestInitialize]
        public void Initialize()
        {
            _tran = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
        }

        [TestMethod]
        public void DALGetAllParksTests()
        {
            DAL testDAL = new DAL(_databaseConnection);

            List<Park> testList = testDAL.GetAllParks();

            CollectionAssert.AllItemsAreNotNull(testList, "Message");
            Assert.AreNotEqual(0, testList.Count);
            Assert.AreEqual("Acadia", testList[0].Name);
            //Assert.AreEqual(3, testList.Count);
        }

        [TestMethod]
        public void DALGetCampgroundsByParkID()
        {
            DAL testDal = new DAL(_databaseConnection);
            
            List<Campground> testList = testDal.GetCampGroundsByParkID(1);

            Assert.AreNotEqual(0, testList.Count);
            Assert.AreEqual("Blackwoods", testList[0].Name);

        }
    }
}
