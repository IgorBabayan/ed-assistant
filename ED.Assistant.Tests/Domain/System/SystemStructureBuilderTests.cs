using ED.Assistant.Domain.Events;
using ED.Assistant.Domain.System;

namespace ED.Assistant.Tests.Domain.System;

[TestClass]
public sealed class SystemStructureBuilderTests
{
	private readonly SystemStructureBuilder _builder = new();

	[TestMethod]
	public void Build_Should_Throw_When_State_Is_Null()
		=> Assert.Throws<ArgumentNullException>(() => _builder.Build(null!));

	[TestMethod]
	public void Build_Should_Throw_When_FSDJump_Is_Null()
	{
		var state = new JournalState();
		Assert.Throws<ArgumentException>(() => _builder.Build(state));
	}

	[TestMethod]
	public void Build_Should_Create_SystemStructure_With_System_Name()
	{
		var state = new JournalState
		{
			FSDJump = new FSDJumpEvent
			{
				StarSystem = "Sol",
				SystemAddress = 123
			}
		};

		var result = _builder.Build(state);

		Assert.AreEqual("Sol", result.Name);
		Assert.AreEqual(0, result.Roots.Count);
	}

	[TestMethod]
	public void Build_Should_Build_Hierarchy_Without_Barycenter()
	{
		var state = new JournalState
		{
			FSDJump = new FSDJumpEvent
			{
				StarSystem = "Test System",
				SystemAddress = 123
			}
		};

		state.Scans[1] = new ScanEvent
		{
			SystemAddress = 123,
			BodyId = 1,
			BodyName = "Test System A",
			StarType = "K"
		};

		state.Scans[2] = new ScanEvent
		{
			SystemAddress = 123,
			BodyId = 2,
			BodyName = "Test System A 1",
			PlanetClass = "Icy body",
			Parents =
			[
				new Parent { Type = "Star", BodyId = 1 },
			new Parent { Type = "Null", BodyId = 0 }
			]
		};

		var result = _builder.Build(state);

		Assert.AreEqual("Test System", result.Name);

		Assert.AreEqual(1, result.Roots.Count);

		var star = result.Roots.Single();
		Assert.AreEqual("Test System A", star.Name);

		Assert.AreEqual(1, star.Children.Count);

		var planet = star.Children.Single();
		Assert.AreEqual("Test System A 1", planet.Name);
	}

	[TestMethod]
	public void Build_Should_Not_Place_Stars_Under_Barycenter_Yet()
	{
		var state = new JournalState
		{
			FSDJump = new FSDJumpEvent
			{
				StarSystem = "Test System",
				SystemAddress = 123
			}
		};

		state.BaryCentres[0] = new BaryCentreEvent
		{
			SystemAddress = 123,
			BodyId = 0
		};

		state.Scans[1] = new ScanEvent
		{
			SystemAddress = 123,
			BodyId = 1,
			BodyName = "Test System A",
			StarType = "K",
			Parents =
			[
				new Parent { Type = "BaryCentre", BodyId = 0 },
			new Parent { Type = "Null", BodyId = 0 }
			]
		};

		var result = _builder.Build(state);

		Assert.AreEqual(1, result.Roots.Count);

		var star = result.Roots.Single();
		Assert.AreEqual("Test System A", star.Name);
	}
}