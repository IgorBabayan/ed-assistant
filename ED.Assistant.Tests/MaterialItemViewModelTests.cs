using ED.Assistant.ViewModels;

namespace ED.Assistant.Tests;

[TestClass]
public sealed class MaterialItemViewModelTests
{
	[TestMethod]
	[DataRow("Raw", 300)]
	[DataRow("Manufactured", 250)]
	[DataRow("Encoded", 250)]
	[DataRow("Unknown", 300)]
	public void MaxCapacity_Should_Return_Correct_Value(string category, int expected)
	{
		var vm = new MaterialItemViewModel
		{
			Category = category
		};

		Assert.AreEqual(expected, vm.MaxCapacity);
	}

	[TestMethod]
	public void StockText_Should_Use_Count_And_MaxCapacity()
	{
		var vm = new MaterialItemViewModel
		{
			Category = "Manufactured",
			Count = 42
		};

		Assert.AreEqual("42 / 250", vm.StockText);
	}
}
