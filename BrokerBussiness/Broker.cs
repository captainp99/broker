using BrokerBussiness.Models;
using BrokerDataAccess.Models;
using BrokerDataAccess.Repository;
using System.Linq;

namespace BrokerBussiness
{
    public class Broker : IBroker
    {
        private readonly IRepository<Equity> _equityRepository;
        private readonly IRepository<Trader> _traderRepository;
        private readonly IRepository<TraderEquity> _traderEquityRepository;
        public Broker(IRepository<Equity> equityRepository, IRepository<Trader> traderRepository, IRepository<TraderEquity> traderEquityRepository)
        {
            _equityRepository = equityRepository;
            _traderRepository = traderRepository;
            _traderEquityRepository = traderEquityRepository;
        }

        public (bool success, string message) BuyEquities(BuyEquitiesRequest request)
        {
            // Check for correct time and day
            if (!Utility.IsValidDayAndTime(request.RequestDateTime)) return (false, "Not a valid day or time");

            var equity = _equityRepository.Get(request.EquityId);
            var trader = _traderRepository.Get(request.TraderId);

            // Check for quity and trader
            if (equity == null || trader == null) return (false, "trader or equity not exist");

            // Check for enough fund to buy
            if (trader.Fund < equity.Value * request.NumberOfEquity) return (false, "not enough fund");

            var allTraderEquity = _traderEquityRepository.GetAll().ToList();
            var traderEquity = allTraderEquity.Where(x => x.TraderId == request.TraderId && x.EquityId == request.EquityId).FirstOrDefault();
            if(traderEquity == null)
            {
                var maxId = allTraderEquity.Max(x => x.TraderEquityId);
                var traderEquityData = new TraderEquity { TraderId = request.TraderId, EquityId = request.EquityId, NumberOfEquity = request.NumberOfEquity, TraderEquityId = maxId + 1 };
                _traderEquityRepository.Add(traderEquityData);
            }
            else
            {
                traderEquity.NumberOfEquity += request.NumberOfEquity;
                _traderEquityRepository.Update(traderEquity, traderEquity.TraderEquityId);
            }
            var sumToBeDeducted = equity.Value * request.NumberOfEquity;
            trader.Fund -= sumToBeDeducted;
            _traderRepository.Update(trader, trader.Id);
            return (true, "success");
        }

        public (bool success, string message) SellEquities(SellEquitiesRequest request)
        {
            // Check for correct time and day
            if (!Utility.IsValidDayAndTime(request.RequestDateTime)) return (false, "Not a valid day or time");

            var equity = _equityRepository.Get(request.EquityId);
            var trader = _traderRepository.Get(request.TraderId);

            // Check for quity and trader
            if (equity == null || trader == null) return (false, "trader or equity not exist");

            var allTraderEquity = _traderEquityRepository.GetAll().ToList();
            var traderEquity = allTraderEquity.Where(x => x.TraderId == request.TraderId && x.EquityId == request.EquityId).FirstOrDefault();

            // Check if trader has enough equity 
            if (traderEquity == null || traderEquity.NumberOfEquity < request.NumberOfEquity) return (false, "trader doesnot have enough holding of equity");

            var sumToBeAdded = equity.Value * request.NumberOfEquity;
            var deduction = sumToBeAdded * 0.0005 > 20 ? sumToBeAdded * 0.0005 : 20;
            sumToBeAdded -= deduction;
            trader.Fund += sumToBeAdded;

            // Check if balance will reach to negative after deducting brockerage
            if (trader.Fund < 0) return (false, "your balance will reach to negative after deducting brockerage");

            traderEquity.NumberOfEquity -= request.NumberOfEquity;
            _traderEquityRepository.Update(traderEquity, traderEquity.TraderEquityId);

            _traderRepository.Update(trader, trader.Id);
            return (true, "success");
        }

        public (bool success, string message) AddFund(AddFundRequest request)
        {
            var trader = _traderRepository.Get(request.TraderId);

            // Check for trader
            if (trader == null) return (false, "trader not exist");

            if (request.FundToBeAdded > 100000) request.FundToBeAdded -= request.FundToBeAdded * 0.0005;
            
            trader.Fund += request.FundToBeAdded;
            _traderRepository.Update(trader, trader.Id);
            return (true, "success");
        }
    }
}
