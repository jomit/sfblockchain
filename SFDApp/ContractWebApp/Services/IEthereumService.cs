using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractWebApp.Services
{
    public interface IEthereumService
    {
        Task<string> DeployContract(string fromAddress);

        Task<string> PlaceOrder(string contractAddress, string fromAddress, string manufacturerAddress, string sku, int quantity);

        Task<string> ManufacturingComplete(string contractAddress, string fromAddress);

        Task<int> GetOrderStatus(string contractAddress, string fromAddress);
    }
}
