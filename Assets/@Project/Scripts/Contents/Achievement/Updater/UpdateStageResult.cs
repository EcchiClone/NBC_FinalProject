using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UpdateStageResult : AchievementUpdater
{
    // 해당 컴포넌트는 CommonUpdater에 부착

    private void Start()
    {
        //Managers.StageActionManager.OnResult += ReportResultData;
    }
    private void ReportResultData(StageData sd)
    {
        // GameManager 생긴 후 활성화
        // 토탈 스코어, 총 킬수, 총 보스 킬수, 총 미니언 킬수 업적에 업데이트

        Managers.AchievementSystem.ReceiveReport("RESULT", "SCORE", sd.bestStage);
        //Managers.AchievementSystem.ReceiveReport("RESULT", "KILL_ENEMY", sd.KilledBoss + sd.KilledMinion);
        //Managers.AchievementSystem.ReceiveReport("RESULT", "KILL_BOSS", sd.KilledBoss);
        //Managers.AchievementSystem.ReceiveReport("RESULT", "KILL_MINION", sd.KilledMinion);
    }
}
