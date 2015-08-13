namespace Toolfactory.RequestTracker.Parser.Processor
{
    public interface IFieldProcessor
    {
        void Process(object obj, string fieldName, string fieldValue);
    }
}