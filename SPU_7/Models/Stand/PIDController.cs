using System;
using System.Collections.Generic;
using System.Linq;

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
    
    private double _kp; // Пропорциональный коэффициент
    private double _ki; // Интегральный коэффициент
    private double _kd; // Дифференциальный коэффициент

    private double _integral;
    private double _previousError;

    private double _minInput;
    private double _maxInput;
    private double _minOutput;
    private double _maxOutput;

    private List<double> _calculatetValues = [];
    
    public double Calculate(double setPoint, double measuredValue, double multiplication = 1)
    {
        var currentValue = Math.Max(_minInput, Math.Min(measuredValue, _maxInput));
        // Вычисляем ошибку
        double error = setPoint - currentValue;

        // Пропорциональная составляющая
        double pTerm = _kp * error * multiplication;

        // Интегральная составляющая
        _integral += error;
        double iTerm = _ki * _integral * multiplication;
         
        // Дифференциальная составляющая
        double dTerm = _kd * (error - _previousError)  * multiplication;
        _previousError = error;

        // Вычисляем выходное значение
        double output = pTerm + iTerm + dTerm;

        // Ограничиваем выходное значение в заданном диапазоне
        output = Math.Max(_minOutput, Math.Min(output, _maxOutput));

        if (_calculatetValues.Count == 5)
        {
            _calculatetValues.RemoveAt(0);
            _calculatetValues.Add(output);
        }
        else
        {
            _calculatetValues.Add(output);
        }

        if (_calculatetValues.Average() == output) _integral /= 2;
        
        return output;
    }
}