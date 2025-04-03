using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBank.Tests
{
    public class Class1Tests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var expected = 5;
            var actual = 5;
            // Act
            // (No action needed for this simple test)
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
