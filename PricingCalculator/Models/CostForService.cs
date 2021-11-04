namespace PricingCalculator.Models
{
    public class CostForService
    {
        public double Cost { get; set; }
        public bool HasItsOwnCostForService { get; set; } = false;
    }
}
