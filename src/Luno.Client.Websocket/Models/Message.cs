using System;
using System.Reactive.Subjects;
using System.Text.Json;
using Luno.Client.Websocket.Json;

namespace Luno.Client.Websocket.Models
{
	/// <summary>
	/// Base class for all messages.
	/// </summary>
	public abstract record Message
	{
		/// <summary>
		/// The message sequence number.
		/// </summary>
		public long Sequence { get; set; }

		internal static bool TryHandle<TResponse>(JsonElement response, ISubject<TResponse> subject)
		{
			TResponse? value;
			try
			{
				value = response.Deserialize<TResponse>(LunoJsonOptions.Default);
			}
			catch (Exception exception)
			{
				throw new Exception($"Failed to deserialize JSON: {JsonSerializer.Serialize(response)}", exception);
			}

			if (value != null)
			{
				subject.OnNext(value);
				return true;
			}

			return false;
		}
	}
}
