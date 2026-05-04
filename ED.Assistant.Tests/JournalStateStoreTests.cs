using ED.Assistant.Data.Models.Events;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.Tests;

[TestClass]
public sealed class JournalStateStoreTests
{
	[TestMethod]
	public void Update_Should_Set_CurrentState()
	{
		var store = new JournalStateStore();
		var state = new JournalState();

		store.Update(state);

		Assert.AreSame(state, store.CurrentState);
	}

	[TestMethod]
	public void Update_Should_Raise_StateChanged()
	{
		var store = new JournalStateStore();
		var state = new JournalState();

		JournalState? receivedState = null;
		var raised = false;

		store.StateChanged += (_, s) =>
		{
			raised = true;
			receivedState = s;
		};

		store.Update(state);

		Assert.IsTrue(raised);
		Assert.AreSame(state, receivedState);
	}
}