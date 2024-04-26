using TMPro;
using UnityEngine;

public class UI_ArmSelector : UI_Selector
{
    [SerializeField] GameObject _nextGroup_L;
    [SerializeField] GameObject _nextGroup_R;
    [SerializeField] TextMeshProUGUI[] _specTexts_L;
    [SerializeField] TextMeshProUGUI[] _nextSpecTexts_L;
    [SerializeField] TextMeshProUGUI[] _specTexts_R;
    [SerializeField] TextMeshProUGUI[] _nextSpecTexts_R;    

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

    public enum ChangeArmMode
    {
        LeftArm,
        RightArm,
    }

    public ChangeArmMode CurrentChangeMode { get; private set; }

    protected override void Init()
    {
        base.Init();

        CurrentChangeMode = ChangeArmMode.LeftArm;

        int createUI = Managers.Module.ArmCount;
        _changeBtns = new UI_ChangeButton[createUI];
        for (int i = 0; i < createUI; i++)
        {
            _changeBtns[i] = Managers.UI.ShowItemUI<UI_ArmChangeBtn>(_contents);
            _changeBtns[i].SetParentUI(this);
        }

        Managers.Module.OnLeftArmChange += UpdateSelectedPartSpecText_L;
        Managers.Module.OnRightArmChange += UpdateSelectedPartSpecText_R;
        ResetText();

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.BackToSelector).onClick.AddListener(BackToSelector);
        GetButton((int)Buttons.L_Btn).onClick.AddListener(() => { CurrentChangeMode = ChangeArmMode.LeftArm; Managers.ActionManager.CallArmModeChange(CurrentChangeMode); });
        GetButton((int)Buttons.R_Btn).onClick.AddListener(() => { CurrentChangeMode = ChangeArmMode.RightArm; Managers.ActionManager.CallArmModeChange(CurrentChangeMode); });
    }    

    public void ResetText()
    {
        int leftID = Managers.Module.GetPartOfIndex<ArmsPart>(Managers.GameManager.PartIndex_LeftArm).ID;
        int rightID = Managers.Module.GetPartOfIndex<ArmsPart>(Managers.GameManager.PartIndex_RightArm).ID;
        PartData leftArmPartData = Managers.Data.GetPartData(leftID);
        PartData rightArmPartData = Managers.Data.GetPartData(rightID);

        UpdateSelectedPartSpecText_L(leftArmPartData);
        UpdateSelectedPartSpecText_R(rightArmPartData);
    }

    private void BackToSelector()
    {
        Managers.ActionManager.CallUndoMenuCam(Define.CamType.Arm_Holder);

        ResetText();        
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void UpdateSelectedPartSpecText_L(PartData armData)
    {
        _nextGroup_L.SetActive(false);        
        _specTexts_L[(int)SpecType.Weight].text = $"{armData.Weight}";

        _specTexts_L[(int)SpecType.Damage].text = $"{armData.Damage}";
        _specTexts_L[(int)SpecType.BulletSpeed].text = $"{armData.BulletSpeed}";
        _specTexts_L[(int)SpecType.FireRate].text = $"{armData.FireRate}";
        _specTexts_L[(int)SpecType.CoolDownTime].text = $"{armData.CoolDownTime}";
        _specTexts_L[(int)SpecType.ProjectilesPerShot].text = $"{armData.ProjectilesPerShot}";
        _specTexts_L[(int)SpecType.ShotErrorRange].text = $"{armData.ShotErrorRange}";
    }

    private void UpdateSelectedPartSpecText_R(PartData armData)
    {
        _nextGroup_R.SetActive(false);
        _specTexts_R[(int)SpecType.Weight].text = $"{armData.Weight}";
                   
        _specTexts_R[(int)SpecType.Damage].text = $"{armData.Damage}";
        _specTexts_R[(int)SpecType.BulletSpeed].text = $"{armData.BulletSpeed}";
        _specTexts_R[(int)SpecType.FireRate].text = $"{armData.FireRate}";
        _specTexts_R[(int)SpecType.CoolDownTime].text = $"{armData.CoolDownTime}";
        _specTexts_R[(int)SpecType.ProjectilesPerShot].text = $"{armData.ProjectilesPerShot}";
        _specTexts_R[(int)SpecType.ShotErrorRange].text = $"{armData.ShotErrorRange}";
    }

    public void DisplayNextPartSpecText_L(PartData nextArmData)
    {
        _nextGroup_L.SetActive(true);        
        _nextSpecTexts_L[(int)SpecType.Weight].text = $"{nextArmData.Weight}";
        
        _nextSpecTexts_L[(int)SpecType.Damage].text = $"{nextArmData.Damage}";
        _nextSpecTexts_L[(int)SpecType.BulletSpeed].text = $"{nextArmData.BulletSpeed}";
        _nextSpecTexts_L[(int)SpecType.FireRate].text = $"{nextArmData.FireRate}";
        _nextSpecTexts_L[(int)SpecType.CoolDownTime].text = $"{nextArmData.CoolDownTime}";
        _nextSpecTexts_L[(int)SpecType.ProjectilesPerShot].text = $"{nextArmData.ProjectilesPerShot}";
        _nextSpecTexts_L[(int)SpecType.ShotErrorRange].text = $"{nextArmData.ShotErrorRange}";
    }

    public void DisplayNextPartSpecText_R(PartData nextArmData)
    {
        _nextGroup_R.SetActive(true);
        _nextSpecTexts_R[(int)SpecType.Weight].text = $"{nextArmData.Weight}";
                       
        _nextSpecTexts_R[(int)SpecType.Damage].text = $"{nextArmData.Damage}";
        _nextSpecTexts_R[(int)SpecType.BulletSpeed].text = $"{nextArmData.BulletSpeed}";
        _nextSpecTexts_R[(int)SpecType.FireRate].text = $"{nextArmData.FireRate}";
        _nextSpecTexts_R[(int)SpecType.CoolDownTime].text = $"{nextArmData.CoolDownTime}";
        _nextSpecTexts_R[(int)SpecType.ProjectilesPerShot].text = $"{nextArmData.ProjectilesPerShot}";
        _nextSpecTexts_R[(int)SpecType.ShotErrorRange].text = $"{nextArmData.ShotErrorRange}";
    }
}
