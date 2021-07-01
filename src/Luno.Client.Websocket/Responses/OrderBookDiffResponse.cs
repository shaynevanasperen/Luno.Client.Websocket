using System;
using System.Collections.Generic;
using Luno.Client.Websocket.Messages;

namespace Luno.Client.Websocket.Responses
{
	/// <summary>
	/// Order book snapshot message.
	/// </summary>
	public record OrderBookDiffResponse : MessageBase
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

	/// <summary>
	/// Luno trade instruction.
	/// </summary>
	public record Trade
	{
		/// <summary>
		/// The maker order id.
		/// </summary>
		public string MakerOrderId { get; init; } = null!;

		/// <summary>
		/// The taker order id.
		/// </summary>
		public string TakerOrderId { get; init; } = null!;

		/// <summary>
		/// The base delta.
		/// </summary>
		public double Base { get; init; }

		/// <summary>
		/// The quote delta.
		/// </summary>
		public double Counter { get; init; }
	}

	/// <summary>
	/// Luno pair trading status instruction.
	/// </summary>
	public record PairStatus
	{
		/// <summary>
		/// The trading status. e.g. "ACTIVE" or "POSTONLY".
		/// </summary>
		public string Status { get; init; } = null!;
	}
}
