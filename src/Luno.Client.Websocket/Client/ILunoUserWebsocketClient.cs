namespace Luno.Client.Websocket.Client;

/// <summary>
/// Luno user websocket client.
/// </summary>
public interface ILunoUserWebsocketClient : ILunoWebsocketClient
{
	/// <summary>
	/// Provided message streams.
	/// </summary>
	LunoUserClientStreams Streams { get; }
}
