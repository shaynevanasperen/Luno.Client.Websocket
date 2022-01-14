using System;
using System.Reactive.Subjects;
using Luno.Client.Websocket.Models;
using Luno.Client.Websocket.Responses;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// All provided streams.
/// You don't need to send subscription request in advance (all are subscribed to by default).
/// </summary>
public class LunoMarketClientStreams : LunoClientStreams
{
	/// <summary>
	/// Creates a new instance.
	/// </summary>
	public LunoMarketClientStreams()
	{
		OrderBookSnapshotStream.Subscribe(response => StatusStream.OnNext(new PairStatus
		{
			Status = response.Status
		}));

		OrderBookDiffStream.Subscribe(response =>
		{
			foreach (var tradeUpdate in response.TradeUpdates)
			{
				TradeStream.OnNext(new Trade
				{
					MakerOrderId = tradeUpdate.MakerOrderId,
					TakerOrderId = tradeUpdate.TakerOrderId,
					Base = tradeUpdate.Base,
					Counter = tradeUpdate.Counter
				});
			}
			if (response.StatusUpdate != null)
				StatusStream.OnNext(new PairStatus
				{
					Status = response.StatusUpdate.Status
				});
		});
	}

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
