using System;
using System.Collections.Generic;
using System.Text;

namespace BrokerBussiness.Models
{
    public class AddFundRequest
    {
        public int TraderId { get; set; }
        public double FundToBeAdded { get; set; }
    }
}
