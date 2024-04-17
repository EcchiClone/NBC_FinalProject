using DG.Tweening;
using UnityEngine;

public class UI_ResultPopup : UI_Popup
{
    enum GameObjects
    {
        ResultPanel,
    }

    enum Buttons
    {
        Retry_Btn,
        Back_Btn,
    }

    enum Texts
    {
        Time_Value,
        Stage_Value,
        BestStage_Value,
        MinionKill_Value,
        BossKill_Value,
        ResearchPoint_Value,
    }

    protected override void Init()
    {
        base.Init();

        Cursor.lockState = CursorLockMode.Confined;

        BindGameObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindTMP(typeof(Texts));

        GetButton((int)Buttons.Retry_Btn).onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scenes.DevScene));
        GetButton((int)Buttons.Back_Btn).onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scenes.MainScene));

        RectTransform rect = GetGameObject((int)GameObjects.ResultPanel).GetComponent<RectTransform>();
        rect.DOAnchorPosY(-50, 2).SetEase(Ease.OutQuad);

        ShowResultAndSave();
    }

    private void ShowResultAndSave()
    {
        SpawnManager data = Managers.SpawnManager;
        StageData savedData = Managers.GameManager.StageData;
        StageData stageData = Managers.SpawnManager.StageData;

        stageData.highestMinionKill = Mathf.Max(savedData.highestMinionKill, stageData.highestMinionKill);
        stageData.highestBossKill = Mathf.Max(savedData.highestBossKill, stageData.highestBossKill);
        stageData.bestTime = Mathf.Max(savedData.bestTime, stageData.bestTime);
        stageData.bestStage = Mathf.Max(savedData.bestStage, stageData.bestStage);

        Managers.GameManager.StageData = stageData;
        Managers.StageActionManager.CallResult(savedData);

        int minutes = (int)data.Timer / 60;
        int seconds = (int)data.Timer % 60;

        GetTMP((int)Texts.Time_Value).text = $"{minutes} : {seconds}";
        GetTMP((int)Texts.Stage_Value).text = $"{data.CurrentStage}";
        GetTMP((int)Texts.BestStage_Value).text = $"{savedData.bestStage}";
        GetTMP((int)Texts.MinionKill_Value).text = $"{data.MinionKillCount}";
        GetTMP((int)Texts.BossKill_Value).text = $"{data.BossKillCount}";
        GetTMP((int)Texts.ResearchPoint_Value).text = $"{data.ResearchPoint}";        
    }
}
