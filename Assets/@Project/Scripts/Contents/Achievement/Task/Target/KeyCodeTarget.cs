using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Task/Target/KeyCode", fileName = "Target_")]
public class KeyCodeTarget : TaskTarget
{
    [SerializeField]
    private KeyCode value;
    public override object Value => value;

    public override bool IsEqual(object target)
    {
        if (target is KeyCode targetKeyCode) // target을 KeyCode로 안전하게 캐스팅
        {
            return value == targetKeyCode; // KeyCode 간의 비교를 수행
        }
        return false; // target이 KeyCode 타입이 아닐 경우 false 반환
    }

}
