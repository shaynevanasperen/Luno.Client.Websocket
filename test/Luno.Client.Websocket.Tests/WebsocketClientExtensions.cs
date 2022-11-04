using System;
using System.Net.WebSockets;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using Websocket.Client;
using Websocket.Client.Models;

namespace Luno.Client.Websocket.Tests;

static class WebsocketClientExtensions
{
	public static void LogReconnection(this IWebsocketClient client, ILogger logger, ReconnectionInfo info) =>
		logger.LogWarning("{ClientName} Websocket reconnected: {ReconnectionType}.", client.Name, info.Type);

	public static void LogDisconnection(this IWebsocketClient client, ILogger logger, DisconnectionInfo info)
	{
		if (info.Type is not DisconnectionType.Exit and not DisconnectionType.ByUser)
		{
			if (info.CloseStatus.HasValue && info.CloseStatusDescription != null)
				logger.LogWarning(@"{ClientName} Websocket disconnected: {DisconnectionType}.
{CloseStatus}
{CloseStatusDescription}", client.Name, info.Type, info.CloseStatus, info.CloseStatusDescription);
			else if (info.CloseStatus.HasValue)
				logger.LogWarning(@"{ClientName} Websocket disconnected: {DisconnectionType}.
{CloseStatus}", client.Name, info.Type, info.CloseStatus);
			else if (info.Exception != null)
				logger.LogWarning(@"{ClientName} Websocket disconnected: {DisconnectionType}.
{Exception}", client.Name, info.Type, info.Exception);
			else
				logger.LogWarning(@"{ClientName} Websocket disconnected: {DisconnectionType}.", client.Name, info.Type);
		}
	}

	public static ClientWebSocket WithDefaultKeepAliveInterval(this ClientWebSocket clientWebSocket) =>
		clientWebSocket.WithKeepAliveInterval(3.Seconds());

	public static ClientWebSocket WithKeepAliveInterval(this ClientWebSocket clientWebSocket, TimeSpan interval)
	{
		clientWebSocket.Options.KeepAliveInterval = interval;
		return clientWebSocket;
	}
}
