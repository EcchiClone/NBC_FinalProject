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
        Cursor.visible = true;

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
        StageData savedData = Managers.GameManager.StageData;
        StageData stageData = Managers.SpawnManager.StageData;

        int minutes = (int)stageData.time / 60;
        int seconds = (int)stageData.time % 60;

        GetTMP((int)Texts.Time_Value).text = $"{minutes:00} : {seconds:00}";
        GetTMP((int)Texts.Stage_Value).text = $"{stageData.stage}";
        GetTMP((int)Texts.BestStage_Value).text = $"{savedData.stage}";
        GetTMP((int)Texts.MinionKill_Value).text = $"{stageData.minionKill}";
        GetTMP((int)Texts.BossKill_Value).text = $"{stageData.bossKill}";
        GetTMP((int)Texts.ResearchPoint_Value).text = $"{stageData.researchPoint}";

        stageData.minionKill = Mathf.Max(savedData.minionKill, stageData.minionKill);
        stageData.bossKill = Mathf.Max(savedData.bossKill, stageData.bossKill);
        stageData.time = Mathf.Max(savedData.time, stageData.time);
        stageData.stage = Mathf.Max(savedData.stage, stageData.stage);
        Managers.GameManager.ResearchPoint += stageData.researchPoint;

        Managers.GameManager.StageData = stageData;
        Managers.StageActionManager.CallResult(savedData);
    }
}
