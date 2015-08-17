using System.Collections.Generic;

namespace Toolfactory.RequestTracker.Model
{
    public interface ICustomField
    {
        IDictionary<string, RtCustomField> CustomFields { get; }
    }
}