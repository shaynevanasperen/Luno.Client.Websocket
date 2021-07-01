namespace Luno.Client.Websocket.Requests
{
	/// <summary>
	/// Authentication message.
	/// </summary>
	public record AuthenticationRequest
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public AuthenticationRequest(string apiKeyId, string apiKeySecret)
		{
			ApiKeyId = apiKeyId;
			ApiKeySecret = apiKeySecret;
		}

		/// <summary>
		/// The API key.
		/// </summary>
		public string ApiKeyId { get; }

		/// <summary>
		/// The API secret.
		/// </summary>
		public string ApiKeySecret { get; }
	}
}
