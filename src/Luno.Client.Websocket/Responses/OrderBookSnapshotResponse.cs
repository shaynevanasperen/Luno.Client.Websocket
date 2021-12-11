using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Models;

namespace Luno.Client.Websocket.Responses;

/// <summary>
/// Order book snapshot message.
/// </summary>
public record OrderBookSnapshotResponse : Message
{
	/// <summary>
	/// The asks.
	/// </summary>
	public IReadOnlyList<Order> Asks { get; init; } = Array.Empty<Order>();

	/// <summary>
	/// The bids.
	/// </summary>
	public IReadOnlyList<Order> Bids { get; init; } = Array.Empty<Order>();

	/// <summary>
	/// The trading status.
	/// </summary>
	public string Status { get; init; } = null!;

	/// <summary>
	/// The timestamp.
	/// </summary>
	[JsonConverter(typeof(DateTimeIntegerMillisecondsConverter))]
	public DateTime Timestamp { get; init; }
}

/// <summary>
/// Luno order.
/// </summary>
public record Order
{
	/// <summary>
	/// The id.
	/// </summary>
	public string Id { get; init; } = null!;

	/// <summary>
	/// The price.
	/// </summary>
	public double Price { get; init; }

	/// <summary>
	/// The volume.
	/// </summary>
	public double Volume { get; set; }
}
