namespace SPU_7.Domain.Extensions;

public class PidController
{

    #region Fields

    //Gains
    private double _kp;
    private double _ki;
    private double _kd;

    //Running Values
    private DateTime _lastUpdate;
    private double _lastPv;
    private double _errSum;

    //Reading/Writing Values
    private Func<double?> _readPv;
    private Func<double> _readSp;
    private Func<double, Task> _writeOv;

    //Max/Min Calculation
    private double _pvMax;
    private double _pvMin;
    private double _outMax;
    private double _outMin;

    //Threading and Timing
    private double _computeHz = 0.2f;
    private Thread _runThread;
    private bool _needRun;

    #endregion

    #region Properties

    public double PGain
    {
        get { return _kp; }
        set { _kp = value; }
    }

    public double Gain
    {
        get { return _ki; }
        set { _ki = value; }
    }

    public double DGain
    {
        get { return _kd; }
        set { _kd = value; }
    }

    public double PvMin
    {
        get { return _pvMin; }
        set { _pvMin = value; }
    }

    public double PvMax
    {
        get { return _pvMax; }
        set { _pvMax = value; }
    }

    public double OutMin
    {
        get { return _outMin; }
        set { _outMin = value; }
    }

    public double OutMax
    {
        get { return _outMax; }
        set { _outMax = value; }
    }

    public bool PIsOk
    {
        get { return _runThread != null; }
    }

    #endregion

    #region Construction / Deconstruction

    public PidController(double pG, double iG, double dG, double pMax, double pMin, double oMax, double oMin,
        Func<double?> pvFunc, Func<double> spFunc, Func<double, Task> outFunc)
    {
        _kp = pG;
        _ki = iG;
        _kd = dG;
        _pvMax = pMax;
        _pvMin = pMin;
        _outMax = oMax;
        _outMin = oMin;
        _readPv = pvFunc;
        _readSp = spFunc;
        _writeOv = outFunc;
    }

    ~PidController()
    {
        Disable();
        _readPv = null;
        _readSp = null;
        _writeOv = null;
    }

    #endregion

    #region Public Methods

    public void Enable()
    {
        if (_runThread != null)
            return;
        
        Reset();

        _runThread = new Thread(Run);
        _needRun = true;
        _runThread.IsBackground = true;
        _runThread.Name = "PID Processor";
        _runThread.Start();
    }

    public void Disable()
    {
        if (_runThread == null)
            return;

        _needRun = false;
        _runThread.Join();
        _runThread = null;
    }

    public void Reset()
    {
        _errSum = 0.0f;
        _lastUpdate = DateTime.Now;
    }

    #endregion

    #region Private Methods

    private double ScaleValue(double value, double valuemin, 
            double valuemax, double scalemin, double scalemax)
    {
        var vPerc = (value - valuemin) / (valuemax - valuemin);
        var bigSpan = vPerc * (scalemax - scalemin);

        var retVal = scalemin + bigSpan;

        return retVal;
    }

    private double Clamp(double value, double min, double max)
    {
        if (value > max)
            return max;
        if (value < min)
            return min;
        return value;
    }

    private async Task Compute()
    {
        if (_readPv == null || _readSp == null || _writeOv == null)
            return;

        var readPv = _readPv();
        if (readPv == null) return;
        var pv = (double)readPv;
        
        var sp =  _readSp();

        //We need to scale the pv to +/- 100%, but first clamp it
        pv = Clamp(pv, _pvMin, _pvMax);
        pv = ScaleValue(pv, _pvMin, _pvMax, -1.0, 1.0);

        //We also need to scale the setpoint
        sp = Clamp(sp, _pvMin, _pvMax);
        sp = ScaleValue(sp, _pvMin, _pvMax, -1.0, 1.0);

        //Now the error is in percent...
        var err = sp - pv;

        var pTerm = err * _kp;
        var iTerm = 0.0;
        var dTerm = 0.0;

        var partialSum = 0.0;
        var nowTime = DateTime.Now;

        if (_lastUpdate != null)
        {
            var dT = (nowTime - _lastUpdate).TotalSeconds;

            //Compute the integral if we have to...
            if (pv >= _pvMin && pv <= _pvMax)
            {
                partialSum = _errSum + dT * err;
                iTerm = _ki * partialSum;
            }

            if (dT != 0.0)
                dTerm = _kd * (pv - _lastPv) / dT;
        }

        _lastUpdate = nowTime;
        _errSum = partialSum;
        _lastPv = pv;

        //Now we have to scale the output value to match the requested scale
        var outReal = pTerm + iTerm + dTerm;

        outReal = Clamp(outReal, -1.0, 1.0);
        outReal = ScaleValue(outReal, -1.0, 1.0, _outMin, _outMax);
        outReal = Math.Round(outReal, 1) / 0.1;
        //Write it out to the world
        await _writeOv(outReal);
    }

    #endregion

    #region Threading

    private async void Run()
    {
        while (_needRun)
        {
            try
            {
                var sleepTime = (int)(1000 / _computeHz);
                Thread.Sleep(sleepTime);
                await Compute();
            }
            catch (Exception e)
            {

            }
        }
    }

    #endregion
}