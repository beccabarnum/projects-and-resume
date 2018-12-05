using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Web.DAL;
using Capstone.Web.Models;

namespace Capstone.Web.Tests
{
    [TestClass()]
    public class ParkServiceSqlDALTests
    {
        private TransactionScope _tran;
        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = npgeek; Integrated Security = True";
        private int _numberOfParks;
        private string parkID;
        private int surveyCount;

        [TestInitialize]
        public void Initialize()
        {
            _tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd;

                conn.Open();
                
                cmd = new SqlCommand("Select Count(*) From park;", conn);
                _numberOfParks = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("select TOP(1) parkCode from park", conn);
                parkID = (string)cmd.ExecuteScalar();

                cmd = new SqlCommand("Select Count(*) From survey_result", conn);
                surveyCount = (int)cmd.ExecuteScalar();

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
        }

        [TestMethod()]
        public void GetAllParksTest()
        {
            ParkServiceSqlDAL test = new ParkServiceSqlDAL(_connectionString);
            List<Park> parkList = test.GetAllParks();
            Assert.AreEqual(_numberOfParks, parkList.Count);//retrieved the number of rows beforehand to compare to the count in the returned list
                      
        }

        [TestMethod()]
        public void GetForecastByParkCodeTest()
        {
            ParkServiceSqlDAL test = new ParkServiceSqlDAL(_connectionString);
            List<Park> parkList = test.GetAllParks();
            Park firstPark = parkList[0];
            string parkID = firstPark.ParkCode;
            List<Weather> forecast = test.GetForecastByParkCode(parkID);
            Assert.IsNotNull(forecast);
        }

        [TestMethod()]
        public void GetSingleParkTest()
        {
            ParkServiceSqlDAL test = new ParkServiceSqlDAL(_connectionString);
            Park testPark = test.GetSinglePark(parkID);
            Assert.IsNotNull(testPark);
        }

        [TestMethod()]
        public void GetAllSurveysTest()
        {
            ParkServiceSqlDAL test = new ParkServiceSqlDAL(_connectionString);
            List<SurveyResultModel> testSurveys = test.GetAllSurveys();
            Assert.IsNotNull(testSurveys);
        }

        [TestMethod()]
        public void SaveNewSurveyTest()
        {
            ParkServiceSqlDAL test = new ParkServiceSqlDAL(_connectionString);
            Survey survey = new Survey();
            survey.ActivityLevel = "active";
            survey.Email = "test@email.com";
            survey.ParkCode = parkID;
            survey.State = "OH";
            bool worked = test.SaveNewSurvey(survey);

            Assert.IsTrue(worked);
        }

    }
}
