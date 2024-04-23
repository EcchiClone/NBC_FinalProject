using DG.Tweening;
using System.Linq;
using UnityEngine;

public class UI_MainMenuPopup : UI_Popup
{
    private UI_SelectorMenu _selector;
    private UI_Achievement _achievement;
    private UI_SettingsPopup _settings;
    private UI_StageSelectPopup _stageSelect;    
    private UI_Credit _credit;

    enum Buttons
    {
        GameStart_Btn,
        Module_Select_Btn,
        Perk_Btn,
        Achievement_Btn,
        Settings_Btn,
        Exit_Btn,
        Guide_Btn,
        Dev_Btn,
    }
    enum Images
    {
        NoticeAchievement_Img,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.GameStart_Btn).onClick.AddListener(OpenStageSelect);
        GetButton((int)Buttons.Perk_Btn).onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scenes.PerkViewerScene));

        GetButton((int)Buttons.Module_Select_Btn).onClick.AddListener(OpenModuleSelector);        
        GetButton((int)Buttons.Achievement_Btn).onClick.AddListener(OpenAchievement);
        GetButton((int)Buttons.Settings_Btn).onClick.AddListener(OpenSettings);
        GetButton((int)Buttons.Exit_Btn).onClick.AddListener(ExitGame);
        GetButton((int)Buttons.Guide_Btn).onClick.AddListener(OpenGuide);
        GetButton((int)Buttons.Dev_Btn).onClick.AddListener(OpenCredit);


    }
    private void OnEnable()
    {
        NoticeAchievementImg();
    }

    private void OpenStageSelect()
    {
        if (_stageSelect == null)
        {
            _stageSelect = Managers.UI.ShowPopupUI<UI_StageSelectPopup>();
            _stageSelect.SetPreviousPopup(this);
        }
        else
            _stageSelect.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
    private void OpenModuleSelector()
    {
        if (_selector == null)
        {
            _selector = Managers.UI.ShowPopupUI<UI_SelectorMenu>();
            _selector.SetPreviousPopup(this);            
        }
        else
            _selector.gameObject.SetActive(true);

        Managers.ActionManager.CallSelectorCam(Define.CamType.Module);
        gameObject.SetActive(false);
    }
    private void OpenAchievement()
    {
        if (_achievement == null)
        {
            _achievement = Managers.UI.ShowPopupUI<UI_Achievement>(); // Set on scene about Achievement UI
            _achievement.SetPreviousPopup(this);    // Set prev value(this(MainMenu))            
        }
        else
            _achievement.gameObject.SetActive(true); // Show UI

        gameObject.SetActive(false); // Disable this(MainMenu)
    }
    private void OpenSettings()
    {
        if (_settings == null)
        {
            _settings = Managers.UI.ShowPopupUI<UI_SettingsPopup>();
            _settings.SetPreviousPopup(this);
        }
        else
            _settings.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
    private void OpenGuide()
    {
        Managers.UI.ShowPopupUI<UI_ControlsGuide>();
    }
    private void OpenCredit()
    {
        if (_credit == null)
        {
            _credit = Managers.UI.ShowPopupUI<UI_Credit>();
            _credit.SetPreviousPopup(this);
        }
        else
            _credit.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    #region ExitGame Method
#if UNITY_EDITOR
    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
#else
    private void ExitGame()
    {
        Application.Quit();
    } 
#endif
    #endregion

    private void NoticeAchievementImg()
    {
        bool waiting = Managers.AchievementSystem.ActiveAchievements.Any(achievement =>
            achievement.Icon != null && achievement.State == AchievementState.WaitingForCompletion);

        var image = GetImage((int)Images.NoticeAchievement_Img);

        // 이미 루프 동작 중인 애니메이션 취소 (중복 실행 방지)
        image.DOKill();

        if (waiting)
        {
            // 불투명도를 1로 설정 후, 불투명도가 0.2에서 1로 변경되는 애니메이션 반복
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
            image.DOFade(0.2f, 0.75f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        else
        {
            // 애니메이션 비활성화 시 불투명도를 0으로 설정
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        }
    }

}