using Windows.Foundation.Collections;
using Personadex.Suspension;

namespace Personadex.Collection
{
    internal interface IVirtualizingVector<T> : IObservableVector<object>, IJsonSerializable
        where T : class, IJsonSerializable
    {
    }
}
