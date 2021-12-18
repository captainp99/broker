using BrokerDataAccess;
using BrokerDataAccess.Models;
using BrokerDataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BrokerDataAccessTests
{
    public class RepositoryTests
    {
        public Repository<Trader> _repository;
        public BrokerDBContext _context;

        private Repository<Trader> Seed()
        {
            var options = new DbContextOptionsBuilder<BrokerDBContext>()
                .UseInMemoryDatabase(databaseName: "BrokerDBContext")
                .Options;


            // Traders 
            var traders = new List<Trader>()
                {
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
                      }
                };

            _context = new BrokerDBContext(options);
            _context.Traders.RemoveRange(_context.Traders);
            _context.Traders.AddRange(traders);
            _context.SaveChanges();
            _repository = new Repository<Trader>(_context);
            return _repository;
        }

        [Fact]
        public void When_GetAll_Then_Return_All_Traders()
        {
            // Arrange
            Seed();

            // Act
            var result = _repository.GetAll().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void When_Get_Then_Return_Traders()
        {
            // Arrange
            Seed();

            // Act
            var result = _repository.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void When_Add_Then_Return_Added_Trader()
        {
            // Arrange
            Seed();

            // Act
            var result = _repository.Add(new Trader()
            {
                Id = 5,
                Name = "Trader5",
                Fund = 20
            });

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void When_Update_Then_Success()
        {
            // Arrange
            Seed();
            var trader = _context.Traders.First();
            trader.Fund = 20;

            // Act
            var result = _repository.Update(trader, trader.Id);

            // Assert
            Assert.Equal(20, result.Fund);
        }

        [Fact]
        public void Update_return_null_if_object_null()
        {
            // Arrange
            Seed();
            Trader trader = null;

            // Act
            var result = _repository.Update(trader);

            // Assert
            Assert.Null(result);
        }
    }
}
