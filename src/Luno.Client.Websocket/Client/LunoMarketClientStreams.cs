using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Luno.Client.Websocket.Responses;

namespace Luno.Client.Websocket.Client
{
	/// <summary>
	/// All provided streams.
	/// You don't need to send subscription request in advance (all are subscribed to by default)
	/// </summary>
	public class LunoMarketClientStreams
	{
		internal readonly Subject<KeepAliveResponse> KeepAliveSubject = new();
		internal readonly Subject<StatusResponse> StatusSubject = new();
		internal readonly Subject<OrderBookSnapshotResponse> OrderBookSnapshotSubject = new();
		internal readonly Subject<OrderBookDiffResponse> OrderBookDiffSubject = new();
		internal readonly Subject<TradeResponse> TradeSubject = new();

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public LunoMarketClientStreams()
		{
			OrderBookSnapshotStream.Subscribe(response => StatusSubject.OnNext(new StatusResponse
			{
				Status = response.Status
			}));

			OrderBookDiffStream.Subscribe(response =>
			{
				foreach (var tradeUpdate in response.TradeUpdates)
				{
					TradeSubject.OnNext(new TradeResponse
					{
						MakerOrderId = tradeUpdate.MakerOrderId,
						TakerOrderId = tradeUpdate.TakerOrderId,
						Base = tradeUpdate.Base,
						Counter = tradeUpdate.Counter
					});
				}
				if (response.StatusUpdate != null)
					StatusSubject.OnNext(new StatusResponse
					{
						Status = response.StatusUpdate.Status
					});
			});
		}

		/// <summary>
		/// Keep alive stream - emits regularly to keep the connection alive
		/// </summary>
		public IObservable<KeepAliveResponse> KeepAliveStream => KeepAliveSubject.AsObservable();

		/// <summary>
		/// Status stream - emits when the trading status changes
		/// </summary>
		public IObservable<StatusResponse> StatusStream => StatusSubject.AsObservable();

		/// <summary>
		/// Order book snapshot stream - emits in response to authentication request
		/// </summary>
		public IObservable<OrderBookSnapshotResponse> OrderBookSnapshotStream => OrderBookSnapshotSubject.AsObservable();

		/// <summary>
		/// Order book diff stream - emits every time order book changes
		/// </summary>
		public IObservable<OrderBookDiffResponse> OrderBookDiffStream => OrderBookDiffSubject.AsObservable();

		/// <summary>
		/// Trade stream - emits for every trade that occurs
		/// </summary>
		public IObservable<TradeResponse> TradeStream => TradeSubject.AsObservable();
	}
}
