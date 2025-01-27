using System.Text.Json;
using System.Threading.Tasks;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Models;
using Luno.Client.Websocket.Responses;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// Luno market websocket client.
/// Use `Streams` to handle messages.
/// </summary>
/// <remarks>
/// Creates a new instance.
/// </remarks>
/// <param name="logger">The logger to use for logging any errors.</param>
/// <param name="client">The client to use for the trade websocket.</param>
/// <param name="pair">The target pair.</param>
public class LunoMarketWebsocketClient(ILogger logger, IWebsocketClient client, string pair) : LunoWebsocketClient(logger, client, "MARKET"), ILunoMarketWebsocketClient
{

	/// <inheritdoc />
	public string Pair { get; } = pair;

	/// <inheritdoc />
	public Task Reconnect() => Client.Reconnect();

	/// <summary>
	/// Provided message streams
	/// </summary>
	public LunoMarketClientStreams Streams { get; } = new();

	/// <inheritdoc />
	protected override void HandleEmptyMessage() => Streams.KeepAliveStream.OnNext(new());

	/// <inheritdoc />
	protected override bool HandleObjectMessage(string message)
	{
		var response = JsonSerializer.Deserialize<JsonElement>(message, LunoJsonOptions.Default);

		if (response.TryGetProperty("trade_updates", out _))
		{
			var diffResponse = Message.TryDeserialize<OrderBookDiff>(response);
			if (diffResponse == null)
				return false;

			if (diffResponse.StatusUpdate != null)
				Streams.StatusStream.OnNext(new()
				{
					Status = diffResponse.StatusUpdate.Status
				});

			foreach (var tradeUpdate in diffResponse.TradeUpdates)
			{
				Streams.TradeStream.OnNext(new()
				{
					MakerOrderId = tradeUpdate.MakerOrderId,
					TakerOrderId = tradeUpdate.TakerOrderId,
					Base = tradeUpdate.Base,
					Counter = tradeUpdate.Counter
				});
			}

			Streams.OrderBookDiffStream.OnNext(diffResponse);

			return true;
		}

		if (response.TryGetProperty("asks", out _))
		{
			var snapshotResponse = Message.TryDeserialize<OrderBookSnapshot>(response);
			if (snapshotResponse == null)
				return false;

			Streams.StatusStream.OnNext(new()
			{
				Status = snapshotResponse.Status
			});

			Streams.OrderBookSnapshotStream.OnNext(snapshotResponse);

			return true;
		}

		return false;
	}
}
