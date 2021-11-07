using System;

namespace PricingCalculator.Models
{
    /// <summary>
    /// Information om eventuell rabatt
    /// </summary>
    public class Discount
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DiscountInPercent { get; set; }
        public bool HasDiscount { get; set; } = false;
        public bool HasDiscountForAPeriod { get; set; } = false;

        public override string ToString()
        {
            return $"StartDate: {StartDate.ToShortDateString()}, EndDate: {EndDate.ToShortDateString()}, DiscountInPercent: {DiscountInPercent}, HasDiscount: {HasDiscount}, HasDiscountForAPeriod: {HasDiscountForAPeriod}";
        }
    }
}
