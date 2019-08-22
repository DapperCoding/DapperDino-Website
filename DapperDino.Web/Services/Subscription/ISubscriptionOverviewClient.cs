﻿using System.Threading.Tasks;
using DapperDino.Models.MollieModels;
using Mollie.Api.Models.Subscription;

namespace DapperDino.Services.Subscription {
    public interface ISubscriptionOverviewClient {
        Task<OverviewModel<SubscriptionResponse>> GetList(string customerId);
        Task<OverviewModel<SubscriptionResponse>> GetListByUrl(string url);
    }
}