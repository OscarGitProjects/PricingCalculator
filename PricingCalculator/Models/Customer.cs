using System;
using System.Text;

namespace PricingCalculator.Models
{
    /// <summary>
    /// Information om en customer
    /// </summary>
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        
        public DateTime StartDateServiceA { get; set; }
        public DateTime StartDateServiceB { get; set; }
        public DateTime StartDateServiceC { get; set; }  
        
        public Discount DiscountForServiceA { get; set; }
        public Discount DiscountForServiceB { get; set; }
        public Discount DiscountForServiceC { get; set; }
        
        public CostForService CostForServiceA { get; set; }
        public CostForService CostForServiceB { get; set; }
        public CostForService CostForServiceC { get; set; }

        public int NumberOfFreeDays { get; set; }


        /// <summary>
        /// Property som returnerar true om användaren har några gratis dagar
        /// Annars returneras false
        /// </summary>
        public bool HasFreeDays {
            get {
                if (NumberOfFreeDays > 0)
                    return true;

                return false;
            }
        }


        /// <summary>
        /// Property som returnerar true om användaren kan använda service a
        /// Annars returneras false
        /// </summary>
        public bool CanUseServiceA { 
            get { 
                if(StartDateServiceA.Date <= DateTime.Now.Date)
                    return true;

                return false;
            } 
        }


        /// <summary>
        /// Property som returnerar true om användaren kan använda service b
        /// Annars returneras false
        /// </summary>
        public bool CanUseServiceB
        {
            get
            {
                if (StartDateServiceB.Date <= DateTime.Now.Date)
                    return true;

                return false;
            }
        }


        /// <summary>
        /// Property som returnerar true om användaren kan använda service c
        /// Annars returneras false
        /// </summary>
        public bool CanUseServiceC
        {
            get
            {
                if (StartDateServiceC.Date <= DateTime.Now.Date)
                    return true;

                return false;
            }
        }


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="iCustomerId">CustomerId</param>
        /// <param name="strCustomerName">CustomerName</param>
        public Customer(int iCustomerId, string strCustomerName)
        {
            CustomerId = iCustomerId;
            CustomerName = strCustomerName;

            // Sätt lite startvärden
            DiscountForServiceA = new Discount();
            DiscountForServiceB = new Discount();
            DiscountForServiceC = new Discount();

            CostForServiceA = new CostForService();
            CostForServiceB = new CostForService();
            CostForServiceC = new CostForService();

            StartDateServiceA = DateTime.Now.AddYears(-100);
            StartDateServiceB = DateTime.Now.AddYears(-100);
            StartDateServiceC = DateTime.Now.AddYears(-100);

            NumberOfFreeDays = 0;
        }


        public override string ToString()
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.AppendLine($"CustomerId: {CustomerId}, CustomerName: {CustomerName}");

            strBuild.AppendLine($"NumberOfFreeDays: {NumberOfFreeDays}");

            strBuild.AppendLine($"StartDateServiceA: {StartDateServiceA.ToShortDateString()}, StartDateServiceB: {StartDateServiceB.ToShortDateString()}, StartDateServiceC: {StartDateServiceC.ToShortDateString()}");

            strBuild.AppendLine("DiscountForServiceA: " + DiscountForServiceA);
            strBuild.AppendLine("DiscountForServiceB: " + DiscountForServiceB);
            strBuild.AppendLine("DiscountForServiceC: " + DiscountForServiceC);

            strBuild.AppendLine("CostForServiceA: " + CostForServiceA);
            strBuild.AppendLine("CostForServiceB: " + CostForServiceB);
            strBuild.AppendLine("CostForServiceC: " + CostForServiceC);

            return strBuild.ToString();
        }
    }
}
