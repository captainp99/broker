using BrokerBussiness.Models;

namespace BrokerBussiness
{
    public interface IBroker
    {
        (bool success, string message) BuyEquities(BuyEquitiesRequest request);
        (bool success, string message) SellEquities(SellEquitiesRequest request);
        (bool success, string message) AddFund(AddFundRequest request);
    }
}
