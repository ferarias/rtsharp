using System.Collections.Generic;

namespace Toolfactory.RequestTracker.Model
{
    public interface IRtCustomFieldObject
    {
        IDictionary<string, RtCustomField> CustomFields { get; }
    }
}