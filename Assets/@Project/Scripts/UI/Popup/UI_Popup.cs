using DG.Tweening;

public class UI_Popup : UI_Base
{
    public UI_Popup _previousPopup { get; protected set; }
    public UI_Popup _sidePopup { get; protected set; }

    protected override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void SetPreviousPopup(UI_Popup popup) => _previousPopup = popup;
    public virtual void SetSidePopup(UI_Popup popup) => _sidePopup = popup;

    public virtual void ActiveSidePopup(bool isOpen)
    {
        if (_sidePopup == null)
            return;

        _sidePopup.gameObject.SetActive(isOpen);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
