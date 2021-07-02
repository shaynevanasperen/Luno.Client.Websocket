namespace Luno.Client.Websocket
{
	/// <summary>
	/// Record for holding Luno secrets.
	/// </summary>
	public sealed record LunoSecrets
	{
		/// <summary>
		/// The API key.
		/// </summary>
		public string ApiKey { get; init; } = null!;

		/// <summary>
		/// The API secret.
		/// </summary>
		public string ApiSecret { get; init; } = null!;
	}
}
