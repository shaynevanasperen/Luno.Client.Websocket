namespace Luno.Client.Websocket.Responses
{
	/// <summary>
	/// Order status message.
	/// </summary>
	public record OrderStatusResponse
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
		public string Status { get; init; } = null!;
	}
}
