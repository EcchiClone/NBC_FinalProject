using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void EnterState(); // State Switch 시 실행 (또는 StateMachine Awake에서 첫 실행)
    void UpdateState(); // RootState(Non-Combat / Combat)포함 하위에 SubState가 있다면 항시 Update
    void ExitState(); // State Switch 시 실행
    void CheckSwitchStates(); // Update문에서 SwitchState 조건을 확인 (Combat -> Non-Combat / Grounded -> Jump 조건등)
    void InitailizeSubState(); // EnterState 실행 시 현재 State의 SetSub해야 할 State가 있는지 확인
}
