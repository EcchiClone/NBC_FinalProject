using System;

public class TutorialManager
{
    public event Action OnTutorialClear;

    private UI_Popup _skipPopup;

    public void TutorialStart()
    {
        UI_BlinderPopup blinder = Managers.UI.ShowPopupUI<UI_BlinderPopup>();
        blinder.DoOpenBlind(2f);
    }

    public void NextDialogueText()
    {
        // To Do - 대사 2/4 이런식이면 3/4 번째 대사 출력하기
    }

    public void SkipDialogueText()
    {
        // To do - 다이얼로그 이펙트 나오는 중이면 끊어버리고 현재 대사 바로 띄우기
    }

    public void TutorialClear()
    {
        OnTutorialClear?.Invoke();
    }

    public void OnTutorialSkipPopup()
    {
        if (_skipPopup != null)
            return;

        _skipPopup = Managers.UI.ShowPopupUI<UI_TutorialSkipPopup>();        
    }

    public void Clear()
    {
        OnTutorialClear = null;
    }
}
