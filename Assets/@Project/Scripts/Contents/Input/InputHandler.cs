using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private void Update()
    {
        Managers.Input.CallInputKeyDown();
    }
}
