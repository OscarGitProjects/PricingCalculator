using NUnit.Framework;
using PricingCalculator.Handlers;
using PricingCalculator.Services;
using System;

namespace NUnit_PricingCalculator_TestProject
{
    public class PriceCalculator_CalculateNumberOfWorkDaysForService_Tests : PricingCalculator_TestBase
    {
        [OneTimeSetUp]
        public void TestSetup()
        {
            var test = this.BuildConfiguration(TestContext.CurrentContext.TestDirectory);
            this.m_PriceCalculateService = new PriceCalculateService(this.BuildConfiguration(TestContext.CurrentContext.TestDirectory), new CustomerHandler());
        }

        [SetUp]
        public void Setup()
        {
        }


        #region Test av metoden CalculateNumberOfWorkDaysForService

        /// <summary>
        /// Test kontrollerar att ArgumentException kastas om Startdatum är efter slutdatum
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfWorkDaysForService_StartDate_After_EndDate_Test()
        {
            // Arrange
            // expected
            DateTime dtStartDate = DateTime.Now.AddDays(1);
            DateTime dtEndDate = DateTime.Now;

            // Act
            // actual
            Assert.Throws<ArgumentException>(() => this.m_PriceCalculateService.CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate));

            // Assert
        }


        /// <summary>
        /// Test kontrollerar att vid samma datum, lördagen 2021-11-06, returneras det 0 arbetsdagar
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfWorkDaysForService_Dates_Is_Saturday_Test()
        {
            // Arrange
            // expected
            DateTime dtStartDate = new DateTime(2021, 11, 06);
            DateTime dtEndDate = new DateTime(2021, 11, 06);

            // Act
            // actual
            int iActualNumberOfDays = this.m_PriceCalculateService.CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0, iActualNumberOfDays);
        }


        /// <summary>
        /// Test kontrollerar antal arbetsdagar är 2. Datum är från lördag 2021-11-06 till tisdag 2021-11-09
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfWorkDaysForService_Dates_Is_saturday_to_tuesday_Test()
        {
            // Arrange
            // expected
            DateTime dtStartDate = new DateTime(2021, 11, 06);  // Lördag
            DateTime dtEndDate = new DateTime(2021, 11, 09);    // tisdag

            // Act
            // actual
            int iActualNumberOfDays = this.m_PriceCalculateService.CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(2, iActualNumberOfDays);
        }


        /// <summary>
        /// Test kontrollerar antal arbetsdagar är 0. Datum är från lördag 2021-11-06 till söndag 2021-11-07
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfWorkDaysForService_Dates_Is_saturday_and_sunday_Test()
        {
            // Arrange
            // expected
            DateTime dtStartDate = new DateTime(2021, 11, 06);  // Lördag
            DateTime dtEndDate = new DateTime(2021, 11, 07);    // Söndag

            // Act
            // actual
            int iActualNumberOfDays = this.m_PriceCalculateService.CalculateNumberOfWorkDaysForService(dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0, iActualNumberOfDays);
        }

        #endregion // End of Region Test av metoden CalculateNumberOfWorkDaysForService
    }
}
