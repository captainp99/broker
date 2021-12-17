using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrokerDataAccess.Models
{
  public class TraderEquity
  {
        public int TraderEquityId { get; set; }
        public int TraderId { get; set; }
        public int EquityId { get; set; }
        public double NumberOfEquity { get; set; }

    }
}
