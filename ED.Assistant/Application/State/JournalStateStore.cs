using ED.Assistant.Domain.Events;

namespace ED.Assistant.Application.State;

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