using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractWebApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ContractWebApp.Services
{
    public class SessionStorageService : IOffChainStorageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionStorageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetContractAddress()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("SupplyChainContract");
        }

        public void SaveContractAddress(string address)
        {
            _httpContextAccessor.HttpContext.Session.SetString("SupplyChainContract", address);
        }
    }
}
