using System.Text.Json;
using System.Threading.Tasks;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Models;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// Luno market websocket client.
/// Use `Streams` to handle messages.
/// </summary>
public class LunoMarketWebsocketClient : LunoWebsocketClient<LunoMarketClientStreams>, ILunoMarketWebsocketClient
{
	/// <summary>
	/// Creates a new instance.
	/// </summary>
	/// <param name="logger">The logger to use for logging any errors.</param>
	/// <param name="client">The client to use for the trade websocket.</param>
	/// <param name="pair">The target pair.</param>
	public LunoMarketWebsocketClient(ILogger logger, IWebsocketClient client, string pair) : base(logger, client, "MARKET")
	{
		Pair = pair;
	}

	/// <inheritdoc />
	public string Pair { get; }

	/// <inheritdoc />
	public Task Reconnect() => Client.Reconnect();

	/// <summary>
	/// Provided message streams
	/// </summary>
	public override LunoMarketClientStreams Streams { get; } = new();

	/// <inheritdoc />
	protected override bool HandleObjectMessage(string message)
	{
		var response = JsonSerializer.Deserialize<JsonElement>(message, LunoJsonOptions.Default);

		if (response.TryGetProperty("trade_updates", out _))
			return Message.TryHandle(response, Streams.OrderBookDiffStream);

		if (response.TryGetProperty("asks", out _))
			return Message.TryHandle(response, Streams.OrderBookSnapshotStream);

		return false;
	}
}
