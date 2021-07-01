using System;
using System.Text.Json;

namespace Luno.Client.Websocket.Json
{
	static class JsonDocumentExtensions
	{
		public static T? ToObject<T>(this JsonDocument document, JsonSerializerOptions? options = null) =>
			document.RootElement.ToObject<T>(options);

		public static object? ToObject(this JsonDocument document, Type returnType, JsonSerializerOptions? options = null) =>
			document.RootElement.ToObject(returnType, options);
	}
}
