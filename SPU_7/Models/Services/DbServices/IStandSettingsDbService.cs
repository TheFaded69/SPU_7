using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPU_7.Models.Stand.Settings.Stand;

namespace SPU_7.Models.Services.DbServices;

public interface IStandSettingsDbService
{
    List<StandSettingsProfile> GetSettingsProfiles();
    
    Task<List<StandSettingsProfile>> GetSettingsProfilesAsync();

    StandSettingsModel GetStandSettings(Guid id);

    Task<StandSettingsModel> GetStandSettingsAsync(Guid id);

    void AddStandSettings(StandSettingsModel standSettingsModel);

    Task AddStandSettingsAsync(StandSettingsModel standSettingsModel);
    
    void EditStandSettings(StandSettingsModel standSettingsModel);

    Task EditStandSettingsAsync(StandSettingsModel standSettingsModel);
    
    void DeleteStandSettings(Guid id);

    Task DeleteStandSettingsAsync(Guid id);
    
}