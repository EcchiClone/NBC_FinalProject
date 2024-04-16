using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeyDown : AchievementUpdater
{
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Managers.AchievementSystem.ReceiveReport("KEY_INPUT", KeyCode.W, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Managers.AchievementSystem.ReceiveReport("KEY_INPUT", KeyCode.A, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Managers.AchievementSystem.ReceiveReport("KEY_INPUT", KeyCode.S, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Managers.AchievementSystem.ReceiveReport("KEY_INPUT", KeyCode.D, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Managers.AchievementSystem.ReceiveReport("KEY_INPUT", KeyCode.Space, 1);
        //}
        // 사용자가 이 프레임에서 입력한 문자열을 가져옵니다.

        if (Managers.Scene.CurrentScene.Scenes == Define.Scenes.Tutorial)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Managers.AchievementSystem.ReceiveReport("KEY_INPUT", KeyCode.Space, 1);
        }
        // 사용자가 이 프레임에서 입력한 문자열을 가져옵니다.
        //string inputString = Input.inputString;
        //if (!string.IsNullOrEmpty(inputString))
        //{
        //    foreach (char c in inputString)
        //    {
        //        KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), c.ToString().ToUpper());
        //        Managers.AchievementSystem.ReceiveReport("KEY_INPUT", key, 1);
        //    }
        //}
    }

}
