using PricingCalculator.Models;
using System;

namespace PricingCalculator.Services
{
    public interface IPriceCalculateService
    {
        double CalculatePrice(CallingService service, Customer customer, DateTime startDate, DateTime endDate);
    }
}