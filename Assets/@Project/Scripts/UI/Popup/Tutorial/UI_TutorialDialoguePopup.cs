using System.Collections;
using TMPro;
using UnityEngine;

public class UI_TutorialDialoguePopup : UI_Popup
{
    [Header("Dialogue Scripts")]
    [SerializeField] TextMeshProUGUI _dialogueText;

    private int _scriptIndex;
    private int _scriptPhase;

    private int _currentScriptLength;
    private string _currentScript;
    private bool _isScripting;

    private TutorialData _currentTutoData;
    private Coroutine _coroutine;

    private readonly float SCRIPTING_INTERVAL_TIME = 0.07f;

    protected override void Init()
    {
        base.Init();

        Managers.Input.OnInputKeyDown += SkipTutorialPanel;
        Managers.Input.OnInputKeyDown += SkipDialogue;
        Managers.Tutorial.OnScriptPhaseUpdate += NextScriptPhase;

        NextScriptPhase();
    }

    private void NextScriptPhase()
    {
        _dialogueText.text = string.Empty;
        _scriptIndex = 0;
        
        _currentTutoData = Managers.Data.GetTutorialData(_scriptPhase);        
        _currentScript = _currentTutoData.Scripts[_scriptIndex];
        _currentScriptLength = _currentTutoData.Scripts.Count;

        _coroutine = StartCoroutine(Co_Scripting());
    }

    private void NextScript()
    {
        _dialogueText.text = string.Empty;        
        _currentScript = _currentTutoData.Scripts[++_scriptIndex];
        _coroutine = StartCoroutine(Co_Scripting());
    }

    private void SkipTutorialPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Managers.GameManager.TutorialClear)
            Managers.Tutorial.OnTutorialSkipPopup();
    }

    private void SkipDialogue()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !Managers.Tutorial.IsMissioning)
        {
            if (_isScripting)
            {
                _isScripting = false;
                StopCoroutine(_coroutine);
                _dialogueText.text = _currentScript;
                CheckGiveMission();
            }
            else
                NextScript();
        }
    }

    private IEnumerator Co_Scripting()
    {
        _isScripting = true;

        foreach (char c in _currentScript)
        {
            _dialogueText.text += c;
            yield return Util.GetWaitSeconds(SCRIPTING_INTERVAL_TIME);
        }

        CheckGiveMission();
        _isScripting = false;
    }

    private void CheckGiveMission()
    {
        if (_scriptIndex >= _currentScriptLength - 1)
        {
            if (_scriptPhase == Managers.Data.GetTutorialDataDictCount() - 1)
            {
                Managers.Tutorial.TutorialClear();
                return;
            }                
            Managers.Tutorial.GiveCurrentMission(_scriptPhase++);
        }            
    }
}
