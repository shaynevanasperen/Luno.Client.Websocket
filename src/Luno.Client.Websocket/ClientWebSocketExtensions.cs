using System;
using System.Net.WebSockets;

namespace Luno.Client.Websocket
{
	/// <summary>
	/// Extension methods for <see cref="ClientWebSocket"/>.
	/// </summary>
	public static class ClientWebSocketExtensions
	{
		/// <summary>
		/// Sets the KeepAlive interval and returns this instance.
		/// </summary>
		/// <param name="clientWebSocket">The websocket.</param>
		/// <param name="interval">The interval.</param>
		/// <returns>The current instance.</returns>
		public static ClientWebSocket WithKeepAliveInterval(this ClientWebSocket clientWebSocket, TimeSpan interval)
		{
			clientWebSocket.Options.KeepAliveInterval = interval;
			return clientWebSocket;
		}
	}
}
