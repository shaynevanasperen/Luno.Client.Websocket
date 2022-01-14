namespace Luno.Client.Websocket.Models;

/// <summary>
/// Base class for all sequenced messages.
/// </summary>
public abstract record SequencedMessage
{
	/// <summary>
	/// The message sequence number.
	/// </summary>
	public long Sequence { get; set; }
}
