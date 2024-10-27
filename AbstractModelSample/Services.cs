namespace AbstractEndpointSample;

public abstract class TransactionService : ITransactionService
{
    public async Task ProcessAsync(Transaction transaction) => await ExecuteAsync(transaction);

    public abstract Task ExecuteAsync(Transaction transaction);
}

public interface ITransactionService
{
    Task ProcessAsync(Transaction transaction);
    Task ExecuteAsync(Transaction transaction);
}

public class DepositService : TransactionService
{
    public override async Task ExecuteAsync(Transaction transaction)
    {
        var deposit = transaction as Deposit;
        await Task.CompletedTask;
    }
}

public class WithdrawService : TransactionService
{
    public override async Task ExecuteAsync(Transaction transaction)
    {
        var withdraw = transaction as Withdraw;
        await Task.CompletedTask;
    }
}

public class TransferService : TransactionService
{
    public override async Task ExecuteAsync(Transaction transaction)
    {
        var transfer = transaction as Transfer;
        await Task.CompletedTask;
    }
}
