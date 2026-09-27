using ED.Assistant.Application.JournalLoading;
using ED.Assistant.Application.State;
using ED.Assistant.Domain.Events;
using ED.Assistant.Presentation.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace ED.Assistant.Tests.Presentation.ViewModels;

[TestClass]
public sealed class LoadableViewModelTests
{
	[TestMethod]
	public async Task StateChangeDuringActivationIsProcessedAfterCurrentUpdate()
	{
		var loader = new Mock<IJournalLoaderService>();
		var store = new JournalStateStore();
		using var cache = new MemoryCache(new MemoryCacheOptions());
		using var viewModel = new TestLoadableViewModel(loader.Object, store, cache);

		store.Update(new JournalState { FileName = "first" });
		await viewModel.FirstStarted.Task.WaitAsync(TimeSpan.FromSeconds(1));

		store.Update(new JournalState { FileName = "second" });
		viewModel.ReleaseFirst.TrySetResult(true);

		await viewModel.SecondApplied.Task.WaitAsync(TimeSpan.FromSeconds(1));

		CollectionAssert.AreEqual(
			new[] { "first", "second" },
			viewModel.AppliedStates.ToArray());
	}

	private sealed class TestLoadableViewModel : LoadableViewModel
	{
		public TaskCompletionSource<bool> FirstStarted { get; } =
			new(TaskCreationOptions.RunContinuationsAsynchronously);

		public TaskCompletionSource<bool> ReleaseFirst { get; } =
			new(TaskCreationOptions.RunContinuationsAsynchronously);

		public TaskCompletionSource<bool> SecondApplied { get; } =
			new(TaskCreationOptions.RunContinuationsAsynchronously);

		public List<string> AppliedStates { get; } = [];

		protected override bool ActivateOnNavigation => true;

		public TestLoadableViewModel(IJournalLoaderService journalLoader,
			IJournalStateStore stateStore, IMemoryCache memoryCache)
			: base(journalLoader, stateStore, memoryCache)
		{
		}

		protected override async Task UpdateFromStateAsync(JournalState state,
			CancellationToken cancellationToken = default)
		{
			AppliedStates.Add(state.FileName ?? string.Empty);

			if (state.FileName == "first")
			{
				FirstStarted.TrySetResult(true);
				await ReleaseFirst.Task.WaitAsync(cancellationToken);
			}

			if (state.FileName == "second")
				SecondApplied.TrySetResult(true);
		}
	}
}
