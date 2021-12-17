using BrokerDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace BrokerDataAccess
{
    [ExcludeFromCodeCoverage]
    public class DataGenerator
    {
      public static void Initialize(IServiceProvider serviceProvider)
      {
        using (var context = new BrokerDBContext(
            serviceProvider.GetRequiredService<DbContextOptions<BrokerDBContext>>()))
        {
          // Look for any Trader
          if (context.Traders.Any())
          {
            return;   // Data was already seeded
          }

          context.Traders.AddRange(
              new Trader
              {
                Id = 1,
                Name = "Trader1",
                Fund = 100
              },
              new Trader
              {
                Id = 2,
                Name = "Trader2",
                Fund = 1000
              },
              new Trader
              {
                Id = 3,
                Name = "Trader3",
                Fund = 1000
              },
              new Trader
              {
                Id = 4,
                Name = "Trader4",
                Fund = 20
              });

        context.Equity.AddRange(
           new Equity
           {
             Id = 1,
             Name = "Equity1",
             Value = 10
           },
           new Equity
           {
             Id = 2,
             Name = "Equity2",
             Value = 10
           },
           new Equity
           {
             Id = 3,
             Name = "Equity3",
             Value = 90
           },
           new Equity
           {
             Id = 4,
             Name = "Equity4",
             Value = 10
           });

        context.TraderEquity.AddRange(
           new TraderEquity
           {
             TraderEquityId = 1,
             TraderId = 1,
             EquityId = 2,
             NumberOfEquity = 9
           },
           new TraderEquity
           {
             TraderEquityId = 2,
             TraderId = 2,
             EquityId = 1,
             NumberOfEquity = 1
           },
           new TraderEquity
           {
             TraderEquityId = 3,
             TraderId = 1,
             EquityId = 3,
             NumberOfEquity = 10
           },
           new TraderEquity
           {
             TraderEquityId = 4,
             TraderId = 4,
             EquityId = 2,
             NumberOfEquity = 5
           });

        context.SaveChanges();
        }
      }
    }
}
