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
public class LunoUserWebsocketClient : LunoWebsocketClient<LunoUserClientStreams>, ILunoUserWebsocketClient
{
	/// <inheritdoc />
	public LunoUserWebsocketClient(ILogger logger, IWebsocketClient client) : base(logger, client, "USER") { }

	/// <summary>
	/// Provided message streams
	/// </summary>
	public override LunoUserClientStreams Streams { get; } = new();

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
