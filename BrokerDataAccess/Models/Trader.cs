using System;
using System.Collections.Generic;
using System.Text;

namespace BrokerDataAccess.Models
{
    public class Trader
    {
        public Trader()
        {
            TraderEquities = new HashSet<TraderEquity>();
        }
        public int Id { get; set; }
          public string Name { get; set; }
          public double Fund { get; set; }

          public virtual ICollection<TraderEquity> TraderEquities { get; set; }
    }
}
