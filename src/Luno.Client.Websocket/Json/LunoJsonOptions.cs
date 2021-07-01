using System.Linq;
using System.Text.Json;

namespace Luno.Client.Websocket.Json
{
	static class LunoJsonOptions
	{
		public static JsonSerializerOptions Default = new(JsonSerializerDefaults.Web)
		{
			PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
		};
	}

	class SnakeCaseNamingPolicy : JsonNamingPolicy
	{
		public static SnakeCaseNamingPolicy Instance { get; } = new();

		public override string ConvertName(string name) => string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
	}
}
