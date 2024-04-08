using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PerkUIBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private PerkVarBehaviour _var;

    private Color _originColor;
    private Image _image;

    private void Awake()
    {
        _var = GetComponent<PerkVarBehaviour>();
        _image = GetComponent<Image>();
        _originColor = _image.color;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(_var.ReturnPerkName());
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _image.color = Color.red;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _image.color = _originColor;
    }
}
