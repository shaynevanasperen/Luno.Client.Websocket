using System.Reactive.Subjects;
using System.Text.Json;
using Luno.Client.Websocket.Json;

namespace Luno.Client.Websocket.Messages
{
	/// <summary>
	/// Base class for all messages.
	/// </summary>
	public abstract record MessageBase
	{
		/// <summary>
		/// The message sequence number.
		/// </summary>
		public long Sequence { get; set; }

		internal static bool TryHandle<TResponse>(JsonElement response, ISubject<TResponse> subject)
		{
			var value = response.ToObject<TResponse>(LunoJsonOptions.Default);
			if (value != null)
			{
				subject.OnNext(value);
				return true;
			}

			return false;
		}
	}
}
