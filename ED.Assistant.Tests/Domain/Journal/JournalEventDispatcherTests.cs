using ED.Assistant.Application.JournalLoading;
using ED.Assistant.Domain.Events;

namespace ED.Assistant.Tests.Domain.Journal;

[TestClass]
public class JournalEventDispatcherTests
{
	[TestMethod]
	public async Task DispatchAsync_Should_Call_Handler_For_Matching_Event()
	{
		var dispatcher = new JournalEventDispatcher();
		CommanderEvent? result = null;

		dispatcher.On<CommanderEvent>("Commander", e => result = e);

		await dispatcher.DispatchAsync(ToAsyncEnumerable([
			"""{"timestamp":"2026-05-01T06:38:44Z","event":"Commander","FID":"F6973096","Name":"LORD.KORVIN"}"""
		]), TestContext.CancellationToken);

		Assert.IsNotNull(result);
		Assert.AreEqual("Commander", result.Event);
		Assert.AreEqual("LORD.KORVIN", result.Name);
		Assert.AreEqual("F6973096", result.FID);
	}

	[TestMethod]
	public async Task DispatchAsync_Should_Not_Call_Handler_For_Different_Event()
	{
		var dispatcher = new JournalEventDispatcher();
		var called = false;

		dispatcher.On<CommanderEvent>("Commander", _ => called = true);

		await dispatcher.DispatchAsync(ToAsyncEnumerable([
			"""{"timestamp":"2026-05-01T06:38:44Z","event":"LoadGame"}"""
		]), TestContext.CancellationToken);

		Assert.IsFalse(called);
	}

	[TestMethod]
	public async Task DispatchAsync_Should_Ignore_Invalid_Json()
	{
		var dispatcher = new JournalEventDispatcher();
		var called = false;

		dispatcher.On<CommanderEvent>("Commander", _ => called = true);

		await dispatcher.DispatchAsync(ToAsyncEnumerable([
			""""{"timestamp":"2026-05-01T06:38:44Z","event":"Commander""""
		]), TestContext.CancellationToken);

		Assert.IsFalse(called);
	}

	private static async IAsyncEnumerable<string> ToAsyncEnumerable(IEnumerable<string> lines)
	{
		foreach (var line in lines)
		{
			yield return line;
			await Task.Yield();
		}
	}

	public TestContext TestContext { get; set; }
}
