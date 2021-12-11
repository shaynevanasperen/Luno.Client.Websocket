using System;
using System.Text.Json;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Responses;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// Luno market websocket client.
/// Use `Streams` to handle messages.
/// </summary>
public abstract class LunoWebsocketClient<TStreams> : IDisposable where TStreams : LunoClientStreams
{
	readonly ILogger _logger;

	readonly string _type;
	readonly IDisposable _clientMessageReceivedSubscription;

	/// <summary>
	/// Creates a new instance.
	/// </summary>
	/// <param name="logger">The logger to use for logging any warnings or errors.</param>
	/// <param name="client">The client to use for the trade websocket.</param>
	/// <param name="type">The type of websocket client (MARKET or USER).</param>
	protected LunoWebsocketClient(ILogger logger, IWebsocketClient client, string type)
	{
		_logger = logger;
		Client = client;
		_type = type;
		_clientMessageReceivedSubscription = Client.MessageReceived.Subscribe(HandleMessage);
	}

	/// <summary>
	/// The underlying websocket client.
	/// </summary>
	protected IWebsocketClient Client { get; }

	/// <summary>
	/// Provided message streams.
	/// </summary>
	public abstract TStreams Streams { get; }

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
		try
		{
			var serialized = JsonSerializer.Serialize(request, LunoJsonOptions.Default);
			Client.Send(serialized);
		}
		catch (Exception e)
		{
			_logger.LogError(e, LogMessage($"Exception while sending message '{request}'. Error: {e.Message}"));
			throw;
		}
	}

	void HandleMessage(ResponseMessage message)
	{
		try
		{
			var messageSafe = (message.Text ?? string.Empty).Trim();

			if (messageSafe.Length == 0 || messageSafe == @"""""")
			{
				Streams.KeepAliveStream.OnNext(new KeepAliveResponse());
				return;
			}
			if (messageSafe.StartsWith("{", StringComparison.OrdinalIgnoreCase))
				if (HandleObjectMessage(messageSafe))
					return;

			_logger.LogWarning(LogMessage($"Unhandled response:  '{messageSafe}'"));
		}
		catch (Exception e)
		{
			_logger.LogError(e, LogMessage("Exception while receiving message"));
		}
	}

	string LogMessage(string message) => $"[LUNO {_type} WEBSOCKET CLIENT] {message}";

	/// <summary>
	/// Handles the message and publishes new stream elements.
	/// </summary>
	/// <param name="message">The message to handle.</param>
	/// <returns>A boolean value to signify whether or not the message could be handled.</returns>
	protected abstract bool HandleObjectMessage(string message);
}
