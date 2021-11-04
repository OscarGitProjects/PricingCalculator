using System;

namespace PricingCalculator.Extensions
{
    /// <summary>
    /// Extension metoder för DateTime
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Metoden kontrollerar om datumet är en arbetsdag dvs måndag till fredag
        /// </summary>
        /// <param name="dtDay">Datum</param>
        /// <returns>true om det var en arbetsdag. Annars returneras false</returns>
        public static bool IsWorkDay(this DateTime dtDay)
        {
            bool bIsWorkDay = true;

            if (dtDay.DayOfWeek == DayOfWeek.Saturday || dtDay.DayOfWeek == DayOfWeek.Sunday)
                bIsWorkDay = false;

            return bIsWorkDay;
        }
    }
}
