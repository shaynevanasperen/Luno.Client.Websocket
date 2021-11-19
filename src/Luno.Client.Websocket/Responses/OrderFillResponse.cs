namespace Luno.Client.Websocket.Responses
{
	/// <summary>
	/// Order fill message.
	/// </summary>
	public record OrderFillResponse
	{
		/// <summary>
		/// Order Id.
		/// </summary>
		public string OrderId { get; init; } = null!;

		/// <summary>
		/// Market Id.
		/// </summary>
		public string MarketId { get; init; } = null!;

		/// <summary>
		/// Status.
		/// </summary>
		public double BaseFill { get; init; }

		/// <summary>
		/// Status.
		/// </summary>
		public double CounterFill { get; init; }

		/// <summary>
		/// Status.
		/// </summary>
		public double BaseDelta { get; init; }

		/// <summary>
		/// Status.
		/// </summary>
		public double CounterDelta { get; init; }

		/// <summary>
		/// Status.
		/// </summary>
		public double BaseFee { get; init; }

		/// <summary>
		/// Status.
		/// </summary>
		public double CounterFee { get; init; }

		/// <summary>
		/// Status.
		/// </summary>
		public double BaseFeeDelta { get; init; }

		/// <summary>
		/// Status.
		/// </summary>
		public double CounterFeeDelta { get; init; }
	}
}
