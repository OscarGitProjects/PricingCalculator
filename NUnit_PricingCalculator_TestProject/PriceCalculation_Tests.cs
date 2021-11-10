using NUnit.Framework;
using PricingCalculator.Handlers;
using PricingCalculator.Models;
using PricingCalculator.Services;
using System;

namespace NUnit_PricingCalculator_TestProject
{
    public class PriceCalculation_Tests : PricingCalculator_TestBase
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

        /// <summary>
        /// Testet kontrollerar att ArgumentNullException kastas om Customer referensen är null
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_Customer_Referense_Is_Null_Test()
        {
            // Arrange
            // expected  
            Customer customer = null;
            DateTime dtStartDate = DateTime.Now;
            DateTime dtEndDate = DateTime.Now;

            // Act
            // actual
            Assert.Throws<ArgumentNullException>(() => m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_A, customer, dtStartDate, dtEndDate));

            // Assert
        }


        /// <summary>
        /// Testet kontrollerar att ArgumentException kastas om StartDatum inte är före slutdatum
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_StartDate_IsNot_Before_end_date_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");
            DateTime dtStartDate = DateTime.Now;
            DateTime dtEndDate = DateTime.Now.AddHours(-1);

            // Act
            // actual
            Assert.Throws<ArgumentException>(() => m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, dtStartDate, dtEndDate));

            // Assert
        }


        #region Region för test av CallingService_C


        /// <summary>
        /// Test kontrollerar att priset blir rätt för en dag utan rabatter eller gratis dagar
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_CallingService_C_One_Day_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");            
            DateTime dtStartDate = DateTime.Now;
            DateTime dtEndDate = DateTime.Now;
            customer.DiscountForServiceC = new Discount();
            //customer.DiscountInPercentForServiceC = 0.0;
            customer.NumberOfFreeDays = 0;

            // Act
            // actual
            double dblActualPrice = this.m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0.4, dblActualPrice);
        }


        /// <summary>
        /// Test kontrollerar att priset blir rätt för två dag utan rabatter eller gratis dagar
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_CallingService_C_Two_Days_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");
            DateTime dtStartDate = DateTime.Now.AddDays(-1);
            DateTime dtEndDate = DateTime.Now;
            customer.DiscountForServiceC = new Discount();
            //customer.DiscountInPercentForServiceC = 0.0;
            customer.NumberOfFreeDays = 0;

            // Act
            // actual
            double dblActualPrice = this.m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0.8, dblActualPrice);
        }


        /// <summary>
        /// Test kontrollerar att priset blir rätt för två dag med en gratis dag
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_CallingService_C_Two_Days_One_free_Day_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");
            DateTime dtStartDate = DateTime.Now.AddDays(-1);
            DateTime dtEndDate = DateTime.Now;
            customer.DiscountForServiceC = new Discount();
            //customer.DiscountInPercentForServiceC = 0.0;
            customer.NumberOfFreeDays = 1;

            // Act
            // actual
            double dblActualPrice = this.m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0.4, dblActualPrice);
        }


        /// <summary>
        /// Test kontrollerar att priset blir rätt för två dag med två gratis dagar
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_CallingService_C_Two_Days_Two_free_Day_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");
            DateTime dtStartDate = DateTime.Now.AddDays(-1);
            DateTime dtEndDate = DateTime.Now;
            customer.DiscountForServiceC = new Discount();
            //customer.DiscountInPercentForServiceC = 0.0;
            customer.NumberOfFreeDays = 2;

            // Act
            // actual
            double dblActualPrice = this.m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0.0, dblActualPrice);
        }


        /// <summary>
        /// Test kontrollerar att priset blir rätt för två dag med tre gratis dagar
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_CallingService_C_Two_Days_Three_free_Day_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");
            DateTime dtStartDate = DateTime.Now.AddDays(-2);
            DateTime dtEndDate = DateTime.Now;

            customer.DiscountForServiceC = new Discount();
            //customer.DiscountInPercentForServiceC = 0.0;
            customer.NumberOfFreeDays = 3;

            // Act
            // actual
            double dblActualPrice = this.m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0.0, dblActualPrice);
        }



        /// <summary>
        /// Test kontrollerar att priset blir rätt för två dag med 10% rabatt
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_CallingService_C_Two_Days_10_percent_discount_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");
            DateTime dtStartDate = DateTime.Now.AddDays(-1);
            DateTime dtEndDate = DateTime.Now;
            
            Discount discount = new Discount();
            discount.DiscountInPercent = 10.0;
            discount.HasDiscount = true;

            customer.DiscountForServiceC = discount;
            customer.NumberOfFreeDays = 0;

            // Act
            // actual
            double dblActualPrice = this.m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0.8 * (double)(1.0 - (customer.DiscountForServiceC.DiscountInPercent / 100.0)), dblActualPrice);
        }


        /// <summary>
        /// Test kontrollerar att priset blir rätt för två dag med 10% rabatt och en dag gratis
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculatePrice_CallingService_C_Two_Days_10_percent_discount_and_one_day_free_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");            
            DateTime dtStartDate = DateTime.Now.AddDays(-1);
            DateTime dtEndDate = DateTime.Now;

            Discount discount = new Discount();
            discount.DiscountInPercent = 10.0;
            discount.HasDiscount = true;

            customer.DiscountForServiceC = discount;

            //customer.DiscountInPercentForServiceC = 10.0;
            customer.NumberOfFreeDays = 1;

            // Act
            // actual
            double dblActualPrice = this.m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(0.4 * (double)(1.0 - (customer.DiscountForServiceC.DiscountInPercent / 100.0)), dblActualPrice);
        }


        #endregion // End of Region för test av CallingService_C
     
    }
}