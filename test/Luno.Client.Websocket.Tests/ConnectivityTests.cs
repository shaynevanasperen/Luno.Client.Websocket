using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Luno.Client.Websocket.Client;
using Luno.Client.Websocket.Requests;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Logging;
using Websocket.Client;
using Xunit.Abstractions;

namespace Luno.Client.Websocket.Tests;

public class ConnectivityTests
{
	readonly ILogger _logger;

	public ConnectivityTests(ITestOutputHelper outputHelper)
	{
		_logger = new XUnitLogger(nameof(ConnectivityTests), outputHelper, new XUnitLoggerOptions());
	}

	[IntegrationBddfyFact]
	async Task ConnectingToUserWebsocketWorks()
	{
		var semaphore = new SemaphoreSlim(0, 1);

		var connection = new WebsocketClient(new Uri(LunoValues.ApiUserWebsocketUrl, UriKind.Absolute), () => new ClientWebSocket()
			.WithDefaultKeepAliveInterval())
		{
			Name = "User",
			ReconnectTimeout = null,
			ErrorReconnectTimeout = null
		};

		var client = new LunoUserWebsocketClient(_logger, connection);

		connection.ReconnectionHappened.Subscribe(info =>
		{
			connection.LogReconnection(_logger, info);
			client.Send(new AuthenticationRequest(LunoApi.Key, LunoApi.Secret));
			semaphore.Release();
		});
		connection.DisconnectionHappened.Subscribe(info => connection.LogDisconnection(_logger, info));

		await connection.Start();
		await semaphore.WaitAsync();
		await Task.Delay(2.Seconds());
	}

	[IntegrationBddfyFact]
	async Task ListeningOnMarketWebsocketWorks()
	{
		var semaphore = new SemaphoreSlim(0, 1);

		const string pair = "XBTZAR";
		var connection = new WebsocketClient(new Uri(LunoValues.ApiMarketWebsocketUrl(pair), UriKind.Absolute), () => new ClientWebSocket()
			.WithDefaultKeepAliveInterval())
		{
			Name = "Market",
			ReconnectTimeout = null,
			ErrorReconnectTimeout = null
		};

		var client = new LunoMarketWebsocketClient(_logger, connection, pair);

		client.Streams.StatusStream.Subscribe(response =>
		{
			_logger.LogInformation(JsonSerializer.Serialize(response));
			if (semaphore.CurrentCount == 0)
				semaphore.Release();
		});

		connection.ReconnectionHappened.Subscribe(info =>
		{
			connection.LogReconnection(_logger, info);
			client.Send(new AuthenticationRequest(LunoApi.Key, LunoApi.Secret));
		});
		connection.DisconnectionHappened.Subscribe(info => connection.LogDisconnection(_logger, info));

		await connection.Start();
		await semaphore.WaitAsync();
		await Task.Delay(2.Seconds());
	}
}
