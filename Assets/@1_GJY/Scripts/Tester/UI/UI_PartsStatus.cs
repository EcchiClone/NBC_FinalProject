using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PartsStatus : UI_Popup
{
    [Header("# Info")]
    [SerializeField] TextMeshProUGUI _descText;

    protected override void Init()
    {
        base.Init();

        Managers.Module.OnInfoChange += ChangeDisPlayInfo;
    }

    private void ChangeDisPlayInfo(string desc) => _descText.text = desc;
}
