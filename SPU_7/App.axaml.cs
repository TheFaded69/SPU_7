using System;
using AutoMapper;
using Avalonia;
using Avalonia.Markup.Xaml;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Prism.DryIoc;
using Prism.Ioc;
using SkiaSharp;
using SPU_7.Database.DbContext;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Mapper;
using SPU_7.Models.Scripts;
using SPU_7.Models.Services.Autorization;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.ViewModels;
using SPU_7.ViewModels.DeviceInformationViewModels;
using SPU_7.ViewModels.ScriptViewModels;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationExecutingViewModels;
using SPU_7.ViewModels.Settings;
using SPU_7.ViewModels.WorkReportViewModels;
using SPU_7.Views;
using SPU_7.Views.ScriptViews;
using SPU_7.Views.ScriptViews.OperationResultViews;
using SPU_7.Views.ScriptViews.OperationViews;
using SPU_7.Views.ScriptViews.OperationViews.OperationConfigurationViews;
using SPU_7.Views.Settings;
using SPU_7.Views.WorkReportViews;
using SPU_7.Views.WorkReportViews.PdfViewerView;


namespace SPU_7;

public partial class App : PrismApplication
{
    public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        protected override AvaloniaObject CreateShell()
        {
            return Container.Resolve<MainWindowView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Services
            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();
            containerRegistry.RegisterSingleton<IAuthorizationService, AuthorizationService>();
            containerRegistry.RegisterSingleton<IStandSettingsService, StandSettingsService>();
            containerRegistry.RegisterSingleton<IStandController, StandController>();
            containerRegistry.RegisterSingleton<IScriptController, ScriptController>();
            containerRegistry.RegisterSingleton<IManualOperationService, ManualOperationService>();
            containerRegistry.RegisterSingleton<ITimerService, TimerService>();
            containerRegistry.RegisterSingleton<IOperationActionService, OperationActionService>();


            //Db
            containerRegistry.Register<IDbContextFactory<DataContext>, DbContextFactory>();

            // DbServices
            containerRegistry.Register<IUsersDbService, UsersDbService>();
            containerRegistry.Register<IRepositoryCreator<DbUsers, Guid>, RepositoryCreator<DbUsers, Guid>>();
            containerRegistry.Register<IScriptDbService, ScriptDbService>();
            containerRegistry.Register<IRepositoryCreator<DbScripts, Guid>, RepositoryCreator<DbScripts, Guid>>();
            containerRegistry.Register<IOperationDbService, OperationDbService>();
            containerRegistry.Register<IRepositoryCreator<DbOperations, Guid>, RepositoryCreator<DbOperations, Guid>>();
            containerRegistry.Register<IStandSettingsDbService, StandSettingsDbService>();
            containerRegistry.Register<IRepositoryCreator<DbStandSettings, Guid>, RepositoryCreator<DbStandSettings, Guid>>();
            containerRegistry.Register<IScriptResultsDbService, ScriptResultsDbService>();
            containerRegistry.Register<IRepositoryCreator<DbScriptResult, Guid>, RepositoryCreator<DbScriptResult, Guid>>();
            containerRegistry.Register<IOperationResultsDbService, OperationResultsDbService>();
            containerRegistry.Register<IRepositoryCreator<DbOperationResult, Guid>, RepositoryCreator<DbOperationResult, Guid>>();
            containerRegistry.Register<IDeviceDbService, DeviceDbService>();
            containerRegistry.Register<IRepositoryCreator<DbDevice, Guid>, RepositoryCreator<DbDevice, Guid>>();
            

            //Other
            containerRegistry.RegisterSingleton<ILogger, Logger>();
            containerRegistry.RegisterInstance(new MapperConfiguration(config =>
            {
                config.AddProfile<StandMapperProfile>();
            }).CreateMapper());

            // Views - Generic
            containerRegistry.RegisterDialog<HelpView, HelpViewModel>();
            containerRegistry.RegisterDialog<AuthorizationView, AuthorizationViewModel>();
            containerRegistry.RegisterDialog<AddUserView, AddUserViewModel>();
            containerRegistry.RegisterDialog<MessageView, MessageViewModel>();
            containerRegistry.RegisterDialog<ConfirmView, ConfirmViewModel>();
            containerRegistry.RegisterDialog<AboutView, AboutViewModel>();
            containerRegistry.RegisterDialog<ScriptMenuView, ScriptMenuViewModel>();
            containerRegistry.RegisterDialog<ScriptEditorView, ScriptEditorViewModel>();
            containerRegistry.RegisterDialog<OperationEditorView, OperationEditorViewModel>();
            containerRegistry.RegisterDialog<StandSettingsView, StandSettingsViewModel>();
            containerRegistry.RegisterDialog<ManualValidationResultView, ManualValidationResultViewModel>();
            containerRegistry.RegisterDialog<WriteDeviceInformationView, WriteDeviceInformationViewModel>();
            containerRegistry.RegisterDialog<WorkReportView, WorkReportViewModel>();
            containerRegistry.RegisterDialog<ResultViewerView, ResultViewerViewModel>();
            containerRegistry.RegisterDialog<NozzleSelectorView, NozzleSelectorViewModel>();
            containerRegistry.RegisterDialog<PdfViewerView, PdfViewerViewModel>();
            containerRegistry.RegisterDialog<MasterDeviceInfoView, MasterDeviceInfoViewModel>();
            
            
        }

        protected override void OnInitialized()
        {
            LiveCharts.Configure(config => 
                    config 
                        .AddDarkTheme()  
                        .HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('Ж'))  // <- Russian 
                        
                // here we use the index as X, and the population as Y 
                        .HasMap<Flow>((flow, index) => new Coordinate(index, flow.flowValue == null ? 0 : (double)flow.flowValue)) 
            ); 
            
        }

        public record Flow(int measureNumber, float? flowValue);
}