using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockBtnBehaviour : MonoBehaviour
{
    [Header("Unlock Button UI")]
    [SerializeField] private TextMeshProUGUI _pointTxt;
    [SerializeField] private GameObject _beforeBtn;
    [SerializeField] private GameObject _afterBtn;
    [SerializeField] private GameObject _rerollBtn;

    private void Awake()
    {
        InitActive();
    }

    private void Start()
    {
        PerkManager.Instance.OnPerkClicked += OnPerkClicked;
    }

    private void Update()
    {
        UpdateRequireText();
        CheckPerkActive();
    }

    private void InitActive()
    {
        _beforeBtn.SetActive(false);
        _afterBtn.SetActive(false);
        _rerollBtn.SetActive(false);
    }

    private void CheckPerkActive()
    {
        PerkInfo perkInfo = PerkManager.Instance.SelectedPerkInfo;
        SubPerkInfo subInfo = PerkManager.Instance.SelectedSubInfo;

        if (perkInfo.Tier != PerkTier.ORIGIN)
        {
            HideRerollBtn();

            if (subInfo != null)
            {
                int realIdx = PerkManager.Instance.ReturnRealIndex(perkInfo.Tier, perkInfo.PositionIdx);
                int subIdx = PerkManager.Instance.ReturnRealSubIndex(perkInfo.Tier, realIdx, subInfo.PositionIdx);

                if (!perkInfo.subPerks[subIdx].IsActive)
                {
                    ShowBeforeBtn();
                }
                else
                {
                    ShowAfterBtn();
                }
            }
            else
            {
                if (!perkInfo.IsActive)
                {
                    ShowBeforeBtn();
                }
                else
                {
                    ShowAfterBtn();
                }
            }
        }
        else
        {
            if (!perkInfo.IsActive)
                ShowRerollBtn();
            else
                ShowAfterBtn();
        }
    }

    private void ShowBeforeBtn()
    {
        _beforeBtn.SetActive(true);
        _afterBtn.SetActive(false);
        HideRerollBtn();
    }

    private void ShowAfterBtn()
    {
        _beforeBtn.SetActive(false);
        _afterBtn.SetActive(true);
        HideRerollBtn();
    }

    private void ShowRerollBtn()
    {
        _beforeBtn.SetActive(false);
        _afterBtn.SetActive(false);
        _rerollBtn.SetActive(true);
    }
    private void HideRerollBtn() => _rerollBtn.SetActive(false);

    private void UpdateRequireText()
    {
        _pointTxt.text = "해금에 " + PerkManager.Instance.RequirePoint.ToString() + " 포인트 필요";
    }

    private void OnPerkClicked(object sender, EventArgs eventArgs)
    {

    }

    public void OnButtonClicked()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.UI_Clicked, transform.position);
        PerkManager.Instance.CallOnUnlockBtnClicked();
    }

}
