using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LowerChangeBtn : MonoBehaviour
{
    public static int IndexOfLowerPart = 0;

    public int currentIndex;

    private void Awake()
    {
        currentIndex = IndexOfLowerPart;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Managers.Module.ChangeLowerPart(currentIndex));        

        string partName = Managers.Module.GetPartName<LowerPart>(currentIndex);
        Text text = GetComponentInChildren<Text>();
        text.text = partName;

        IndexOfLowerPart++;
    }
}
