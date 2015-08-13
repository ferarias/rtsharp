using System;
using System.Collections.Generic;
using Toolfactory.RequestTracker.Helper;
using Toolfactory.RequestTracker.Model;

namespace Toolfactory.RequestTracker.Parser.Processor
{
    public sealed class FieldProcessorRegistry
    {
        #region "Singleton"
        private static readonly Lazy<FieldProcessorRegistry> Singleton = new Lazy<FieldProcessorRegistry>(() => new FieldProcessorRegistry());
        public static FieldProcessorRegistry Instance { get { return Singleton.Value; } } 
        #endregion

        private readonly IDictionary<string, IFieldProcessor> _customFieldProcessors;
        private readonly IDictionary<string, IFieldProcessor> _standardFieldProcessors;
        private readonly IDictionary<Type, IFieldProcessor> _typeFieldProcessors;

        private FieldProcessorRegistry()
        {
            _typeFieldProcessors = new Dictionary<Type, IFieldProcessor>()
            {
                {typeof (long?), LongFieldProcessor.Instance},
                {typeof (DateTime?), DateTimeFieldProcessor.Instance},
                {typeof (string), DefaultFieldProcessor.Instance}
            };
            _standardFieldProcessors = new Dictionary<string, IFieldProcessor>();
            _customFieldProcessors = new Dictionary<string, IFieldProcessor>();
        }


        public IFieldProcessor GetTicketFieldProcessor(RtTicket ticket, string ticketFieldName)
        {
            IFieldProcessor fieldProcessor = null;

            var m = RtCustomField.CustomFieldNameRegex.Matches(ticketFieldName);
            if (m.Count > 0)
            {
                var match = m[0];
                var groupCollection = match.Groups;
                fieldProcessor = _customFieldProcessors[groupCollection[1].ToString()] ?? DefaultCustomFieldProcessor.Instance;
            }
            else
            {
                if (_standardFieldProcessors.ContainsKey(ticketFieldName))
                    fieldProcessor = _standardFieldProcessors[ticketFieldName];
                if (fieldProcessor != null) return fieldProcessor;

                try
                {
                    var p = typeof(RtTicket).GetProperty(ticketFieldName.UppercaseFirst());
                    var t = p.PropertyType;
                    fieldProcessor = _typeFieldProcessors[t];
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception while processing property '{0}'", ticketFieldName.UppercaseFirst());
                }
            }


            return fieldProcessor ?? DefaultFieldProcessor.Instance;
        }
    }
}