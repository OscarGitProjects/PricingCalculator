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


            double dblPrice = 0.0;
            double dblBasePrice = 0.0;
            int iDays = 0;
            string strServiceBaseCost = String.Empty;
            bool bBasePriceIsValid = false;

            if (service == CallingService.SERVICE_A)
            {// Service A = € 0,2 / working day (monday-friday)

                // TODO Service A

                strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceA");
                bBasePriceIsValid = Double.TryParse(strServiceBaseCost, out dblBasePrice);

                if (String.IsNullOrWhiteSpace(strServiceBaseCost) || bBasePriceIsValid == false)
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePrice(). ServiceBaseCost:ServiceA isnt valid");

            }
            else if (service == CallingService.SERVICE_B)
            {// Service B = € 0,24 / working day (monday-friday)

                // TODO Service B

                strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceB");
                bBasePriceIsValid = Double.TryParse(strServiceBaseCost, out dblBasePrice);

                if (String.IsNullOrWhiteSpace(strServiceBaseCost) || bBasePriceIsValid == false)
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePrice(). ServiceBaseCost:ServiceB isnt valid");

            }
            else if (service == CallingService.SERVICE_C)
            {// Service C = € 0,4 / day (monday-sunday)

                strServiceBaseCost = m_Config.GetValue<string>("ServiceBaseCost:ServiceC");
                bBasePriceIsValid = Double.TryParse(strServiceBaseCost, out dblBasePrice);

                if (String.IsNullOrWhiteSpace(strServiceBaseCost) || bBasePriceIsValid == false)
                    throw new InvalidServiceBaseCostInAppsettingsException("PriceCalculateService->CalculatePrice(). ServiceBaseCost:ServiceC isnt valid");

                if (startDate.Date == endDate.Date)
                    iDays = 1;
                else
                    iDays = (endDate.Date - startDate.Date).Days;


                iDays = iDays - customer.NumberOfFreeDays;

                if (iDays < 0)// Vill inte ha negativ kostnad
                    iDays = 0;

                dblPrice = dblBasePrice * Double.Parse(iDays.ToString());

                if(customer.DiscountInPercentForServiceC > 0.0)
                {// Kunden har rabatt på service c
                    dblPrice = dblPrice * (double)(1.0 - (double)(customer.DiscountInPercentForServiceC / Double.Parse("100,0")));
                }
            }

            return dblPrice;
        }
    }
}
