using PricingCalculator.Models;
using System;

namespace PricingCalculator.Services
{
    public interface IPriceCalculateService
    {
        double CalculatePrice(CallingService service, Customer customer, DateTime startDate, DateTime endDate);
        double CalculatePriceForServiceA(Customer customer, DateTime startDate, DateTime endDate);
        double CalculatePriceForServiceB(Customer customer, DateTime startDate, DateTime endDate);
        double CalculatePriceForServiceC(Customer customer, DateTime startDate, DateTime endDate);
        int CalculateNumberOfWorkDaysForService(DateTime startDate, DateTime endDate);
        int CalculateNumberOfDaysInPeriodForService(Customer customer, CallingService callingService, DateTime startDate, DateTime endDate, bool bOnlyWeekDays = false);
    }
}