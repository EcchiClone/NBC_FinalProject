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
        }
    }
    private void ReportResultData(StageData sd)
    {
        Managers.AchievementSystem.ReceiveReport("RESULT", "HIGHEST_STAGE", Managers.SpawnManager.CurrentStage);
    }
}
