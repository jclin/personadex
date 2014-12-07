using System;
using System.Collections.Generic;
using Personadex.Utils;

namespace Personadex.Collection
{
    internal sealed class BatchRetrievedEventArgs<T> : EventArgs where T : class
    {
        public IReadOnlyList<IndexItemPair<T>> ItemBatch
        {
            get;
            private set;
        }

        public BatchRetrievedEventArgs(IReadOnlyList<IndexItemPair<T>> itemBatch)
        {
            ItemBatch = itemBatch;
        }
    }
}