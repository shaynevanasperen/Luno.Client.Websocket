using System.Reactive.Subjects;
using Luno.Client.Websocket.Responses;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// Base class for Luno streams.
/// </summary>
public abstract class LunoClientStreams
{
	/// <summary>
	/// Keep alive stream - emits regularly to keep the connection alive
	/// </summary>
	public readonly Subject<KeepAlive> KeepAliveStream = new();
}
