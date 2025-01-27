using System;
using System.Reactive.Subjects;
using System.Text.Json;
using Luno.Client.Websocket.Json;

namespace Luno.Client.Websocket.Models;

/// <summary>
/// Base class for all sequenced messages.
/// </summary>
static class Message
{
	internal static bool TryHandle<TResponse>(JsonElement response, ISubject<TResponse> subject)
	{
		var value = TryDeserialize<TResponse>(response);

		if (value != null)
		{
			subject.OnNext(value);
			return true;
		}

		return false;
	}

	internal static TResponse? TryDeserialize<TResponse>(JsonElement response)
	{
		try
		{
			return response.Deserialize<TResponse>(LunoJsonOptions.Default);
		}
		catch (Exception exception)
		{
			throw new($"Failed to deserialize JSON: {JsonSerializer.Serialize(response)}", exception);
		}
	}
}
