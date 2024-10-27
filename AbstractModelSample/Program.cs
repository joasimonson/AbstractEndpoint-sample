using AbstractEndpointSample;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new TransactionJsonConverter());
});

builder.Services.AddScoped<DepositService>();
builder.Services.AddScoped<WithdrawService>();
builder.Services.AddScoped<TransferService>();

builder.Services.AddTransient<TransactionDelegate>(provider => type =>
{
    return type switch
    {
        TransactionType.Deposit => provider.GetRequiredService<DepositService>(),
        TransactionType.Withdraw => provider.GetRequiredService<WithdrawService>(),
        TransactionType.Transfer => provider.GetRequiredService<TransferService>(),
        _ => throw new NotImplementedException($"Transaction '{type}' not implemented"),
    };
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/transaction", (Transaction transaction, [FromServices]TransactionDelegate resolver) =>
{
    resolver(transaction.TransactionType).ProcessAsync(transaction);

    return Results.Ok("Transaction completed");
});

app.Run();