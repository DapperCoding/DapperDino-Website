using System.Threading.Tasks;
using DapperDino.Core.Mollie;
using Mollie.Api.Models.Mandate;

namespace DapperDino.Services.Mandate {
    public interface IMandateOverviewClient {
        Task<OverviewModel<MandateResponse>> GetList(string customerId);
        Task<OverviewModel<MandateResponse>> GetListByUrl(string url);
    }
}