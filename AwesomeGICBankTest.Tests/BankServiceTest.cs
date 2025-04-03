using BankingApplication.Entity;
using BankingApplication.Interface;
using BankingApplication.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBankTest.Tests
{
    public class BankServiceTest
    {
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly IBankService _bankService;

        public BankServiceTest()
        {
            _mockValidationService = new Mock<IValidationService>();
            _bankService = new BankService(_mockValidationService.Object);

        }
        [Fact]
        public void AddTransaction_NewAccount_ShouldCreateAccountAndAddTransaction()
        {
            _mockValidationService.Setup(v => v.ValidateTransaction(It.IsAny<Account>(), 'D', 100.00m)).Returns(true);
            var result = _bankService.AddTransaction("AC001", new DateTime(2023, 6, 26), 'D', 100.00m);

            Assert.True(result);
            var transactions = _bankService.GetTransactionsForAccount("AC001");
            Assert.Single(transactions);
            Assert.Equal("20230626-01", transactions[0].TransactionId);
            Assert.Equal(100.00m, transactions[0].Amount);
        }
        [Fact]
        public void AddTransaction_WithdrawalWithInsufficientBalance_ShouldReturnFalse()
        {
            _mockValidationService.Setup(v => v.ValidateTransaction(It.IsAny<Account>(), 'W', 100.00m)).Returns(false);
            var result = _bankService.AddTransaction("AC001", new DateTime(2023, 6, 26), 'W', 100.00m);
            Assert.False(result);
        }

        [Fact]
        public void AddInterestRule_ValidRule_ShouldReturnTrue()
        {
            _mockValidationService.Setup(v => v.ValidateInterestRate(2.20m)).Returns(true);
            var result = _bankService.AddInterestRule(new DateTime(2023, 6, 15), "RULE03", 2.20m);

            Assert.True(result);
            var rules = _bankService.GetInterestRules();
            Assert.Single(rules);
            Assert.Equal("RULE03", rules[0].RuleId);
            Assert.Equal(2.20m, rules[0].Rate);
        }

        [Theory]
        [InlineData(101.00)]  // Invalid rate (too high)
        [InlineData(-2.20)]  // Negative rate
        [InlineData(0.00)]  // Zero rate
        public void AddInterestRule_InvalidRates_ShouldReturnFalse(decimal rate)
        {
            _mockValidationService.Setup(v => v.ValidateInterestRate(rate)).Returns(false);
            var result = _bankService.AddInterestRule(new DateTime(2023, 6, 15), "RULE03", rate);
            Assert.False(result);
        }

        [Fact]
        public void GetTransactionsForAccount_NoTransactions_ShouldReturnEmptyList()
        {
            var transactions = _bankService.GetTransactionsForAccount("AC001");
            Assert.Empty(transactions);
        }

        [Fact]
        public void GetInterestRules_MultipleRules_ShouldReturnOrderedList()
        {
            _mockValidationService.Setup(v => v.ValidateInterestRate(2.20m)).Returns(true);
            _mockValidationService.Setup(v => v.ValidateInterestRate(1.95m)).Returns(true);

            _bankService.AddInterestRule(new DateTime(2023, 6, 15), "RULE03", 2.20m);
            _bankService.AddInterestRule(new DateTime(2023, 1, 1), "RULE01", 1.95m);

            var rules = _bankService.GetInterestRules();
            Assert.Equal(2, rules.Count);
            Assert.Equal("RULE01", rules[0].RuleId);
            Assert.Equal("RULE03", rules[1].RuleId);
        }

        [Theory]
        [InlineData("NON_EXISTENT_ACCOUNT", "Account not found.")]  // Account not found
        [InlineData("AC001", "Account not found.")]  // Existing but no transactions
        public void GenerateStatement_ShouldReturnExpectedResult(string accountId, string expectedMessage)
        {
            var statement = _bankService.GenerateStatement(accountId, 2023, 6);
            Assert.Equal(expectedMessage, statement);
        }

        [Theory]
        [InlineData('X', 100.00)]  // Invalid Transaction Type
        [InlineData('D', -100.00)]  // Negative Amount
        public void AddTransaction_InvalidCases_ShouldReturnFalse(char transactionType, decimal amount)
        {
            var result = _bankService.AddTransaction("AC001", new DateTime(2023, 6, 26), transactionType, amount);
            Assert.False(result);
        }

        [Theory]
        [InlineData("INVALID_ACCOUNT")]  // Invalid account
        [InlineData("NON_EXISTENT_ACCOUNT")]  // Non-existent account
        public void GetTransactionsForAccount_InvalidAccount_ShouldReturnEmptyList(string accountId)
        {
            var transactions = _bankService.GetTransactionsForAccount(accountId);
            Assert.Empty(transactions);
        }
    }

}
