using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UpperSelector : MonoBehaviour
{
    public GameObject _uiSelector;
    public Transform _contents;

    private void Start()
    {
        int createUI = Managers.Module.UpperPartsCount;

        for (int i = 0; i < createUI; i++)
            Instantiate(_uiSelector, _contents);
    }
}
