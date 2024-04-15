using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TutorialDialoguePopup : UI_Popup
{
    [Header("Dialogue Scripts")]
    [SerializeField][TextArea] string[] _scripts;
    [SerializeField] TextMeshProUGUI _dialogueText;

    private int _scriptIndex;
    private bool _isTexting;

    protected override void Init()
    {
        base.Init();


    }

    private void NextScript()
    {
        _dialogueText.text = string.Empty;
        _dialogueText.text = _scripts[_scriptIndex];
        _scriptIndex++;
    }

    private void HideScript()
    {
        _dialogueText.text = string.Empty;
    }
}
