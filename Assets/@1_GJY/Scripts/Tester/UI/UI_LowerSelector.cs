using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LowerSelector : UI_Popup
{
    [SerializeField] private Transform _contents;

    enum Buttons
    {
        BackToSelector,
    }

    protected override void Init()
    {
        base.Init();

        int createUI = Managers.Module.LowerPartsCount;

        for (int i = 0; i < createUI; i++)
            Managers.UI.ShowItemUI<UI_LowerChangeBtn>(_contents);

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.BackToSelector).onClick.AddListener(BackToSelector);
    }

    private void BackToSelector()
    {
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
