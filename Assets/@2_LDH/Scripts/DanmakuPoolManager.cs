using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DanmakuPoolManager : MonoBehaviour
{
    // PoolManager는 일단 이 정도로 마무리해도 괜찮을 듯. 부모 오브젝트 설정 정도만 하면 깔끔할 듯.
    // 나중에 Managers 구현에 따라 수정만 조금 하면 될 듯 하다.

    // + 탄막 종류의 확대 등 손 볼 곳 꽤 있음. 오브젝트 풀에는 현재 한 종류의 탄막 뿐
    // + 즉, 여러 종류의 탄막을 미리 풀에 넣어두어야 하는 것을 고려하게 될 가능성이 가장 큼.

    public static DanmakuPoolManager instance;

    public int defaultCapacity;
    public int maxPoolSize;
    public GameObject bulletPrefab;

    public IObjectPool<GameObject> Pool { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }

    private void Init()
    {
        Pool = new ObjectPool<GameObject>(
            CreatePooledItem, 
            OnTakeFromPool,
            OnReturnedToPool, 
            OnDestroyPoolObject,
            true,
            defaultCapacity,
            maxPoolSize
            );

        // 미리 오브젝트를 생성
        for (int i = 0; i < defaultCapacity; i++)
        {
            DanmakuController bullet = CreatePooledItem().GetComponent<DanmakuController>();
            bullet.Pool.Release(bullet.gameObject);
        }
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(bulletPrefab);
        poolGo.GetComponent<DanmakuController>().Pool = this.Pool;
        return poolGo;
    }

    // 사용
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
}