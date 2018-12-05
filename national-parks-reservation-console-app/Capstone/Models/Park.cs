using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Park
    {
        /// <summary>
        /// Park ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Park name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date park was established
        /// </summary>
        public DateTime EstablishDate { get; set; }

        /// <summary>
        /// State park is in
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Surface area of park
        /// </summary>
        public int Area { get; set; }

        /// <summary>
        /// Number of annual visitors
        /// </summary>
        public int AnnualVisitors { get; set; }

        /// <summary>
        /// Description of park
        /// </summary>
        public string Description { get; set; }
    }
}
