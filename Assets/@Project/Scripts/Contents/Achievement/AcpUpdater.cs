using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AcpUpdater : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TmpText;
    void Update()
    {
        TmpText.text = $"ACP : {Managers.GameManager.gameData.achievementCoin.ToString()}";
    }
}

