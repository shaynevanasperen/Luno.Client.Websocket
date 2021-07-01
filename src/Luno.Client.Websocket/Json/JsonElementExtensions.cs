using System;
using System.Buffers;
using System.Text.Json;

namespace Luno.Client.Websocket.Json
{
	static class JsonElementExtensions
	{
		public static T? ToObject<T>(this JsonElement element, JsonSerializerOptions? options = null)
		{
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter))
				element.WriteTo(writer);

			return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
		}

		public static object? ToObject(this JsonElement element, Type returnType, JsonSerializerOptions? options = null)
		{
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter))
				element.WriteTo(writer);

			return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, returnType, options);
		}
	}
}
