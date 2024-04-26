using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitParticle : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Co_Inactive());
    }

    private IEnumerator Co_Inactive()
    {
        yield return Util.GetWaitSeconds(1f);

        gameObject.SetActive(false);
    }
}
