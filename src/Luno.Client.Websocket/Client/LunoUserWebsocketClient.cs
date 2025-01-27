using System.Text.Json;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Models;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// Luno account websocket client.
/// Use `Streams` to handle messages.
/// </summary>
/// <inheritdoc />
public class LunoUserWebsocketClient(ILogger logger, IWebsocketClient client) : LunoWebsocketClient(logger, client, "USER"), ILunoUserWebsocketClient
{

	/// <summary>
	/// Provided message streams
	/// </summary>
	public LunoUserClientStreams Streams { get; } = new();

	/// <inheritdoc />
	protected override bool HandleObjectMessage(string message)
	{
		var response = JsonSerializer.Deserialize<JsonElement>(message, LunoJsonOptions.Default);

		if (response.TryGetProperty("type", out var type))
		{
			var update = type.GetString();

			if (update == "order_status")
				return Message.TryHandle(response.GetProperty("order_status_update"), Streams.OrderStatusUpdateStream);

			if (update == "order_fill")
				return Message.TryHandle(response.GetProperty("order_fill_update"), Streams.OrderFillUpdateStream);

			if (update == "balance_update")
				return Message.TryHandle(response.GetProperty("balance_update"), Streams.BalanceUpdateStream);
		}

		return false;
	}
}
