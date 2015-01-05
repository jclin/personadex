using System;
using System.Threading.Tasks;

namespace Personadex.Suspension
{
    internal interface IAppState
    {
        DateTime? LastUtcPersistTime
        {
            get;
        }

        void WriteOutFrom<T>(string key, T value);

        bool ReadInto<T>(string key, ref T value);

        Task PersistStateAsync();

        Task<bool> RestoreStateAsync();
    }
}
