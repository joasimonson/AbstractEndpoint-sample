using System.Text.Json.Serialization;
using System.Text.Json;

namespace AbstractEndpointSample;

public class TransactionJsonConverter : JsonConverter<Transaction>
{
    public override Transaction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (JsonDocument.TryParseValue(ref reader, out var document))
        {
            var transactionType = GetTransactionType(document.RootElement);

            Transaction? transaction = transactionType switch
            {
                TransactionType.Deposit => JsonSerializer.Deserialize<Deposit>(document.RootElement.GetRawText(), options),
                TransactionType.Withdraw => JsonSerializer.Deserialize<Withdraw>(document.RootElement.GetRawText(), options),
                TransactionType.Transfer => JsonSerializer.Deserialize<Transfer>(document.RootElement.GetRawText(), options),
                _ => throw new JsonException("Transaction type not mapped.")
            };

            if (transaction == null)
                throw new JsonException("Transaction could not be parsed.");

            return transaction;
        }

        throw new JsonException("Invalid format json transaction");
    }

    public override void Write(Utf8JsonWriter writer, Transaction value, JsonSerializerOptions options) => throw new NotImplementedException();

    private static TransactionType GetTransactionType(JsonElement jsonElement)
    {
        var prop = jsonElement.EnumerateObject().FirstOrDefault(el => string.Compare(el.Name, "transactionType", true) is 0);

        var a = prop.Value.GetString();
        if (prop.Value.ValueKind == JsonValueKind.Undefined || !Enum.TryParse(a, true, out TransactionType type))
            throw new JsonException("Invalid transaction type.");

        return type;
    }
}