using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Web.Models;

namespace Capstone.Web.DAL
{
    /// <summary>
    /// implemented in the ParkServiceSqlDAL in order to call methods
    /// </summary>
    public interface IParkServiceDAL
    {
        List<Park> GetAllParks();

        List<Weather> GetForecastByParkCode(string parkCode);

        Park GetSinglePark(string parkid);

        List<SurveyResultModel> GetAllSurveys();

        bool SaveNewSurvey(Survey survey);


    }
}
