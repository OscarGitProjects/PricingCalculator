using PricingCalculator.Models;
using PricingCalculator.Services;
using System;

namespace PricingCalculator.Handlers
{
    public class CustomerHandler : ICustomerHandler
    {
        /// <summary>
        /// Metoden returnerar objekt med rabatt för användning av vald Service
        /// </summary>
        /// <param name="callingService">Vilken tjänst är det som anropas</param>
        /// <param name="customer">Referens till customer objekt</param>
        /// <returns>Discount objekt. Om service inte är vald returneras null</returns>
        /// <exception cref="ArgumentNullException">Kastats om referensen till Customer objektet är null</exception>
        public Discount GetDiscount(CallingService callingService, Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("CustomerHandler->GetDiscount(). Referensen till Customer objektet är null"); 

            if (callingService == CallingService.SERVICE_A)
                return customer.DiscountForServiceA;
            else if (callingService == CallingService.SERVICE_B)
                return customer.DiscountForServiceB;
            else if (callingService == CallingService.SERVICE_C)
                return customer.DiscountForServiceC;

            return null;
        }


        /// <summary>
        /// Metoden returnera objekt med kostnaden för att använda en vald service
        /// </summary>
        /// <param name="callingService">Vilken tjänst är det som anropas</param>
        /// <param name="customer">Referens till customer objekt</param>
        /// <returns>CostForService objekt. Om service inte är vald returneras null</returns>
        /// <exception cref="ArgumentNullException">Kastats om referensen till Customer objektet är null</exception>
        public CostForService GetCostForService(CallingService callingService, Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("CustomerHandler->GetCostForService(). Referensen till Customer objektet är null");

            if (callingService == CallingService.SERVICE_A)
                return customer.CostForServiceA;
            else if (callingService == CallingService.SERVICE_B)
                return customer.CostForServiceB;
            else if (callingService == CallingService.SERVICE_C)
                return customer.CostForServiceC;

            return null;
        }


        /// <summary>
        /// Metoden returnera den string med vilket json element man skall hämta från appsettings.json filen
        /// </summary>
        /// <param name="callingService">Vilken tjänst är det som anropas</param>
        /// <param name="customer">Referens till customer objekt</param>
        /// <returns>json element som skall hämtas från appsettings.json. Om service inte är vald returneras en tom sträng</returns>
        /// <exception cref="ArgumentNullException">Kastats om referensen till Customer objektet är null</exception>
        public string GetConfigValueString(CallingService callingService, Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("CustomerHandler->GetConfigValueString(). Referensen till Customer objektet är null");

            if (callingService == CallingService.SERVICE_A)
                return "ServiceBaseCost:ServiceA";
            else if (callingService == CallingService.SERVICE_B)
                return "ServiceBaseCost:ServiceB";
            else if (callingService == CallingService.SERVICE_C)
                return "ServiceBaseCost:ServiceC";

            return String.Empty;
        }


        /// <summary>
        /// true om vi bara skall ta betalt för arbetsdagar. Annars retruneras false
        /// Vilket som skall användas beror på vilken service som används
        /// </summary>
        /// <param name="callingService">Vilken tjänst är det som anropas</param>
        /// <param name="customer">Referens till customer objekt</param>
        /// <returns>true om vi bara skall ta betalt för arbetsdagar. Annars retruneras false. Om service inte är vald returneras false</returns>
        /// <exception cref="ArgumentNullException">Kastats om referensen till Customer objektet är null</exception>
        public bool OnlyWorkingDays(CallingService callingService, Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("CustomerHandler->OnlyWorkingDays(). Referensen till Customer objektet är null");

            if (callingService == CallingService.SERVICE_A)
                return true;
            else if (callingService == CallingService.SERVICE_B)
                return true;
            else if (callingService == CallingService.SERVICE_C)
                return false;

            return false;
        }


        /// <summary>
        /// Anropas för att se om customer får använda vald service
        /// </summary>
        /// <param name="callingService">Vilken tjänst är det som anropas</param>
        /// <param name="customer">Referens till customer objekt</param>
        /// <returns>true om det går att använda vald service. Annars returneras false</returns>
        /// <exception cref="ArgumentNullException">Kastats om referensen till Customer objektet är null</exception>
        public bool CanUseService(CallingService callingService, Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("CustomerHandler->CanUseService(). Referensen till Customer objektet är null");

            if (callingService == CallingService.SERVICE_A)
                return customer.CanUseServiceA;
            else if (callingService == CallingService.SERVICE_B)
                return customer.CanUseServiceB;
            else if (callingService == CallingService.SERVICE_C)
                return customer.CanUseServiceC;

            return false;
        }
    }
}
