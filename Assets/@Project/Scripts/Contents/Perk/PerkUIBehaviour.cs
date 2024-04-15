using System;
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

    private void Start()
    {
        ImageInit();
        PerkManager.Instance.OnUnlockBtnClicked += OnUnlockBtnClicked;
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
        PerkManager.Instance.SelectedSubInfo = null;
        PerkManager.Instance.SelectedPerkDistance = _var.ReturnPrevDistance();
    }

    private void CheckPerkActive()
    {
        PerkInfo perkInfo = PerkManager.Instance.SelectedPerkInfo;

        if (perkInfo.IsActive)
        {
            _image.color = Color.gray;
            _originColor = Color.gray;
        }
    }

    private void ImageInit()
    {
        PerkInfo perkInfo = _var.ReturnPerkInfo();
        ContentInfo contentInfo = _var.ReturnContentInfo();
        PerkTier tier = perkInfo.Tier;
        PerkType type = contentInfo.type;

        _image.sprite = Resources.Load<Sprite>($"Images/Perk/T{(int)tier}_{(int)type}");
    }

    private void OnUnlockBtnClicked(object sender, EventArgs eventArgs)
    {
        // CheckPerkActive();
    }
}
