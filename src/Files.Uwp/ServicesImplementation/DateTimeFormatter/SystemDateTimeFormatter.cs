using Files.Uwp.Extensions;
using System;

namespace Files.Uwp.ServicesImplementation.DateTimeFormatter
{
    internal class SystemDateTimeFormatter : AbstractDateTimeFormatter
    {
        public override string Name => "SystemTimeStyle".GetLocalizedResource();

        public override string ToShortLabel(DateTimeOffset offset)
        {
            if (offset.Year is <= 1601 or >= 9999)
            {
                return " ";
            }
            return offset.ToLocalTime().ToString("g");
        }
    }
}
