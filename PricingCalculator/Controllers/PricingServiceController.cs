using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PricingCalculator.Models;
using PricingCalculator.Services;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PricingCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingServiceController : ControllerBase
    {
        /// <summary>
        /// Referens till en service där man hämtar information om en customer
        /// </summary>
        private readonly ICustomerService m_CustomerService;

        /// <summary>
        /// Referens till en service där man beräknar kostnaden
        /// </summary>
        private readonly IPriceCalculateService m_PriceCalculateService;


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="customerService">Referens till en service där man hämtar information om en customer</param>
        /// <param name="priceCalculateService">Referens till en service där man beräknar kostnaden</param>
        public PricingServiceController(ICustomerService customerService, IPriceCalculateService priceCalculateService)
        {
            this.m_CustomerService = customerService;
            this.m_PriceCalculateService = priceCalculateService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Pris</returns>        
        /// <response code="200">Ok och priset returneras</response>
        /// <response code="403">Returneras om customer inte kan använda servisen</response>
        /// <response code="404">Returneras om customer med sökt customer id inte finns</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [HttpGet("ServiceA/{customerId}/{startDate}/{endDate}")]
        public async Task<ActionResult<string>> ServiceC(int customerId, DateTime startDate, DateTime endDate)
        {
            m_CustomerService.CreateCustomers(5);
            Customer customer = m_CustomerService.GetCustomer(customerId);
            if (customer == null)
                return NotFound($"Hittade inte customer med id {customerId}");

            if (customer.CanUseServiceC)
            {
                double price = m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, startDate, endDate);
                return Ok(price.ToString());
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }
    }
}
