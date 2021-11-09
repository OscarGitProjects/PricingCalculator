using PricingCalculator.Services;
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

        public CallingService CallingService { get; set; } = CallingService.NA;

        /// <summary>
        /// Metoden returnerar objekt med rabatt för användning av vald Service
        /// Vald service sätts med CallingService
        /// </summary>
        /// <returns>Discount objekt. Om service inte är vald returneras null</returns>
        public Discount GetDiscount()
        {
            if (CallingService == CallingService.SERVICE_A)
                return this.DiscountForServiceA;
            else if (CallingService == CallingService.SERVICE_B)
                return this.DiscountForServiceB;
            else if (CallingService == CallingService.SERVICE_C)
                return this.DiscountForServiceC;

            return null;
        }

        /// <summary>
        /// Metoden returnera objekt med kostnaden för att använda en vald service
        /// Vald service sätts med CallingService
        /// </summary>
        /// <returns>CostForService objekt. Om service inte är vald returneras null</returns>
        public CostForService GetCostForService()
        {
            if (CallingService == CallingService.SERVICE_A)
                return this.CostForServiceA;
            else if (CallingService == CallingService.SERVICE_B)
                return this.CostForServiceB;
            else if (CallingService == CallingService.SERVICE_C)
                return this.CostForServiceC;

            return null;
        }


        /// <summary>
        /// Metoden returnera den string med vilket json element man skall hämta från appsettings.json filen
        /// Vald service sätts med CallingService
        /// </summary>
        /// <returns>json element som skall hämtas från appsettings.json. Om service inte är vald returneras en tom sträng</returns>
        public string GetConfigValueString()
        {
            if (CallingService == CallingService.SERVICE_A)
                return "ServiceBaseCost:ServiceA";
            else if (CallingService == CallingService.SERVICE_B)
                return "ServiceBaseCost:ServiceB";
            else if (CallingService == CallingService.SERVICE_C)
                return "ServiceBaseCost:ServiceC";

            return String.Empty;
        }


        /// <summary>
        /// true om vi bara skall ta betalt för arbetsdagar. Annars retruneras false
        /// Vilket som skall användas beror på vilken service som används
        /// Vald service sätts med CallingService
        /// </summary>
        /// <returns>true om vi bara skall ta betalt för arbetsdagar. Annars retruneras false. Om service inte är vald returneras false</returns>
        public bool OnlyWorkingDays()
        {
            if (CallingService == CallingService.SERVICE_A)
                return true;
            else if (CallingService == CallingService.SERVICE_B)
                return true;
            else if (CallingService == CallingService.SERVICE_C)
                return false;

            return false;
        }


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
        /// Anropas för att se om customer får använda vald service
        /// Vald service sätts med CallingService
        /// </summary>
        /// <returns>true om det går att använda vald service. Annars returneras false</returns>
        public bool CanUseService()
        {
            if (CallingService == CallingService.SERVICE_A)
                return CanUseServiceA;
            else if (CallingService == CallingService.SERVICE_B)
                return CanUseServiceB;
            else if (CallingService == CallingService.SERVICE_C)
                return CanUseServiceC;

            return false;
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


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="iCustomerId">CustomerId</param>
        /// <param name="strCustomerName">CustomerName</param>
        /// <param name="callingService">Vilken service är det som skall användas</param>
        public Customer(int iCustomerId, string strCustomerName, CallingService callingService) : this(iCustomerId, strCustomerName)
        {
            this.CallingService = callingService;
        }


        public override string ToString()
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.AppendLine($"CustomerId: {CustomerId}, CustomerName: {CustomerName}");

            strBuild.AppendLine($"NumberOfFreeDays: {NumberOfFreeDays}, CallingService: {CallingService}");

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
