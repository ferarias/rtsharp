using System;
using System.Globalization;

namespace Toolfactory.RequestTracker.Parser.Processor
{
    class DateTimeFieldProcessor : GenericFieldProcessor
    {
        #region "Singleton"
        private static readonly Lazy<DateTimeFieldProcessor> Singleton = new Lazy<DateTimeFieldProcessor>(() => new DateTimeFieldProcessor());
        public static DateTimeFieldProcessor Instance { get { return Singleton.Value; } }
        private DateTimeFieldProcessor() { }
        #endregion

        public override void Process(object ticket, string fieldName, string fieldValue)
        {
            DateTime myDate;
            if (DateTime.TryParseExact(fieldValue, "ddd MMM dd HH:mm:ss yyyy", 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out myDate))
                CopyProperty(ticket, fieldName, myDate);

        }
    }
}
