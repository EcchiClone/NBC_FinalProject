using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class UpdateStageResult : AchievementUpdater
{
    // 해당 컴포넌트는 CommonUpdater에 부착
    private readonly string targetSceneName = "DevScene";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName)
        {
            Managers.StageActionManager.OnResult += ReportResultData;
            Debug.Log("ReportResultData 등록!");
        }
    }
    private void ReportResultData(StageData sd)
    {
        // 토탈 스코어, 총 킬수, 총 보스 킬수, 총 미니언 킬수 업적에 업데이트 등
        Debug.Log("실!!행!!");
        Managers.AchievementSystem.ReceiveReport("RESULT", "SCORE", sd.bestStage);
        Debug.Log(sd.bestStage);
        //Managers.AchievementSystem.ReceiveReport("RESULT", "SCORE", sd.bestTime);
        //Managers.AchievementSystem.ReceiveReport("RESULT", "KILL_ENEMY", sd.KilledBoss + sd.KilledMinion);
        //Managers.AchievementSystem.ReceiveReport("RESULT", "KILL_BOSS", sd.KilledBoss);
        //Managers.AchievementSystem.ReceiveReport("RESULT", "KILL_MINION", sd.KilledMinion);
    }
}
