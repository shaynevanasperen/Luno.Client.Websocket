namespace Luno.Client.Websocket.Models;

/// <summary>
/// Luno pair trading status event.
/// </summary>
public record PairStatus
{
	/// <summary>
	/// The trading status. e.g. "ACTIVE" or "POSTONLY".
	/// </summary>
	public string Status { get; init; } = null!;
}
