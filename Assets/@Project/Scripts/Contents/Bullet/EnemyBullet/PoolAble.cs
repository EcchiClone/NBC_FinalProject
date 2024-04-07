using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PoolAble : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    //public void ReleaseObject()
    //{
    //    Pool.Release(gameObject);
    //}

    public void ReleaseObject(float releaseTimer)
    {
        if (gameObject.activeInHierarchy)
        {
            //EnemyBulletPoolManager.instance.ScheduleRelease(gameObject, releaseTimer);
            StartCoroutine("Co_DisableWithTimer", releaseTimer);
        }
        else
        {
            Debug.LogWarning("해당 오브젝트는 이미 inactive 상태: " + gameObject.name);
        }
    }

    private IEnumerator Co_DisableWithTimer(float releaseTimer)
    {
        yield return Util.GetWaitSeconds(releaseTimer);
        gameObject.SetActive(false);
    }
    

}