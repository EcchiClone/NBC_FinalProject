using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class PlayerUIBehaviour : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private TextMeshProUGUI _pointPanelTxt;
    [SerializeField] private TextMeshProUGUI _seedPanelTxt;
    [SerializeField] private TextMeshProUGUI _inspectorPanelTxt;
    [SerializeField] private TextMeshProUGUI _positionPanelTxt;
    [SerializeField] private TextMeshProUGUI _controlInfoTxt;

    private int _currentPoint;
    private string _currentSeed;

    private PerkInfo _currentInfos;
    private ContentInfo _currentContents;
    private SubPerkInfo _currentSubInfos;
    private string _perkName;
    private string _perkDescription;

    private void Start()
    {
        _controlInfoTxt.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Perk Table", "ControlInfo", LocalizationSettings.SelectedLocale);
    }

    private void LateUpdate()
    {
        GetPlayerInfos();
        UpdateTextInfos();
    }

    private void GetPlayerInfos()
    {
        _currentPoint = Managers.GameManager.ResearchPoint;
        _currentSeed = PerkManager.Instance.CurrentSeed;
        _currentInfos = PerkManager.Instance.SelectedPerkInfo;
        _currentContents = PerkManager.Instance.SelectedContentInfo;
        _currentSubInfos = PerkManager.Instance.SelectedSubInfo;
    }

    private void UpdateTextInfos()
    {
        string pointTxt = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Perk Table", "CurrentPoints", LocalizationSettings.SelectedLocale);
        string seedTxt = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Perk Table", "CurrentSeed", LocalizationSettings.SelectedLocale);

        _pointPanelTxt.text = _currentPoint.ToString() + pointTxt;
        _seedPanelTxt.text = seedTxt + _currentSeed;
        _positionPanelTxt.text = _currentInfos.Tier.ToString() + " | " + _currentInfos.PositionIdx.ToString();
        _positionPanelTxt.text += _currentSubInfos != null ? $" - {_currentSubInfos.PositionIdx}" : "";
        _inspectorPanelTxt.text = _currentContents.name + "\n\n" + _currentContents.description;
    }
}
