using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class UI_DmagePopup : UI_Popup
{
    [SerializeField] TextMeshProUGUI _damageText;
    [SerializeField] float _fadeTime;

    private RectTransform _rect;
    private Vector3 _hitPos;
    private Vector3 _startPos;

    private void Awake()
    {
        _rect = _damageText.gameObject.GetComponent<RectTransform>();
    }

    public void Setup(Vector3 pos, int damage)
    {
        _damageText.fontSize = damage <= 50 ? 30 : damage <= 100 ? 50 : 80;
        _hitPos = pos;

        _damageText.text = $"{damage}";
        _damageText.transform.position = Camera.main.WorldToScreenPoint(RandomizePosition(pos));
        _rect.DOAnchorPosY(_rect.anchoredPosition.y + 10, _fadeTime);
        
        _damageText.DOFade(0, _fadeTime).OnComplete(() => gameObject.SetActive(false));
    }

    private Vector3 RandomizePosition(Vector3 origin)
    {
        Vector3 randomPos = origin + Random.insideUnitSphere * 0.5f;        
        randomPos += Vector3.up;
        return randomPos;
    }

    private void OnDisable()
    {
        _damageText.color = Color.white;
        _rect.anchoredPosition = Vector2.zero;
        ObjectPooler.ReturnToPool(gameObject);        
    }
}
