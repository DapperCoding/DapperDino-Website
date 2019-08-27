using DapperDino.Core.Mollie;
using Mollie.Api.Models.Payment.Response;
using System.Threading.Tasks;

namespace DapperDino.Services.Payment {
    public interface IPaymentStorageClient {
        Task<PaymentResponse> Create(CreatePaymentModel model, int orderId);
        Task<PaymentResponse> GetById(string paymentId);
    }
}