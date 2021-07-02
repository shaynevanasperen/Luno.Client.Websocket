using System;
using System.Net.WebSockets;
using System.Text.Json;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Models;
using Luno.Client.Websocket.Requests;
using Luno.Client.Websocket.Responses;
using Websocket.Client;

namespace Luno.Client.Websocket.Client
{
	/// <summary>
	/// Luno websocket client.
	/// Use `Streams` to handle messages.
	/// </summary>
	public class LunoWebsocketClient : IDisposable
	{
		/// <summary>
		/// Creates a real live websocket connection to Luno.
		/// </summary>
		/// <param name="pair">The trading pair with which all messages are concerned.</param>
		/// <param name="secrets">The secrets.</param>
		/// <returns>A connected Luno websocket client.</returns>
		public static LunoWebsocketClient Create(string pair, LunoSecrets secrets)
		{
			const string baseAddress = "wss://ws.luno.com/api/1/stream/";

			var websocketClient = new WebsocketClient(new Uri($"{baseAddress}{pair}", UriKind.Absolute), () => new ClientWebSocket()
				.WithKeepAliveInterval(TimeSpan.FromSeconds(3)))
			{
				Name = $"Luno ({pair})"
			};

			var client = new LunoWebsocketClient(websocketClient, pair);

			websocketClient.ReconnectionHappened.Subscribe(_ => client.Send(new AuthenticationRequest(secrets.ApiKey, secrets.ApiSecret)));

			return client;
		}

		readonly IDisposable _clientMessageReceivedSubscription;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="client">The client to use for the websocket.</param>
		/// <param name="targetPair">The target pair.</param>
		public LunoWebsocketClient(IWebsocketClient client, string targetPair)
		{
			Client = client;
			TargetPair = targetPair;
			_clientMessageReceivedSubscription = Client.MessageReceived.Subscribe(HandleMessage);
		}

		/// <summary>
		/// The websocket client.
		/// </summary>
		public IWebsocketClient Client { get; }

		/// <summary>
		/// The trading pair with which all messages are concerned.
		/// </summary>
		public string TargetPair { get; }

		/// <summary>
		/// Provided message streams
		/// </summary>
		public LunoClientStreams Streams { get; } = new();

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
			Client.Send(serialized);
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

		bool HandleRawMessage(string _)
		{
			Streams.KeepAliveSubject.OnNext(new KeepAliveResponse());
			return true;
		}

		bool HandleObjectMessage(string message)
		{
			var response = JsonSerializer.Deserialize<JsonElement>(message, LunoJsonOptions.Default);

			if (response.TryGetProperty("trade_updates", out _))
				return Message.TryHandle(response, Streams.OrderBookDiffSubject);

			if (response.TryGetProperty("asks", out _))
				return Message.TryHandle(response, Streams.OrderBookSnapshotSubject);

			return Message.TryHandle(response, Streams.KeepAliveSubject);
		}
	}
}
