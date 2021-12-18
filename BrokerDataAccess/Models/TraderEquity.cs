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
