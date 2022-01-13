namespace Luno.Client.Websocket.Models;

/// <summary>
/// Balance update.
/// </summary>
public record BalanceUpdate
{
	/// <summary>
	/// Account Id.
	/// </summary>
	public long AccountId { get; init; }

	/// <summary>
	/// Row Index.
	/// </summary>
	public int RowIndex { get; init; }

	/// <summary>
	/// Balance.
	/// </summary>
	public double Balance { get; init; }

	/// <summary>
	/// Balance Delta.
	/// </summary>
	public double BalanceDelta { get; init; }

	/// <summary>
	/// Available.
	/// </summary>
	public double Available { get; init; }

	/// <summary>
	/// Available Delta.
	/// </summary>
	public double AvailableDelta { get; init; }
}
