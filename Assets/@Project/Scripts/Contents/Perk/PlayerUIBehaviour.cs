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
    [SerializeField] private TextMeshProUGUI _positionPanelTxt;

    private int _currentPoint;
    private string _currentSeed;

    private PerkInfo _currentInfos;
    private ContentInfo _currentContents;
    private SubPerkInfo _currentSubInfos;
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
        _currentInfos = PerkManager.Instance.SelectedPerkInfo;
        _currentContents = PerkManager.Instance.SelectedContentInfo;
        _currentSubInfos = PerkManager.Instance.SelectedSubInfo;
    }

    private void UpdateTextInfos()
    {
        _pointPanelTxt.text = _currentPoint.ToString() + " Points";
        _seedPanelTxt.text = "Current Seed: " + _currentSeed;
        _positionPanelTxt.text = _currentInfos.Tier.ToString() + " - " + _currentInfos.PositionIdx.ToString();
        _positionPanelTxt.text += _currentSubInfos != null ? $" - {_currentSubInfos.PositionIdx}" : "";
        _inspectorPanelTxt.text = _currentContents.name + "\n\n" + _currentContents.description;
    }
}
