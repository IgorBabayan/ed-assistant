using Avalonia.Threading;
using ED.Assistant.Application.Catalog;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Domain.Config;
using ED.Assistant.Presentation.Collections;
using ED.Assistant.Presentation.Helpers.Exobiology;
using ED.Assistant.Presentation.ViewModels.System;

namespace ED.Assistant.Presentation.ViewModels.Exobiology;

public sealed partial class ExobiologyViewModel : LoadableViewModel
{
	private readonly ISettingsStorage _settingsStorage;
	private readonly IGenusCatalog _genusCatalog;
	private readonly IPathFinder _pathFinder;

	private long? _previousSystemAddress;
	
	private IReadOnlyList<OrganicPlanetViewModel> _previousPlanets = [];

	public BulkObservableCollection<OrganicPlanetViewModel> Planets { get; } = new();
	
	public bool HasBiologicalSignals => Planets.Count > 0;
	
	[ObservableProperty]
	public partial bool HideExcludedSignals { get; set; }

	public ExobiologyViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache, IGenusCatalog genusCatalog, ISettingsStorage settingsStorage, IPathFinder pathFinder)
		: base(journalLoader, stateStore, memoryCache)
	{
		_genusCatalog = genusCatalog;
		_settingsStorage = settingsStorage;
		_pathFinder = pathFinder;

		_settingsStorage.SettingsSaved += OnSettingsSaved;
		_ = LoadSettingsAsync();
	}
	
	protected override void OnDispose()
	{
		_settingsStorage.SettingsSaved -= OnSettingsSaved;
		base.OnDispose();
	}

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		// Copy before the first await: the watcher keeps mutating the shared state
		var snapshot = new JournalState
		{
			FSDJump = state.FSDJump,
			Location = state.Location
		};

		foreach (var pair in state.Scans)
			snapshot.Scans.Add(pair.Key, pair.Value);

		foreach (var pair in state.FSSSignals)
			snapshot.FSSSignals.Add(pair.Key, pair.Value);

		foreach (var pair in state.SAASignals)
			snapshot.SAASignals.Add(pair.Key, pair.Value);

		snapshot.Organics.AddRange(state.Organics);

		IReadOnlyList<OrganicPlanetViewModel> planets = [];
		
		if (snapshot.CurrentSystemAddress is { } address)
		{
			// Seed data: loaded from the database once, then served from memory
			var catalog = await _genusCatalog.GetAllAsync(cancellationToken);

			planets = ExobiologyDisplayBuilder.Build(
				snapshot,
				catalog,
				_previousSystemAddress == address
					? _previousPlanets
					: null);
		}
		
		cancellationToken.ThrowIfCancellationRequested();
		
		_previousPlanets = planets;
		_previousSystemAddress = snapshot.CurrentSystemAddress;
		
		await Dispatcher.UIThread.InvokeAsync(() =>
		{
			cancellationToken.ThrowIfCancellationRequested();

			Planets.ReplaceAll(planets);
			OnPropertyChanged(nameof(HasBiologicalSignals));
		});
	}

	private void OnSettingsSaved(AppSettings settings)
		=> Dispatcher.UIThread.Post(() => HideExcludedSignals = settings.HideExcludedSignals);
	
	private async Task LoadSettingsAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
			HideExcludedSignals = settings.HideExcludedSignals;
		}
		catch (Exception)
		{
		}
	}
}