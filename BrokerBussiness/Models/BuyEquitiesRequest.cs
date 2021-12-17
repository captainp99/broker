using System;
using System.Collections.Generic;
using System.Text;

namespace BrokerBussiness.Models
{
    public class BuyEquitiesRequest
    {
        public int TraderId { get; set; }
        public int EquityId { get; set; }
        public double NumberOfEquity { get; set; }
        public DateTime RequestDateTime { get; set; }
    }
}
