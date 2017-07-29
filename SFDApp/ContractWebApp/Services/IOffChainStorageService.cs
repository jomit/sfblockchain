using ContractWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractWebApp.Services
{
    public interface IOffChainStorageService
    {
        void SaveContractAddress(string address);

        string GetContractAddress();
    }
}
