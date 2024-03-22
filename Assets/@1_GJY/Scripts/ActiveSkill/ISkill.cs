using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    bool IsActive { get; }
    void UseSkill(PlayerStateMachine stateMachine);
    IEnumerator Co_CoolDown();
}
