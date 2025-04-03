public static class Messages
{
    public static string WelcomeMessage => "Welcome to the Banking Application!";
    public static string InputTransactionOption => "Press T to input a transaction.";
    public static string DefineInterestRuleOption => "Press I to define an interest rule.";
    public static string PrintStatementOption => "Press P to print a statement.";
    public static string QuitOption => "Press Q to quit.";
    public static string EnterSymbol => "> ";
    public static string GoodbyeMessage => "Thank you for using the Banking Application. Goodbye!";
    public static string InvalidOptionMessage => "Invalid option. Please try again.";
    public static string EnterTransactionDetails => "Please enter transaction details in <Date> <Account> <Type> <Amount> format (or enter blank to go back to main menu):";
    public static string TransactionSuccess => "Transaction was successful.";
    public static string TransactionFailed => "Transaction failed.";
    public static string InvalidTransactionFormat => "Invalid transaction format.";
    public static string EnterInterestRuleDetails => "Please enter interest rules details in <Date> <RuleId> <Rate in %> format \n(or enter blank to go back to main menu):";
    public static string InterestRuleSuccess => "Interest rule added successfully.";
    public static string InterestRuleFailed => "Interest rule addition failed.";
    public static string InvalidInterestRuleFormat => "Invalid interest rule format.";
    public static string EnterStatementDetails => "Please enter account and month to generate the statement <Account> <Year><Month>\n(or enter blank to go back to main menu):";
    public static string InvalidStatementFormat => "Invalid statement format.";
}
