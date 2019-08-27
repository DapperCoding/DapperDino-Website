using System.Threading.Tasks;
using AutoMapper;
using DapperDino.Core.Mollie;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.PaymentMethod;

namespace DapperDino.Services.PaymentMethod {
    public class PaymentMethodOverviewClient : OverviewClientBase<PaymentMethodResponse>, IPaymentMethodOverviewClient {
        private readonly IPaymentMethodClient _paymentMethodClient;

        public PaymentMethodOverviewClient(IMapper mapper, IPaymentMethodClient paymentMethodClient) : base(mapper) {
            this._paymentMethodClient = paymentMethodClient;
        }

        public async Task<OverviewModel<PaymentMethodResponse>> GetList() {
            return this.Map(await this._paymentMethodClient.GetPaymentMethodListAsync());
        }
    }
}