global using System;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using System.Threading;
global using System.Linq;
global using System.Text.Json.Serialization;
global using System.Collections.ObjectModel;
global using IOPath = System.IO.Path;

global using CommunityToolkit.Mvvm.Input;
global using CommunityToolkit.Mvvm.ComponentModel;

global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

global using Avalonia;
global using Avalonia.Controls;
global using Avalonia.Media;

global using ED.Assistant.Application.JournalLoading;
global using ED.Assistant.Application.State;
global using ED.Assistant.Presentation.ViewModels;
global using ED.Assistant.Domain.Events;
global using ED.Assistant.Data.Biology;