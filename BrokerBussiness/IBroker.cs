

using BrokerBussiness.Models;
using BrokerDataAccess.Models;
using System.Collections.Generic;

namespace BrokerBussiness
{
    public interface IBroker
    {
        (bool success, string message) BuyEquities(BuyEquitiesRequest request);
        (bool success, string message) SellEquities(SellEquitiesRequest request);
        (bool success, string message) AddFund(AddFundRequest request);
        //List<Trader> GetAllTraderWithEquities();
    }
}
