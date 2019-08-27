using System.Threading.Tasks;
using DapperDino.Core.Mollie;
using Mollie.Api.Models.Customer;

namespace DapperDino.Services.Customer {
    public interface ICustomerOverviewClient {
        Task<OverviewModel<CustomerResponse>> GetList();
        Task<OverviewModel<CustomerResponse>> GetListByUrl(string url);
    }
}