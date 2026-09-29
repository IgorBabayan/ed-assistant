using ED.Assistant.Presentation.Converters;
using System.Text.Json;

namespace ED.Assistant.Application.JournalLoading;

sealed class JournalEventDispatcher : IJournalEventDispatcher
{
	// One shared instance: System.Text.Json caches type metadata per options object,
	// so creating options per batch rebuilt that cache on every watcher tick.
	private static readonly JsonSerializerOptions DefaultOptions = CreateDefaultOptions();

	private readonly JsonSerializerOptions _jsonOptions;
	private readonly Dictionary<string, List<IEventSubscription>> _subscriptions = new(StringComparer.OrdinalIgnoreCase);
	private readonly List<Action<IJournalEvent>> _anySubscriptions = [];

	private interface IEventSubscription
	{
		IJournalEvent? Handle(string line);
	}

	private sealed class EventSubscription<TEvent> : IEventSubscription
		where TEvent : IJournalEvent
	{
		private readonly Action<TEvent> _handler;
		private readonly JsonSerializerOptions _jsonOptions;

		public EventSubscription(Action<TEvent> handler, JsonSerializerOptions jsonOptions)
		{
			_handler = handler;
			_jsonOptions = jsonOptions;
		}

		public IJournalEvent? Handle(string line)
		{
			try
			{
				var journalEvent = JsonSerializer.Deserialize<TEvent>(line, _jsonOptions);
				if (journalEvent is null)
					return null;

				_handler(journalEvent);
				return journalEvent;
			}
			catch (JsonException)
			{
				return null;
			}
		}
	}

	/// <param name="jsonOptions">
	/// Optional custom options. They are used as-is (not modified), so they must already
	/// contain every converter the journal events need.
	/// </param>
	public JournalEventDispatcher(JsonSerializerOptions? jsonOptions = null)
		=> _jsonOptions = jsonOptions ?? DefaultOptions;

	public void OnAny(Action<IJournalEvent> handler) => _anySubscriptions.Add(handler);

	public void On<TEvent>(string eventName, Action<TEvent> handler)
		where TEvent : IJournalEvent
	{
		if (!_subscriptions.TryGetValue(eventName, out var list))
			_subscriptions[eventName] = list = [];

		list.Add(new EventSubscription<TEvent>(handler, _jsonOptions));
	}

	public async Task DispatchAsync(IAsyncEnumerable<string> lines, CancellationToken cancellationToken = default)
	{
		await foreach (var line in lines.WithCancellation(cancellationToken))
		{
			// Read the event name once and look it up, instead of scanning
			// the whole line once per subscription.
			var eventName = JournalLine.ReadEventName(line);
			if (eventName is null || !_subscriptions.TryGetValue(eventName, out var subscriptions))
				continue;

			foreach (var subscription in subscriptions)
			{
				var journalEvent = subscription.Handle(line);
				if (journalEvent is null)
					continue;

				foreach (var anyHandler in _anySubscriptions)
					anyHandler(journalEvent);
			}
		}
	}

	private static JsonSerializerOptions CreateDefaultOptions()
	{
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		options.Converters.Add(new ParentConverter());
		return options;
	}
}