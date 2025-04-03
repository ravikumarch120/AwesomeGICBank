namespace AwesomeGICBankTest.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var expected = 5;
            var actual = 2 + 3;
            // Act
            // (No action needed in this test)
            // Assert
            Assert.Equal(expected, actual);

        }
    }
}
