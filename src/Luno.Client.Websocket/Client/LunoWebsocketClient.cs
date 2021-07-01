using System;
using System.Text.Json;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Messages;
using Websocket.Client;

namespace Luno.Client.Websocket.Client
{
	/// <summary>
	/// Luno websocket client.
	/// Use `Streams` to handle messages.
	/// </summary>
	public class LunoWebsocketClient : IDisposable
	{
		readonly IWebsocketClient _client;
		readonly IDisposable _clientMessageReceivedSubscription;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="client">The client to use for the websocket.</param>
		public LunoWebsocketClient(IWebsocketClient client)
		{
			_client = client;
			_clientMessageReceivedSubscription = _client.MessageReceived.Subscribe(HandleMessage);
		}

		/// <summary>
		/// Provided message streams
		/// </summary>
		public LunoClientStreams ClientStreams { get; } = new();

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

		bool HandleRawMessage(string message)
		{
			var response = JsonSerializer.Deserialize<JsonElement>(message, LunoJsonOptions.Default);

			return MessageBase.TryHandle(response, ClientStreams.KeepAliveSubject);
		}

		bool HandleObjectMessage(string message)
		{
			var response = JsonSerializer.Deserialize<JsonElement>(message, LunoJsonOptions.Default);

			if (response.TryGetProperty("trade_updates", out _))
				return MessageBase.TryHandle(response, ClientStreams.OrderBookDiffSubject);

			if (response.TryGetProperty("status", out _))
				return MessageBase.TryHandle(response, ClientStreams.OrderBookSnapshotSubject);

			return MessageBase.TryHandle(response, ClientStreams.KeepAliveSubject);
		}
	}
}
