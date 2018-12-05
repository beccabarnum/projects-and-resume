using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Holds all properties and data of a weather object
    /// </summary>
    public class Weather
    {
        public string ParkCode { get; set; }
        public int FiveDayValue { get; set; }
        public string Forecast { get; set; }
        /// <summary>
        /// bool that changes based on weather the temp is fahrenheit
        /// </summary>
        public bool IsFahrenheit { get; set; }
        /// <summary>
        /// Set low temperature when read in from DAL starts in fahrenheit
        /// </summary>
        public int Low { get; set; }
        /// <summary>
        /// Set high temperature when read in from DAL starts in fahrenheit
        /// </summary>
        public int High { get; set; }

        /// <summary>
        /// changes value from Fahrenheit to Celsius or from Celsius to Fahrenheit for low
        /// </summary>
        public int LowCalc
        {
            get
            {
                if (IsFahrenheit == false)
                {
                    return (int)(((double)Low - 32) / 1.8);
                }
                else
                {
                    return Low;
                }
            }
        }
        /// <summary>
        /// changes value from Fahrenheit to Celsius or from Celsius to Fahrenheit for high
        /// </summary>      
        public int HighCalc
        {
            get
            {
                if (IsFahrenheit == false)
                {
                    return (int)(((double)High - 32) / 1.8);
                }
                else
                {
                    return High;
                }
            }
        }
        /// <summary>
        /// returns advice for the forecast on the view
        /// </summary>
        /// <returns></returns>
        public string GetAdviceForecast()
        {
            string advice = "";

            if(Forecast == "rain")
            {
                advice = "Pack rain gear and waterproof shoes!";
            }
            else if (Forecast == "sunny")
            {
                advice = "Remember to pack sunblock!";
            }
            else if (Forecast == "snow")
            {
                advice = "Don't forget to pack snowshoes!";
            }
            else if (Forecast == "thunderstorms")
            {
                advice = "Seek shelter and avoid hiking on exposed ridges.";
            }
            return advice;
        }
        /// <summary>
        /// returns list of advice dependent on temperature value
        /// </summary>
        public List<string> GetAdviceTemperature()
        {
            List<string> adviceList = new List<string>();

            if (IsFahrenheit)
            {
                if(HighCalc > 75)
                {
                    adviceList.Add("It's going to be a hot one. Make sure to bring an extra gallon of water.");
                }
                if(HighCalc - LowCalc > 20)
                {
                    adviceList.Add("Wear breathable layers.");
                }
                if(LowCalc < 20)
                {
                    adviceList.Add("***Warning * **exposure to frigid temperatures can cause frostbite or hypothermia. " +
                        "Areas most prone to frostbite are uncovered skin and the extremities. " +
                        "Limit time outside, dress in layers, and cover exposed skin.");
                }
            }
            else
            {
                if (HighCalc > 23.888)
                {
                    adviceList.Add("It's going to be a hot one. Make sure to bring an extra gallon of water.");
                }
                if (HighCalc - LowCalc > 11.11)
                {
                    adviceList.Add("Wear breathable layers.");
                }
                if (LowCalc < -6.666)
                {
                    adviceList.Add("***Warning * **exposure to frigid temperatures can cause frostbite or hypothermia. " +
                        "Areas most prone to frostbite are uncovered skin and the extremities. " +
                        "Limit time outside, dress in layers, and cover exposed skin.");
                }
            }
            return adviceList;
        }
    }
}
