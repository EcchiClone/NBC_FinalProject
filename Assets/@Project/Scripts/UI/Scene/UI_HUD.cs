using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_HUD : UI_Scene
{
    private Dictionary<Define.Parts_Location, TextMeshProUGUI> _ammoTextDict = new Dictionary<Define.Parts_Location, TextMeshProUGUI>();

    [Header("Aim")]
    [SerializeField] GameObject _crossHair;
    [SerializeField] GameObject _lockOnIndicator;
    [SerializeField] GameObject _bossAPBar;

    [Header("AP")]
    [SerializeField] Image _apFill;
    [SerializeField] TextMeshProUGUI _apValueText;

    [Header("Ammo")]
    [SerializeField] TextMeshProUGUI _ammoAL;
    [SerializeField] TextMeshProUGUI _ammoAR;
    [SerializeField] TextMeshProUGUI _ammoSL;
    [SerializeField] TextMeshProUGUI _ammoSR;

    [Header("Repair")]
    [SerializeField] Image _repairFill;

    [Header("Booster")]
    [SerializeField] Image _boosterFill;

    [Header("Booster")]
    [SerializeField] Image _bossAPFill;

    [Header("GameOver")]
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] Button _returnBtn;

    [Header("Stage")]
    [SerializeField] TextMeshProUGUI _countDownTime;
    [SerializeField] TextMeshProUGUI _spawnedCount;
    [SerializeField] TextMeshProUGUI _killCount;

    private Transform _target;

    protected override void Init()
    {
        base.Init();

        _ammoTextDict.Add(Define.Parts_Location.Weapon_Arm_L, _ammoAL);
        _ammoTextDict.Add(Define.Parts_Location.Weapon_Arm_R, _ammoAR);
        _ammoTextDict.Add(Define.Parts_Location.Weapon_Shoulder_L, _ammoSL);
        _ammoTextDict.Add(Define.Parts_Location.Weapon_Shoulder_R, _ammoSR);

        ModuleStatus.OnChangeArmorPoint += ChangeAPValue;
        ModuleStatus.OnChangeBoosterGauge += ChangeBoosterValue;

        // vvvvv 무기 사용 제외 모든 HUD 정보를 갱신하도록 Action 구독 - ActionManager 에 Action 몰아넣기
        Managers.ActionManager.OnLockOnTarget += GetTargetedEnemy;
        Managers.ActionManager.OnReleaseTarget += ReleaseTarget;        
        Managers.ActionManager.OnCoolDownRepair += (percent) => _repairFill.fillAmount = percent;        
        Managers.ActionManager.OnBossAPChanged += (percent) => _bossAPFill.fillAmount = percent;

        // vvvvv 무기 사용 시 잔탄 수 UI 표기 해주도록 Action 구독 - 무기사용이 이뤄지는 WeaponBase 에서 Action 작성
        Managers.Module.CurrentLeftArmPart.Weapon.OnWeaponFire += AmmoTextChange;
        Managers.Module.CurrentRightArmPart.Weapon.OnWeaponFire += AmmoTextChange;
        Managers.Module.CurrentLeftShoulderPart.Weapon.OnWeaponFire += AmmoTextChange;
        Managers.Module.CurrentRightShoulderPart.Weapon.OnWeaponFire += AmmoTextChange;
                
        Managers.StageActionManager.OnEnemySpawned += CountSpawndEnemies;
        Managers.StageActionManager.OnEnemyKilled += CountRemainEnemies;
        Managers.StageActionManager.OnCountDownActive += CountDownTime;

        Managers.ActionManager.OnPlayerDead += () => _gameOverPanel.SetActive(true);
        _returnBtn.onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scenes.MainScene));
    }

    private void CountSpawndEnemies(int value)
    {
        _spawnedCount.text = $"{value}";
    }

    private void CountRemainEnemies(int value)
    {
        _killCount.text = $"{value}";
    }

    private void CountDownTime(float remianTime)
    {        
        if(remianTime <= 0.1)
        {
            _countDownTime.text = "";
            return;
        }

        int minutes = (int)remianTime / 60;
        int seconds = (int)remianTime % 60;        

        _countDownTime.text = $"{minutes:00}:{seconds:00}";
    }

    private void AmmoTextChange(int ammo, bool isCoolDown, bool isReloadable, Define.Parts_Location type)
    {
        if (_ammoTextDict.TryGetValue(type, out TextMeshProUGUI text) == true)
            text.text = ammo > 0 ? isCoolDown ? $"<color=red>{ammo}</color>" : $"{ammo}" : isReloadable ? "<color=red>RELOAD</color>" : $"<color=red>EMPTY</color>";
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

    private void ChangeBoosterValue(float totalBooster, float remainBooster)
    {
        _boosterFill.fillAmount = remainBooster / totalBooster;
    }

    private void Update()
    {
        if (!_lockOnIndicator.activeSelf || _target == null)
            return;

        _lockOnIndicator.transform.position = Camera.main.WorldToScreenPoint(_target.position);
    }
}
