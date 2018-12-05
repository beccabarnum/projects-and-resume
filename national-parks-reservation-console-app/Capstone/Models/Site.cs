using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Site
    {
        public int Site_Id { get; set; }
        public int SiteNum { get; set; }
        public int MaxOcc { get; set; }
        public string Accessible { get; set; }
        public int MaxRvLen { get; set; }
        public string DisplayMRL
        {
            get
            {
                if(MaxRvLen == 0)
                {
                    return "N/A";
                }
                else
                {
                    return MaxRvLen.ToString();
                }
            }
        }
        public string Utility { get; set; }
        public decimal Cost { get; set; }

    }
}
