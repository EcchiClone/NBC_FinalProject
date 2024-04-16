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
    }

    private void CheckPerkActive()
    {
        PerkInfo perkInfo = PerkManager.Instance.SelectedPerkInfo;
        SubPerkInfo subInfo = PerkManager.Instance.SelectedSubInfo;

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

    private void ShowBeforeBtn()
    {
        _beforeBtn.SetActive(true);
        _afterBtn.SetActive(false);
    }

    private void ShowAfterBtn()
    {
        _beforeBtn.SetActive(false);
        _afterBtn.SetActive(true);
    }

    private void UpdateRequireText()
    {
        _pointTxt.text = "Require " + PerkManager.Instance.RequirePoint.ToString() + " Points";
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
