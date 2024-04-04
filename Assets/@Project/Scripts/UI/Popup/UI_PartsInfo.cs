using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PartsInfo : UI_Popup
{
    [Header("# Info")]
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _descText;

    protected override void Init()
    {
        base.Init();

        Managers.Module.OnInfoChange += ChangeDisPlayInfo;
    }

    private void ChangeDisPlayInfo(string name, string desc)
    {
        _nameText.text = name;
        _descText.text = desc;
    }
}
