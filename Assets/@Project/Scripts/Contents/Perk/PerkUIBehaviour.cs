using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PerkUIBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isPointerIn;

    private Color _originColor;
    private Image _image;

    private void Awake()
    {
        _isPointerIn = false;
        _image = GetComponent<Image>();
        _originColor = _image.color;
        // TODO: DOTween 알아보기
    }

    private void Update()
    {
        ChangePerkColor();
    }

    private void ChangePerkColor()
    {
        if (_isPointerIn)
        {
            _image.color = Color.red;
        }
        else
        {
            _image.color = _originColor;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _isPointerIn = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _isPointerIn = false;
    }
}
