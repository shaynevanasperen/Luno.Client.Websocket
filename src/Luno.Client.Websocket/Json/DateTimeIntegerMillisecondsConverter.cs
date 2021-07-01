using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Luno.Client.Websocket.Json
{
	sealed class DateTimeIntegerMillisecondsConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
			DateTime.UnixEpoch.AddMilliseconds(reader.GetInt64());

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) =>
			writer.WriteNumberValue((long)value.Subtract(DateTime.UnixEpoch).TotalMilliseconds);
	}
}
