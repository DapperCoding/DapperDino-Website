using System.Threading.Tasks;
using AutoMapper;
using DapperDino.Core.Mollie;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace DapperDino.Services.Payment {
    public class PaymentStorageClient : IPaymentStorageClient {
        private readonly IPaymentClient _paymentClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentStorageClient(IPaymentClient paymentClient, IMapper mapper, IConfiguration configuration) {
            this._paymentClient = paymentClient;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public async Task<PaymentResponse> Create(CreatePaymentModel model, int orderId) {
            PaymentRequest paymentRequest = this._mapper.Map<PaymentRequest>(model);
            paymentRequest.RedirectUrl = $"{this._configuration["DefaultRedirectUrl"]}Client/Orders/{orderId}";

            return await this._paymentClient.CreatePaymentAsync(paymentRequest);
        }

        public async Task<PaymentResponse> GetById(string paymentId)
        {
            return await this._paymentClient.GetPaymentAsync(paymentId);
        }
    }
}