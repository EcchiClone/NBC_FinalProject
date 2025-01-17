using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

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

    private readonly float SCRIPTING_INTERVAL_TIME = 0.05f;

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
        //_currentScript = _currentTutoData.Scripts[_scriptIndex];
        _currentScript = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Scripting Table", $"T{_scriptPhase}{_scriptIndex}", LocalizationSettings.SelectedLocale);
        _currentScriptLength = _currentTutoData.Scripts.Count;

        _coroutine = StartCoroutine(Co_Scripting());
    }

    private void NextScript()
    {
        if (Managers.GameManager.TutorialClear)
            return;

        _dialogueText.text = string.Empty;        
        //_currentScript = _currentTutoData.Scripts[++_scriptIndex];
        _currentScript = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Scripting Table", $"T{_scriptPhase}{++_scriptIndex}", LocalizationSettings.SelectedLocale);
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
        string colorText = string.Empty;
        bool coloring = false;
        int colorStack = 0;
        foreach (char c in _currentScript)
        {
            if (c == '<')
                coloring = true;
            if (c == '>')
            {
                colorStack++;
                if (colorStack == 2)
                    coloring = false;
            }
            
            if (coloring)
            {
                colorText += c;                
                continue;
            }                
            else if(coloring == false && string.IsNullOrEmpty(colorText) == false)
            {
                _dialogueText.text += colorText;
                colorText = string.Empty;                
            }
                
            _dialogueText.text += c;
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Tutorial_Dialogue, Vector3.zero);
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
