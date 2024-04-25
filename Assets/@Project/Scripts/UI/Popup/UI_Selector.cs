using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Selector : UI_Popup
{
    [SerializeField] protected Transform _contents;
    [SerializeField] protected UI_ChangeButton[] _changeBtns;

    private void OnEnable()
    {
        if (_isInit)
        {
            foreach (var changeBtn in _changeBtns)
            {
                changeBtn.CheckUnlockedPart();            
            }                
        }            
    }
}
