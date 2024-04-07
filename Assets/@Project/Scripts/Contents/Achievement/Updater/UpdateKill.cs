using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKill : AchievementUpdater
{
    // 대충 이거 Kill로 업적 업데이트 해줄 아이들에게 달아주기. Disable 될 경우 업데이트됨.
    private void OnDisable()
    {
        value = 1;
        Debug.Log("Update OnDisable");
        Report();
    }
}
