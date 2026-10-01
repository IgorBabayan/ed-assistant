using ED.Assistant.Extensions;
using ED.Assistant.Presentation.ViewModels.ConfirmDialog;
using ED.Assistant.Presentation.ViewModels.Import;
using ED.Assistant.Presentation.ViewModels.Plugin;
using ED.Assistant.Presentation.ViewModels.Settings;
using ED.Assistant.Presentation.Views.ConfirmDialog;
using ED.Assistant.Presentation.Views.Import;
using ED.Assistant.Presentation.Views.Plugin;
using ED.Assistant.Presentation.Views.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Application.Dialog;

internal class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task<TResult?> ShowDialogAsync<TViewModel, TResult>(TViewModel viewModel)
        where TViewModel : BaseViewModel
    {
        // The topmost window, so a dialog opened from another dialog shows above it
        var owner = Utils.GetTopWindow();
        Window dialog = viewModel switch
        {
            ConfirmDialogViewModel => _serviceProvider.GetRequiredService<ConfirmDialogWindow>(),
            SettingsViewModel => _serviceProvider.GetRequiredService<SettingsWindow>(),
            ImportFolderViewModel => _serviceProvider.GetRequiredService<ImportFolderWindow>(),
            PluginSettingsDialogViewModel => _serviceProvider.GetRequiredService<PluginSettingsDialogWindow>(),

            _ => throw new InvalidOperationException($"No dialog registered for {typeof(TViewModel).Name}")
        };

        dialog.DataContext = viewModel;
        dialog.Width = owner.Bounds.Width;
        dialog.Height = owner.Bounds.Height;
        dialog.Position = owner.Position;
        return await dialog.ShowDialog<TResult?>(owner);
    }
}