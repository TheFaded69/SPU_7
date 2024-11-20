using System;
using System.Collections.Generic;
using System.Linq;
using SPU_7.Common.Device;

namespace SPU_7.Models.Stand;

public class PIDController
{
    public PIDController(double kp, double ki, double kd, double minInput, double maxInput, double minOutput, double maxOutput)
    {
        _kp = kp;
        _ki = ki;
        _kd = kd;
        _minInput = minInput;
        _maxInput = maxInput;
        _minOutput = minOutput;
        _maxOutput = maxOutput;

        _integral = 0;
        _previousError = 0;
    }

    public double _kp; // Пропорциональный коэффициент
    public double _ki; // Интегральный коэффициент
    public double _kd; // Дифференциальный коэффициент

    private double _integral;
    private double _previousError;
    private double _previousValue;

    private double _minInput;
    private double _maxInput;
    private double _minOutput;
    private double _maxOutput;

    private List<double> _calculatedValues = [];
    
    public double? Calculate(double setPoint, double measuredValue, MasterDeviceType deviceType , double targetDifference ,double multiplication = 1)
    {
        if (measuredValue >= setPoint * (1 - targetDifference) && measuredValue <= setPoint * (1+targetDifference) && _previousValue != 0)
        {
            var output2 = _previousValue;

            return output2;
        }

        var currentValue = Math.Max(_minInput, Math.Min(measuredValue, _maxInput));
        // Вычисляем ошибку
        var error = setPoint - currentValue;

        // Пропорциональная составляющая
        var pTerm = _kp * error * multiplication;

        // Интегральная составляющая
        _integral += error;
        var iTerm = _ki * _integral * multiplication;
         
        // Дифференциальная составляющая
        var dTerm = _kd * (error - _previousError)  * multiplication;
        _previousError = error;


        // Вычисляем выходное значение
        var output = pTerm + iTerm + dTerm;

        // Ограничиваем выходное значение в заданном диапазоне
        output = Math.Max(_minOutput, Math.Min(output, _maxOutput));

        switch (_calculatedValues.Count)
        {
            //заносим последние 5 значений в список
            case 5:
                _calculatedValues.RemoveAt(0);
                _calculatedValues.Add(output);
                break;
            case > 5:
            {
                while (_calculatedValues.Count >= 5)
                {
                    _calculatedValues.RemoveAt(0);
                }
            
                _calculatedValues.Add(output);
                break;
            }
            default:
                _calculatedValues.Add(output);
                break;
        }

        //Если Расход превышает цель последние 5 измерений при максимальном выходе в 50 Гц то сбрасываем интегральную ошибку чтобы вывести систему из
        //статического состояния (слишком большая интегральная ошибка накопилась)
        if (measuredValue >= setPoint * 1.05 && Math.Abs(_calculatedValues.Average() - _maxOutput) >= 0 && _calculatedValues.Count == 5)
            _integral /= 2;
        //Если расход не может достигнуть цель на максильной частоте в течении 5 последних измерений - нет смысла регулировать систему, нужно менять расход в 
        //сценарии, другой переход ставить, другой СГ
        /*else if (measuredValue <= setPoint * 0.95 && Math.Abs(_calculatedValues.Average() - _maxOutput) >= 0 && _calculatedValues.Count == 5)
            return null;*/

        
        
        _previousValue = output;
        return output;
    }
}