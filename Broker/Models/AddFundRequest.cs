using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class AddFundRequest
    {
        public int TraderId { get; set; }
        public double FundToBeAdded { get; set; }
    }
}
