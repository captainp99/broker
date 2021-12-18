using System.Collections.Generic;

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
