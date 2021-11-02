using Microsoft.Extensions.Configuration;
using PricingCalculator.Repository;
using PricingCalculator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_PricingCalculator_TestProject
{
    public class PricingCalculator_TestBase
    {
        /// <summary>
        /// Referens till en service där man hämtar information om en customer från repository
        /// </summary>
        protected ICustomerRepository m_CustomerRepository;

        /// <summary>
        /// Referens till en service där man beräknar kostnaden
        /// </summary>
        protected IPriceCalculateService m_PriceCalculateService;

        public IConfigurationRoot BuildConfiguration(string testDirectory)
        {
            return new ConfigurationBuilder()
                .SetBasePath(testDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }
    }
}
