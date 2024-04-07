using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ShoulderSelector : UI_Popup
{
    [SerializeField] GameObject _nextGroup_L;
    [SerializeField] GameObject _nextGroup_R;
    [SerializeField] TextMeshProUGUI[] _specTexts_L;
    [SerializeField] TextMeshProUGUI[] _nextSpecTexts_L;
    [SerializeField] TextMeshProUGUI[] _specTexts_R;
    [SerializeField] TextMeshProUGUI[] _nextSpecTexts_R;
    [SerializeField] private Transform _contents;

    enum Buttons
    {
        BackToSelector,
        L_Btn,
        R_Btn,
    }

    enum SpecType
    {
        Weight,
        Damage,
        BulletSpeed,
        FireRate,
        CoolDownTime,
        ProjectilesPerShot,
        ShotErrorRange,
    }

    public enum ChangeShoulderMode
    {
        LeftShoulder,
        RightShoulder,
    }

    public ChangeShoulderMode CurrentChangeMode { get; private set; }    

    protected override void Init()
    {
        base.Init();

        CurrentChangeMode = ChangeShoulderMode.LeftShoulder;

        int createUI = Managers.Module.ShoulderWeaponPartsCount;

        for (int i = 0; i < createUI; i++)
            Managers.UI.ShowItemUI<UI_ShoulderChangeBtn>(_contents).SetParentUI(this);

        Managers.Module.OnLeftShoulderChange += UpdateSelectedPartSpecText_L;
        Managers.Module.OnRightSoulderChange += UpdateSelectedPartSpecText_R;
        ResetText();

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.BackToSelector).onClick.AddListener(BackToSelector);
        GetButton((int)Buttons.L_Btn).onClick.AddListener(() => { CurrentChangeMode = ChangeShoulderMode.LeftShoulder; Managers.ActionManager.CallShoulderModeChange(CurrentChangeMode); });
        GetButton((int)Buttons.R_Btn).onClick.AddListener(() => { CurrentChangeMode = ChangeShoulderMode.RightShoulder; Managers.ActionManager.CallShoulderModeChange(CurrentChangeMode); });
    }

    public void ResetText()
    {
        int leftShoulderID = Managers.Module.GetPartOfIndex<ShouldersPart>(Managers.Module.CurrentLeftShoulderIndex).ID;
        int rightShoulderID = Managers.Module.GetPartOfIndex<ShouldersPart>(Managers.Module.CurrentRightShoulderIndex).ID;
        PartData LeftShoulderPartData = Managers.Data.GetPartData(leftShoulderID);
        PartData RightShoulderPartData = Managers.Data.GetPartData(rightShoulderID);

        UpdateSelectedPartSpecText_L(LeftShoulderPartData);
        UpdateSelectedPartSpecText_R(RightShoulderPartData);
    }

    private void BackToSelector()
    {
        Managers.ActionManager.CallUndoMenuCam(Define.CamType.Shoulder_Holder);

        ResetText();
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void UpdateSelectedPartSpecText_L(PartData shoulderData)
    {
        _nextGroup_L.SetActive(false);
        _specTexts_L[(int)SpecType.Weight].text = $"{shoulderData.Weight}";

        _specTexts_L[(int)SpecType.Damage].text = $"{shoulderData.Damage}";
        _specTexts_L[(int)SpecType.BulletSpeed].text = $"{shoulderData.BulletSpeed}";
        _specTexts_L[(int)SpecType.FireRate].text = $"{shoulderData.FireRate}";
        _specTexts_L[(int)SpecType.CoolDownTime].text = $"{shoulderData.CoolDownTime}";
        _specTexts_L[(int)SpecType.ProjectilesPerShot].text = $"{shoulderData.ProjectilesPerShot}";
        _specTexts_L[(int)SpecType.ShotErrorRange].text = $"{shoulderData.ShotErrorRange}";
    }

    private void UpdateSelectedPartSpecText_R(PartData shoulderData)
    {
        _nextGroup_R.SetActive(false);
        _specTexts_R[(int)SpecType.Weight].text = $"{shoulderData.Weight}";

        _specTexts_R[(int)SpecType.Damage].text = $"{shoulderData.Damage}";
        _specTexts_R[(int)SpecType.BulletSpeed].text = $"{shoulderData.BulletSpeed}";
        _specTexts_R[(int)SpecType.FireRate].text = $"{shoulderData.FireRate}";
        _specTexts_R[(int)SpecType.CoolDownTime].text = $"{shoulderData.CoolDownTime}";
        _specTexts_R[(int)SpecType.ProjectilesPerShot].text = $"{shoulderData.ProjectilesPerShot}";
        _specTexts_R[(int)SpecType.ShotErrorRange].text = $"{shoulderData.ShotErrorRange}";
    }

    public void DisplayNextPartSpecText_L(PartData nextShoulderData)
    {
        _nextGroup_L.SetActive(true);
        _nextSpecTexts_L[(int)SpecType.Weight].text = $"{nextShoulderData.Weight}";

        _nextSpecTexts_L[(int)SpecType.Damage].text = $"{nextShoulderData.Damage}";
        _nextSpecTexts_L[(int)SpecType.BulletSpeed].text = $"{nextShoulderData.BulletSpeed}";
        _nextSpecTexts_L[(int)SpecType.FireRate].text = $"{nextShoulderData.FireRate}";
        _nextSpecTexts_L[(int)SpecType.CoolDownTime].text = $"{nextShoulderData.CoolDownTime}";
        _nextSpecTexts_L[(int)SpecType.ProjectilesPerShot].text = $"{nextShoulderData.ProjectilesPerShot}";
        _nextSpecTexts_L[(int)SpecType.ShotErrorRange].text = $"{nextShoulderData.ShotErrorRange}";
    }

    public void DisplayNextPartSpecText_R(PartData nextShoulderData)
    {
        _nextGroup_R.SetActive(true);
        _nextSpecTexts_R[(int)SpecType.Weight].text = $"{nextShoulderData.Weight}";

        _nextSpecTexts_R[(int)SpecType.Damage].text = $"{nextShoulderData.Damage}";
        _nextSpecTexts_R[(int)SpecType.BulletSpeed].text = $"{nextShoulderData.BulletSpeed}";
        _nextSpecTexts_R[(int)SpecType.FireRate].text = $"{nextShoulderData.FireRate}";
        _nextSpecTexts_R[(int)SpecType.CoolDownTime].text = $"{nextShoulderData.CoolDownTime}";
        _nextSpecTexts_R[(int)SpecType.ProjectilesPerShot].text = $"{nextShoulderData.ProjectilesPerShot}";
        _nextSpecTexts_R[(int)SpecType.ShotErrorRange].text = $"{nextShoulderData.ShotErrorRange}";
    }
}
