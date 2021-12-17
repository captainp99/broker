using Broker.Models;
using BrokerBussiness;
using BrokerDataAccess;
using BrokerDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrokerController : ControllerBase
    {
        private readonly IBroker _broker;

        public BrokerController(IBroker broker)
        {
            _broker = broker;
        }

        //[HttpGet("getall")]
        //public List<Trader> GetAll()
        //{
        //    return _broker.GetAllTraderWithEquities();
        //}

        [HttpPost("buy-equities")]
        public IActionResult BuyEquities([FromBody] BuyEquitiesRequest request)
        {
            var req = new BrokerBussiness.Models.BuyEquitiesRequest()
            {
                EquityId = request.EquityId,
                TraderId = request.TraderId,
                NumberOfEquity = request.NumberOfEquity,
                RequestDateTime = DateTime.Now
            };
            var (success, message) = _broker.BuyEquities(req);
            if (!success) return BadRequest(message);
            return Ok(message);
        }

        [HttpPost("sell-equities")]
        public IActionResult SellEquities([FromBody] SellEquitiesRequest request)
        {
            var req = new BrokerBussiness.Models.SellEquitiesRequest()
            {
                EquityId = request.EquityId,
                TraderId = request.TraderId,
                NumberOfEquity = request.NumberOfEquity,
                RequestDateTime = DateTime.Now
            };
            var (success, message) = _broker.SellEquities(req);
            if (!success) return BadRequest(message);
            return Ok(message);
        }

        [HttpPost("add-funds")]
        public IActionResult AddFunds([FromBody] AddFundRequest request)
        {
            var req = new BrokerBussiness.Models.AddFundRequest()
            {
                TraderId = request.TraderId,
                FundToBeAdded = request.FundToBeAdded
            };
            var (success, message) = _broker.AddFund(req);
            if (!success) return BadRequest(message);
            return Ok(message);
        }
    }
}
