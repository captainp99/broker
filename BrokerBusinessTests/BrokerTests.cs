using BrokerBussiness;
using BrokerBussiness.Models;
using BrokerDataAccess.Models;
using BrokerDataAccess.Repository;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace BrokerBusinessTests
{
    public class BrokerTests
    {
        private readonly Mock<IRepository<Equity>> _equityRepository;
        private readonly Mock<IRepository<Trader>> _traderRepository;
        private readonly Mock<IRepository<TraderEquity>> _traderEquityRepository;
        private readonly Broker _brocker;

        public BrokerTests()
        {
            _equityRepository = new Mock<IRepository<Equity>>(); ;
            _traderRepository = new Mock<IRepository<Trader>>();
            _traderEquityRepository = new Mock<IRepository<TraderEquity>>();
            _brocker = new Broker(_equityRepository.Object, _traderRepository.Object, _traderEquityRepository.Object);

        }

        [Fact]
        public void BuyEquities_should_return_error_if_invalid_dateTime()
        {
            // Act + Arrange
            var (success, message) = _brocker.BuyEquities(new BuyEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 18, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("Not a valid day or time", message);
        }

        [Fact]
        public void BuyEquities_should_return_error_if_enquity_not_exist()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(null as Equity);
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader());

            // Act 
            var (success, message) = _brocker.BuyEquities(new BuyEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("trader or equity not exist", message);
        }

        [Fact]
        public void BuyEquities_should_return_error_if_trader_not_exist()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity());
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(null as Trader);

            // Act 
            var (success, message) = _brocker.BuyEquities(new BuyEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("trader or equity not exist", message);
        }

        [Fact]
        public void BuyEquities_should_return_error_if_trader_does_not_have_sufficient_fund()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 10});
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 5});

            // Act
            var (success, message) = _brocker.BuyEquities(new BuyEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("not enough fund", message);
        }

        [Fact]
        public void BuyEquities_should_return_success_when_add_new_equity_for_trader()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 10, Name = "ICICI" });
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 500, Name = "B" });
            _traderEquityRepository.Setup(x => x.GetAll()).Returns(new List<TraderEquity>() { new TraderEquity { TraderId = 1, EquityId = 2,  NumberOfEquity = 1, TraderEquityId = 1} });

            // Act
            var (success, message) = _brocker.BuyEquities(new BuyEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.True(success);
            Assert.Equal("success", message);
            _traderEquityRepository.Verify(x => x.Add(It.IsAny<TraderEquity>()), Times.Once);
            _traderEquityRepository.Verify(x => x.Update(It.IsAny<TraderEquity>(), It.IsAny<int>()), Times.Never);
            _traderRepository.Verify(x => x.Update(It.IsAny<Trader>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void BuyEquities_should_return_success_when_add_new_equity_for_trader_2()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 10, Name = "ICICI" });
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 500, Name = "B" });
            _traderEquityRepository.Setup(x => x.GetAll()).Returns(new List<TraderEquity>() { new TraderEquity { TraderId = 2, EquityId = 2, NumberOfEquity = 1, TraderEquityId = 1 } });

            // Act
            var (success, message) = _brocker.BuyEquities(new BuyEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.True(success);
            Assert.Equal("success", message);
            _traderEquityRepository.Verify(x => x.Add(It.IsAny<TraderEquity>()), Times.Once);
            _traderEquityRepository.Verify(x => x.Update(It.IsAny<TraderEquity>(), It.IsAny<int>()), Times.Never);
            _traderRepository.Verify(x => x.Update(It.IsAny<Trader>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void BuyEquities_should_return_success_when_update_existing_equity_for_trader()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 10, Name = "ICICI" });
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 500, Name = "B" });
            _traderEquityRepository.Setup(x => x.GetAll()).Returns(new List<TraderEquity>() { new TraderEquity { TraderId = 1, EquityId = 1, NumberOfEquity = 1, TraderEquityId = 1 } });

            // Act
            var (success, message) = _brocker.BuyEquities(new BuyEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.True(success);
            Assert.Equal("success", message);
            _traderEquityRepository.Verify(x => x.Add(It.IsAny<TraderEquity>()), Times.Never);
            _traderEquityRepository.Verify(x => x.Update(It.IsAny<TraderEquity>(), It.IsAny<int>()), Times.Once);
            _traderRepository.Verify(x => x.Update(It.IsAny<Trader>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void SellEquities_should_return_error_if_invalid_dateTime()
        {
            // Act + Arrange
            var (success, message) = _brocker.SellEquities(new SellEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 18, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("Not a valid day or time", message);
        }

        [Fact]
        public void SellEquities_should_return_error_if_enquity_not_exist()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(null as Equity);
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader());

            // Act 
            var (success, message) = _brocker.SellEquities(new SellEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("trader or equity not exist", message);
        }

        [Fact]
        public void SellEquities_should_return_error_if_trader_not_exist()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity());
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(null as Trader);

            // Act 
            var (success, message) = _brocker.SellEquities(new SellEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("trader or equity not exist", message);
        }

        [Fact]
        public void SellEquities_should_return_error_if_trader_does_not_hold_requested_equity()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 10, Name = "ICICI" });
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 500, Name = "B" });
            _traderEquityRepository.Setup(x => x.GetAll()).Returns(new List<TraderEquity>() { new TraderEquity { TraderId = 1, EquityId = 2, NumberOfEquity = 1, TraderEquityId = 1 } });

            // Act
            var (success, message) = _brocker.SellEquities(new SellEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("trader doesnot have enough holding of equity", message);
        }

        [Fact]
        public void SellEquities_should_return_error_if_trader_does_not_hold_requested_equity_2()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 10, Name = "ICICI" });
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 500, Name = "B" });
            _traderEquityRepository.Setup(x => x.GetAll()).Returns(new List<TraderEquity>() { new TraderEquity { TraderId = 2, EquityId = 1, NumberOfEquity = 1, TraderEquityId = 1 } });

            // Act
            var (success, message) = _brocker.SellEquities(new SellEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("trader doesnot have enough holding of equity", message);
        }

        [Fact]
        public void SellEquities_should_return_error_when_does_not_hold_sufficient_equity()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 10, Name = "ICICI" });
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 500, Name = "B" });
            _traderEquityRepository.Setup(x => x.GetAll()).Returns(new List<TraderEquity>() { new TraderEquity { TraderId = 1, EquityId = 1, NumberOfEquity = 0.5, TraderEquityId = 1 } });

            // Act
            var (success, message) = _brocker.SellEquities(new SellEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("trader doesnot have enough holding of equity", message);
        }

        [Fact]
        public void SellEquities_should_return_error_when_does_not_have_enough_fund_after_deducting_brockergae()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 10, Name = "ICICI" });
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 5, Name = "B" });
            _traderEquityRepository.Setup(x => x.GetAll()).Returns(new List<TraderEquity>() { new TraderEquity { TraderId = 1, EquityId = 1, NumberOfEquity = 2, TraderEquityId = 1 } });

            // Act
            var (success, message) = _brocker.SellEquities(new SellEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.False(success);
            Assert.Equal("your balance will reach to negative after deducting brockerage", message);
        }

        [Fact]
        public void SellEquities_should_return_success_if_opeartion_is_success()
        {
            // Arrange
            _equityRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity() { Id = 1, Value = 1000000, Name = "ICICI" });
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { Id = 1, Fund = 20, Name = "B" });
            _traderEquityRepository.Setup(x => x.GetAll()).Returns(new List<TraderEquity>() { new TraderEquity { TraderId = 1, EquityId = 1, NumberOfEquity = 2, TraderEquityId = 1 } });

            // Act
            var (success, message) = _brocker.SellEquities(new SellEquitiesRequest { TraderId = 1, EquityId = 1, NumberOfEquity = 1, RequestDateTime = new DateTime(2021, 12, 17, 9, 1, 1) });

            // Assert
            Assert.True(success);
            Assert.Equal("success", message);
            _traderEquityRepository.Verify(x => x.Update(It.IsAny<TraderEquity>(), It.IsAny<int>()), Times.Once);
            _traderRepository.Verify(x => x.Update(It.IsAny<Trader>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void AddFund_should_return_error_if_trader_not_exist()
        {
            // Arrange
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(null as Trader);

            // Act 
            var (success, message) = _brocker.AddFund(new AddFundRequest { TraderId = 1, FundToBeAdded = 100 });

            // Assert
            Assert.False(success);
            Assert.Equal("trader not exist", message);
        }

        [Fact]
        public void AddFund_should_return_success_if_added()
        {
            // Arrange
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader());

            // Act 
            var (success, message) = _brocker.AddFund(new AddFundRequest { TraderId = 1, FundToBeAdded = 100 });

            // Assert
            Assert.True(success);
            _traderRepository.Verify(x => x.Update(It.IsAny<Trader>(), It.IsAny<int>()), Times.Once);
            Assert.Equal("success", message);
        }

        [Fact]
        public void AddFund_should_return_success_if_added_after_deducting_brockerage()
        {
            // Arrange
            _traderRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader());

            // Act 
            var (success, message) = _brocker.AddFund(new AddFundRequest { TraderId = 1, FundToBeAdded = 2000000 });

            // Assert
            Assert.True(success);
            _traderRepository.Verify(x => x.Update(It.IsAny<Trader>(), It.IsAny<int>()), Times.Once);
            Assert.Equal("success", message);
        }
    }
}
