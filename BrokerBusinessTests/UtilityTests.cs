using BrokerBussiness;
using System;
using Xunit;

namespace BrokerBusinessTests
{
    public class UtilityTests
    {
        [Fact]
        public void IsValidDayAndTime_should_return_false_If_Day_is_Sat()
        {
            // Act + Arrange
            var res = Utility.IsValidDayAndTime(new DateTime(2021, 12, 18, 9, 1, 1));

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void IsValidDayAndTime_should_return_false_If_Day_is_Sunday()
        {
            // Act + Arrange
            var res = Utility.IsValidDayAndTime(new DateTime(2021, 12, 19, 9, 1, 1));

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void IsValidDayAndTime_should_return_false_If_Day_is_Correct_But_time_is_before_9()
        {
            // Act + Arrange
            var res = Utility.IsValidDayAndTime(new DateTime(2021, 12, 17, 8, 59, 1));

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void IsValidDayAndTime_should_return_false_If_Day_is_Correct_But_time_is_after_3()
        {
            // Act + Arrange
            var res = Utility.IsValidDayAndTime(new DateTime(2021, 12, 17, 15, 1, 0));

            // Assert
            Assert.False(res);
        }

        [Fact]
        public void IsValidDayAndTime_should_return_true_If_both_day_time_is_correct()
        {
            // Act + Arrange
            var res = Utility.IsValidDayAndTime(new DateTime(2021, 12, 17, 9, 1, 0));

            // Assert
            Assert.True(res);
        }
    }
}
