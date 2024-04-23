using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AchievementAlarm : MonoBehaviour
{
    // **진짜진짜 구조 내려놓고 일단 필요한것만**
    // Description 적기
    // Dotween 애니메이션(화면 위쪽에서 내려왔다가 다시 올라가기 or FadeIn과 FadeOut으로 화면 중앙에서 나타났다가 사라지기)
    public GameObject Parent { get; set; }
    private CanvasGroup canvasGroup;

    [SerializeField] private GameObject completeMsg;
    [SerializeField] private TextMeshProUGUI descriptionMsg;

    [SerializeField] private Graphic[] graphicsBackground;
    [SerializeField] private Graphic[] graphicsContents;

    public string DescriptionMsg
    {
        set
        {
            descriptionMsg.text = value;
        }
    }

    private void OnEnable()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Fade();
    }

    private void Fade()
    {
        // Fadein
        foreach (Graphic graphic in graphicsBackground)
            graphic.DOFade(0.5f, 0.5f);
        foreach (Graphic graphic in graphicsContents)
            graphic.DOFade(1f, 0.5f);
        // 완료 도장
        completeMsg.transform.DOScale(Vector3.one * 1.5f, 0.3f).OnComplete(() =>
        {
            completeMsg.transform.DOScale(Vector3.one, 0.1f);
        });
        completeMsg.transform.DORotate(Vector3.zero, 0.5f);
        // 코루틴으로 out, 비활성화
        StartCoroutine("FadeOutAndDisable");
    }

    private IEnumerator FadeOutAndDisable()
    {
        // 3초 후 out
        yield return Util.GetWaitSeconds(3.0f);

        float fadeTime = 1f;

        foreach (Graphic graphic in graphicsBackground)
            graphic.DOFade(0, fadeTime);
        foreach (Graphic graphic in graphicsContents)
            graphic.DOFade(0, fadeTime);

        StartCoroutine("DoDisable", fadeTime);
    }
    private IEnumerator DoDisable(float fadeTime)
    {
        yield return Util.GetWaitSeconds(fadeTime);

        Parent.GetComponent<UI_AchievementAlarmTable>().RemoveItem(gameObject);

        Destroy(gameObject, 0.1f);

        yield return null;
    }


}