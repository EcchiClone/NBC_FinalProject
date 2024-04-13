using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class ObstacleSpawner : MonoBehaviour // 스태틱으로 부르면 사용하도록?
{
    NavMeshSurface navMeshSurface;
    private int _groundLayer;
    private int _unwalkableLayer;
    private int combinedLayerMask;

    [SerializeField]private GameObject[] _obstaclePrefabs;
    private GameObject _currentObstacle;


    private void Awake()
    {
        GetOrAddComponent(out navMeshSurface);
        _groundLayer = LayerMask.NameToLayer("Ground");
        _unwalkableLayer = LayerMask.NameToLayer("Unwalkable");
        combinedLayerMask = (1 << _groundLayer) | (1 << _unwalkableLayer);
    }

    public void SpawnObstacle()
    {
        if (_currentObstacle != null)
            RemoveObstacle();

        int random = UnityEngine.Random.Range(0, _obstaclePrefabs.Length);
        _currentObstacle = Instantiate(_obstaclePrefabs[random]);

        RebuildNavMesh();
        PathRequestManager.instance.CreateGrid(); // A* 노드 생성
    }

    public void RemoveObstacle()
    {
        if (_currentObstacle == null)
            return;

        navMeshSurface.RemoveData();
        Destroy(_currentObstacle);
        _currentObstacle = null;
        RebuildNavMesh();
    }
    
    private void RebuildNavMesh()
    {
        if (null == navMeshSurface) 
            throw new ArgumentNullException("navMeshSurface is null");

        navMeshSurface.RemoveData();
        navMeshSurface.layerMask = combinedLayerMask;
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
