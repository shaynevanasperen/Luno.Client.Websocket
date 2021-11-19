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
	public class LunoUserClientStreams
	{
		internal readonly Subject<KeepAliveResponse> KeepAliveSubject = new();
		internal readonly Subject<OrderStatusResponse> OrderStatusSubject = new();
		internal readonly Subject<OrderFillResponse> OrderFillSubject = new();

		/// <summary>
		/// Keep alive stream - emits regularly to keep the connection alive
		/// </summary>
		public IObservable<OrderStatusResponse> KeepAliveStream => OrderStatusSubject.AsObservable();

		/// <summary>
		/// Order status stream - emits when order status changes
		/// </summary>
		public IObservable<OrderFillResponse> OrderStatusStream => OrderFillSubject.AsObservable();

		/// <summary>
		/// Order fill stream - emits when new order fills occur
		/// </summary>
		public IObservable<OrderFillResponse> OrderFillStream => OrderFillSubject.AsObservable();
	}
}
