using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_SelectorMenu : UI_Popup
{
    private UI_Popup[] _partsMenus = new UI_Popup[2];
    private UnityAction _camAction;

    enum Buttons
    {
        UpperParts_Btn,
        LowerParts_Btn,
        BackToMain,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackToMain).onClick.AddListener(BackToMain);
        GetButton((int)Buttons.UpperParts_Btn).onClick.AddListener(() => OpenUpperParts<UI_UpperSelector>((int)Buttons.UpperParts_Btn));
        GetButton((int)Buttons.LowerParts_Btn).onClick.AddListener(() => OpenUpperParts<UI_LowerSelector>((int)Buttons.LowerParts_Btn));
    }

    public void BindCamAction(UnityAction camAction)
    {
        _camAction = camAction;
    }

    private void OpenUpperParts<T>(int index) where T : UI_Popup
    {
        if (_partsMenus[index] == null)
        {
            _partsMenus[index] = Managers.UI.ShowPopupUI<T>();
            _partsMenus[index].SetPreviousPopup(this);
        }
        else
            _partsMenus[index].gameObject.SetActive(true);            

        gameObject.SetActive(false);
    }

    private void BackToMain()
    {        
        _previousPopup.gameObject.SetActive(true);
        _camAction.Invoke();
        gameObject.SetActive(false);
    }
}
