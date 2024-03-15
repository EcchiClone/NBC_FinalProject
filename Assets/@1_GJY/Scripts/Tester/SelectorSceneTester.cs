using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorSceneTester : MonoBehaviour
{
    private void Awake()
    {
        Managers.Module.CreateSelectorModule();
    }
}
