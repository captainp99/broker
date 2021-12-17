using System;
using System.Collections.Generic;
using System.Text;

namespace BrokerDataAccess.Models
{
  public class Equity
  {
        public Equity()
        {
            TraderEquities = new HashSet<TraderEquity>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public virtual ICollection<TraderEquity> TraderEquities { get; set; }
    }
}
