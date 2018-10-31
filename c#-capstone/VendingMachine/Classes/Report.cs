using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone
{
    public class Report
    {
        private const int BeginningStock = 5;

        public static void GenerateSalesReport(Dictionary<string, VendingItem> inventory)
        {
            string fileName = "SalesReport.txt";
            string fileDirectory = Environment.CurrentDirectory;
            string fullPath = Path.Combine(fileDirectory, fileName);

            double result = 0;
            if (!File.Exists(fullPath))
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    //sw.WriteLine("Sales Report");

                    foreach (VendingItem item in inventory.Values)
                    {
                        sw.WriteLine($"{item.Name}|{5 - item.Qty}");
                        result += item.Price * (5 - item.Qty);
                    }
                    sw.WriteLine();
                    sw.WriteLine($"**TOTAL SALES** {result.ToString("C")}");
                }
            }
            else
             {
                Dictionary<string, int> reportDict = new Dictionary<string, int>();
                List<string> reportList = new List<string>();
                string key = "";
                int val = 0;

                using (StreamReader sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        string fileToSplit = sr.ReadLine();
                        if (fileToSplit.Contains('|'))
                        {
                            string[] keyValue = fileToSplit.Split('|');
                            key = keyValue[0];
                            val = int.Parse(keyValue[1]);

                            reportDict.Add(key, val);
                            
                            int quantity = 0;
                            foreach (VendingItem item in inventory.Values)
                            {
                                if (item.Name == key)
                                {
                                    quantity = item.Qty;
                                    val += (BeginningStock - quantity);
                                    reportDict[key] = val;

                                    string keyString = key;
                                    string valueString = val.ToString();
                                    string[] keyValueArray = new string[] { keyString, "|", valueString };
                                    string keyValString0 = keyValueArray[0].ToString();
                                    string keyValString1 = keyValueArray[1].ToString();
                                    string keyValString2 = keyValueArray[2].ToString();
                                    string keyValString = keyValString0 + keyValString1 + keyValString2;
                                    reportList.Add(keyValString);

                                    result += item.Price * val;
                                }
                            }
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
                    sw.WriteLine("Sales Report");

                    foreach (string item in reportList)
                    {
                        sw.WriteLine($"{item}");
                    }

                    sw.WriteLine();
                    sw.WriteLine($"**TOTAL SALES** {result.ToString("C")}");

                }
            }
        }
    }
}
