using System;

public class ModuleActionManager
{
    public event Action<float, float> OnChangeArmorPoint;
    public event Action<float, float> OnChangeBoosterGauge;

    public void CallChangeArmorPoint(float value1, float value2) => OnChangeArmorPoint?.Invoke(value1, value2);
    public void CallChangeBoosterGauge(float value1, float value2) => OnChangeBoosterGauge?.Invoke(value1, value2);

    public void Clear()
    {
        OnChangeArmorPoint = null;
        OnChangeBoosterGauge = null;
    }
}
