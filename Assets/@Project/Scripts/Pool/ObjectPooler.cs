using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerEditor : Editor
{
    const string INFO = "풀링한 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
        "    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
        "    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
    }
}
#endif

public class ObjectPooler : MonoBehaviour
{
    static ObjectPooler inst;
    void Awake() => inst = this;

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] Pool[] pools;
    List<GameObject> spawnObjects;
    Dictionary<string, Queue<GameObject>> poolDictionary;
    readonly string INFO = " 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
        "    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
        "    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";



    public static GameObject SpawnFromPool(string tag, Vector3 position) =>
        inst._SpawnFromPool(tag, position, Quaternion.identity);

    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
        inst._SpawnFromPool(tag, position, rotation);

    public static T SpawnFromPool<T>(string tag, Vector3 position) where T : Component
    {
        GameObject obj = inst._SpawnFromPool(tag, position, Quaternion.identity);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    public static T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = inst._SpawnFromPool(tag, position, rotation);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    public static List<GameObject> GetAllPools(string tag)
    {
        if (!inst.poolDictionary.ContainsKey(tag))
            throw new Exception($"Pool with tag {tag} doesn't exist.");

        return inst.spawnObjects.FindAll(x => x.name == tag);
    }

    public static List<T> GetAllPools<T>(string tag) where T : Component
    {
        List<GameObject> objects = GetAllPools(tag);

        if (!objects[0].TryGetComponent(out T component))
            throw new Exception("Component not found");

        return objects.ConvertAll(x => x.GetComponent<T>());
    }

    public static void ReturnToPool(GameObject obj)
    {
        if (!inst.poolDictionary.ContainsKey(obj.name))
            throw new Exception($"Pool with tag {obj.name} doesn't exist.");

        // 오브젝트의 위치와 회전을 초기화
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        // Rigidbody가 있다면 velocity와 angularVelocity 초기화
        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        inst.poolDictionary[obj.name].Enqueue(obj);
    }

    [ContextMenu("GetSpawnObjectsInfo")]
    void GetSpawnObjectsInfo()
    {
        foreach (var pool in pools)
        {
            int count = spawnObjects.FindAll(x => x.name == pool.tag).Count;
            Debug.Log($"{pool.tag} count : {count}");
        }
    }

    GameObject _SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
            throw new Exception($"Pool with tag {tag} doesn't exist.");

        // 큐에 없으면 새로 추가
        Queue<GameObject> poolQueue = poolDictionary[tag];
        if (poolQueue.Count <= 0)
        {
            Pool pool = Array.Find(pools, x => x.tag == tag);
            var obj = CreateNewObject(pool.tag, pool.prefab);
            ArrangePool(obj);
        }

        // 큐에서 꺼내서 사용
        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    void Start()
    {
        spawnObjects = new List<GameObject>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 미리 생성
        foreach (Pool pool in pools)
        {
            poolDictionary.Add(pool.tag, new Queue<GameObject>());
            for (int i = 0; i < pool.size; i++)
            {
                var obj = CreateNewObject(pool.tag, pool.prefab);
                ArrangePool(obj);
            }

            // OnDisable에 ReturnToPool 구현여부와 중복구현 검사
            if (poolDictionary[pool.tag].Count <= 0)
                Debug.LogError($"{pool.tag}{INFO}");
            else if (poolDictionary[pool.tag].Count != pool.size)
                Debug.LogError($"{pool.tag}에 ReturnToPool이 중복됩니다");
        }
    }

    GameObject CreateNewObject(string tag, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = tag;
        obj.SetActive(false); // 비활성화시 ReturnToPool을 하므로 Enqueue가 됨
        return obj;
    }

    void ArrangePool(GameObject obj)
    {
        // 추가된 오브젝트 묶어서 정렬
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
            else if (transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
        }
    }

    //void Start()
    //{
    //    spawnObjects = new List<GameObject>();
    //    poolDictionary = new Dictionary<string, Queue<GameObject>>();
    //    StartCoroutine(TrackAllPoolsCompletion());

    //    foreach (Pool pool in pools)
    //    {
    //        StartCoroutine(InitializePoolAsync(pool, 10, 1000)); // 한 번에 10개씩 오브젝트 생성
    //    }
    //}
    //IEnumerator InitializePoolAsync(Pool pool, int chunkSize, int logInterval)
    //{
    //    int count = 0;
    //    poolDictionary.Add(pool.tag, new Queue<GameObject>());

    //    for (int i = 0; i < pool.size; i++)
    //    {
    //        var obj = CreateNewObject(pool.tag, pool.prefab);
    //        ArrangePool(obj);
    //        count++;

    //        if (count % logInterval == 0)
    //        {
    //            Debug.Log($"{pool.tag} 의 풀 생성중 : {count} 개의 오브젝트.");
    //        }

    //        if ((i + 1) % chunkSize == 0)
    //            yield return null;
    //    }

    //    Debug.Log($"{pool.tag} 의 풀 생성 완료 : {count} 개의 오브젝트.");

    //    //OnDisable에 ReturnToPool 구현여부와 중복구현 검사
    //    if (poolDictionary[pool.tag].Count <= 0)
    //        Debug.LogError($"{pool.tag}{INFO}");
    //    else if (poolDictionary[pool.tag].Count != pool.size)
    //        Debug.LogError($"{pool.tag}에 ReturnToPool이 중복됩니다");
    //}
    //GameObject CreateNewObject(string tag, GameObject prefab)
    //{
    //    var obj = Instantiate(prefab, transform);
    //    obj.name = tag;
    //    obj.SetActive(false);
    //    return obj;
    //}
    //void ArrangePool(GameObject obj)
    //{
    //    //오브젝트를 풀에 추가
    //    obj.transform.SetParent(transform);
    //    poolDictionary[obj.name].Enqueue(obj);
    //}
    //IEnumerator TrackAllPoolsCompletion()
    //{
    //    bool allDone = false;
    //    while (!allDone)
    //    {
    //        yield return new WaitForSeconds(1); // 에브리 세컨드 체크
    //        allDone = true;
    //        foreach (Pool pool in pools)
    //        {
    //            if (poolDictionary[pool.tag].Count < pool.size)
    //            {
    //                allDone = false;
    //                break;
    //            }
    //        }
    //    }
    //    Debug.Log("모든 풀 생성 종료.");
    //}
}