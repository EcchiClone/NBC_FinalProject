using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeTime : MonoBehaviour
{
    [SerializeField] float _lifeTime;

    public void Setup() => StartCoroutine(Co_LifeTime());

    private IEnumerator Co_LifeTime()
    {
        yield return Util.GetWaitSeconds(_lifeTime);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Util.GetPooler(PoolingType.Player).ReturnToPool(gameObject);
    }
}
