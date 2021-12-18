namespace Broker.Models
{
    public class BuyEquitiesRequest
    {
        public int TraderId { get; set; }
        public int EquityId { get; set; }
        public double NumberOfEquity { get; set; }
    }
}
