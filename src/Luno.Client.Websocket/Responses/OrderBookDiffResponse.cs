using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Luno.Client.Websocket.Json;
using Luno.Client.Websocket.Models;

namespace Luno.Client.Websocket.Responses
{
	/// <summary>
	/// Order book snapshot message.
	/// </summary>
	public record OrderBookDiffResponse : Message
	{
		/// <summary>
		/// The asks.
		/// </summary>
		public IReadOnlyList<Trade> TradeUpdates { get; init; } = Array.Empty<Trade>();

		/// <summary>
		/// The create update.
		/// </summary>
		public Create? CreateUpdate { get; init; }

		/// <summary>
		/// The delete update.
		/// </summary>
		public Delete? DeleteUpdate { get; init; }

		/// <summary>
		/// The status update.
		/// </summary>
		public PairStatus? StatusUpdate { get; init; }

		/// <summary>
		/// The timestamp.
		/// </summary>
		[JsonConverter(typeof(DateTimeIntegerMillisecondsConverter))]
		public DateTime Timestamp { get; init; }
	}

	/// <summary>
	/// Luno create instruction.
	/// </summary>
	public record Create
	{
		/// <summary>
		/// The order id.
		/// </summary>
		public string OrderId { get; init; } = null!;

		/// <summary>
		/// The type. Either "BID" or "ASK".
		/// </summary>
		public string Type { get; init; } = null!;

		/// <summary>
		/// The price.
		/// </summary>
		public double Price { get; init; }

		/// <summary>
		/// The volume.
		/// </summary>
		public double Volume { get; init; }
	}

	/// <summary>
	/// Luno delete instruction.
	/// </summary>
	public record Delete
	{
		/// <summary>
		/// The order id.
		/// </summary>
		public string OrderId { get; init; } = null!;
	}
}
