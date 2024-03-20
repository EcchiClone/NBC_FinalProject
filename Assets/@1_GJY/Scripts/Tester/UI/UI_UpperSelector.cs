using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpperSelector : UI_Popup
{
    [SerializeField] private Transform _contents;    

    enum Buttons
    {        
        BackToSelector,
    }

    protected override void Init()
    {
        base.Init();

        int createUI = Managers.Module.UpperPartsCount;

        for (int i = 0; i < createUI; i++)
            Managers.UI.ShowItemUI<UI_UpperChangeBtn>(_contents).SetParentUI(this);


        BindButton(typeof(Buttons));
        GetButton((int)Buttons.BackToSelector).onClick.AddListener(BackToSelector);
    }

    private void BackToSelector()
    {
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
