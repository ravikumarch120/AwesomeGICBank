using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Xunit;
using Assert = Xunit.Assert;

namespace AwesomeGICBank.Tests
{
    public class BankTests
    {
        [Fact]
        public void CalculateInterest_ShouldReturnCorrectInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC001";
            var transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 6, 1), Id = "20230601-01", Type = "D", Amount = 150.00m },
                new Transaction { Date = new DateTime(2023, 6, 26), Id = "20230626-01", Type = "W", Amount = 20.00m },
                new Transaction { Date = new DateTime(2023, 6, 26), Id = "20230626-02", Type = "W", Amount = 100.00m }
            };
            bank.AddAccount(account, transactions);

            var interestRules = new List<InterestRule>
            {
                new InterestRule { Date = new DateTime(2023, 1, 1), Id = "RULE01", Rate = 1.95m },
                new InterestRule { Date = new DateTime(2023, 5, 20), Id = "RULE02", Rate = 1.90m },
                new InterestRule { Date = new DateTime(2023, 6, 15), Id = "RULE03", Rate = 2.20m }
            };
            bank.AddInterestRules(interestRules);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(0.39m, interest);
        }

        [Fact]
        public void CalculateInterest_NoTransactions_ShouldReturnZeroInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC002";
            bank.AddAccount(account, new List<Transaction>());

            var interestRules = new List<InterestRule>
            {
                new InterestRule { Date = new DateTime(2023, 1, 1), Id = "RULE01", Rate = 1.95m }
            };
            bank.AddInterestRules(interestRules);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(0m, interest);
        }

        [Fact]
        public void CalculateInterest_NoInterestRules_ShouldReturnZeroInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC003";
            var transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 6, 1), Id = "20230601-01", Type = "D", Amount = 150.00m }
            };
            bank.AddAccount(account, transactions);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(0m, interest);
        }

        [Fact]
        public void CalculateInterest_SingleTransaction_ShouldReturnCorrectInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC004";
            var transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 6, 1), Id = "20230601-01", Type = "D", Amount = 150.00m }
            };
            bank.AddAccount(account, transactions);

            var interestRules = new List<InterestRule>
            {
                new InterestRule { Date = new DateTime(2023, 1, 1), Id = "RULE01", Rate = 1.95m }
            };
            bank.AddInterestRules(interestRules);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(0.80m, interest);
        }

        [Fact]
        public void CalculateInterest_MultipleTransactionsDifferentDays_ShouldReturnCorrectInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC005";
            var transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 6, 1), Id = "20230601-01", Type = "D", Amount = 150.00m },
                new Transaction { Date = new DateTime(2023, 6, 15), Id = "20230615-01", Type = "D", Amount = 100.00m }
            };
            bank.AddAccount(account, transactions);

            var interestRules = new List<InterestRule>
            {
                new InterestRule { Date = new DateTime(2023, 1, 1), Id = "RULE01", Rate = 1.95m }
            };
            bank.AddInterestRules(interestRules);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(1.28m, interest);
        }

        [Fact]
        public void CalculateInterest_MultipleTransactionsSameDay_ShouldReturnCorrectInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC006";
            var transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 6, 1), Id = "20230601-01", Type = "D", Amount = 150.00m },
                new Transaction { Date = new DateTime(2023, 6, 1), Id = "20230601-02", Type = "D", Amount = 100.00m }
            };
            bank.AddAccount(account, transactions);

            var interestRules = new List<InterestRule>
            {
                new InterestRule { Date = new DateTime(2023, 1, 1), Id = "RULE01", Rate = 1.95m }
            };
            bank.AddInterestRules(interestRules);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(1.28m, interest);
        }

        [Fact]
        public void CalculateInterest_InterestRuleChangeDuringMonth_ShouldReturnCorrectInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC007";
            var transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 6, 1), Id = "20230601-01", Type = "D", Amount = 150.00m },
                new Transaction { Date = new DateTime(2023, 6, 15), Id = "20230615-01", Type = "D", Amount = 100.00m }
            };
            bank.AddAccount(account, transactions);

            var interestRules = new List<InterestRule>
            {
                new InterestRule { Date = new DateTime(2023, 1, 1), Id = "RULE01", Rate = 1.95m },
                new InterestRule { Date = new DateTime(2023, 6, 15), Id = "RULE02", Rate = 2.20m }
            };
            bank.AddInterestRules(interestRules);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(1.36m, interest);
        }

        [Fact]
        public void CalculateInterest_TransactionsOnLastDayOfMonth_ShouldReturnCorrectInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC008";
            var transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 6, 30), Id = "20230630-01", Type = "D", Amount = 150.00m }
            };
            bank.AddAccount(account, transactions);

            var interestRules = new List<InterestRule>
            {
                new InterestRule { Date = new DateTime(2023, 1, 1), Id = "RULE01", Rate = 1.95m }
            };
            bank.AddInterestRules(interestRules);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(0.01m, interest);
        }

        [Fact]
        public void CalculateInterest_NegativeBalancePrevention_ShouldReturnCorrectInterest()
        {
            // Arrange
            var bank = new Bank();
            var account = "AC009";
            var transactions = new List<Transaction>
            {
                new Transaction { Date = new DateTime(2023, 6, 1), Id = "20230601-01", Type = "D", Amount = 150.00m },
                new Transaction { Date = new DateTime(2023, 6, 15), Id = "20230615-01", Type = "W", Amount = 200.00m }
            };
            bank.AddAccount(account, transactions);

            var interestRules = new List<InterestRule>
            {
                new InterestRule { Date = new DateTime(2023, 1, 1), Id = "RULE01", Rate = 1.95m }
            };
            bank.AddInterestRules(interestRules);

            var month = new DateTime(2023, 6, 1);

            // Act
            var interest = bank.CalculateInterest(account, month);

            // Assert
            Assert.Equal(0.40m, interest);
        }
    }
}