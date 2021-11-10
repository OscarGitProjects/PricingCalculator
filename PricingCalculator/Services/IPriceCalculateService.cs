using PricingCalculator.Models;
using System;

namespace PricingCalculator.Services
{
    public interface IPriceCalculateService
    {
        double CalculatePrice(CallingService callingService, Customer customer, DateTime startDate, DateTime endDate);
        double CalculatePriceForService(CallingService callingService, Customer customer, DateTime dtStartDate, DateTime dtEndDate);
        int CalculateNumberOfWorkDaysForService(DateTime startDate, DateTime endDate);
        int CalculateNumberOfDiscountedDaysInPeriodForService(CallingService callingService, Customer customer, DateTime startDate, DateTime endDate, bool bOnlyWeekDays = false);
    }
}