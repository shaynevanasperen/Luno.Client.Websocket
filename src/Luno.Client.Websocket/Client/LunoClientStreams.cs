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
	public class LunoClientStreams
	{
		internal readonly Subject<KeepAliveResponse> KeepAliveSubject = new();
		internal readonly Subject<StatusResponse> StatusSubject = new();
		internal readonly Subject<OrderBookSnapshotResponse> OrderBookSnapshotSubject = new();
		internal readonly Subject<OrderBookDiffResponse> OrderBookDiffSubject = new();
		internal readonly Subject<TradeResponse> TradeSubject = new();

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public LunoClientStreams()
		{
			OrderBookDiffStream.Subscribe(response =>
			{
				foreach (var tradeUpdate in response.TradeUpdates)
				{
					TradeSubject.OnNext(new TradeResponse
					{
						OrderId = tradeUpdate.MakerOrderId,
						Volume = tradeUpdate.Base
					});
					TradeSubject.OnNext(new TradeResponse
					{
						OrderId = tradeUpdate.TakerOrderId,
						Volume = tradeUpdate.Base
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
		/// Order book diff stream - emits ever time order book changes
		/// </summary>
		public IObservable<OrderBookDiffResponse> OrderBookDiffStream => OrderBookDiffSubject.AsObservable();

		/// <summary>
		/// Trade stream - emits for every trade that occurs
		/// </summary>
		public IObservable<OrderBookDiffResponse> TradeStream => OrderBookDiffSubject.AsObservable();
	}
}
