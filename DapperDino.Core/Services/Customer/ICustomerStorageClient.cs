using DapperDino.Core.Mollie;
using System.Threading.Tasks;

namespace DapperDino.Services.Customer {
    public interface ICustomerStorageClient {
        Task Create(CreateCustomerModel model);
    }
}