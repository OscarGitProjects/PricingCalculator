using PricingCalculator.Models;
using System;

namespace PricingCalculator.Services
{
    public interface IPriceCalculateService
    {
        double CalculatePrice(CallingService service, Customer customer, DateTime startDate, DateTime endDate);
        double CalculatePriceForService(Customer customer, CallingService callingService, DateTime dtStartDate, DateTime dtEndDate);
        int CalculateNumberOfWorkDaysForService(DateTime startDate, DateTime endDate);
        int CalculateNumberOfDiscountedDaysInPeriodForService(Customer customer, CallingService callingService, DateTime startDate, DateTime endDate, bool bOnlyWeekDays = false);
    }
}