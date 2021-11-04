﻿using System;

namespace PricingCalculator.Models
{
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

            StartDateServiceA = DateTime.Now.AddYears(1);
            StartDateServiceB = DateTime.Now.AddYears(1);
            StartDateServiceC = DateTime.Now.AddYears(1);

            NumberOfFreeDays = 0;
        }


        public override string ToString()
        {
            // TODO 

            return "CustomerId: " + CustomerId + ", CustomerName: " + CustomerName;
        }
    }
}
