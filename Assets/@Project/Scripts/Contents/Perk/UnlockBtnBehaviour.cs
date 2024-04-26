using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
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
        SetLocaleString();
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

    private void SetLocaleString()
    {
        _beforeBtn.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Perk Table", "UnlockBtnBefore", LocalizationSettings.SelectedLocale);
        _afterBtn.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Perk Table", "UnlockBtnAfter", LocalizationSettings.SelectedLocale);
        _rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Perk Table", "RerollBtn", LocalizationSettings.SelectedLocale);
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
        string requireTxt = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Perk Table", "RequirePoints", LocalizationSettings.SelectedLocale);
        string require = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Perk Table", "Require", LocalizationSettings.SelectedLocale);
        _pointTxt.text = require + PerkManager.Instance.RequirePoint.ToString() + requireTxt;
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
