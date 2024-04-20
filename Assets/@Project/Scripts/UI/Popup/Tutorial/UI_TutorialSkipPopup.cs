using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialSkipPopup : UI_Popup
{
    [SerializeField] Button _yesBtn;
    [SerializeField] Button _noBtn;

    protected override void Init()
    {
        base.Init();

        Cursor.lockState = CursorLockMode.Confined;
        _yesBtn.onClick.AddListener(() => { ClosePopupUI(); Managers.Tutorial.TutorialClear(); });
        _noBtn.onClick.AddListener(() => { ClosePopupUI(); Cursor.lockState = CursorLockMode.Locked; });
    }
}
