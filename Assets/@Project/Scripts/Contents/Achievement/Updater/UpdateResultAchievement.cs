using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UpdateResultAchievement : AchievementUpdater
{
    // 해당 컴포넌트는 CommonUpdater에 부착

    private void Awake()
    {
        // GameManager.결과이벤트에 구독
        // Managers.GameManager.onResult += ReportResultData;
    }
    private void ReportResultData()
    {
        // GameManager 생긴 후 활성화
        // 토탈 스코어, 총 킬수, 총 보스 킬수, 총 미니언 킬수 업적에 업데이트

        //AchievementSystem.Instance.ReceiveReport("RESULT", "SCORE", Managers.GameManager.Score);
        //AchievementSystem.Instance.ReceiveReport("RESULT", "KILL_ENEMY", Managers.GameManager.KilledBoss + Managers.GameManager.KilledMinion);
        //AchievementSystem.Instance.ReceiveReport("RESULT", "KILL_BOSS", Managers.GameManager.KilledBoss);
        //AchievementSystem.Instance.ReceiveReport("RESULT", "KILL_MINION", Managers.GameManager.KilledMinion);
    }

}
