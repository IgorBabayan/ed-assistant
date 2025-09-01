using EdAssistant.Models.Enums;
using System;
using EdAssistant.ViewModels.Pages;

namespace EdAssistant.Services.Navigate;

public interface INavigationService
{
    void NavigateTo(DockEnum dock);
    PageViewModel? Current { get; }
    event EventHandler? CurrentChanged;
}