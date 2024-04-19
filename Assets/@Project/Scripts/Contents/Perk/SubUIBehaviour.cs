using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SubUIBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SubVarBehaviour _var;

    private Color _originColor;
    private Color _afterColor;
    private Image _image;

    private void Awake()
    {
        _var = GetComponent<SubVarBehaviour>();
        _image = gameObject.GetComponent<Image>();
        _originColor = _image.color;
        _afterColor = new Color(139f / 255f, 255f / 255f, 143f / 255f);
    }

    private void Start()
    {
        ImageInit();
        PerkManager.Instance.OnUnlockBtnClicked += OnUnlockBtnClicked;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        SetSelectedPerkInfo();
        PerkManager.Instance.CallOnPerkClicked();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.UI_Clicked, transform.position);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _image.color = Color.red;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.UI_Entered, transform.position);
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

    private void CheckPerkActive()
    {
        PerkInfo perkInfo = PerkManager.Instance.SelectedPerkInfo;
        SubPerkInfo subInfo = PerkManager.Instance.SelectedSubInfo;

        if (subInfo != null && perkInfo == _var.ReturnPerkInfo())
        {
            if (subInfo.IsActive && subInfo.PositionIdx == _var.ReturnSubInfo().PositionIdx)
            {
                _image.color = _afterColor;
                _originColor = _afterColor;
            }
        }
    }

    private void ImageInit()
    {
        SubPerkInfo subInfo = _var.ReturnSubInfo();

        if (subInfo.IsActive)
        {
            _image.color = _afterColor;
            _originColor = _afterColor;
        }
    }

    private void OnUnlockBtnClicked(object sender, EventArgs eventArgs)
    {
        CheckPerkActive();
    }
}
