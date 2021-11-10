using Microsoft.Extensions.Configuration;
using PricingCalculator.Exceptions;
using PricingCalculator.Extensions;
using PricingCalculator.Handlers;
using PricingCalculator.Models;
using System;
//using System.Runtime.CompilerServices;


//[assembly: InternalsVisibleTo("NUnit_PricingCalculator_TestProject")]
namespace PricingCalculator.Services
{
    public enum CallingService
    {
        NA = 0,
        SERVICE_A = 1,
        SERVICE_B = 2,
        SERVICE_C = 3
    }


    /// <summary>
    /// Service för att beräkna kostnaden
    /// </summary>
    public class PriceCalculateService : IPriceCalculateService
    {
        private readonly IConfiguration m_Config;
        private readonly ICustomerHandler m_CustomerHandler;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="config">Referens till ett objekt där mankan hämta konfigurationer</param>
        /// <param name="customerHandler">Referens till ett objekt där man kan hämta information från ett customer objekt</param>
        public PriceCalculateService(IConfiguration config, ICustomerHandler customerHandler)
        {
            this.m_Config = config;
            this.m_CustomerHandler = customerHandler;
        }

        /// <summary>
        /// Metoden beräknar kostnaden
        /// </summary>
        /// <param name="callingService">Information om vilken service som vi skall beräkna kostnaden för</param>
        /// <param name="customer">Customer objekt med information</param>
        /// <param name="dtStartDate">Startdatum</param>
        /// <param name="dtEndDate">Slutdatum</param>
        /// <returns>Kostnaden</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        /// <exception cref="InvalidServiceBaseCostInAppsettingsException">Kastas om ServiceBaseCost data i Appsettings.json inte är korrekt</exception>
        public double CalculatePrice(CallingService callingService, Customer customer, DateTime dtStartDate, DateTime dtEndDate)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePrice(). Referensen till customer är null");

            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculatePrice(). StartDatum är inte före slutdatum");


            double dblCost = 0.0;

            try
            {
                // Service A = € 0,2 / working day (monday-friday)
                // Service B = € 0,24 / working day (monday-friday)
                // Service C = € 0,4 / day (monday-sunday)
                dblCost = CalculatePriceForService(callingService, customer, dtStartDate, dtEndDate);
            }
            catch
            {
                throw;
            }

            return dblCost;
        }


        /// <summary>
        /// Metoden beräknar kostnaden för att använda en tjänst under en period som är angiven av startdatum och slutdatum
        /// </summary>
        /// <param name="callingService">Information om vilken service som vi skall beräkna kostnaden för</param>
        /// <param name="customer">Customer som vill använda service</param>
        /// <param name="callingService">Anropande service</param>
        /// <param name="dtStartDate">Startdatum för användningen av service</param>
        /// <param name="dtEndDate">Slutdatum för användningen av service</param>
        /// <returns>Kostnaden för att använda service under angiven period</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        /// <exception cref="InvalidServiceBaseCostInAppsettingsException">Kastas om ServiceBaseCost data i Appsettings.json inte är korrekt</exception>
        public double CalculatePriceForService(CallingService callingService, Customer customer, DateTime dtStartDate, DateTime dtEndDate)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePriceForService(). Referensen till customer är null");

            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculatePriceForService(). StartDatum är inte före slutdatum");


            double dblCost = 0.0;
            double dblBaseCost = 0.0;

            
            CostForService costForService = m_CustomerHandler.GetCostForService(callingService, customer); 
            Discount discount = m_CustomerHandler.GetDiscount(callingService, customer); 
            string strConfigValue = m_CustomerHandler.GetConfigValueString(callingService, customer); 
            bool bOnlyWorkingDays = m_CustomerHandler.OnlyWorkingDays(callingService, customer); 
            int iDays = 0;

