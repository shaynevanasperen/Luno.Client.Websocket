using System;
using System.Text.Json.Serialization;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Models;

namespace Luno.Client.Websocket.Responses;

/// <summary>
/// Order message.
/// </summary>
public record OrderUpdateResponse
{
	/// <summary>
	/// Type of update.
	/// </summary>
	public string Type { get; init; } = null!;

	/// <summary>
	/// The timestamp.
	/// </summary>
	[JsonConverter(typeof(DateTimeIntegerMillisecondsConverter))]
	public DateTime Timestamp { get; init; }

	/// <summary>
	/// Order status update.
	/// </summary>
	public OrderStatusUpdate? OrderStatusUpdate { get; init; }

	/// <summary>
	/// Order fill update.
	/// </summary>
	public OrderFillUpdate? OrderFillUpdate { get; init; }
}
