using AutoMapper;
using Newtonsoft.Json;
using SPU_7.Database.Models;
using SPU_7.Models.Scripts;
using SPU_7.Models.Scripts.Operations.Configurations.Extensions;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Stand.Settings.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.ViewModels.ScriptViewModels;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;
using SPU_7.ViewModels.Settings;
using SPU_7.ViewModels.WorkReportViewModels;

namespace SPU_7.Models.Mapper
{
    public class StandMapperProfile : Profile
    {
        public StandMapperProfile()
        {
            #region Db

            CreateMap<DbUsers, User>()
                .ReverseMap();
            CreateMap<DbScripts, ScriptViewModel>()
                .ReverseMap();
            CreateMap<DbOperations, OperationViewModel>()
                .ReverseMap();
            CreateMap<StandSettingsModel, DbStandSettings>()
                .ForMember(dm => dm.StandSettings,
                    r => r.MapFrom(ssm => JsonConvert.SerializeObject(ssm)));
            CreateMap<DbStandSettings, StandSettingsProfile>()
                .ReverseMap();
            
            CreateMap<DbScriptResult, ScriptReportViewModel>()
                .ReverseMap();
            CreateMap<DbScriptResult, ScriptResult>()
                .ReverseMap();
            
            CreateMap<DbOperationResult, OperationReportViewModel>()
                .ReverseMap();
            CreateMap<DbOperationResult, OperationResult>()
                .ReverseMap()
                .ForMember(dbOperationResult => dbOperationResult.Result,
                    r => r.MapFrom(operationResult => JsonConvert.SerializeObject(operationResult,
                        Formatting.Indented,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })));
            
            CreateMap<DbDevice, DeviceInformation>()
                .ReverseMap();

            #endregion

            #region Settings

            CreateMap<ComparatorSettingsModel, ComparatorSettingsViewModel>()
                .ReverseMap();
            CreateMap<StandSettingsNozzleModel, StandSettingsNozzleViewModel>()
                .ReverseMap();
            CreateMap<StandSettingsLineModel, StandSettingsLineViewModel>()
                .ReverseMap();
            CreateMap<StandSettingsPulseMeterModel, StandSettingsPulseMeterViewModel>()
                .ReverseMap();
            CreateMap<StandSettingsValveModel, StandSettingsValveViewModel>()
                .ReverseMap();
            CreateMap<StandSettingsDeviceModel, StandSettingsDeviceViewModel>()
                .ReverseMap();
            CreateMap<StandSettingsModel, StandSettingsViewModel>()
                .ReverseMap();
            CreateMap<StandSettingsSolenoidValveModel, StandSettingsSolenoidValveViewModel>()
                .ReverseMap();

            #endregion

            CreateMap<ValidationPulseMeterConfiguration, ValidationPulseMeterConfigurationViewModel>()
                .ReverseMap();
        }
    }
}
