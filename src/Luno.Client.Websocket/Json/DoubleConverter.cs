using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Luno.Client.Websocket.Json
{
	sealed class DoubleConverter : JsonConverter<double>
	{
		public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			try
			{
				return reader.TokenType == JsonTokenType.String
					? double.Parse(reader.GetString()!, NumberStyles.Float, CultureInfo.InvariantCulture)
					: reader.GetDouble();
			}
			catch (Exception exception)
			{
				throw new Exception($"Invalid value at index {reader.TokenStartIndex}.", exception);
			}
		}

		public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options) =>
			writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
	}
}
