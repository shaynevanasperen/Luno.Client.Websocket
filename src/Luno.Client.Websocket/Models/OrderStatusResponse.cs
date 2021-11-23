namespace Luno.Client.Websocket.Models
{
	/// <summary>
	/// Order status update.
	/// </summary>
	public record OrderStatusUpdate
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
