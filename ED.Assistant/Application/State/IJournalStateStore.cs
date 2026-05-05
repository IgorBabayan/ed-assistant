namespace ED.Assistant.Application.State;

public interface IJournalStateStore
{
	JournalState CurrentState { get; }
	event EventHandler<JournalState>? StateChanged;

	void Update(JournalState state);
}
