using ED.Assistant.Data.Models.Events;

namespace ED.Assistant.Services.Journal;

public interface IJournalStateStore
{
	JournalState State { get; }
	event EventHandler<JournalState>? StateChanged;

	void Update(JournalState state);
}

class JournalStateStore : IJournalStateStore
{
	private readonly object _lock = new();

	private JournalState _state = new();

	public JournalState State
	{
		get
		{
			lock (_lock)
			{
				return _state;
			}
		}
	}

	public event EventHandler<JournalState>? StateChanged;

	public void Update(JournalState newState)
	{
		if (newState is null)
			throw new ArgumentNullException(nameof(newState));

		lock (_lock)
		{
			_state = newState;
		}

		StateChanged?.Invoke(this, newState);
	}
}