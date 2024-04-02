using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;
using UnityEngine.Events;

public class UI_Achievement : UI_Popup
{
    private UI_Popup[] _partsMenus = new UI_Popup[4];
    private UnityAction _camAction;

    [SerializeField] TextMeshProUGUI[] _specTexts;

    enum Buttons
    {
        AllAchievements_Btn,
        ProgressAchievements_Btn,
        CompletedAchievements_Btn,
        Back_Btn,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Back_Btn).onClick.AddListener(BackToMain);
        // 이하 버튼 세개에 대해서는 수정 예정
        //GetButton((int)Buttons.AllAchievements_Btn).onClick.AddListener(() => OpenParts<UI_UpperSelector>((int)Buttons.AllAchievements_Btn));
        //GetButton((int)Buttons.ProgressAchievements_Btn).onClick.AddListener(() => OpenParts<UI_LowerSelector>((int)Buttons.ProgressAchievements_Btn));
        //GetButton((int)Buttons.CompletedAchievements_Btn).onClick.AddListener(() => OpenParts<UI_ArmSelector>((int)Buttons.CompletedAchievements_Btn));
    }

    public void BindCamAction(UnityAction camAction)
    {
        _camAction = camAction;
    }
    private void BackToMain()
    {
        _previousPopup.gameObject.SetActive(true);
        _camAction.Invoke();
        gameObject.SetActive(false);
    }
}
