using ED.Assistant.Application.JournalLoading;
using ED.Assistant.Domain.Events;

namespace ED.Assistant.Application.State;

class JournalStateAggregator : IJournalStateAggregator
{
	private readonly IJournalEventDispatcher _dispatcher;

	public JournalStateAggregator(IJournalEventDispatcher dispatcher) => _dispatcher = dispatcher;

	public void RegisterLast<TEvent>(string eventName, Action<TEvent> setter) 
		where TEvent : class, IJournalEvent => _dispatcher.On<TEvent>(eventName, setter);

	public void RegisterByKey<TEvent, TKey>(string eventName, Func<TEvent, TKey> keySelector,
		IDictionary<TKey, TEvent> target)
		where TEvent : class, IJournalEvent
		where TKey : notnull
			=> _dispatcher.On<TEvent>(eventName, e =>
				{
					 var key = keySelector(e);
					 target[key] = e;
				});
}