using PricingCalculator.Models;
using System;

namespace PricingCalculator.Services
{
    public interface IPriceCalculateService
    {
        double CalculatePrice(Customer customer, DateTime startDate, DateTime endDate);
        double CalculatePriceForService(Customer customer, DateTime dtStartDate, DateTime dtEndDate);
        int CalculateNumberOfWorkDaysForService(DateTime startDate, DateTime endDate);
        int CalculateNumberOfDiscountedDaysInPeriodForService(Customer customer, DateTime startDate, DateTime endDate, bool bOnlyWeekDays = false);
    }
}