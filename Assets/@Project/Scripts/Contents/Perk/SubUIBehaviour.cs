using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SubUIBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SubVarBehaviour _var;

    private Color _originColor;
    private Image _image;

    private void Awake()
    {
        _var = GetComponent<SubVarBehaviour>();
        _image = GetComponent<Image>();
        _originColor = _image.color;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        SetSelectedPerkInfo();
        PerkManager.Instance.CallOnPerkClicked();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _image.color = Color.red;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _image.color = _originColor;
    }

    private void SetSelectedPerkInfo()
    {
        PerkManager.Instance.SelectedPerkInfo = _var.ReturnPerkInfo();
        PerkManager.Instance.SelectedContentInfo = _var.ReturnContentInfo();
        PerkManager.Instance.SelectedSubInfo = _var.ReturnSubInfo();
        PerkManager.Instance.SelectedPerkDistance = 0f;
    }
}
