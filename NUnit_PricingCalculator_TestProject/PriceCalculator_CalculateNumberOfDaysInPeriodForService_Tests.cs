using NUnit.Framework;
using PricingCalculator.Services;

namespace NUnit_PricingCalculator_TestProject
{
    public class PriceCalculator_CalculateNumberOfDaysInPeriodForService_Tests : PricingCalculator_TestBase
    {
        [OneTimeSetUp]
        public void TestSetup()
        {
            var test = this.BuildConfiguration(TestContext.CurrentContext.TestDirectory);
            this.m_PriceCalculateService = new PriceCalculateService(this.BuildConfiguration(TestContext.CurrentContext.TestDirectory));
        }

        [SetUp]
        public void Setup()
        {
        }


        #region Test av metoden CalculateNumberOfDaysInPeriodForService

        // TODO

        #endregion // End of region Test av metoden CalculateNumberOfDaysInPeriodForService
    }
}
