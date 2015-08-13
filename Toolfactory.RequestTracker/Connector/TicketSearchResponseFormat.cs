using System;
using System.Collections.Generic;
using System.Linq;

namespace Toolfactory.RequestTracker.Connector
{
    public sealed class TicketSearchResponseFormat
    {
        private static readonly TicketSearchResponseFormat IdOnly = new TicketSearchResponseFormat("IDONLY", InnerEnum.IdOnly, "i");
        private static readonly TicketSearchResponseFormat IdAndSubject = new TicketSearchResponseFormat("IDANDSUBJECT", InnerEnum.IdAndSubject, "s");
        public static readonly TicketSearchResponseFormat MultiLine = new TicketSearchResponseFormat("MULTILINE", InnerEnum.MultiLine, "l");
        private static readonly IList<TicketSearchResponseFormat> ValueList = new List<TicketSearchResponseFormat>();
        private static int _nextOrdinal = 0;
        private readonly InnerEnum _innerEnumValue;
        private readonly string _nameValue;
        private readonly int _ordinalValue;

        static TicketSearchResponseFormat()
        {
            ValueList.Add(IdOnly);
            ValueList.Add(IdAndSubject);
            ValueList.Add(MultiLine);
        }

        private TicketSearchResponseFormat(string name, InnerEnum innerEnum, string formatString)
        {
            FormatString = formatString;
            _nameValue = name;
            _ordinalValue = _nextOrdinal++;
            _innerEnumValue = innerEnum;
        }

        public string FormatString { get; private set; }

        private static IEnumerable<TicketSearchResponseFormat> Values()
        {
            return ValueList;
        }

        public InnerEnum InnerEnumValue()
        {
            return _innerEnumValue;
        }

        public int Ordinal()
        {
            return _ordinalValue;
        }

        public static TicketSearchResponseFormat ValueOf(string name)
        {
            foreach (var enumInstance in Values().Where(enumInstance => enumInstance._nameValue == name))
            {
                return enumInstance;
            }
            throw new ArgumentException(name);
        }

        public override string ToString()
        {
            return _nameValue;
        }

        public enum InnerEnum
        {
            IdOnly,
            IdAndSubject,
            MultiLine
        }


    }
}