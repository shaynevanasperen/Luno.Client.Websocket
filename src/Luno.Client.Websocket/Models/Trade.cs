namespace Luno.Client.Websocket.Models;

/// <summary>
/// Luno trade event.
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
