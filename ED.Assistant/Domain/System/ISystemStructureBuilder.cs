using ED.Assistant.Domain.Events;

namespace ED.Assistant.Domain.System;

public interface ISystemStructureBuilder
{
	SystemStructure Build(JournalState state);
}
