using ED.Assistant.Domain.Events;

namespace ED.Assistant.Application.JournalLoading;

public interface IJournalEventDispatcher
{
	void OnAny(Action<IJournalEvent> handler);
	void On<TEvent>(string eventName, Action<TEvent> handler) where TEvent : IJournalEvent;
	Task DispatchAsync(IAsyncEnumerable<string> lines, CancellationToken cancellationToken = default);
}
