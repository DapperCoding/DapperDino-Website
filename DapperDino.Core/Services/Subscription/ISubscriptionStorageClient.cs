using System.Threading.Tasks;
using DapperDino.Core.Mollie;

namespace DapperDino.Services.Subscription {
    public interface ISubscriptionStorageClient {
        Task Create(CreateSubscriptionModel model);
    }
}