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


        /// <summary>
        /// Metoden kontrollerar att datumet, dvs DateTime.Now.Date, är inom intervallet dtStartDate och dtEndDate. 
        /// Inkluderar dtStartDate och dtEndDate
        /// </summary>
        /// <param name="dtDate">Datum som vi vill kontrollera om det är inom intervallet</param>
        /// <param name="dtStartDate">Startdatum för intervallet. Kontroll inklusive datumet</param>
        /// <param name="dtEndDate">Slutdatum för intervallet. Kontroll inklusive datumet</param>
        /// <returns>true om datumet är inom intervallet. Annars returneras false</returns>
        public static bool IsInRange(this DateTime dtDate, DateTime dtStartDate, DateTime dtEndDate)
        {            
            bool bIsInRange = false;

            if (dtDate.Date >= dtStartDate.Date && dtDate.Date <= dtEndDate.Date)
                bIsInRange = true;

            return bIsInRange;
        }
    }
}
