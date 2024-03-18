using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        // 오브젝트 이름
        public string objectName;
        // 오브젝트 풀에서 관리할 오브젝트
        public GameObject perfab;
        // 몇개를 미리 생성 해놓을건지
        public int count;
    }


    public static EnemyBulletPoolManager instance;

    // 오브젝트풀 매니저 준비 완료표시
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // 생성할 오브젝트의 key값지정을 위한 변수
    private string objectName;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // 오브젝트풀에서 오브젝트를 새로 생성할때 사용할 딕셔너리
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        //Application.targetFrameRate = 60; // 임시작성
        if (instance == null)
        {
            instance = this;
        }

        else
            Destroy(this.gameObject);

        Init();
    }

    IObjectPool<GameObject> pool;
    int nowActiveItem;

    private void Init()
    {
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);
            
            if (goDic.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} 이미 등록된 오브젝트입니다.", objectInfos[idx].objectName);
                return;
            }

            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }
        IsReady = true;
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        //Debug.Log("생성");
        nowActiveItem++; // 갯수추적
        GameObject poolGo = Instantiate(goDic[objectName]);
        poolGo.GetComponent<PoolAble>().Pool = ojbectPoolDic[objectName];
        return poolGo;
    }

    // 대여
    private void OnTakeFromPool(GameObject poolGo)
    {
        //Debug.Log("대여");
        nowActiveItem++; // 갯수추적
        poolGo.SetActive(true);
    }

    // 반환 -> 대량 탄막에는 지양. 큐 사용한 배치처리로 프레임당 횟수 제한
    private void OnReturnedToPool(GameObject poolGo)
    {
        nowActiveItem--; // 갯수추적
        poolGo.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        nowActiveItem--; // 갯수추적
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;
        //Debug.Log(objectName);

        if (goDic.ContainsKey(goName) == false)
        {
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", goName);
            return null;
        }

        return ojbectPoolDic[goName].Get();
    }

    // 배치 처리를 통한 반환 최적화
    private Queue<GameObject> releaseQueue = new Queue<GameObject>();
    public int releaseBatchSize = 100; // 한 프레임에 반환할 오브젝트 수

    private void Update()
    {
        ProcessReleaseQueue();
        print($"현재 Active 수 : {nowActiveItem}");
    }
    private void ProcessReleaseQueue()
    {
        for (int i = 0; i < releaseBatchSize && releaseQueue.Count > 0; i++)
        {
            GameObject poolGo = releaseQueue.Dequeue(); // 안전 검사: 오브젝트가 아직 활성화 상태인지 확인

            if (poolGo.activeInHierarchy)
            {
                this.pool.Release(poolGo);
                //poolGo.SetActive(false); // 비활성화하거나 반환 로직 수행 > 허... 됐다. 
            }
        }
    }

    public void ScheduleRelease(GameObject poolGo, float delay)
    {
        StartCoroutine(Co_ScheduleRelease(poolGo, delay));
    }

    private IEnumerator Co_ScheduleRelease(GameObject poolGo, float delay)
    {
        yield return new WaitForSeconds(delay);
        releaseQueue.Enqueue(poolGo); // 반환 대기열에 추가
    }

}