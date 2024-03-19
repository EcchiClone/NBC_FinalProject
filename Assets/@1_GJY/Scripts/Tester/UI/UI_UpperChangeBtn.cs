using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpperChangeBtn : UI_Item
{
    [SerializeField] TextMeshProUGUI _partText;

    public int currentIndex;
    public static int IndexOfUpperPart = 0;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfUpperPart;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Managers.Module.ChangeUpperPart(currentIndex));

        string partName = Managers.Module.GetPartName<UpperPart>(currentIndex);        
        _partText.text = partName;

        IndexOfUpperPart++;
    }
}
