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
            DanmakuPoolManager.instance.ScheduleRelease(gameObject, releaseTimer);
        }
        else
        {
            Debug.LogWarning("해당 오브젝트는 이미 inactive 상태: " + gameObject.name);
        }
    }
    

}