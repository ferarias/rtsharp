using System;
using Toolfactory.RequestTracker.Helper;

namespace Toolfactory.RequestTracker.Parser.Processor
{
    public abstract class GenericFieldProcessor : IFieldProcessor
	{
		public abstract void Process(object obj, string fieldName, string fieldValue);

		protected static void CopyProperty(object obj, string name, object value)
		{
			try
			{
                ReflectionHelper.SetProperty(obj, name.UppercaseFirst(), value);
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex);
			}
		}

	}

}