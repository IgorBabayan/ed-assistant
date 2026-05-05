using ED.Assistant.Presentation.Converters;
using System.Text.Json;

namespace ED.Assistant.Application.JournalLoading;

sealed class JournalEventDispatcher : IJournalEventDispatcher
{
	private readonly JsonSerializerOptions _jsonOptions;
	private readonly List<IEventSubscription> _subscriptions = [];
	private readonly List<Action<IJournalEvent>> _anySubscriptions = [];

	private interface IEventSubscription
	{
		bool CanHandle(string line);
		IJournalEvent? Handle(string line);
	}

	public sealed class EventSubscription<TEvent> : IEventSubscription
		where TEvent : IJournalEvent
	{
		private readonly string _eventName;
		private readonly string _pattern;
		private readonly Action<TEvent> _handler;
		private readonly JsonSerializerOptions _jsonOptions;

		public EventSubscription(string eventName, Action<TEvent> handler, JsonSerializerOptions jsonOptions)
		{
			_eventName = eventName;
			_pattern = $"\"event\":\"{eventName}\"";
			_handler = handler;
			_jsonOptions = jsonOptions;
		}

		public bool CanHandle(string line) => line.Contains(_pattern, StringComparison.OrdinalIgnoreCase);

		public IJournalEvent? Handle(string line)
		{
			try
			{
				var journalEvent = JsonSerializer.Deserialize<TEvent>(line, _jsonOptions);
				if (journalEvent is null)
					return null;

				if (!string.Equals(journalEvent.Event, _eventName, StringComparison.OrdinalIgnoreCase))
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

	public JournalEventDispatcher(JsonSerializerOptions? jsonOptions = null)
	{
		_jsonOptions = jsonOptions ?? new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		_jsonOptions.Converters.Add(new ParentConverter());
	}

	public void OnAny(Action<IJournalEvent> handler) => _anySubscriptions.Add(handler);

	public void On<TEvent>(string eventName, Action<TEvent> handler)
	   where TEvent : IJournalEvent => _subscriptions.Add(new EventSubscription<TEvent>(eventName, handler, _jsonOptions));

	public async Task DispatchAsync(IAsyncEnumerable<string> lines, CancellationToken cancellationToken = default)
	{
		await foreach (var line in lines.WithCancellation(cancellationToken))
		{
			foreach (var subscription in _subscriptions)
			{
				if (!subscription.CanHandle(line))
					continue;

				var journalEvent = subscription.Handle(line);
				if (journalEvent is null)
					continue;

				foreach (var anyHandler in _anySubscriptions)
					anyHandler(journalEvent);
			}
		}
	}
}
