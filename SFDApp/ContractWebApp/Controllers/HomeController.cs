using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Nethereum.RPC.Eth.DTOs;
using ContractWebApp.Models;
using ContractWebApp.Services;

namespace ContractWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOffChainStorageService db;
        private readonly IEthereumService ethereum;
        private string userAccountAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
        private string manufacturerAddress = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
        public HomeController(IOffChainStorageService storageService, IEthereumService ethereumService)
        {
            db = storageService;
            ethereum = ethereumService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeployContract()
        {
            var userAccountAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var contractAddress = await ethereum.DeployContract(userAccountAddress);

            db.SaveContractAddress(contractAddress);

            ViewBag.Result = "Contract Deployed at => " + contractAddress;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var contractAddress = db.GetContractAddress();

            var result = await ethereum.PlaceOrder(contractAddress, userAccountAddress, manufacturerAddress, "Product1", 100);
            ViewBag.Result = "Order Placed";

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ManufacturingComplete()
        {
            var contractAddress = db.GetContractAddress();
            var result = await ethereum.ManufacturingComplete(contractAddress, manufacturerAddress);
            ViewBag.Result = "Manufacturing Complete";

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CheckOrderStatus()
        {
            var contractAddress = db.GetContractAddress();
            var result = await ethereum.GetOrderStatus(contractAddress, userAccountAddress);
            ViewBag.Result = "Order Status => " + result + "    [0 = OrderPlaced | 1 = ManufacturingComplete]";

            return View("Index");
        }
    }
}
