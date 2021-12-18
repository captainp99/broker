using System;

namespace BrokerBussiness
{
    public static class Utility
    {
        public static bool IsValidDayAndTime(DateTime dateTime)
        {
            return dateTime.Hour >= 9 && dateTime.Hour <= 14 && dateTime.DayOfWeek != DayOfWeek.Sunday && dateTime.DayOfWeek != DayOfWeek.Saturday;
        }
    }
}
