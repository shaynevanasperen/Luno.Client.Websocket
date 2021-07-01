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
		internal readonly Subject<OrderBookSnapshotResponse> OrderBookSnapshotSubject = new();
		internal readonly Subject<OrderBookDiffResponse> OrderBookDiffSubject = new();

		/// <summary>
		/// Keep alive stream - emits regularly to keep the connection alive
		/// </summary>
		public IObservable<KeepAliveResponse> KeepAliveStream => KeepAliveSubject.AsObservable();

		/// <summary>
		/// Order book snapshot stream - emits in response to authentication request
		/// </summary>
		public IObservable<OrderBookSnapshotResponse> OrderBookSnapshotStream => OrderBookSnapshotSubject.AsObservable();

		/// <summary>
		/// Order book diff stream - emits ever time order book changes
		/// </summary>
		public IObservable<OrderBookDiffResponse> OrderBookDiffStream => OrderBookDiffSubject.AsObservable();
	}
}
