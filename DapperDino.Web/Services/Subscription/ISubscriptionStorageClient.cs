using System.Threading.Tasks;
using DapperDino.Models.MollieModels;

namespace DapperDino.Services.Subscription {
    public interface ISubscriptionStorageClient {
        Task Create(CreateSubscriptionModel model);
    }
}