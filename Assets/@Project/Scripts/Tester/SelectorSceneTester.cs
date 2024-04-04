using UnityEngine;
using static Define;

public class SelectorSceneTester : MonoBehaviour
{
    [SerializeField] GameObject[] _cineCams;

    private void Awake()
    {
        Texture2D cursor = Resources.Load<Texture2D>("Cursor");
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);

        Managers.Module.CreateSelectorModule();
        UI_MainMenuPopup mainUI = Managers.UI.ShowPopupUI<UI_MainMenuPopup>();
        Managers.ActionManager.OnSelectorCam += CamChange;
        Managers.ActionManager.OnUndoMenuCam += CamInActive;
    }

    public void CamChange(CamType camType) => _cineCams[(int)camType].SetActive(true);
    public void CamInActive(CamType camType) => _cineCams[(int)camType].SetActive(false);
}
