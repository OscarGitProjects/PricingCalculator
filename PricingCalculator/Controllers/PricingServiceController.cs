using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PricingCalculator.Models;
using PricingCalculator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns></returns>
        /// <response code="200">TODO</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [HttpGet("ServiceA/{customerId}/{startDate}/{endDate}")]
        public async Task<ActionResult<string>> ServiceC(int customerId, DateTime startDate, DateTime endDate)
        {
            Customer customer = m_CustomerService.GetCustomer(customerId);

            // TODO KONTROLLERA ATT KUNDEN HAR TILLGÅNG TILL SERVICE

            double price = m_PriceCalculateService.CalculatePrice(CallingService.SERVICE_C, customer, startDate, endDate);

            return Ok(price.ToString());
        }


        // GET: api/<PricingServiceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PricingServiceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PricingServiceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PricingServiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PricingServiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
