using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    bool IsActive { get; }
    void UseSkill(Module module);
    IEnumerator Co_CoolDown();
}
