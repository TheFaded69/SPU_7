using System;

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
    
    public double Calculate(double setPoint, double measuredValue)
    {
        var currentValue = Math.Max(_minInput, Math.Min(measuredValue, _maxInput));
        // Вычисляем ошибку
        double error = setPoint - currentValue;

        // Пропорциональная составляющая
        double pTerm = _kp * error;

        // Интегральная составляющая
        _integral += error;
        double iTerm = _ki * _integral;
        
        // Дифференциальная составляющая
        double dTerm = _kd * (error - _previousError);
        _previousError = error;

        // Вычисляем выходное значение
        double output = pTerm + iTerm + dTerm;

        // Ограничиваем выходное значение в заданном диапазоне
        output = Math.Max(_minOutput, Math.Min(output, _maxOutput));

        return output;
    }
}