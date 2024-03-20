using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LowerChangeBtn : UI_Item
{
    [SerializeField] TextMeshProUGUI _partText;
    
    public int currentIndex;
    public static int IndexOfLowerPart = 0;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfLowerPart;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Managers.Module.ChangeLowerPart(currentIndex));

        string partName = Managers.Module.GetPartName<LowerPart>(currentIndex);        
        _partText.text = partName;

        IndexOfLowerPart++;
    }
}
