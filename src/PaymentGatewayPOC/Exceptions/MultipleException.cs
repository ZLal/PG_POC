using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace PaymentGatewayPOC.Exceptions;

[Serializable]
public class MultipleException : Exception
{
    public IReadOnlyList<Exception> InnerExceptions { get; }

    public MultipleException()
    {
        InnerExceptions = [];
    }

    public MultipleException(string message)
        : base(message)
    {
        InnerExceptions = [];
    }

    public MultipleException(string message, Exception innerException)
        : base(message, innerException)
    {
        InnerExceptions = [innerException];
    }

    public MultipleException(string message, IEnumerable<Exception> innerExceptions)
        : base(message, innerExceptions?.FirstOrDefault())
    {
        InnerExceptions = innerExceptions?.ToList().AsReadOnly() ?? new List<Exception>().AsReadOnly();
    }

    protected MultipleException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        var exceptions = (Exception[]?)info.GetValue(nameof(InnerExceptions), typeof(Exception[]));
        InnerExceptions = new ReadOnlyCollection<Exception>(exceptions ?? []);
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(InnerExceptions), InnerExceptions.ToArray(), typeof(Exception[]));
    }
}
