using DG.Tweening;

public class UI_Popup : UI_Base
{
    protected UI_Popup _previousPopup;

    protected override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);        
    }

    public virtual void SetPreviousPopup(UI_Popup popup)
    {
        _previousPopup = popup;
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
