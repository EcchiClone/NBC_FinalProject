using UnityEngine;
using static Define;

public class SelectorScene : BaseScene
{
    [SerializeField] GameObject[] _cineCams;

    public override void Init()
    {
        Scenes = Scenes.MainScene;
        Cursor.lockState = CursorLockMode.Confined;

        Managers.Module.CreateSelectorModule();
        UI_MainMenuPopup mainUI = Managers.UI.ShowPopupUI<UI_MainMenuPopup>();
        Managers.ActionManager.OnSelectorCam += CamChange;
        Managers.ActionManager.OnUndoMenuCam += CamInActive;

        UI_ChangeButton.IndexOfLowerPart = 0;
        UI_ChangeButton.IndexOfUpperPart = 0;
        UI_ChangeButton.IndexOfArmPart = 0;
        UI_ChangeButton.IndexOfShoulderPart = 0;
    }

    public void CamChange(CamType camType) => _cineCams[(int)camType].SetActive(true);
    public void CamInActive(CamType camType) => _cineCams[(int)camType].SetActive(false);

    public override void Clear()
    {
        Managers.Clear();
    }
}
