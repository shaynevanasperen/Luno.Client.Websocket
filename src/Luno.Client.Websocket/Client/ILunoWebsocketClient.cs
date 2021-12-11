using System;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// Luno websocket client.
/// </summary>
public interface ILunoWebsocketClient : IDisposable
{
	/// <summary>
	/// Serializes request and sends via websocket client.
	/// </summary>
	/// <param name="request">Request/message to be sent</param>
	void Send<T>(T request);
}
