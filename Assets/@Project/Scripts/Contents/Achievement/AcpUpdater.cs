using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AcpUpdater : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TmpText;
    void Start()
    {
        TmpText.text = $"ACP : {Managers.GameManager.AchievementPoint}";
        Managers.GameManager.OnACPChanged += (acp) => TmpText.text = $"ACP : {acp}";
    }
}

