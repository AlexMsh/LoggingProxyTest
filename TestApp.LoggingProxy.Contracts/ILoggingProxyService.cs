using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestApp.LoggingProxy.Contracts
{
    public interface ILoggingProxyService<in T,  TT>
    {
        Task<IEnumerable<TT>> GetMessagesAsync();

        Task<IEnumerable<TT>> PostMessageAsync(T records);
    }
}
