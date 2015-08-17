using System;
using Toolfactory.RequestTracker.Model;

namespace Toolfactory.RequestTracker.Parser.Processor
{
    public sealed class DefaultCustomFieldProcessor : IFieldProcessor
    {
        #region "Singleton"
        private static readonly Lazy<DefaultCustomFieldProcessor> Singleton = new Lazy<DefaultCustomFieldProcessor>(() => new DefaultCustomFieldProcessor());
        public static DefaultCustomFieldProcessor Instance { get { return Singleton.Value; } }
        private DefaultCustomFieldProcessor() { }
        #endregion
        
        public void Process(object @object, string fieldName, string fieldValue)
        {
            var m = RtCustomField.CustomFieldNameRegex.Matches(fieldName);

            if (m.Count <= 0 || (!(@object is ICustomField))) return;
            var cfObject = (ICustomField) @object;
            var match = m[0]; // only one match in this case
            var groupCollection = match.Groups;
            cfObject.CustomFields[groupCollection[1].ToString()] = new RtCustomField(groupCollection[1].ToString(), fieldValue);
        }
    }
}