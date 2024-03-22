using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_HUD : UI_Scene
{
    [Header("Aim")]
    [SerializeField] GameObject _crossHair;
    [SerializeField] GameObject _lockOnIndicator;
    [SerializeField] GameObject _bossAPBar;

    [Header("AP")]
    [SerializeField] Image _apFill;
    [SerializeField] TextMeshProUGUI _apValueText;

    [Header("Repair")]
    [SerializeField] Image _repairFill;

    [Header("Booster")]
    [SerializeField] Image _boosterFill;

    [Header("Booster")]
    [SerializeField] Image _bossAPFill;

    [Header("GameOver")]
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] Button _returnBtn;

    private Transform _target;    

    protected override void Init()
    {
        base.Init();

        Managers.ActionManager.OnLockOnTarget += GetTargetedEnemy;
        Managers.ActionManager.OnReleaseTarget += ReleaseTarget;
        PlayerStatus.OnChangeArmorPoint += ChangeAPValue;
        Managers.ActionManager.OnCoolDownRepair += (percent) => _repairFill.fillAmount = percent;
        Managers.ActionManager.OnCoolDownBooster += (percent) => _boosterFill.fillAmount = percent;
        Managers.ActionManager.OnBossAPChanged += (percent) => _bossAPFill.fillAmount = percent;

        _returnBtn.onClick.AddListener(() => SceneManager.LoadScene(0));
    }    

    private void GetTargetedEnemy(Transform target)
    {
        _target = target;
        _crossHair.SetActive(false);
        _lockOnIndicator.SetActive(true);
        _bossAPBar.SetActive(true);
    }

    private void ReleaseTarget()
    {
        _target = null;
        _crossHair.SetActive(true);
        _lockOnIndicator.SetActive(false);
        _bossAPBar.SetActive(false);
    }

    private void ChangeAPValue(float totalAP, float remainAP)
    {
        _apFill.fillAmount = remainAP / totalAP;
        _apValueText.text = $"{(int)remainAP}";
    }

    private void Update()
    {
        if (!_lockOnIndicator.activeSelf || _target == null)
            return;

        _lockOnIndicator.transform.position = Camera.main.WorldToScreenPoint(_target.position);        
    }
}
