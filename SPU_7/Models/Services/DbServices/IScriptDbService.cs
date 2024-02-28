using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPU_7.Common.Device;
using SPU_7.Common.Stand;
using SPU_7.ViewModels.ScriptViewModels;

namespace SPU_7.Models.Services.DbServices;

public interface IScriptDbService
{
    List<ScriptViewModel> GetAllScripts();
    Task<List<ScriptViewModel>> GetAllScriptsAsync();

    List<ScriptViewModel> GetFilterScripts(StandType? standType, DeviceType? deviceType);
    Task<List<ScriptViewModel>> GetFilterScriptsAsync(StandType? standType, DeviceType? deviceType);
    
    bool AddScript(ScriptViewModel script);
    Task<bool> AddScriptAsync(ScriptViewModel script);
    
    bool EditScript(ScriptViewModel script);
    Task<bool> EditScriptAsync(ScriptViewModel script);

    bool DeleteScript(Guid id);
    Task<bool> DeleteScriptAsync(Guid id);


}