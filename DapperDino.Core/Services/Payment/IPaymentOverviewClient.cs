﻿using System.Threading.Tasks;
using DapperDino.Core.Mollie;
using Mollie.Api.Models.Payment.Response;
namespace DapperDino.Services.Payment {
    public interface IPaymentOverviewClient {
        Task<OverviewModel<PaymentResponse>> GetList();
        Task<OverviewModel<PaymentResponse>> GetListByUrl(string url);
    }
}