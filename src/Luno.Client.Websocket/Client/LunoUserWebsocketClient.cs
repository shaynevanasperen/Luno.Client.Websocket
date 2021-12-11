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

		if (response.TryGetProperty("type", out var type) && type.GetString() is "order_status" or "order_fill")
			return Message.TryHandle(response, Streams.OrderUpdateStream);

		return false;
	}
}
