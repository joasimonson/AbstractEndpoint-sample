namespace AbstractEndpointSample;

public abstract class Transaction(string fromAccountId, decimal amount)
{
    public string FromAccountId { get; protected set; } = fromAccountId;
    public decimal Amount { get; protected set; } = amount;
    public abstract TransactionType TransactionType { get; }
}

public class Deposit(string fromAccountId, decimal amount) : Transaction(fromAccountId, amount)
{
    public override TransactionType TransactionType => TransactionType.Deposit;
}

public class Withdraw(string fromAccountId, decimal amount) : Transaction(fromAccountId, amount)
{
    public override TransactionType TransactionType => TransactionType.Withdraw;
}

public class Transfer(string fromAccountId, string toAccountId, decimal amount) : Transaction(fromAccountId, amount)
{
    public string ToAccountId { get; protected set; } = toAccountId;

    public override TransactionType TransactionType => TransactionType.Transfer;
}
