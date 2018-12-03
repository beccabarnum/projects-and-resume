using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Change
    {
        public double TotalAmountEntered { get; }
        public double TotalPurchasePrice { get; }
        public int NumOfQuarters { get; set; }
        public int NumOfDimes { get; set; }
        public int NumOfNickels { get; set; }
        public double AmountOwed { get; set; }

        public Change(double totalAmountEntered, double totalPurchasePrice)
        {
            TotalAmountEntered = totalAmountEntered;
            TotalPurchasePrice = totalPurchasePrice;
            double amountOwed = TotalAmountEntered * 100 - TotalPurchasePrice * 100;
            AmountOwed = amountOwed;
            MakeChange();
        }

        private void MakeChange()
        {
            int numOfDimes = 0;
           
            int numOfNickels = 0;

            int numOfQuarters = (int)(AmountOwed / 25);

            NumOfQuarters = numOfQuarters;

            int changeRemaining = (int)(AmountOwed % 25);

            if (changeRemaining > 0)
            {
                numOfDimes = (changeRemaining / 10);
                NumOfDimes = numOfDimes;
                changeRemaining = (changeRemaining % 10);

                if (changeRemaining > 0)
                {
                    numOfNickels = (changeRemaining / 5);
                    NumOfNickels = numOfNickels;
                }
            }
        }
    }
}
