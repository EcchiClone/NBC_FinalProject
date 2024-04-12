using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneBtn : MonoBehaviour
{
    public void LoadMainScene()
    {
        PerkManager.Instance.SavePerkSequence();
        SceneManager.LoadScene("MainScene");
    }
}
