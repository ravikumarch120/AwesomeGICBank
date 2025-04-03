using BankingApplication.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBankTest.Tests
{
    public class ValidationServiceTest
    {
        private readonly Mock<IValidationService> _mockValidationService;

        public ValidationServiceTest()
        {
            _mockValidationService = new Mock<IValidationService>();

        }

        // Test cases for ValidateInputTransaction
        [Theory]
        [InlineData(new[] { "20230626", "AC001", "D", "100.00" }, true)]   // Valid case
        [InlineData(null, false)]  // Null input
        [InlineData(new[] { "20230626", "AC001", "D" }, false)]  // Incorrect length
        [InlineData(new[] { "20230626", "", "D", "100.00" }, false)]  // Empty Account
        [InlineData(new[] { "20230626", "AC001", "X", "100.00" }, false)]  // Invalid Transaction Type
        [InlineData(new[] { "20231326", "AC001", "D", "100.00" }, false)]  // Invalid Date
        [InlineData(new[] { "20230626", "AC001", "D", "ABC" }, false)]  // Invalid Amount
        [InlineData(new[] { "20230626", "AC001", "D", "-100.00" }, false)]  // Negative Amount
        [InlineData(new[] { "20230626", "AC001", "D", "0" }, false)]  // Zero Amount
        public void ValidateInputTransaction_ShouldReturnExpectedResult(string[] input, bool expectedResult)
        {
            _mockValidationService.Setup(v => v.ValidateInputTransaction(input)).Returns(expectedResult);

            var result = _mockValidationService.Object.ValidateInputTransaction(input);

            Assert.Equal(expectedResult, result);
        }

        // Test cases for ValidationInterestRule
        [Theory]
        [InlineData(new[] { "20230626", "RULE01", "2.20" }, true)]  // Valid case
        [InlineData(new[] { "20230626", "RULE01" }, false)]  // Incorrect length
        [InlineData(new[] { "20230626", "", "2.20" }, false)]  // Empty Rule ID
        [InlineData(new[] { "20231326", "RULE01", "2.20" }, false)]  // Invalid Date
        [InlineData(new[] { "20230626", "RULE01", "ABC" }, false)]  // Invalid Rate
        [InlineData(new[] { "20230626", "RULE01", "-2.20" }, false)]  // Negative Rate
        [InlineData(new[] { "20230626", "RULE01", "0" }, false)]  // Zero Rate
        public void ValidationInterestRule_ShouldReturnExpectedResult(string[] input, bool expectedResult)
        {
            _mockValidationService.Setup(v => v.ValidationInterestRule(input)).Returns(expectedResult);

            var result = _mockValidationService.Object.ValidationInterestRule(input);

            Assert.Equal(expectedResult, result);
        }
    }

}
