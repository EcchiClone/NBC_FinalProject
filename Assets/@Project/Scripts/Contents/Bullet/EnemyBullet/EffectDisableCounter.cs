using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDisableCounter : MonoBehaviour
{
    [SerializeField] private float releaseTimer = 5f;
    void Start()
    {
        StartCoroutine("Co_DisableWithTimer", releaseTimer);
    }
    private IEnumerator Co_DisableWithTimer(float releaseTimer)
    {
        yield return Util.GetWaitSeconds(releaseTimer);
        gameObject.SetActive(false);
    }
}
