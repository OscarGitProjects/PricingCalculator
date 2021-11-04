using Microsoft.Extensions.Configuration;
using PricingCalculator.Exceptions;
using PricingCalculator.Models;
using System;

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
        /// <param name="startDate">Startdatum</param>
        /// <param name="endDate">Slutdatum</param>
        /// <returns>Kostnaden</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        /// <exception cref="System.ArgumentException">Anropande Service är inte satt dvs. service == CallingService.NA</exception>
        /// <exception cref="InvalidServiceBaseCostInAppsettingsException">Kastas om ServiceBaseCost data i Appsettings.json inte är korrekt</exception>
        public double CalculatePrice(CallingService service, Customer customer, DateTime startDate, DateTime endDate)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePrice(). Referensen till customer är null");

            if (startDate > endDate)
                throw new ArgumentException("PriceCalculateService->CalculatePrice(). StartDatum är inte före slutdatum");

            if(service == CallingService.NA)
                throw new ArgumentException("PriceCalculateService->CalculatePrice(). Anropande Service är inte satt");


            double dblCost = 0.0;
            double dblBaseCost = 0.0;
            int iDays = 0;
            string strServiceBaseCost = String.Empty;
            bool bBaseCostIsValid = false;

            if (service == CallingService.SERVICE_A)
            {// Service A = € 0,2 / working day (monday-friday)

                try
                {
                    dblCost = CalculatePriceForServiceA(customer, startDate, endDate);
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
                    dblCost = CalculatePriceForServiceB(customer, startDate, endDate);
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
                    dblCost = CalculatePriceForServiceC(customer, startDate, endDate);
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
        /// <param name="startDate">Startdatum för användningen av service</param>
        /// <param name="endDate">Slutdatum för användningen av service</param>
        /// <returns>Kostnaden för att använda service under angiven period</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        private double CalculatePriceForServiceA(Customer customer, DateTime startDate, DateTime endDate)
        {
            // TODO GÖR KLART

            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePriceForServiceA(). Referensen till customer är null");

            if (startDate > endDate)
                throw new ArgumentException("PriceCalculateService->CalculatePriceForServiceA(). StartDatum är inte före slutdatum");

            double dblCost = 0.0;
            double dblBaseCost = 0.0;
            int iDays = 0;
            string strServiceBaseCost = String.Empty;
            bool bBaseCostIsValid = false;

            // TODO Service A
            // Hämta baskostnaden för att använda servicen
            if (customer.CostForServiceA.HasItsOwnCostForService)
            {
                dblBaseCost = customer.CostForServiceA.Cost;
            }
            else
            {// Vi hämtar baskostnaden från appsettings.json filen
                strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceA");
                bBaseCostIsValid = Double.TryParse(strServiceBaseCost, out dblBaseCost);

                if (bBaseCostIsValid == false || String.IsNullOrWhiteSpace(strServiceBaseCost))
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePriceForServiceA(). ServiceBaseCost:ServiceA isnt valid");
            }

            return dblBaseCost;
        }


        /// <summary>
        /// Metoden beräknar kostnaden för att använda service B
        /// </summary>
        /// <param name="customer">Customer som vill använda service</param>
        /// <param name="startDate">Startdatum för användningen av service</param>
        /// <param name="endDate">Slutdatum för användningen av service</param>
        /// <returns>Kostnaden för att använda service under angiven period</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        private double CalculatePriceForServiceB(Customer customer, DateTime startDate, DateTime endDate)
        {
            // TODO GÖR KLART

            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePriceForServiceB(). Referensen till customer är null");

            if (startDate > endDate)
                throw new ArgumentException("PriceCalculateService->CalculatePriceForServiceB(). StartDatum är inte före slutdatum");

            double dblCost = 0.0;
            double dblBaseCost = 0.0;
            int iDays = 0;
            string strServiceBaseCost = String.Empty;
            bool bBaseCostIsValid = false;

            // TODO Service A
            // Hämta baskostnaden för att använda servicen
            if (customer.CostForServiceB.HasItsOwnCostForService)
            {
                dblBaseCost = customer.CostForServiceB.Cost;
            }
            else
            {// Vi hämtar baskostnaden från appsettings.json filen
                strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceB");
                bBaseCostIsValid = Double.TryParse(strServiceBaseCost, out dblBaseCost);

                if (bBaseCostIsValid == false || String.IsNullOrWhiteSpace(strServiceBaseCost))
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePriceForServiceB(). ServiceBaseCost:ServiceB isnt valid");
            }

            return dblBaseCost;
        }


        /// <summary>
        /// Metoden beräknar kostnaden för att använda service c
        /// </summary>
        /// <param name="customer">Customer som vill använda service</param>
        /// <param name="startDate">Startdatum för användningen av service</param>
        /// <param name="endDate">Slutdatum för användningen av service</param>
        /// <returns>Kostnaden för att använda service under angiven period</returns>
        /// <exception cref="System.ArgumentNullException">Undantaget kastas om referensen till Customer objektet är null</exception>
        /// <exception cref="System.ArgumentException">StartDatum inte är före slutdatum</exception>
        private double CalculatePriceForServiceC(Customer customer, DateTime startDate, DateTime endDate)
        {
            if (customer == null)
                throw new ArgumentNullException("PriceCalculateService->CalculatePriceForServiceC(). Referensen till customer är null");

            if (startDate > endDate)
                throw new ArgumentException("PriceCalculateService->CalculatePriceForServiceC(). StartDatum är inte före slutdatum");


            double dblCost = 0.0;
            double dblBaseCost = 0.0;
            int iDays = 0;
            string strServiceBaseCost = String.Empty;
            bool bBaseCostIsValid = false;

            // Hämta baskostnaden för att använda servicen
            if (customer.CostForServiceC.HasItsOwnCostForService)
            {
                dblBaseCost = customer.CostForServiceC.Cost;
            }
            else
            {// Vi hämtar baskostnaden från appsettings.json filen
                strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceC");
                bBaseCostIsValid = Double.TryParse(strServiceBaseCost, out dblBaseCost);

                if (bBaseCostIsValid == false || String.IsNullOrWhiteSpace(strServiceBaseCost))
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePrice(). ServiceBaseCost:ServiceC isnt valid");
            }


            if (startDate.Date == endDate.Date)
                iDays = 1;
            else
                iDays = (endDate.Date - startDate.Date).Days;

            if (customer.HasFreeDays)
                iDays = iDays - customer.NumberOfFreeDays;

            if (iDays <= 0)// Vi har inga dagar som vi ska beräkna kostnader för
                return dblCost;

            dblCost = dblBaseCost * Double.Parse(iDays.ToString());

            if (customer.DiscountForServiceC.HasDiscount)
            {// Kunden har rabatt på service c

                if (customer.DiscountForServiceC.HasDiscountForAPeriod)
                {// Kunden har rabatt under en period. Kontrollera hur många av dagarna som är inom perioden

                    int iNumberOfDaysInPeriod = CalculateNumberOfDaysInPeriod(customer, startDate, endDate);

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
                }
                else
                {// Kunden har alltid rabatt

                    dblCost = dblCost * (double)(1.0 - (double)(customer.DiscountForServiceC.DiscountInPercent / Double.Parse("100,0")));
                }
            }

            return dblCost;
        }


        private int CalculateNumberOfDaysInPeriod(Customer customer, DateTime startDate, DateTime endDate)
        {
            // TODO

            throw new NotImplementedException();
        }
    }
}
