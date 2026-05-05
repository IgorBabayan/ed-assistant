namespace ED.Assistant.Domain.System;

public interface ISystemStructureBuilder
{
	SystemStructure Build(JournalState state);
}
