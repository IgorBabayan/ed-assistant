using ED.Assistant.Data.Models.Events;

namespace ED.Assistant.Services.Journal;

public interface IJournalStateStore
{
	JournalState CurrentState { get; }
	event EventHandler<JournalState>? StateChanged;

	void Update(JournalState state);
}

class JournalStateStore : IJournalStateStore
{
	private readonly object _lock = new();

	private JournalState _currentState = new();

	public JournalState CurrentState
	{
		get
		{
			lock (_lock)
			{
				return _currentState;
			}
		}
	}

	public event EventHandler<JournalState>? StateChanged;

	public void Update(JournalState newState)
	{
		ArgumentNullException.ThrowIfNull(newState);

		lock (_lock)
		{
			_currentState = newState;
		}

		StateChanged?.Invoke(this, newState);
	}
}