namespace Luno.Client.Websocket.Models;

/// <summary>
/// Order fill update.
/// </summary>
public record OrderFillUpdate
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
	/// Base Fill.
	/// </summary>
	public double BaseFill { get; init; }

	/// <summary>
	/// Counter Fill.
	/// </summary>
	public double CounterFill { get; init; }

	/// <summary>
	/// Base Delta.
	/// </summary>
	public double BaseDelta { get; init; }

	/// <summary>
	/// Counter Delta.
	/// </summary>
	public double CounterDelta { get; init; }

	/// <summary>
	/// Base Fee.
	/// </summary>
	public double BaseFee { get; init; }

	/// <summary>
	/// Counter Fee.
	/// </summary>
	public double CounterFee { get; init; }

	/// <summary>
	/// Base Fee Delta.
	/// </summary>
	public double BaseFeeDelta { get; init; }

	/// <summary>
	/// Counter Fee Delta.
	/// </summary>
	public double CounterFeeDelta { get; init; }
}
