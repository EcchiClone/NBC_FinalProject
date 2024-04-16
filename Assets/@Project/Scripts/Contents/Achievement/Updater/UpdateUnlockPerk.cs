using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUnlockPerk : AchievementUpdater
{
    public void UnlockPerk()
    {
        try
        {
            Report();
        }
        catch
        {
            Debug.Log("Unlock Perk 업적 갱신에 실패하였습니다.");
        }
    }
    public void ResetPerk()
    {
        try
        {
            Report(0);
        }
        catch
        {
            Debug.Log("Unlock Perk 업적 갱신(초기화)에 실패하였습니다.");
        }
    }
}
