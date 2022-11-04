using System.Reactive.Subjects;
using Luno.Client.Websocket.Models;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// All provided streams.
/// You don't need to send subscription request in advance (all are subscribed to by default).
/// </summary>
public class LunoUserClientStreams
{
	/// <summary>
	/// Order status stream - emits when order status changes
	/// </summary>
	public readonly Subject<OrderStatusUpdate> OrderStatusUpdateStream = new();

	/// <summary>
	/// Order fill stream - emits when order fill changes
	/// </summary>
	public readonly Subject<OrderFillUpdate> OrderFillUpdateStream = new();

	/// <summary>
	/// Balance update stream - emits when balance changes
	/// </summary>
	public readonly Subject<BalanceUpdate> BalanceUpdateStream = new();
}
