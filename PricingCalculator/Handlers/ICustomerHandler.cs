using PricingCalculator.Models;
using PricingCalculator.Services;

namespace PricingCalculator.Handlers
{
    public interface ICustomerHandler
    {
        bool CanUseService(CallingService callingService, Customer customer);
        string GetConfigValueString(CallingService callingService, Customer customer);
        CostForService GetCostForService(CallingService callingService, Customer customer);
        Discount GetDiscount(CallingService callingService, Customer customer);
        bool OnlyWorkingDays(CallingService callingService, Customer customer);
    }
}