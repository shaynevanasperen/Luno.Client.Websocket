using System;
using System.Text.Json;
using Luno.Client.Websocket.Json;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace Luno.Client.Websocket.Client;

/// <summary>
/// Luno websocket client.
/// </summary>
public abstract class LunoWebsocketClient : IDisposable
{
	readonly ILogger _logger;

	readonly IDisposable _clientMessageReceivedSubscription;

	/// <summary>
	/// Creates a new instance.
	/// </summary>
	/// <param name="logger">The logger to use for logging any warnings or errors.</param>
	/// <param name="client">The client to use for the trade websocket.</param>
	protected LunoWebsocketClient(ILogger logger, IWebsocketClient client)
	{
		_logger = logger;
		Client = client;
		_clientMessageReceivedSubscription = Client.MessageReceived.Subscribe(HandleMessage);
	}

	/// <summary>
	/// The type of the websocket client (MARKET or USER).
	/// </summary>
	protected abstract string Type { get; }

	/// <summary>
	/// The underlying websocket client.
	/// </summary>
	protected IWebsocketClient Client { get; }

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
				HandleEmptyMessage();
				return;
			}

			if (messageSafe.StartsWith("{", StringComparison.OrdinalIgnoreCase))
				if (HandleObjectMessage(messageSafe))
					return;

			_logger.LogWarning(LogMessage($"Unhandled response: '{messageSafe}'"));
		}
		catch (Exception e)
		{
			_logger.LogError(e, LogMessage("Exception while receiving message"));
		}
	}

	string LogMessage(string message) => $"[{Client.Name ?? "LUNO " + Type} WEBSOCKET CLIENT] {message}";

	/// <summary>
	/// Handles the message and publishes new stream elements.
	/// </summary>
	protected virtual void HandleEmptyMessage() { }

	/// <summary>
	/// Handles the message and publishes new stream elements.
	/// </summary>
	/// <param name="message">The message to handle.</param>
	/// <returns>A boolean value to signify whether the message could be handled.</returns>
	protected abstract bool HandleObjectMessage(string message);
}
