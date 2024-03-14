using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpperChangeBtn : MonoBehaviour
{
    public static int IndexOfUpperPart = 0;

    public int currentIndex;

    private void Awake()
    {
        currentIndex = IndexOfUpperPart;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Managers.Module.ChangeUpperPart(currentIndex));

        string partName = Managers.Module.GetPartName<UpperPart>(currentIndex);
        Text text = GetComponentInChildren<Text>();
        text.text = partName;

        IndexOfUpperPart++;
    }
}
