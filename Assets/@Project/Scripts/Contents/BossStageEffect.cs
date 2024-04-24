using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BossStageEffect : MonoBehaviour
{
    [SerializeField] Renderer[] _renderers;
    [Header("Dark")]
    [SerializeField] Color _darkColor;
    [Header("Boss")]
    [SerializeField] Color _bossColor;    
    [Header("Origin")]
    [SerializeField] Color _originColor;

    [SerializeField] string emissionProperty;

    private readonly float WAIT_TIME = 1f;
    private readonly float FADE_TIME_TO_DARK = 1f;

    private readonly float BOSS_COLOR_INTENSITY = 8;
    private readonly float FADE_TIME_TO_BOSS = 0.5f;
    
    private readonly float ORIGIN_COLOR_INTENSITY = 8f;
    private readonly float FADE_TIME_TO_ORIGIN = 1f;    

    private void Awake()
    {        
        Color originColor = _originColor * Mathf.LinearToGammaSpace(ORIGIN_COLOR_INTENSITY);
        foreach (var renderer in _renderers)
            renderer.material.SetColor(emissionProperty, originColor);
        Managers.StageActionManager.OnBossStage += ChangeToBoss;
        Managers.StageActionManager.OnBossKilled += ChangeToOrigin;
    }

    private void ChangeToBoss()
    {
        Color finalColor = _bossColor * Mathf.LinearToGammaSpace(BOSS_COLOR_INTENSITY);

        foreach (var renderer in _renderers)
            ChangeToDark(() => renderer.material.DOColor(finalColor, emissionProperty, FADE_TIME_TO_BOSS).SetDelay(WAIT_TIME).SetEase(Ease.Linear));
    }

    private void ChangeToOrigin()
    {
        Color finalColor = _originColor * Mathf.LinearToGammaSpace(ORIGIN_COLOR_INTENSITY);

        foreach (var renderer in _renderers)
            ChangeToDark(() => renderer.material.DOColor(finalColor, emissionProperty, FADE_TIME_TO_ORIGIN).SetDelay(WAIT_TIME).SetEase(Ease.Linear));
    }

    private void ChangeToDark(UnityAction action)
    {
        foreach (var renderer in _renderers)
            renderer.material.DOColor(_darkColor, emissionProperty, FADE_TIME_TO_DARK).SetEase(Ease.Linear).OnComplete(() => action.Invoke());        
    }
}
