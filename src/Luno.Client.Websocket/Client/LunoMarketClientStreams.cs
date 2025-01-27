using System.Reactive.Subjects;
using Luno.Client.Websocket.Models;
using Luno.Client.Websocket.Responses;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// All provided streams.
/// You don't need to send subscription request in advance (all are subscribed to by default).
/// </summary>
public class LunoMarketClientStreams
{
	/// <summary>
	/// Keep alive stream - emits regularly to keep the connection alive
	/// </summary>
	public readonly Subject<KeepAlive> KeepAliveStream = new();

	/// <summary>
	/// Status stream - emits when the trading status changes.
	/// </summary>
	public readonly Subject<PairStatus> StatusStream = new();

	/// <summary>
	/// Order book snapshot stream - emits in response to authentication request.
	/// </summary>
	public readonly Subject<OrderBookSnapshot> OrderBookSnapshotStream = new();

	/// <summary>
	/// Order book diff stream - emits every time order book changes.
	/// </summary>
	public readonly Subject<OrderBookDiff> OrderBookDiffStream = new();

	/// <summary>
	/// Trade stream - emits for every trade that occurs.
	/// </summary>
	public readonly Subject<Trade> TradeStream = new();
}
