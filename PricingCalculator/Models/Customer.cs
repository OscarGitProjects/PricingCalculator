using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingCalculator.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double DiscountInPercentForServiceA { get; set; }
        public double DiscountInPercentForServiceB { get; set; }
        public double DiscountInPercentForServiceC { get; set; }
        public DateTime StartDateServiceA { get; set; }
        public DateTime StartDateServiceB { get; set; }
        public DateTime StartDateServiceC { get; set; }
        public int NumberOfFreeDays { get; set; }


        public Customer(int iCustomerId, string strCustomerName)
        {
            CustomerId = iCustomerId;
            CustomerName = strCustomerName;
        }


        public override string ToString()
        {
            // TODO 

            return "CustomerId: " + CustomerId + ", CustomerName: " + CustomerName;
        }
    }
}
