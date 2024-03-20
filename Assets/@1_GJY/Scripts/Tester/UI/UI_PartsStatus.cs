using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PartsStatus : UI_Popup
{
    [Header("# Info")]
    [SerializeField] TextMeshProUGUI _descText;

    [Header("# Specs")]
    [SerializeField] TextMeshProUGUI _apText;
    [SerializeField] TextMeshProUGUI _weightText;
    [SerializeField] TextMeshProUGUI _primaryAttackText;
    [SerializeField] TextMeshProUGUI _secondaryAttackText;
    [SerializeField] TextMeshProUGUI _secondaryCoolDownText;

    protected override void Init()
    {
        base.Init();

        Managers.Module.OnInfoChange += ChangeDisPlayInfo;
    }

    private void ChangeDisPlayInfo(string desc) => _descText.text = desc;
}
