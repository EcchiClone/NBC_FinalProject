using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BlinderPopup : UI_Popup
{
    [SerializeField] Image _blinder;

    public void DoCloseBlind(float time)
    {
        _blinder.color = new Color(0, 0, 0, 0);
        _blinder.gameObject.SetActive(true);
        _blinder.DOFade(1, time);        
        // To Do - 씬을 넘길지도?
    }

    public void DoOpenBlind(float time)
    {
        _blinder.color = new Color(0, 0, 0, 1);
        _blinder.gameObject.SetActive(true);
        _blinder.DOFade(0, time).SetEase(Ease.InQuad).SetDelay(2f).OnComplete(ClosePopupUI);        
    }
}
