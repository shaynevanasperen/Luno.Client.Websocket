namespace Luno.Client.Websocket
{
	/// <summary>
	/// Luno static urls
	/// </summary>
	public static class LunoValues
	{
		/// <summary>
		/// Luno url to websocket API for a particular market
		/// </summary>
		public static string ApiMarketWebsocketUrl(string pair) => $"wss://ws.luno.com/api/1/stream/{pair}";

		/// <summary>
		/// Luno url to websocket API for user account
		/// </summary>
		public const string ApiUserWebsocketUrl = "wss://ws.luno.com/api/1/userstream";
	}
}
