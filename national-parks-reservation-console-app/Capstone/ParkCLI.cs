using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class ParkCLI
    {
        private NationalParks NP { get; set; }

        public ParkCLI(NationalParks park)
        {
            NP = park;
        }

        public void DisplayParks()
        {
            
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Select a Park for further details");
                
                foreach(var parkNum in NP.Parks)
                {
                    Console.WriteLine("".PadRight(4, ' ') + $"{parkNum.Key}) {parkNum.Value.Name}");
                }
                Console.WriteLine("".PadRight(4, ' ') + "Q) Quit");

                char key = Console.ReadKey().KeyChar;

                try
                {
                    int keyToInt = int.Parse(key.ToString());
                    if (NP.Parks.ContainsKey(keyToInt))
                    {
                        NP.SelectPark(keyToInt);
                        ParkMenu();
                    }
                }
                catch (FormatException)
                {
                }

                if (key == 'q' || key == 'Q')
                {
                    exit = true;
                }
            }
        }

        private void ParkMenu()
        {
            bool exit = false;
            NP.CGs = NP.DictOfCampGrounds(NP.Park.ID);
            while (!exit)
            {
                Console.Clear();
                PrintPark();

                Console.WriteLine();
                Console.WriteLine("Select a Command");
                Console.WriteLine("".PadRight(4, ' ') + "1) View Campgrounds".PadLeft(10, ' '));
                Console.WriteLine("".PadRight(4, ' ') + "2) Search for Reservation");
                Console.WriteLine("".PadRight(4, ' ') + "3) Return to Previous Screen".PadLeft(10, ' '));
                //Console.WriteLine("".PadRight(4, ' ') + "4) View all reservations in the next 30 days");
                char key = Console.ReadKey().KeyChar;

                if(key == '3')
                {
                    exit = true;
                }
                else if(key == '1')
                {
                    NP.CGs = NP.DictOfCampGrounds(NP.Park.ID);
                    ViewCampgrounds();
                }
                else if (key == '2')
                {
                    NP.CGs = NP.DictOfCampGrounds(NP.Park.ID);
                    ViewAllSites();
                }
            }
        }

        private void ViewAllSites()
        {
            Dictionary<Campground, Dictionary<int, Site>> SitesByCG = new Dictionary<Campground, Dictionary<int, Site>>();
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                try
                {
                    DateInput();
                    exit = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input a valid date. Press a key to enter new date.");
                    Console.ReadKey();
                }
                catch (NegativeTimeSpanException)
                {
                    Console.WriteLine("Departure date must be after arrival date.");
                    Console.ReadKey();
                }
            }

            Console.WriteLine();
            Console.WriteLine("".PadRight(5, ' ') +
                                "Campground Name".PadRight(NP.LongestCampGroundName + 2, ' ') +
                                "Open".PadRight(15, ' ') +
                                "Close".PadRight(15, ' ') +
                                "Daily Fee");
            Console.WriteLine();
            foreach (var campground in NP.CGs)
            {
                NP.Campground = campground.Value;
                Console.Write($"#{campground.Key}".PadRight(5, ' '));
                PrintCG(campground.Value);

                Dictionary<int, Site> siteDict = NP.GetSiteByDate(campground.Value.CampgroundID, NP.ReservationFrom, NP.ReservationTo);
                PrintSites(siteDict);
                SitesByCG[NP.Campground] = siteDict;
                
                Console.WriteLine();
            }

            Console.Write("Which campground (enter 0 to cancel)? ");
            try
            {
                int cgInput = int.Parse(Console.ReadLine());


                if (cgInput == 0)
                {
                }
                else if (NP.CGs.ContainsKey(cgInput) && SitesByCG[NP.CGs[cgInput]].Count > 0)
                {
                    NP.Campground = NP.CGs[cgInput];
                    Console.WriteLine();
                    Console.Write("Which site should be reserved? (enter 0 to cancel) ");
                    int siteSelection = int.Parse(Console.ReadLine());
                    if (siteSelection == 0)
                    {
                    }
                    else if (SitesByCG[NP.Campground].ContainsKey(siteSelection))
                    {
                        Console.Write("What name should the reservation be made under? ");
                        string reservationName = Console.ReadLine();
                        Console.WriteLine();
                        int reservationID = NP.MakeReservation(SitesByCG[NP.Campground][siteSelection].Site_Id, reservationName, NP.ReservationFrom, NP.ReservationTo);
                        Console.WriteLine($"The reservation has been made and the confirmation id is {reservationID}");
                        Console.Write("Press any key to return to Park Details");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Please select a site by site number");
                        Console.ReadKey();
                    }
                }
                else
                {
                    if (NP.CGs.ContainsKey(cgInput) && SitesByCG[NP.CGs[cgInput]].Count == 0)
                    {
                        Console.WriteLine("There are no available sites at selected campground.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Select valid campground. Press any key to continue.");
                        Console.ReadKey();
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Input a number.");
                Console.ReadKey();
            }

            
        }

        private void ViewCampgrounds()
        {
            
            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                Console.WriteLine($"{NP.Park.Name } National Park Campgrounds");
                PrintCGs(NP.CGs);

                Console.WriteLine();
                Console.WriteLine("Select a command");
                Console.WriteLine("".PadRight(4, ' ') + "1) Search for Available Reservation");
                Console.WriteLine("".PadRight(4, ' ') + "2) Return to Previous Screen");
                char choice = Console.ReadKey().KeyChar;

                if (choice == '2')
                {
                    exit = true;
                }
                else if (choice == '1')
                {
                    SelectCampGround();
                }
            }
        }

        private void SelectCampGround()
        {
            NP.CGs = NP.DictOfCampGrounds(NP.Park.ID);
            bool exit = false;
            
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"{NP.Park.Name } National Park Campgrounds");
                PrintCGs(NP.CGs);
                Console.WriteLine();
                Console.Write("Which campground (enter 0 to cancel)? ");
                int cgInput = 0;
                string cgInputStr = Console.ReadLine();

                if (cgInputStr == "0")
                {
                    exit = true;
                    break;
                }
                try
                {
                    cgInput = int.Parse(cgInputStr);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input valid campground.");
                    Console.ReadKey();
                    exit = true;
                    break;
                }
                try
                {
                    NP.Campground = NP.CGs[cgInput];
                    DateInput();
                    ViewCampgroundSites();
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Input valid campground.");
                    Console.ReadKey();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid date. Press any key to enter new date.");
                    Console.ReadKey();
                }
                catch (NegativeTimeSpanException)
                {
                    Console.WriteLine("Departure date must be after arrival date.");
                    Console.ReadKey();
                }
                
                
            }
        }

        private void ViewCampgroundSites()
        {
            Dictionary<int ,Site> siteDict = NP.GetSiteByDate(NP.Campground.CampgroundID, NP.ReservationFrom, NP.ReservationTo);

            

            if (siteDict.Count != 0)
            {
                PrintSites(siteDict);
                Console.WriteLine();
                Console.Write("Which site should be reserved? (enter 0 to cancel) ");
                try
                {
                    int siteSelection = int.Parse(Console.ReadLine());
                    if (siteSelection == 0)
                    {
                    }
                    else if (siteDict.ContainsKey(siteSelection))
                    {
                        Console.Write("What name should the reservation be made under? ");
                        string reservationName = Console.ReadLine();
                        Console.WriteLine();
                        int reservationID = NP.MakeReservation(siteDict[siteSelection].Site_Id, reservationName, NP.ReservationFrom, NP.ReservationTo);
                        Console.WriteLine($"The reservation has been made and the confirmation id is {reservationID}");
                        Console.Write("Press any key to return to Park Details");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Please select a site by site number");
                        Console.ReadKey();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter valid site.");
                    Console.ReadKey();
                }
                
            }
            else
            {
                PrintSites(siteDict);
                Console.ReadKey();
            }
        }

        public void DateInput()
        {
            char yn = 'n';
            if(NP.ReservationFrom != default(DateTime) && NP.ReservationTo!= default(DateTime) )
            {
                
                Console.WriteLine($"Keep previous date range ({NP.ReservationFrom.ToShortDateString()} to {NP.ReservationTo.ToShortDateString()})? Y/N");
                yn = Console.ReadKey().KeyChar;
                
            }
            Console.WriteLine("\b");
            if (yn == 'n')
            {
                Console.Write("What is the arrival date? yyyy/mm/dd ");
                string reservationFrom = Console.ReadLine();
                Console.Write("What is the departure date? yyyy/mm/dd ");
                string reservationTo = Console.ReadLine();
                NP.SetReservation(reservationFrom, reservationTo);
            }
        }
        

        private void PrintPark()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{NP.Park.Name} National Park");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Locaction:".PadRight(20, ' ') + NP.Park.Location);
            Console.WriteLine("Established:".PadRight(20, ' ') + NP.Park.EstablishDate.ToShortDateString());
            Console.WriteLine("Area:".PadRight(20, ' ') + $"{NP.Park.Area.ToString("N0")} sq km");
            Console.WriteLine("Annual Visitors:".PadRight(20, ' ') + NP.Park.AnnualVisitors.ToString("N0"));
            Console.WriteLine();
            Console.WriteLine(NP.Park.Description);
        }

        private void PrintCGs(Dictionary<int, Campground> campgrounds)
        {
            Console.WriteLine();
            Console.WriteLine("".PadRight(5, ' ') +
                              "Name".PadRight(NP.LongestCampGroundName + 2, ' ') +
                              "Open".PadRight(15, ' ') +
                              "Close".PadRight(15, ' ') +
                              "Daily Fee");
            foreach (var campground in campgrounds)
            {
                Console.Write($"#{campground.Key}".PadRight(5, ' '));
                PrintCG(campground.Value);
            }
        }
        
        private void PrintCG (Campground campground)
        {
            Console.WriteLine($"{campground.Name}".PadRight(NP.LongestCampGroundName + 2, ' ') +
                              $"{Campground.monthsDict[campground.OpenFromMonth]}".PadRight(15, ' ') +
                              $"{Campground.monthsDict[campground.OpenToMonth]}".PadRight(15, ' ') +
                              $"{campground.DailyFee.ToString("C")}");
        }

        private void PrintSites(Dictionary<int, Site> siteDict)
        {
            
            if (siteDict.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("There are no available sites in this date range at this campground.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Results Matching Your Search Criteria");
                Console.WriteLine("Site No.".PadRight(10, ' ') +
                                  "Max Occup.".PadRight(12, ' ') +
                                  "Accessible?".PadRight(12, ' ') +
                                  "Max RV Length".PadRight(15, ' ') +
                                  "Utility".PadRight(10, ' ') +
                                  "Cost".PadRight(10, ' '));

                foreach (var site in siteDict)
                {
                    Console.WriteLine(site.Value.SiteNum.ToString().PadRight(10, ' ') +
                                      site.Value.MaxOcc.ToString().PadRight(12, ' ') +
                                      site.Value.Accessible.ToString().PadRight(12, ' ') +
                                      site.Value.DisplayMRL.PadRight(15, ' ') +
                                      site.Value.Utility.ToString().PadRight(10, ' ') +
                                      NP.ReservationCost.ToString("C"));
                }
            }
        }
    }
}
