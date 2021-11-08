using Microsoft.Extensions.Configuration;
using PricingCalculator.Exceptions;
using PricingCalculator.Extensions;
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

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="config"></param>
        public PriceCalculateService(IConfiguration config)
        {
            this.m_Config = config;
        }

        /// <summary>
        /// Metoden beräknar kostnaden
        /// </summary>
        /// <param name="service">Enum med information om vilken service det är som anropades. Olika servisar har olika kostnader</param>
        /// <param name="customer">Customer objekt med information</param>
        /// <param name="dtStartDate">Startdatum</param>
        /// <param name="dtEndDate">Slutdatum</param>
        /// <returns>Kostnaden</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        /// <exception cref="System.ArgumentException">Anropande Service är inte satt dvs. service == CallingService.NA</exception>
        /// <exception cref="InvalidServiceBaseCostInAppsettingsException">Kastas om ServiceBaseCost data i Appsettings.json inte är korrekt</exception>
        public double CalculatePrice(CallingService service, Customer customer, DateTime dtStartDate, DateTime dtEndDate)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePrice(). Referensen till customer är null");

            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculatePrice(). StartDatum är inte före slutdatum");

            if (service == CallingService.NA)
                throw new ArgumentException("PriceCalculateService->CalculatePrice(). Anropande Service är inte satt");


            double dblCost = 0.0;

            if (service == CallingService.SERVICE_A)
            {// Service A = € 0,2 / working day (monday-friday)

                try
                {
                    dblCost = CalculatePriceForServiceA(customer, dtStartDate, dtEndDate);
                }
                catch
                {
                    throw;
                }

            }
            else if (service == CallingService.SERVICE_B)
            {// Service B = € 0,24 / working day (monday-friday)

                try
                {
                    dblCost = CalculatePriceForServiceB(customer, dtStartDate, dtEndDate);
                }
                catch
                {
                    throw;
                }
            }
            else if (service == CallingService.SERVICE_C)
            {// Service C = € 0,4 / day (monday-sunday)

                try
                {
                    dblCost = CalculatePriceForServiceC(customer, dtStartDate, dtEndDate);
                }
                catch
                {
                    throw;
                }
            }

            return dblCost;
        }


        /// <summary>
        /// Metoden beräknar kostnaden för att använda service A
        /// </summary>
        /// <param name="customer">Customer som vill använda service</param>
        /// <param name="dtStartDate">Startdatum för användningen av service</param>
        /// <param name="dtEndDate">Slutdatum för användningen av service</param>
        /// <returns>Kostnaden för att använda service under angiven period</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        /// <exception cref="System.ArgumentException">Anropande Service till metoden CalculateNumberOfDaysInPeriodForService är inte satt dvs. callingService == CallingService.NA</exception>
        public double CalculatePriceForServiceA(Customer customer, DateTime dtStartDate, DateTime dtEndDate)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePriceForServiceA(). Referensen till customer är null");

            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculatePriceForServiceA(). StartDatum är inte före slutdatum");

            double dblCost = 0.0;
            double dblBaseCost = 0.0;


            // Hämta baskostnaden för att använda servicen
            if (customer.CostForServiceA.HasItsOwnCostForService)
            {
                dblBaseCost = customer.CostForServiceA.Cost;
            }
            else
            {// Vi hämtar baskostnaden från appsettings.json filen
                string strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceA");
                bool bBaseCostIsValid = Double.TryParse(strServiceBaseCost, out dblBaseCost);

                if (bBaseCostIsValid == false || String.IsNullOrWhiteSpace(strServiceBaseCost))
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePriceForServiceA(). ServiceBaseCost:ServiceA isnt valid");
            }


            // Vi räknar bara arbetsdagar dvs måndag till fredag
            int iDays = CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate);

            if (customer.HasFreeDays)
                iDays = iDays - customer.NumberOfFreeDays;

            if (iDays <= 0)// Vi har inga dagar som vi ska beräkna kostnader för
                return dblCost;


            dblCost = dblBaseCost * Double.Parse(iDays.ToString());

            if (customer.DiscountForServiceA.HasDiscount)
            {// Kunden har rabatt på service a

                if (customer.DiscountForServiceA.HasDiscountForAPeriod)
                {// Kunden har rabatt under en period

                    // Kontrollera hur många av dagarna som är inom perioden
                    int iNumberOfDaysInPeriod = CalculateNumberOfDaysInPeriodForService(customer, CallingService.SERVICE_A, dtStartDate, dtEndDate, true);

                    if (iNumberOfDaysInPeriod > 0)
                    {// Kunden har rabatt för några dagar

                        // Rabatterad kostnad
                        double dblCostWithDiscount = dblBaseCost * Double.Parse((iNumberOfDaysInPeriod).ToString());
                        dblCostWithDiscount = dblCostWithDiscount * (double)(1.0 - (double)(customer.DiscountForServiceA.DiscountInPercent / Double.Parse("100,0")));

                        if (iDays > iNumberOfDaysInPeriod)
                        {// Det finns dagar som kunden inte skall ha rabatt för

                            double dblCostNoDiscount = dblBaseCost * Double.Parse((iDays - iNumberOfDaysInPeriod).ToString());
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

                    dblCost = dblCost * (double)(1.0 - (double)(customer.DiscountForServiceA.DiscountInPercent / Double.Parse("100,0")));
                }
            }

            return dblCost;
        }


        /// <summary>
        /// Metoden beräknar kostnaden för att använda service B
        /// </summary>
        /// <param name="customer">Customer som vill använda service</param>
        /// <param name="dtStartDate">Startdatum för användningen av service</param>
        /// <param name="dtEndDate">Slutdatum för användningen av service</param>
        /// <returns>Kostnaden för att använda service under angiven period</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        /// <exception cref="System.ArgumentException">Anropande Service till metoden CalculateNumberOfDaysInPeriodForService är inte satt dvs. callingService == CallingService.NA</exception>
        public double CalculatePriceForServiceB(Customer customer, DateTime dtStartDate, DateTime dtEndDate)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePriceForServiceB(). Referensen till customer är null");

            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculatePriceForServiceB(). StartDatum är inte före slutdatum");

            double dblCost = 0.0;
            double dblBaseCost = 0.0;


            // Hämta baskostnaden för att använda servicen
            if (customer.CostForServiceB.HasItsOwnCostForService)
            {
                dblBaseCost = customer.CostForServiceB.Cost;
            }
            else
            {// Vi hämtar baskostnaden från appsettings.json filen
                string strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceB");
                bool bBaseCostIsValid = Double.TryParse(strServiceBaseCost, out dblBaseCost);

                if (bBaseCostIsValid == false || String.IsNullOrWhiteSpace(strServiceBaseCost))
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePriceForServiceB(). ServiceBaseCost:ServiceB isnt valid");
            }


            // Vi räknar bara arbetsdagar dvs måndag till fredag
            int iDays = CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate);


            if (customer.HasFreeDays)
                iDays = iDays - customer.NumberOfFreeDays;

            if (iDays <= 0)// Vi har inga dagar som vi ska beräkna kostnader för
                return dblCost;


            dblCost = dblBaseCost * Double.Parse(iDays.ToString());

            if (customer.DiscountForServiceB.HasDiscount)
            {// Kunden har rabatt på service b

                if (customer.DiscountForServiceB.HasDiscountForAPeriod)
                {// Kunden har rabatt under en period

                    // Kontrollera hur många av dagarna som är inom perioden
                    int iNumberOfDaysInPeriod = CalculateNumberOfDaysInPeriodForService(customer, CallingService.SERVICE_B, dtStartDate, dtEndDate, true);

                    if (iNumberOfDaysInPeriod > 0)
                    {// Kunden har rabatt för några dagar

                        // Rabatterad kostnad
                        double dblCostWithDiscount = dblBaseCost * Double.Parse((iNumberOfDaysInPeriod).ToString());
                        dblCostWithDiscount = dblCostWithDiscount * (double)(1.0 - (double)(customer.DiscountForServiceB.DiscountInPercent / Double.Parse("100,0")));

                        if (iDays > iNumberOfDaysInPeriod)
                        {// Det finns dagar som kunden inte skall ha rabatt för

                            double dblCostNoDiscount = dblBaseCost * Double.Parse((iDays - iNumberOfDaysInPeriod).ToString());
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

                    dblCost = dblCost * (double)(1.0 - (double)(customer.DiscountForServiceB.DiscountInPercent / Double.Parse("100,0")));
                }
            }

            return dblCost;
        }


        /// <summary>
        /// Metoden beräknar kostnaden för att använda service c
        /// </summary>
        /// <param name="customer">Customer som vill använda service</param>
        /// <param name="dtStartDate">Startdatum för användningen av service</param>
        /// <param name="dtEndDate">Slutdatum för användningen av service</param>
        /// <returns>Kostnaden för att använda service under angiven period</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        /// <exception cref="System.ArgumentException">Anropande Service till metoden CalculateNumberOfDaysInPeriodForService är inte satt dvs. callingService == CallingService.NA</exception>
        public double CalculatePriceForServiceC(Customer customer, DateTime dtStartDate, DateTime dtEndDate)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePriceForServiceC(). Referensen till customer är null");

            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculatePriceForServiceC(). StartDatum är inte före slutdatum");


            double dblCost = 0.0;
            double dblBaseCost = 0.0;

            // Hämta baskostnaden för att använda servicen
            if (customer.CostForServiceC.HasItsOwnCostForService)
            {
                dblBaseCost = customer.CostForServiceC.Cost;
            }
            else
            {// Vi hämtar baskostnaden från appsettings.json filen
                string strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceC");
                bool bBaseCostIsValid = Double.TryParse(strServiceBaseCost, out dblBaseCost);

                if (bBaseCostIsValid == false || String.IsNullOrWhiteSpace(strServiceBaseCost))
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePrice(). ServiceBaseCost:ServiceC isnt valid");
            }


            int iDays = (dtEndDate.Date - dtStartDate.Date).Days;
            iDays++;

            if (customer.HasFreeDays)
                iDays = iDays - customer.NumberOfFreeDays;

            if (iDays <= 0)// Vi har inga dagar som vi ska beräkna kostnader för
                return dblCost;

            dblCost = dblBaseCost * Double.Parse(iDays.ToString());

            if (customer.DiscountForServiceC.HasDiscount)
            {// Kunden har rabatt på service c

                if (customer.DiscountForServiceC.HasDiscountForAPeriod)
                {// Kunden har rabatt under en period

                    // Kontrollera hur många av dagarna som är inom perioden
                    int iNumberOfDaysInPeriod = CalculateNumberOfDaysInPeriodForService(customer, CallingService.SERVICE_C, dtStartDate, dtEndDate, false);

                    if (iNumberOfDaysInPeriod > 0)
                    {// Kunden har rabatt för några dagar

                        // Rabatterad kostnad
                        double dblCostWithDiscount = dblBaseCost * Double.Parse((iNumberOfDaysInPeriod).ToString());
                        dblCostWithDiscount = dblCostWithDiscount * (double)(1.0 - (double)(customer.DiscountForServiceC.DiscountInPercent / Double.Parse("100,0")));

                        if (iDays > iNumberOfDaysInPeriod)
                        {// Det finns dagar som kunden inte skall ha rabatt för

                            double dblCostNoDiscount = dblBaseCost * Double.Parse((iDays - iNumberOfDaysInPeriod).ToString());
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

                    dblCost = dblCost * (double)(1.0 - (double)(customer.DiscountForServiceC.DiscountInPercent / Double.Parse("100,0")));
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
        /// <param name="customer">Customer</param>
        /// <param name="callingService">Anropande service</param>
        /// <param name="dtStartDate">Startdatum</param>
        /// <param name="dtEndDate">Slutdatum</param>
        /// <param name="bOnlyWeekDays">true om vi bara skall räkna måndag till och med fredag. false innebär att vi räknar alla veckans dagar. default false</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        /// <exception cref="System.ArgumentException">Anropande Service är inte satt dvs. callingService == CallingService.NA</exception>
        public int CalculateNumberOfDaysInPeriodForService(Customer customer, CallingService callingService, DateTime dtStartDate, DateTime dtEndDate, bool bOnlyWeekDays = false)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculateNumberOfDaysInPeriodForService(). Referensen till customer är null");

            if (dtStartDate > dtEndDate)
                throw new ArgumentException("PriceCalculateService->CalculateNumberOfDaysInPeriodForService(). StartDatum är inte före slutdatum");

            if (callingService == CallingService.NA)
                throw new ArgumentException("PriceCalculateService->CalculateNumberOfDaysInPeriodForService(). Anropande Service är inte satt");


            int iNumberOfDays = 0;

            // Hämta uppgifter om eventuella rabatter
            Discount discount = null;
            if (callingService == CallingService.SERVICE_A)
                discount = customer.DiscountForServiceA;
            else if (callingService == CallingService.SERVICE_B)
                discount = customer.DiscountForServiceB;
            else if (callingService == CallingService.SERVICE_C)
                discount = customer.DiscountForServiceC;

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