            if(bOnlyWorkingDays)
            {
                // Vi räknar bara arbetsdagar dvs måndag till fredag
                iDays = CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate);
            }
            else
            {
                // Vi räknar alla veckans dagar
                iDays = (dtEndDate.Date - dtStartDate.Date).Days;
                iDays++;
            }

            //if (callingService == CallingService.SERVICE_A)
            //{
            //    discount = customer.DiscountForServiceA;
            //    costForService = customer.CostForServiceA;
            //    strConfigValue = "ServiceBaseCost:ServiceA";
            //    bOnlyWeekDays = true;
            //    // Vi räknar bara arbetsdagar dvs måndag till fredag
            //    iDays = CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate);
            //}
            //else if (callingService == CallingService.SERVICE_B)
            //{
            //    discount = customer.DiscountForServiceB; 
            //    costForService = customer.CostForServiceB;
            //    strConfigValue = "ServiceBaseCost:ServiceB";
            //    bOnlyWeekDays = true;
            //    // Vi räknar bara arbetsdagar dvs måndag till fredag
            //    iDays = CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate);
            //}
            //else if (callingService == CallingService.SERVICE_C)
            //{
            //    discount = customer.DiscountForServiceC;
            //    costForService = customer.CostForServiceC;
            //    strConfigValue = "ServiceBaseCost:ServiceC";
            //    bOnlyWeekDays = false;
            //    // Vi räknar alla veckans dagar
            //    iDays = (dtEndDate.Date - dtStartDate.Date).Days;
            //    iDays++;
            //}


            // Hämta baskostnaden för att använda servicen
            if (costForService.HasItsOwnCostForService)
            {
                dblBaseCost = costForService.Cost;
            }
            else
            {// Vi hämtar baskostnaden från appsettings.json filen
                string strServiceBaseCost = m_Config.GetValue<string>(strConfigValue);
                bool bBaseCostIsValid = Double.TryParse(strServiceBaseCost, out dblBaseCost);

                if (bBaseCostIsValid == false || String.IsNullOrWhiteSpace(strServiceBaseCost))
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePriceForService(). ServiceBaseCost:Service isnt valid");
            }

            if (customer.HasFreeDays)
                iDays = iDays - customer.NumberOfFreeDays;

            if (iDays <= 0)// Vi har inga dagar som vi ska beräkna kostnader för
                return dblCost;


            dblCost = dblBaseCost * Double.Parse(iDays.ToString());

            if (discount.HasDiscount)
            {// Kunden har rabatt på service

                if (discount.HasDiscountForAPeriod)
                {// Kunden har rabatt under en period

                    // Kontrollera hur många av dagarna som är inom perioden
                    int iNumberDiscountedOfDaysInPeriod = CalculateNumberOfDiscountedDaysInPeriodForService(callingService, customer, dtStartDate, dtEndDate, bOnlyWorkingDays);

                    if (iNumberDiscountedOfDaysInPeriod > 0)
                    {// Kunden har rabatt för några dagar

                        // Rabatterad kostnad
                        double dblCostWithDiscount = dblBaseCost * Double.Parse((iNumberDiscountedOfDaysInPeriod).ToString());
                        dblCostWithDiscount = dblCostWithDiscount * (double)(1.0 - (double)(discount.DiscountInPercent / Double.Parse("100,0")));

                        if (iDays > iNumberDiscountedOfDaysInPeriod)
                        {// Det finns dagar som kunden inte skall ha rabatt för

                            double dblCostNoDiscount = dblBaseCost * Double.Parse((iDays - iNumberDiscountedOfDaysInPeriod).ToString());
                            dblCost = dblCostWithDiscount + dblCostNoDiscount;
                        }
                        else
                        {
                            dblCost = dblCostWithDiscount;
                        }
                    }
                    // Om kunden inte har rabatt under en period, så har vi redan beräknat kostnaden
                }
                else
                {// Kunden har alltid rabatt

                    dblCost = dblCost * (double)(1.0 - (double)(discount.DiscountInPercent / Double.Parse("100,0")));
                }
            }

            return dblCost;
        }


        /// <summary>
        /// Metoden beräknar hur många arbetsdagar dvs. måndag till fredag som det är mellan startdatum och slutdatum
        /// </summary>
        /// <param name="dtStartDate">Startdatum</param>
        /// <param name="dtEndDate">Slutdatum</param>
        /// <returns>Antalet dagar mellan startdatum och slutdatum. Vi räknar bara veckodagar dvs måndag till och med fredag</returns>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        public int CalculateNumberOfWorkDaysForService(DateTime dtStartDate, DateTime dtEndDate)
        {
            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculateNumberOfWorkDaysForService(). StartDatum är inte före slutdatum");

            int iNumberOfWorkDays = 0;
                
            int iDays = (dtEndDate.Date - dtStartDate.Date).Days;
            iDays++;
            
            DateTime dtTmpDate = dtStartDate;

            for (int i = 0; i < iDays; i++)
            {
                if (dtTmpDate.IsWorkDay())
                    iNumberOfWorkDays++;

                dtTmpDate = dtTmpDate.AddDays(1);
            }

            return iNumberOfWorkDays;
        }


        /// <summary>
        /// Kontrollera hur många av dagarna som är inom perioden för rabatt. Den perioden finns i customer objektet
        /// </summary>
        /// <param name="callingService">Information om vilken service som vi skall beräkna kostnaden för</param>
        /// <param name="customer">Customer</param>
        /// <param name="dtStartDate">Startdatum</param>
        /// <param name="dtEndDate">Slutdatum</param>
        /// <param name="bOnlyWeekDays">true om vi bara skall räkna måndag till och med fredag. false innebär att vi räknar alla veckans dagar. default false</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        public int CalculateNumberOfDiscountedDaysInPeriodForService(CallingService callingService, Customer customer, DateTime dtStartDate, DateTime dtEndDate, bool bOnlyWeekDays = false)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculateNumberOfDiscountedDaysInPeriodForService(). Referensen till customer är null");

            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculateNumberOfDiscountedDaysInPeriodForService(). StartDatum är inte före slutdatum");


            int iNumberOfDays = 0;

            // Hämta uppgifter om eventuella rabatter
            Discount discount = m_CustomerHandler.GetDiscount(callingService, customer); //customer.GetDiscount();           

            if (discount != null && discount.HasDiscount && discount.HasDiscountForAPeriod)
            {
                if ((discount.StartDate.Date >= dtStartDate.Date && discount.StartDate.Date <= dtEndDate) ||
                    (discount.EndDate.Date >= dtStartDate && discount.EndDate.Date <= dtEndDate))
                {// Rabattens Startdate är inom intervallet eller Rabattens Slutdate är inom intervallet
   
                    int iDays = (discount.EndDate.Date - discount.StartDate.Date).Days;
                    iDays++;

                    DateTime dtTmpDiscountDate = discount.StartDate;

                    for (int i = 0; i < iDays; i++)
                    {
                        if (bOnlyWeekDays)
                        {// Räkna bara veckodagar dvs måndag till fredag

                            // https://extensionmethod.net/csharp/datetime/intersects
                            // DateTime.Now.IsInRange(dtStartDate, dtEndDate);
                            // DateTime.Now.IsWorkDay
                            // iNumberOfDays

                            if (dtTmpDiscountDate.IsInRange(dtStartDate, dtEndDate) && dtTmpDiscountDate.IsWorkDay())
                                iNumberOfDays++;
                        }
                        else
                        {// Räkna alla dagar i veckan

                            if (dtTmpDiscountDate.IsInRange(dtStartDate, dtEndDate))
                                iNumberOfDays++;
                        }

                        dtTmpDiscountDate = dtTmpDiscountDate.AddDays(1);
                    }
                }
            }

            return iNumberOfDays;
        }
    }
}
