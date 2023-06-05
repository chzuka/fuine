global using System;
global using System.Buffers;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Net.Http;
global using System.Net.Http.Json;
global using System.Net.NetworkInformation;
global using System.Net.WebSockets;
global using System.Reflection;
global using System.Runtime.InteropServices;
global using System.Security;
global using System.Security.Principal;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Data;
global using System.Windows.Interop;
global using System.Windows.Media;
global using System.Windows.Threading;

global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;

global using Fuine.Helpers;
global using Fuine.Helpers.DragDrop;
global using Fuine.Helpers.EfficiencyMode;
global using Fuine.Helpers.Job;
global using Fuine.Helpers.Loopback;
global using Fuine.Models.App.Configs;
global using Fuine.Models.App.Connections;
global using Fuine.Models.App.Logs;
global using Fuine.Models.App.Proxies;
global using Fuine.Models.Clash;
global using Fuine.Models.Clash.Configs;
global using Fuine.Models.Clash.Connections;
global using Fuine.Models.Clash.Logs;
global using Fuine.Models.Clash.Proxies;
global using Fuine.Services;
global using Fuine.ViewModels.Pages;
global using Fuine.ViewModels.Pages.Settings;
global using Fuine.ViewModels.Windows;
global using Fuine.Views.Pages;
global using Fuine.Views.Pages.Settings;
global using Fuine.Views.Windows;

global using LiveChartsCore;
global using LiveChartsCore.SkiaSharpView;
global using LiveChartsCore.SkiaSharpView.Painting;
global using LiveChartsCore.SkiaSharpView.Painting.Effects;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Toolkit.Uwp.Notifications;
global using Microsoft.Win32.SafeHandles;
global using Microsoft.Win32;

global using SkiaSharp;

global using Tomlyn;
global using Tomlyn.Model;

global using System.Reactive.Linq;
global using DynamicData;
global using DynamicData.Binding;

global using Wpf.Ui.Appearance;
global using Wpf.Ui.Common;
global using Wpf.Ui.Contracts;
global using Wpf.Ui.Controls;
global using Wpf.Ui.Controls.Window;
global using Wpf.Ui.Controls.IconElements;
global using Wpf.Ui.Controls.Navigation;
global using Wpf.Ui.Services;

global using IDataObject_Com = System.Runtime.InteropServices.ComTypes.IDataObject;



