using System.Reactive.Subjects;
using Luno.Client.Websocket.Responses;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// All provided streams.
/// You don't need to send subscription request in advance (all are subscribed to by default).
/// </summary>
public class LunoUserClientStreams : LunoClientStreams
{
	/// <summary>
	/// Order status stream - emits when order status changes
	/// </summary>
	public readonly Subject<OrderUpdateResponse> OrderUpdateStream = new();
}
