using System.Diagnostics;
using TestStack.BDDfy.Xunit;

namespace Luno.Client.Websocket.Tests;

sealed class IntegrationBddfyFactAttribute : BddfyFactAttribute
{
	public const string OnlyRunsWhenDebuggerAttached = "Only runs when debugger is attached.";

	public IntegrationBddfyFactAttribute() => Skip = Debugger.IsAttached
		? null
		: OnlyRunsWhenDebuggerAttached;
}
