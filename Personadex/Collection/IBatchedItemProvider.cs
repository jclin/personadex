using System;

namespace Personadex.Collection
{
    internal interface IBatchedItemProvider<T> where T : class
    {
        void GetCountAsync();
        void RetrieveBatchForItemAsync(int vectorIndex);

        event EventHandler<long> CountRetrieved;
        event EventHandler<BatchRetrievedEventArgs<T>> ItemBatchRetrieved;
    }
}
