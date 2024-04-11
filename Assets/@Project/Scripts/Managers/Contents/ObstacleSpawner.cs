using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class ObstacleSpawner : MonoBehaviour // 스태틱으로 부르면 사용하도록?
{
    NavMeshSurface navMeshSurface;
    [SerializeField] LayerMask navMeshLayerMask;

    [SerializeField]private GameObject[] _obstaclePrefabs;
    private GameObject _currentObstacle;

    private bool _isObstacleSpawnDone = false;// 코루틴 끝났는지 확인 변수

    private void Awake()
    {
        GetOrAddComponent(out navMeshSurface);
        navMeshLayerMask = LayerMask.GetMask("Ground");
    }

    public void SpawnObstacle()
    {
        _isObstacleSpawnDone = false;
        if (_currentObstacle != null)
            RemoveObstacle();

        int random = UnityEngine.Random.Range(0, _obstaclePrefabs.Length);
        _currentObstacle = Instantiate(_obstaclePrefabs[random]);

        RebuildNavMesh();
        PathRequestManager.instance.CreateGrid(); // A* 노드 생성

        _isObstacleSpawnDone = true;
    }

    public void RemoveObstacle()
    {
        if (_currentObstacle == null)
            return;

        navMeshSurface.RemoveData();
        Destroy(_currentObstacle);
        _currentObstacle = null;
    }
    
    private void RebuildNavMesh()
    {
        if (null == navMeshSurface) 
            throw new ArgumentNullException("navMeshSurface is null");

        navMeshSurface.RemoveData();
        navMeshSurface.layerMask = navMeshLayerMask;
        navMeshSurface.BuildNavMesh();
    }

    public void GetOrAddComponent<T>(out T reference) where T : Component
    {
        reference = GetComponent<T>();
        if (null == reference)
            reference = gameObject.AddComponent<T>();
    }

    private void OnDrawGizmos()
    {
        
    }
}
