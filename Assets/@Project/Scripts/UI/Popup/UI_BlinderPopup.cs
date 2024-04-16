using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_BlinderPopup : UI_Popup
{
    [SerializeField] Image _blinder;

    public void DoCloseBlind(float time, UnityAction action = null)
    {
        _blinder.color = new Color(0, 0, 0, 0);
        _blinder.gameObject.SetActive(true);
        _blinder.DOFade(2f, time).SetEase(Ease.Linear).SetDelay(1f).OnComplete(() => action?.Invoke());
    }

    public void DoOpenBlind(float time, UnityAction action = null)
    {
        _blinder.color = new Color(0, 0, 0, 1);
        _blinder.gameObject.SetActive(true);
        _blinder.DOFade(0, time).SetEase(Ease.InQuad).SetDelay(1f).OnComplete(() =>
        {
            action?.Invoke();
            Destroy(gameObject);
        });
    }
}
