using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using System.Data.SqlClient;

namespace Capstone.Web.DAL
{
    /// <summary>
    /// Reads information in from Database and assigns them to class
    /// </summary>
    public class ParkServiceSqlDAL :IParkServiceDAL
    {
        /// <summary>
        /// Member variable to store connection string for future access
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// passes connection string into constructor for ParkServiceSqlDal
        /// </summary>
        /// <param name="connectionString"></param>
        public ParkServiceSqlDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Home/detail DALs

        /// <summary>
        /// Gathers a list of All Park Objects from DAL
        /// </summary>
        /// <returns></returns>
        public List<Park> GetAllParks()
        {
            List<Park> parkList = new List<Park>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "select * from park";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //ASK CHRIS do you recommend we only read in properties that we use? or read in all for potential use later?
                    Park parkModel = new Park();
                    parkModel.ParkCode = reader["parkCode"].ToString();
                    parkModel.ParkName = reader["parkName"].ToString();
                    parkModel.State = reader["state"].ToString();
                    parkModel.Acreage = (int)reader["acreage"];
                    parkModel.ElevationInFeet = (int)reader["elevationInFeet"];
                    parkModel.NumberOfCampsites = (int)reader["numberOfCampsites"];
                    parkModel.ClimateType = reader["climate"].ToString();
                    parkModel.YearFounded = (int)reader["yearFounded"];
                    parkModel.AnnualVisitorCount = (int)reader["annualVisitorCount"];
                    parkModel.InspirationalQuote = reader["inspirationalQuote"].ToString();
                    parkModel.QuoteSource = reader["inspirationalQuoteSource"].ToString();
                    parkModel.ParkDescription = reader["parkDescription"].ToString();
                    parkModel.EntryFee = (int)reader["entryFee"];
                    parkModel.NumberOfSpecies = (int)reader["numberOfAnimalSpecies"];
                    parkModel.MilesofTrail = (float)reader["milesOfTrail"];

                    parkList.Add(parkModel);
                }
            }
            return parkList;
        }

        #endregion

        #region Weather DALS
        /// <summary>
        /// Returns a List of Weather Objects for the specific parkCode passed in
        /// </summary>
        /// <param name="parkCode"></param>
        /// <returns></returns>
        public List<Weather> GetForecastByParkCode(string parkCode)
        {
            List<Weather> fiveDayForecast = new List<Weather>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "Select * from weather " +
                             "Where parkCode = @parkCode";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Weather weatherModel = new Weather();

                    weatherModel.ParkCode = reader["parkCode"].ToString();
                    weatherModel.FiveDayValue = Convert.ToInt32(reader["fiveDayForecastValue"]);
                    weatherModel.Low = Convert.ToInt32(reader["low"]);
                    weatherModel.High = (int)reader["high"];
                    weatherModel.Forecast = reader["forecast"].ToString();
                    weatherModel.IsFahrenheit = true;
                    fiveDayForecast.Add( weatherModel);
                }
            }
            return fiveDayForecast;
        }
        #endregion

        #region Detail
        /// <summary>
        /// Returns a Single park based on the park id string passed in
        /// </summary>
        /// <param name="parkid"></param>
        /// <returns></returns>
        public Park GetSinglePark(string parkid)
        {
            Park parkModel = new Park();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                string sql = "select * from park where parkCode = @parkid";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkid", parkid);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    
                    parkModel.ParkCode = reader["parkCode"].ToString();
                    parkModel.ParkName = reader["parkName"].ToString();
                    parkModel.State = reader["state"].ToString();
                    parkModel.Acreage = (int)reader["acreage"];
                    parkModel.ElevationInFeet = (int)reader["elevationInFeet"];
                    parkModel.NumberOfCampsites = (int)reader["numberOfCampsites"];
                    parkModel.ClimateType = reader["climate"].ToString();
                    parkModel.YearFounded = (int)reader["yearFounded"];
                    parkModel.AnnualVisitorCount = (int)reader["annualVisitorCount"];
                    parkModel.InspirationalQuote = reader["inspirationalQuote"].ToString();
                    parkModel.QuoteSource = reader["inspirationalQuoteSource"].ToString();
                    parkModel.ParkDescription = reader["parkDescription"].ToString();
                    parkModel.EntryFee = (int)reader["entryFee"];
                    parkModel.NumberOfSpecies = (int)reader["numberOfAnimalSpecies"];
                    parkModel.MilesofTrail = (float)reader["milesOfTrail"];
                    
                }
            }

            return parkModel;

        }
        #endregion

        #region Survey DALs
        /// <summary>
        /// Returns all survey objects stored in database
        /// </summary>
        /// <returns></returns>
        public List<SurveyResultModel> GetAllSurveys()
        {
            List<SurveyResultModel> surveyResults = new List<SurveyResultModel>();

            string query = @"SELECT park.parkDescription, park.parkName, park.parkCode, COUNT(surveyId) as 'surveycount' " +
                                        "FROM survey_result "+
                                        "join park on park.parkCode = survey_result.parkCode " +
                                        "GROUP BY park.parkCode, park.parkName, park.parkDescription " +
                                        "Order BY surveycount Desc, park.parkName ASC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SurveyResultModel favcount = new SurveyResultModel();
                    favcount.ParkName = Convert.ToString(reader["parkName"]);
                    favcount.ParkCode = Convert.ToString(reader["parkCode"]);
                    favcount.SurveyCount = Convert.ToInt32(reader["surveycount"]);
                    favcount.ParkDesc = Convert.ToString(reader["parkDescription"]);
                    surveyResults.Add(favcount);
                }
            }

            return surveyResults;
        }
        /// <summary>
        /// Adds new survey to the database
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public bool SaveNewSurvey(Survey survey)
        {
            bool result = false;

            string query = @"INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) VALUES (@ParkCode, @Email, @State, @ActivityLevel)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ParkCode", survey.ParkCode);
                cmd.Parameters.AddWithValue("@Email", survey.Email);
                cmd.Parameters.AddWithValue("@State", survey.State);
                cmd.Parameters.AddWithValue("@ActivityLevel", survey.ActivityLevel);
                int numberOfRowsAffected = cmd.ExecuteNonQuery();
                if (numberOfRowsAffected > 0)
                {
                    result = true;
                }
            }

            return result;
        }
        #endregion
    }
}