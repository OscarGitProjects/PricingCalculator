using System;

namespace PricingCalculator.Exceptions
{
    /// <summary>
    /// Exception som skall kastas när man skall lägga till en customer till repository och en customer med customerId redan finns
    /// </summary>
    public class CustomerAlreadyExistsException : Exception
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public CustomerAlreadyExistsException() { }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="text">Text som skall visas</param>
        public CustomerAlreadyExistsException(string text) : base(text)
        { }
    }
}
