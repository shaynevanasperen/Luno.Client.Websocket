namespace Luno.Client.Websocket.Responses
{
	/// <summary>
	/// Trade message.
	/// </summary>
	record TradeResponse : Trade
	{
		/// <summary>
		/// The order id.
		/// </summary>
		public string OrderId { get; init; } = null!;

		/// <summary>
		/// The volume.
		/// </summary>
		public double Volume { get; init; }
	}
}
