using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractWebApp.Services
{
    public class EthereumService : IEthereumService
    {
        public async Task<string> DeployContract(string fromAddress)
        {
            var web3 = new Web3();
            var unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, GetPassword(), 60);

            var transactionHash = await web3.Eth.DeployContract.SendRequestAsync(GetContractAbi(), GetContractByteCode(), fromAddress, new HexBigInteger(4700000));
            var receipt = await MineAndGetReceiptAsync(web3, transactionHash);
            return receipt.ContractAddress;
        }

        public async Task<string> PlaceOrder(string contractAddress, string fromAddress, string manufacturerAddress, string sku, int quantity)
        {
            var web3 = new Web3();
            var unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, GetPassword(), 60);

            var contract = web3.Eth.GetContract(GetContractAbi(), contractAddress);
            var placeOrderFunction = contract.GetFunction("placeOrder");

            var transactionHash = await placeOrderFunction.SendTransactionAsync(fromAddress, new HexBigInteger(900000), null, manufacturerAddress, sku, quantity);
            var receipt = await MineAndGetReceiptAsync(web3, transactionHash);

            return await placeOrderFunction.CallAsync<string>();
        }

        public async Task<string> ManufacturingComplete(string contractAddress, string fromAddress)
        {
            var web3 = new Web3();

            var unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, GetPassword(), 60);
            var contract = web3.Eth.GetContract(GetContractAbi(), contractAddress);
            var manufacturingDoneFunction = contract.GetFunction("manufacturingComplete");

            var transactionHash = await manufacturingDoneFunction.SendTransactionAsync(fromAddress, new HexBigInteger(900000), null);
            var receipt = await MineAndGetReceiptAsync(web3, transactionHash);

            return await manufacturingDoneFunction.CallAsync<string>();
        }

        public async Task<int> GetOrderStatus(string contractAddress, string fromAddress)
        {
            var web3 = new Web3();
            var unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(fromAddress, GetPassword(), 60);
            var contract = web3.Eth.GetContract(GetContractAbi(), contractAddress);
            var getOrderStatusFunction = contract.GetFunction("getOrderStatus");

            var transactionHash = await getOrderStatusFunction.SendTransactionAsync(fromAddress, new HexBigInteger(900000), null);
            var receipt = await MineAndGetReceiptAsync(web3, transactionHash);

            return await getOrderStatusFunction.CallAsync<int>();
        }

        private string GetContractAbi()
        {
            return @"[{""constant"":false,""inputs"":[{""name"":""orderManufacturer"",""type"":""address""},{""name"":""orderSku"",""type"":""string""},{""name"":""orderQuantity"",""type"":""uint256""}],""name"":""placeOrder"",""outputs"":[],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[],""name"":""manufacturingComplete"",""outputs"":[],""payable"":false,""type"":""function""},{""constant"":false,""inputs"":[],""name"":""getOrderStatus"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""type"":""function""},{""inputs"":[],""payable"":false,""type"":""constructor""}]";
        }

        private string GetContractByteCode()
        {
            var hex = "0x";
            return hex + "6060604052341561000f57600080fd5b5b33600160006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055505b5b6103a5806100626000396000f30060606040526000357c0100000000000000000000000000000000000000000000000000000000900463ffffffff16806321d4209c14610054578063d37c8470146100d9578063d81d26ae146100ee575b600080fd5b341561005f57600080fd5b6100d7600480803573ffffffffffffffffffffffffffffffffffffffff1690602001909190803590602001908201803590602001908080601f01602080910402602001604051908101604052809392919081815260200183838082843782019150505050505091908035906020019091905050610117565b005b34156100e457600080fd5b6100ec6101fb565b005b34156100f957600080fd5b6101016102b1565b6040518082815260200191505060405180910390f35b600160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614151561017357600080fd5b826000806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555081600290805190602001906101c99291906102d4565b50806003819055506000600460006101000a81548160ff021916908360018111156101f057fe5b02179055505b505050565b6000809054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614151561025657600080fd5b6000600181111561026357fe5b600460009054906101000a900460ff16600181111561027e57fe5b14151561028a57600080fd5b6001600460006101000a81548160ff021916908360018111156102a957fe5b02179055505b565b6000600460009054906101000a900460ff1660018111156102ce57fe5b90505b90565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061031557805160ff1916838001178555610343565b82800160010185558215610343579182015b82811115610342578251825591602001919060010190610327565b5b5090506103509190610354565b5090565b61037691905b8082111561037257600081600090555060010161035a565b5090565b905600a165627a7a723058207d7d9aa3a5c0e3a5d67605ae2d2b5d302ddf34cdb717225f585f68b322864e5c0029";
        }

        private string GetPassword()
        {
            return "password";
        }

        private async Task<TransactionReceipt> MineAndGetReceiptAsync(Web3 web3, string transactionHash)
        {
            var miningResult = await web3.Miner.Start.SendRequestAsync(6);
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }
            miningResult = await web3.Miner.Stop.SendRequestAsync();
            return receipt;
        }
    }
}
