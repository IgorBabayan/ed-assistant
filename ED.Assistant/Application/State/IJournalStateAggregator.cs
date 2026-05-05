namespace ED.Assistant.Application.State;

interface IJournalStateAggregator
{
	void RegisterLast<TEvent>(string eventName, Action<TEvent> setter)
		where TEvent : class, IJournalEvent;

	void RegisterByKey<TEvent, TKey>(string eventName, Func<TEvent, TKey> keySelector,
		IDictionary<TKey, TEvent> target)
		where TEvent : class, IJournalEvent
		where TKey : notnull;
}
