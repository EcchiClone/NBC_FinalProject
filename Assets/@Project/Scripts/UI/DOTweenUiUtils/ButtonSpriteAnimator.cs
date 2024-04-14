using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpriteAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    public Sprite pressedSprite;
    public Image buttonImage;
    public float fadeDuration = 0.2f;

    void Start()
    {
        buttonImage.sprite = normalSprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.DOFade(0, fadeDuration / 2).OnComplete(() =>
        {
            buttonImage.sprite = highlightedSprite;
            buttonImage.DOFade(1, fadeDuration / 2);
        });
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.DOFade(fadeDuration, fadeDuration / 2).OnComplete(() =>
        {
            buttonImage.sprite = normalSprite;
            buttonImage.DOFade(1, fadeDuration / 2);
        });
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.DOFade(0, fadeDuration / 2).OnComplete(() =>
        {
            buttonImage.sprite = pressedSprite;
            buttonImage.DOFade(1, fadeDuration / 2);
        });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.DOFade(0, fadeDuration / 2).OnComplete(() =>
        {
            buttonImage.sprite = normalSprite;
            buttonImage.DOFade(1, fadeDuration / 2);
        });
    }

}
