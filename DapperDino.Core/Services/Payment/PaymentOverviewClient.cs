using System.Threading.Tasks;
using AutoMapper;
using DapperDino.Core.Mollie;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Payment.Response;

namespace DapperDino.Services.Payment {
    public class PaymentOverviewClient : OverviewClientBase<PaymentResponse>, IPaymentOverviewClient {
        private readonly IPaymentClient _paymentClient;

        public PaymentOverviewClient(IMapper mapper, IPaymentClient paymentClient) : base(mapper) {
            this._paymentClient = paymentClient;
        }

        public async Task<OverviewModel<PaymentResponse>> GetList()
        {
            return this.Map(await this._paymentClient.GetPaymentListAsync());
        }

        public async Task<OverviewModel<PaymentResponse>> GetListByUrl(string url)
        {
            return this.Map(await this._paymentClient.GetPaymentListAsync(this.CreateUrlObject(url)));
        }
    }
}