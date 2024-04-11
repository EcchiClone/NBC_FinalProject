using UnityEngine;
using static Define;

public class SelectorScene : MonoBehaviour
{
    [SerializeField] GameObject[] _cineCams;

    private void Awake()
    {
        Managers.Module.CreateSelectorModule();
        UI_MainMenuPopup mainUI = Managers.UI.ShowPopupUI<UI_MainMenuPopup>();
        Managers.ActionManager.OnSelectorCam += CamChange;
        Managers.ActionManager.OnUndoMenuCam += CamInActive;
    }

    public void CamChange(CamType camType) => _cineCams[(int)camType].SetActive(true);
    public void CamInActive(CamType camType) => _cineCams[(int)camType].SetActive(false);
}
