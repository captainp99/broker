using Broker.Controllers;
using BrokerBussiness;
using BrokerBussiness.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BrokerApiTests.Controllers
{
    public class BrokerControllerTests
    {
        private readonly Mock<IBroker> _brocker;
        private readonly BrokerController _contoller;
        public BrokerControllerTests()
        {
            _brocker = new Mock<IBroker>();
            _contoller = new BrokerController(_brocker.Object);
        }

        [Fact]
        public void BuyEquities_Should_Return_Success_If_Operation_Succed()
        {
            // Arrange
            _brocker.Setup(x => x.BuyEquities(It.IsAny<BuyEquitiesRequest>())).Returns((true, "success"));

            // Act
            var res = _contoller.BuyEquities(new Broker.Models.BuyEquitiesRequest { EquityId = 1, NumberOfEquity = 2, TraderId = 3 });

            // Assert
            var okResult = res as ObjectResult;
            Assert.NotNull(res);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("success", okResult.Value.ToString());
            _brocker.Verify(x => x.BuyEquities(It.IsAny<BuyEquitiesRequest>()), Times.Once);
        }

        [Fact]
        public void BuyEquities_Should_Return_BadRequest_If_Operation_Fail()
        {
            // Arrange
            _brocker.Setup(x => x.BuyEquities(It.IsAny<BuyEquitiesRequest>())).Returns((false, "fail"));

            // Act
            var res = _contoller.BuyEquities(new Broker.Models.BuyEquitiesRequest { EquityId = 1, NumberOfEquity = 2, TraderId = 3 });

            // Assert
            var result = res as ObjectResult;
            Assert.NotNull(res);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("fail", result.Value.ToString());
            _brocker.Verify(x => x.BuyEquities(It.IsAny<BuyEquitiesRequest>()), Times.Once);
        }

        [Fact]
        public void SellEquities_Should_Return_Success_If_Operation_Succed()
        {
            // Arrange
            _brocker.Setup(x => x.SellEquities(It.IsAny<SellEquitiesRequest>())).Returns((true, "success"));

            // Act
            var res = _contoller.SellEquities(new Broker.Models.SellEquitiesRequest { EquityId = 1, NumberOfEquity = 2, TraderId = 3 });

            // Assert
            var okResult = res as ObjectResult;
            Assert.NotNull(res);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("success", okResult.Value.ToString());
            _brocker.Verify(x => x.SellEquities(It.IsAny<SellEquitiesRequest>()), Times.Once);
        }

        [Fact]
        public void SellEquities_Should_Return_BadRequest_If_Operation_Fail()
        {
            // Arrange
            _brocker.Setup(x => x.SellEquities(It.IsAny<SellEquitiesRequest>())).Returns((false, "fail"));

            // Act
            var res = _contoller.SellEquities(new Broker.Models.SellEquitiesRequest { EquityId = 1, NumberOfEquity = 2, TraderId = 3 });

            // Assert
            var result = res as ObjectResult;
            Assert.NotNull(res);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("fail", result.Value.ToString());
            _brocker.Verify(x => x.SellEquities(It.IsAny<SellEquitiesRequest>()), Times.Once);
        }

        [Fact]
        public void AddFunds_Should_Return_Success_If_Operation_Succed()
        {
            // Arrange
            _brocker.Setup(x => x.AddFund(It.IsAny<AddFundRequest>())).Returns((true, "success"));

            // Act
            var res = _contoller.AddFunds(new Broker.Models.AddFundRequest { TraderId = 3, FundToBeAdded = 100 });

            // Assert
            var okResult = res as ObjectResult;
            Assert.NotNull(res);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("success", okResult.Value.ToString());
            _brocker.Verify(x => x.AddFund(It.IsAny<AddFundRequest>()), Times.Once);
        }

        [Fact]
        public void AddFunds_Should_Return_BadRequest_If_Operation_Fail()
        {
            // Arrange
            _brocker.Setup(x => x.AddFund(It.IsAny<AddFundRequest>())).Returns((false, "fail"));

            // Act
            var res = _contoller.AddFunds(new Broker.Models.AddFundRequest { TraderId = 3, FundToBeAdded = 100 });

            // Assert
            var result = res as ObjectResult;
            Assert.NotNull(res);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("fail", result.Value.ToString());
            _brocker.Verify(x => x.AddFund(It.IsAny<AddFundRequest>()), Times.Once);
        }
    }
}
