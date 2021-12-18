using BrokerDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BrokerDataAccess
{
    public class BrokerDBContext : DbContext
  {
    public BrokerDBContext(DbContextOptions<BrokerDBContext> options)
        : base(options) { }

    public DbSet<Equity> Equity { get; set; }
    public DbSet<Trader> Traders { get; set; }
    public DbSet<TraderEquity> TraderEquity { get; set; }
  }
}
