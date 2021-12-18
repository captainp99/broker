namespace Broker.Models
{
    public class SellEquitiesRequest
    {
        public int TraderId { get; set; }
        public int EquityId { get; set; }
        public double NumberOfEquity { get; set; }
    }
}
