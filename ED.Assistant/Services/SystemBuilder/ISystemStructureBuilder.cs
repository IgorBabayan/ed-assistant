using ED.Assistant.Data.Models.Events;

namespace ED.Assistant.Services.SystemBuilder;

public interface ISystemStructureBuilder
{
	SystemStructure Build(JournalState state);
}

class SystemStructureBuilder : ISystemStructureBuilder
{
	public SystemStructure Build(JournalState state)
	{
		if (state is null)
			throw new ArgumentNullException(nameof(state));

		if (state.FSDJump is null)
			throw new ArgumentException($"{nameof(state.FSDJump)} event is required to build system structure.", nameof(state));

		var systemStructure = new SystemStructure
		{
			Name = state.FSDJump.StarSystem
		};

		BuildRootLevel(systemStructure, state);


		return systemStructure;
	}

	private void BuildRootLevel(SystemStructure systemStructure, JournalState state)
	{
		var baryCenters = (state.BaryCentres?.Values?.Where(x => x.SystemAddress == state.FSDJump!.SystemAddress)
			?? []).ToList();
		if (baryCenters.Any())
		{
			for (var index = 0; index < baryCenters.Count; index++)
			{
				var baryCenter = baryCenters[index];
				systemStructure.Bodies.Add(new()
				{
					BodyId = baryCenter.BodyId,
					Type = "Bary Center",
					Name = $"Bary Center {index + 1}"
				});
			}
		}
		else
		{

		}
	}
}