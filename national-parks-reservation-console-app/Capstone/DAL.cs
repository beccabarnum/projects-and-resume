using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Capstone
{
    public class DAL
    {
        private string _connectionString;

        public DAL(string connecitonString)
        {
            _connectionString = connecitonString;
        }

        public List<Park> GetAllParks()
        {
            List<Park> allParks = new List<Park>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();

                string SQL_ParksQuery = "select park_id, " +
                                                "name, " +
                                                "location, " +
                                                "establish_date, " +
                                                "area, " +
                                                "visitors, " +
                                                "description " +
                                        "from park " +
                                        "order by name";

                cmd.CommandText = SQL_ParksQuery;
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var park = new Park();
                    park.ID = (int)reader["park_id"];
                    park.Name = (string)reader["name"];
                    park.Location = (string)reader["location"];
                    park.EstablishDate = (DateTime)reader["establish_date"];
                    park.Area = (int)reader["area"];
                    park.AnnualVisitors = (int)reader["visitors"];
                    park.Description = (string)reader["description"];
                    allParks.Add(park);
                }
            }
            return allParks;
        }

        public List<Campground> GetCampGroundsByParkID(int parkID)
        {
            List<Campground> campgrounds = new List<Campground>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();

                string SQL_CampgroundQuery = "Select campground_id, " +
                                                    "park_id, " +
                                                    "name, " +
                                                    "open_from_mm, " +
                                                    "open_to_mm, " +
                                                    "daily_fee " +
                                            "from campground " +
                                            "where park_id = " + parkID.ToString() +
                                            " Order By name";

                cmd.CommandText = SQL_CampgroundQuery;
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var cg = new Campground();
                    cg.CampgroundID = (int)reader["campground_id"];
                    cg.ParkID = (int)reader["park_id"];
                    cg.Name = (string)reader["name"];
                    cg.OpenFromMonth = (int)reader["open_from_mm"];
                    cg.OpenToMonth = (int)reader["open_to_mm"];
                    cg.DailyFee = (decimal)reader["daily_fee"];
                    campgrounds.Add(cg);
                }

            }
            return campgrounds;
        }

        

        public List<Site> GetSitesByDates(int cgID, DateTime inputFrom, DateTime inputTo)
        {

            List<Site> siteList = new List<Site>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();

                //Reservation end/start
                //Tentative reservation end/start

                //tres start can't be after res start and before res end
                //tresstart.compare to res start must be -1 
                //tresstart.compare to res end must be 1

                //tres end can't be after res start
                //tresend.compare to res start must be -1

                string SQL_SiteQuery =  "select top(5) site_id, " +
                                                "campground_id, " +
                                                "site_number, " +
                                                "max_occupancy, " +
                                                "accessible, " +
                                                "max_rv_length, " +
                                                "utilities " +
                                        "from site " +
                                        "where campground_id = @cgid and " +
                                              "DATEPART(m, @inputfrom) between (select open_from_mm from campground where campground_id = @cgid) and (select open_to_mm from campground where campground_id = @cgid) and " +
                                              "DATEPART(m, @inputto) between (select open_from_mm from campground where campground_id = @cgid) and (select open_to_mm from campground where campground_id = @cgid) and " +
                                              "site_id not in " +
                                                "(select site_id " +
                                                "from reservation where @inputfrom <= to_date And @inputto >= from_date)";

                cmd.Parameters.AddWithValue("@cgid", cgID);
                cmd.Parameters.AddWithValue("@inputfrom", inputFrom);
                cmd.Parameters.AddWithValue("@inputto", inputTo);
                cmd.CommandText = SQL_SiteQuery;
                cmd.Connection = conn;


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var site = new Site();
                    site.Site_Id = (int)reader["site_id"];
                    site.SiteNum = (int)reader["site_number"];
                    site.MaxOcc = (int)reader["max_occupancy"];
                    bool Accessible = (bool)reader["accessible"];
                    site.Accessible = Accessible ? "Yes" : "No";
                    site.MaxRvLen = (int)reader["max_rv_length"];
                    bool Utility = (bool)reader["utilities"];
                    site.Utility = Utility ? "Yes" : "N/A";
                    siteList.Add(site);
                }
            }


            return siteList;
        }

        public int MakeReservation (int siteId, string name, DateTime fromDate, DateTime toDate)
        {
            int reservationId = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string SQL_ReservationQuery = "Insert Into reservation (site_id, name, from_date, to_date) " +
                                              "Values(@siteId, @name, @fromDate, @toDate)";

                SqlCommand cmd = new SqlCommand(SQL_ReservationQuery + "Select Cast(Scope_Identity() as int);", conn);

                cmd.Parameters.AddWithValue("@siteId", siteId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                reservationId = (int)cmd.ExecuteScalar();
                
            }

            return reservationId;
        }
    }
}
