using System;

namespace PricingCalculator.Exceptions
{
    /// <summary>
    /// Exception som skall kastas när ServiceBaseCost data i Appsettings.json inte är korrekt
    /// </summary>
    public class InvalidServiceBaseCostInAppsettingsException : Exception
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public InvalidServiceBaseCostInAppsettingsException() { }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="text">Text som skall visas</param>
        public InvalidServiceBaseCostInAppsettingsException(string text) : base(text)
        { }
    }
}
