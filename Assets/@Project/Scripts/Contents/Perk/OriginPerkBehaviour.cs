using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class OriginPerkBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{

    private PerkInfo _perkInfo;
    private ContentInfo _contentInfo;

    private void Awake()
    {
        InitInfos();
    }

    private void Start()
    {
        PerkManager.Instance.OnUnlockBtnClicked += OnUnlockBtnClicked;
    }

    private void InitInfos()
    {
        _perkInfo = new PerkInfo(PerkTier.ORIGIN, 0, 0, false);
        _contentInfo = new ContentInfo();
        _contentInfo.name = "[CORE X7-9900K]";
        _contentInfo.description = "기체의 핵심 프로세서입니다. 현재 진행상황을 초기화 할 수 있습니다. 해금으로 얻은 능력치는 유지되며, 사용한 포인트는 돌려받을 수 없습니다.";
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        SetSelectedPerkInfo();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.UI_Clicked, transform.position);
        PerkManager.Instance.CallOnPerkClicked();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.UI_Entered, transform.position);
    }

    private void SetSelectedPerkInfo()
    {
        PerkManager.Instance.SelectedPerkInfo = _perkInfo;
        PerkManager.Instance.SelectedContentInfo = _contentInfo;
        PerkManager.Instance.SelectedSubInfo = null;
    }

    public void SetOriginIsActiveFalse()
    {
        _perkInfo.IsActive = false;
    }

    private void OnUnlockBtnClicked(object sender, EventArgs eventArgs)
    {
        // CheckPerkActive();
    }
}
