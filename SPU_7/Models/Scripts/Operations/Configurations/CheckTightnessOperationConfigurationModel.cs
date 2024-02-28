using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.Models.Scripts.Operations.Configurations;

public class CheckTightnessOperationConfigurationModel : BaseOperationConfigurationModel
{
    /// <summary>
    /// Давление вакуума в балоне
    /// </summary>
    public float PressureResiverMinimum { get; set; }

    /// <summary>
    /// Давление разряжения на ДРД, кПа
    /// </summary>
    public float PressureVacuumMinimum { get; set; }

    /// <summary>
    /// Время ожидания вакуума
    /// </summary>
    public int VacuumWaitTime { get; set; }

    /// <summary>
    /// Выбранное сопло
    /// </summary>
    public StandSettingsNozzleModel SelectedStandSettingsNozzleModel { get; set; }


    /// <summary>
    /// Время стабилизации, мин
    /// </summary>
    public int StabilizationTime { get; set; }

    /// <summary>
    /// Время теста, мин
    /// </summary>
    public int TestTime { get; set; }

    /// <summary>
    /// Максимально допустимая разница давления до и после теста
    /// </summary>
    public float PressureDifferenceMaximum { get; set; }

    public double SelectedNozzleValue { get; set; }
}