using System.Threading.Tasks;
using DapperDino.Models.MollieModels;
using Mollie.Api.Models.PaymentMethod;

namespace DapperDino.Services.PaymentMethod {
    public interface IPaymentMethodOverviewClient {
        Task<OverviewModel<PaymentMethodResponse>> GetList();
    }
}