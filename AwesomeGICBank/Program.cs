// See https://aka.ms/new-console-template for more information
using System;
using System.Globalization;
using BankingApplication.Interface;
using BankingApplication.Service;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeGICBank
{
    class Program
    {
        private static IBankService _bankService;
        private static IValidationService _validationService;

        public static void Main(string[] args)
        {
            SetupService();
            Applaunch();
        }

        #region Setup Method
        public static void Applaunch()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Messages.WelcomeMessage);
                Console.WriteLine(Messages.InputTransactionOption);
                Console.WriteLine(Messages.DefineInterestRuleOption);
                Console.WriteLine(Messages.PrintStatementOption);
                Console.WriteLine(Messages.QuitOption);
                Console.Write(Messages.EnterSymbol);
                var input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "T":
                        InputTransaction();
                        break;
                    case "I":
                        DefineInterestRule();
                        break;
                    case "P":
                        PrintStatement();
                        break;
                    case "Q":
                        running = false;
                        Console.WriteLine(Messages.GoodbyeMessage);
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine(Messages.InvalidOptionMessage);
                        break;
                }
            }
        }

        public static void SetupService()
        {
            // Set up dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IValidationService, ValidationService>()
                .AddSingleton<IBankService, BankService>()
                .BuildServiceProvider();

            _bankService = serviceProvider.GetRequiredService<IBankService>();
            _validationService = serviceProvider.GetRequiredService<IValidationService>();
        }

        #endregion

        #region Function Method
        public static void InputTransaction()
        {
            try
            {
                Console.WriteLine(Messages.EnterTransactionDetails);
                Console.Write(Messages.EnterSymbol);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return;

                Console.WriteLine(Environment.NewLine);
                var parts = input.Split(' ');
                if (_validationService.ValidateInputTransaction(parts))
                {
                    DateTime.TryParseExact(parts[0], "yyyyMMdd", null, DateTimeStyles.None, out DateTime date);
                    bool success = _bankService.AddTransaction(parts[1], date, parts[2][0], decimal.Parse(parts[3]));
                    if (success)
                    {
                        Console.WriteLine(Messages.TransactionSuccess);
                        PrintAccountStatement(parts[1]);
                    }
                    else
                    {
                        Console.WriteLine(Messages.TransactionFailed);
                    }
                }
                else
                {
                    Console.WriteLine(Messages.InvalidTransactionFormat);
                }
            }
            catch
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Messages.TransactionFailed);
            }
        }

        private static void PrintAccountStatement(string accountNumber)
        {
            var transactions = _bankService.GetTransactionsForAccount(accountNumber);
            if (transactions == null || transactions.Count == 0)
            {
                Console.WriteLine("No transactions found for the account.");
                return;
            }

            Console.WriteLine($"Account: {accountNumber}");
            Console.WriteLine("| Date     | Txn Id      | Type | Amount |");

            foreach (var txn in transactions)
            {
                Console.WriteLine($"| {txn.Date:yyyyMMdd} | {txn.TransactionId} | {txn.Type}    | {txn.Amount,8:F2} |");
            }
        }

        public static void DefineInterestRule()
        {
            try
            {
                Console.WriteLine(Messages.EnterInterestRuleDetails);
                Console.Write(Messages.EnterSymbol);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return;

                Console.WriteLine(Environment.NewLine);
                var parts = input.Split(' ');
                if (_validationService.ValidationInterestRule(parts))
                {
                    DateTime.TryParseExact(parts[0], "yyyyMMdd", null, DateTimeStyles.None, out DateTime date);
                    bool success = _bankService.AddInterestRule(date, parts[1], decimal.Parse(parts[2]));
                    if (success)
                    {
                        Console.WriteLine(Messages.InterestRuleSuccess);
                        PrintInterestRules();
                    }
                    else
                    {
                        Console.WriteLine(Messages.InterestRuleFailed);
                    }
                }
                else
                {
                    Console.WriteLine(Messages.InvalidInterestRuleFormat);
                }
            }
            catch
            {
                Console.WriteLine(Messages.InterestRuleFailed);
            }
        }

        private static void PrintInterestRules()
        {
            Console.WriteLine("Interest rules:");
            Console.WriteLine("| Date     | RuleId | Rate (%) |");

            foreach (var rule in _bankService.GetInterestRules())
            {
                Console.WriteLine($"| {rule.Date:yyyyMMdd} | {rule.RuleId} | {rule.Rate,8:F2} |");
            }
        }

        public static void PrintStatement()
        {
            Console.WriteLine(Messages.EnterStatementDetails);
            Console.Write(Messages.EnterSymbol);
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return;

            Console.WriteLine(Environment.NewLine);
            var parts = input.Split(' ');
            if (parts.Length == 2 && int.TryParse(parts[1].Substring(0, 4), out int year) && int.TryParse(parts[1].Substring(4), out int month))
            {
                var statement = _bankService.GenerateStatement(parts[0], year, month);
                Console.WriteLine(statement);
            }
            else
            {
                Console.WriteLine(Messages.InvalidStatementFormat);
            }
        }

        #endregion
    }
}