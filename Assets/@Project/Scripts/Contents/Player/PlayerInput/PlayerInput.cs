using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public IA_Player InputAction {  get; private set; }
    public IA_Player.PlayerInputActions Actions { get; private set; }

    private void Awake()
    {
        InputAction = new IA_Player();
        Actions = InputAction.PlayerInput;
    }

    private void OnEnable()
    {
        InputAction.Enable();
    }

    private void OnDisable()
    {
        InputAction.Disable();
    }
}
