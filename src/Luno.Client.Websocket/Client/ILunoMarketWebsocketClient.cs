using System.Threading.Tasks;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// Luno market websocket client.
/// </summary>
public interface ILunoMarketWebsocketClient : ILunoWebsocketClient
{
	/// <summary>
	/// The trading pair with which all messages are concerned.
	/// </summary>
	string Pair { get; }

	/// <summary>
	/// Closes current websocket stream and perform a new connection to the server.
	/// In case of connection error it doesn't throw an exception, but tries to reconnect indefinitely.
	/// Use this method in combination with a subscription to `ReconnectionHappened` which sends the authentication request,
	/// thus triggering a new snapshot to be sent from the server so that consumers can rebuild the orderbook from scratch.
	/// </summary>
	Task Reconnect();

	/// <summary>
	/// Provided message streams.
	/// </summary>
	LunoMarketClientStreams Streams { get; }
}
