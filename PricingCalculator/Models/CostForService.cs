namespace PricingCalculator.Models
{
    /// <summary>
    /// Information om eventuell egen kostnad för att använda en service
    /// Om det inte finns en egen kostnad för en service, används en bas kostnad som finnas i appsettings.json filen
    /// </summary>
    public class CostForService
    {
        public double Cost { get; set; }
        public bool HasItsOwnCostForService { get; set; } = false;

        public override string ToString()
        {
            return $"Cost: {Cost}, HasItsOwnCostForService: {HasItsOwnCostForService}";
        }
    }
}
