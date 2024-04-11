using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeyDown : AchievementUpdater
{
    public KeyCode keyCode;
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            Managers.AchievementSystem.ReceiveReport("KEY_INPUT", keyCode, 1);
        }
    }
}
