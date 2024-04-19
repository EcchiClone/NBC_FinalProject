using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_FirstPlayPopup : UI_Popup
{    
    [SerializeField] private GameObject[] _infos;

    private int index = 0;

    public UnityAction OnGameStart;

    protected override void Init()
    {
        base.Init();

        Managers.Input.OnInputKeyDown += SkipInfo;
    }

    private void SkipInfo()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (index < _infos.Length - 1)
            {
                _infos[index++].gameObject.SetActive(false);
                _infos[index].gameObject.SetActive(true);
            }
            else
            {
                Managers.Input.OnInputKeyDown -= SkipInfo;
                OnGameStart?.Invoke();
                ClosePopupUI();
            }                
        }
    }
}
