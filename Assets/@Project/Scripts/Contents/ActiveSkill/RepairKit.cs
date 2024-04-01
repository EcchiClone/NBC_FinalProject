using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : ISkill
{
    public bool _isAcitve = true;
    private readonly float COOL_DOWN_TIME = 5f;

    public bool IsActive => _isAcitve;

    public void UseSkill(Module module)
    {
        if (!_isAcitve)
            return;
        
        _isAcitve = false;
        module.ModuleStatus.Repair();
    }

    public IEnumerator Co_CoolDown()
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / COOL_DOWN_TIME;

            Managers.ActionManager.CallUseRePair(percent);

            yield return null;
        }

        _isAcitve = true;        
    }
}
