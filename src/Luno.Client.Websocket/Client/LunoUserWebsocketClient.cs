using System;
using System.Text.Json;
using System.Threading.Tasks;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Models;
using Luno.Client.Websocket.Responses;
using Websocket.Client;

namespace Luno.Client.Websocket.Client
{
	/// <summary>
	/// Luno account websocket client.
	/// Use `Streams` to handle messages.
	/// </summary>
	public class LunoUserWebsocketClient : IDisposable
	{
		readonly IWebsocketClient _client;
		readonly IDisposable _clientMessageReceivedSubscription;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="client">The client to use for the websocket.</param>
		public LunoUserWebsocketClient(IWebsocketClient client)
		{
			_client = client;
			_clientMessageReceivedSubscription = _client.MessageReceived.Subscribe(HandleMessage);
		}

		/// <summary>
		/// Provided message streams
		/// </summary>
		public LunoUserClientStreams Streams { get; } = new();

		/// <summary>
		/// Cleanup everything
		/// </summary>
		public void Dispose() => _clientMessageReceivedSubscription.Dispose();

		/// <summary>
		/// Serializes request and sends via websocket client.
		/// </summary>
		/// <param name="request">Request/message to be sent</param>
		public void Send<T>(T request)
		{
			var serialized = JsonSerializer.Serialize(request, LunoJsonOptions.Default);
			_client.Send(serialized);
		}

		/// <summary>
		/// Closes current websocket stream and perform a new connection to the server.
		/// In case of connection error it doesn't throw an exception, but tries to reconnect indefinitely.
		/// Use this method in combination with a subscription to `ReconnectionHappened` which sends the authentication request,
		/// thus triggering a new snapshot to be sent from the server so that consumers can rebuild the orderbook from scratch.
		/// </summary>
		public Task Reconnect() => _client.Reconnect();

		void HandleMessage(ResponseMessage message)
		{
			bool handled;
			var messageSafe = (message.Text ?? string.Empty).Trim();

			if (messageSafe.StartsWith("{", StringComparison.OrdinalIgnoreCase))
			{
				handled = HandleObjectMessage(messageSafe);
				if (handled)
					return;
			}

			handled = HandleRawMessage(messageSafe);
			if (handled)
				return;

			throw new Exception($"Unhandled response:  '{messageSafe}'");
		}

		bool HandleRawMessage(string _)
		{
			Streams.KeepAliveSubject.OnNext(new KeepAliveResponse());
			return true;
		}

		bool HandleObjectMessage(string message)
		{
			var response = JsonSerializer.Deserialize<JsonElement>(message, LunoJsonOptions.Default);

			if (response.TryGetProperty("status", out _))
				return Message.TryHandle(response, Streams.OrderStatusSubject);

			if (response.TryGetProperty("base_fill", out _))
				return Message.TryHandle(response, Streams.OrderFillSubject);

			return Message.TryHandle(response, Streams.KeepAliveSubject);
		}
	}
}
