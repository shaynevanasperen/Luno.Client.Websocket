namespace Luno.Client.Websocket.Responses
{
	/// <summary>
	/// Status message.
	/// </summary>
	public record StatusResponse
	{
		/// <summary>
		/// The status.
		/// </summary>
		public string Status { get; init; } = null!;
	}
}
