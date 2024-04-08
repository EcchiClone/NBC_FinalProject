using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIBehaviour : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private TextMeshProUGUI _pointPanelTxt;
    [SerializeField] private TextMeshProUGUI _seedPanelTxt;
    [SerializeField] private TextMeshProUGUI _inspectorPanelTxt;

    private int _currentPoint;
    private string _currentSeed;
    private string _perkName;
    private string _perkDescription;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        GetPlayerInfos();
        UpdateTextInfos();
    }

    private void GetPlayerInfos()
    {
        _currentPoint = PerkManager.Instance.PlayerPoint;
        _currentSeed = PerkManager.Instance.CurrentSeed;
    }

    private void UpdateTextInfos()
    {
        _pointPanelTxt.text = _currentPoint.ToString() + " Points";
        _seedPanelTxt.text = "Current Seed: " + _currentSeed;
    }
}
