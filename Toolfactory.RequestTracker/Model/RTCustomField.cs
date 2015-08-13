using System.Text.RegularExpressions;

namespace Toolfactory.RequestTracker.Model
{
    public class RtCustomField
    {
        public static readonly Regex CustomFieldNameRegex = new Regex("CF.\\{(.*)\\}");

        public string Name { get; set; }
        public object Value { get; set; }

        public RtCustomField(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return "RTCustomField [name=" + Name + ", value=" + Value + "]";
        }
    }
}