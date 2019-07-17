using System.Threading.Tasks;
using AutoMapper;
using DapperDino.Models.MollieModels;
using DapperDino.Services;
using DapperDino.Services.Subscription;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Subscription;
using Mollie.Api.Models.Url;

namespace Mollie.WebApplicationCoreExample.Services.Subscription {
    public class SubscriptionOverviewClient : OverviewClientBase<SubscriptionResponse>, ISubscriptionOverviewClient {
        private readonly ISubscriptionClient _subscriptionClient;

        public SubscriptionOverviewClient(IMapper mapper, ISubscriptionClient subscriptionClient) : base(mapper) {
            this._subscriptionClient = subscriptionClient;
        }

        public async Task<OverviewModel<SubscriptionResponse>> GetList(string customerId) {
            return this.Map(await this._subscriptionClient.GetSubscriptionListAsync(customerId));
        }

        public async Task<OverviewModel<SubscriptionResponse>> GetListByUrl(string url) {
            return this.Map(await this._subscriptionClient.GetSubscriptionListAsync(this.CreateUrlObject(url)));
        }
    }
}