using System;
using System.Text.RegularExpressions;

namespace Toolfactory.RequestTracker.Parser.Processor
{
    public class LongFieldProcessor : GenericFieldProcessor
    {
        #region "Singleton"
        private static readonly Lazy<LongFieldProcessor> Singleton = new Lazy<LongFieldProcessor>(() => new LongFieldProcessor());
        public static LongFieldProcessor Instance { get { return Singleton.Value; } }
        private LongFieldProcessor() { }
        #endregion

        private static readonly Regex LongRegex = new Regex(@"^.*?(-??\d+).*?$");

        public override void Process(object ticket, string fieldName, string fieldValue)
        {
            var matches = LongRegex.Matches(fieldValue);
            if (matches.Count <= 0) return;
            var match = matches[0];
            var groupCollection = match.Groups;
            CopyProperty(ticket, fieldName, long.Parse(groupCollection[1].ToString()));
        }
    }
}