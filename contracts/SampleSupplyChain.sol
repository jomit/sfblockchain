pragma solidity ^0.4.10;

contract SampleSupplyChain {
    address manufacturer;
    address client;
    string sku;
    uint quantity;
    OrderStatus orderStatus;
    
    enum OrderStatus{OrderPlaced, ManufacturingComplete}

    function SampleSupplyChain(){
        client =  msg.sender;
    }

    function placeOrder(address orderManufacturer, string orderSku, uint orderQuantity) {
        if(msg.sender != client){
           throw;
        }
        manufacturer = orderManufacturer;
        sku = orderSku;
        quantity = orderQuantity;
        orderStatus = OrderStatus.OrderPlaced;
    }

    function manufacturingComplete(){

       if(msg.sender != manufacturer){
           throw;
       }
       if (orderStatus != OrderStatus.OrderPlaced){
           throw;
       }
       orderStatus = OrderStatus.ManufacturingComplete;
    }

    function getOrderStatus() returns (uint){
       return uint(orderStatus);
    }
}