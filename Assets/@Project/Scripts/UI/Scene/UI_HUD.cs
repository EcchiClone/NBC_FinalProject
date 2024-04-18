using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUD : UI_Scene
{
    private Dictionary<Define.Parts_Location, TextMeshProUGUI> _ammoTextDict = new Dictionary<Define.Parts_Location, TextMeshProUGUI>();

    [Header("Aim")]
    [SerializeField] GameObject _crossHair;
    [SerializeField] GameObject _lockOnIndicator;
    [SerializeField] GameObject _targetAPBar;

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
    [SerializeField] Image _targetAPFill;    

    private Transform _target;

    protected override void Init()
    {
        base.Init();

        _ammoTextDict.Add(Define.Parts_Location.Weapon_Arm_L, _ammoAL);
        _ammoTextDict.Add(Define.Parts_Location.Weapon_Arm_R, _ammoAR);
        _ammoTextDict.Add(Define.Parts_Location.Weapon_Shoulder_L, _ammoSL);
        _ammoTextDict.Add(Define.Parts_Location.Weapon_Shoulder_R, _ammoSR);
                
        Managers.ModuleActionManager.OnChangeArmorPoint += ChangeAPValue;        
        Managers.ModuleActionManager.OnChangeBoosterGauge += ChangeBoosterValue;

        // vvvvv 무기 사용 제외 모든 HUD 정보를 갱신하도록 Action 구독 - ActionManager 에 Action 몰아넣기
        Managers.ActionManager.OnLockOnTarget += GetTargetedEnemy;
        Managers.ActionManager.OnReleaseTarget += ReleaseTarget;        
        Managers.ActionManager.OnCoolDownRepair += (percent) => _repairFill.fillAmount = percent;        
        Managers.ActionManager.OnTargetAPChanged += (percent) => _targetAPFill.fillAmount = percent;

        // vvvvv 무기 사용 시 잔탄 수 UI 표기 해주도록 Action 구독 - 무기사용이 이뤄지는 WeaponBase 에서 Action 작성
        Managers.Module.CurrentLeftArmPart.Weapon.OnWeaponFire += AmmoTextChange;
        Managers.Module.CurrentRightArmPart.Weapon.OnWeaponFire += AmmoTextChange;
        Managers.Module.CurrentLeftShoulderPart.Weapon.OnWeaponFire += AmmoTextChange;
        Managers.Module.CurrentRightShoulderPart.Weapon.OnWeaponFire += AmmoTextChange;        

        if (Managers.Scene.CurrentScene.Scenes != Define.Scenes.TutorialScene)
            Managers.UI.ShowPopupUI<UI_StageInfoPopup>();

        float ap = Managers.Module.CurrentModule.ModuleStatus.Armor;
        ChangeAPValue(ap, ap);
    }

    private void AmmoTextChange(int ammo, bool isCoolDown, bool isReloadable, Define.Parts_Location type)
    {
        if (_ammoTextDict.TryGetValue(type, out TextMeshProUGUI text) == true)
            text.text = ammo > 0 ? isCoolDown ? $"<color=red>{ammo}</color>" : $"{ammo}" : isReloadable ? "<color=red>RELOAD</color>" : $"<color=red>EMPTY</color>";
    }

    private void GetTargetedEnemy(Transform target, float percent)
    {
        _target = target;
        _crossHair.SetActive(false);
        _lockOnIndicator.SetActive(true);
        _targetAPBar.SetActive(true);
        _targetAPFill.fillAmount = percent;
    }

    private void ReleaseTarget()
    {
        _target = null;
        _crossHair.SetActive(true);
        _lockOnIndicator.SetActive(false);
        _targetAPBar.SetActive(false);
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
