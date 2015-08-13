using System;

namespace Toolfactory.RequestTracker.Parser.Processor
{
    public sealed class DefaultFieldProcessor : GenericFieldProcessor
    {
        #region "Singleton"
        private static readonly Lazy<DefaultFieldProcessor> Singleton = new Lazy<DefaultFieldProcessor>(() => new DefaultFieldProcessor());
        public static DefaultFieldProcessor Instance { get { return Singleton.Value; } } 
        private DefaultFieldProcessor() {}
        #endregion

        public override void Process(object @object, string fieldName, string fieldValue)
        {
            CopyProperty(@object, fieldName, fieldValue);
        }
    }
}