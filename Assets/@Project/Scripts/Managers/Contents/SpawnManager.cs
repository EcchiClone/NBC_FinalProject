using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private List<Vector3> _groundCell = new List<Vector3>(); // 탐지 전의 모든 지상 셀 (어떤 요소와도 겹치지 않음)
    private List<Vector3> _rooftopCell = new List<Vector3>(); // 탐지 전의 모든 옥상 셀 (어떤 요소와도 겹치지 않음)

    private HashSet<Vector3> _groundTempPoint = new HashSet<Vector3>(); // 탐지 후 중복 요소 제거용
    private HashSet<Vector3> _rooftopTempPoint = new HashSet<Vector3>();

    public List<Vector3> _groundSpawnPoints = new List<Vector3>(); // 탐지 후 찾아낸 스폰 포인트
    public List<Vector3> _rooftopSpawnPoints = new List<Vector3>();

    public Vector3 gridWorldSize; // 맵의 3차원 크기
    public float cellRadius;
    int gridSizeX, gridSizeY, gridSizeZ;
    private float _cellDiameter;

    //rooftop
    float _rooftopDetectPoint = 150f;
    float _rooftopDetectDistance = 140f;

    // TEMP
    private GameObject spiderPrefab;
    private GameObject ballPrefab;
    private GameObject turretPrefab;
    private List<GameObject> _groundUnitList = new List<GameObject>();
    private List<GameObject> _rooftopUnitList = new List<GameObject>();

    public SpawnManager()
    {
        gridWorldSize = new Vector3(100, 100, 100);
        cellRadius = 5;

        _cellDiameter = cellRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _cellDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _cellDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / _cellDiameter);
    }

    public void Initialize()
    {
        spiderPrefab = Resources.Load<GameObject>("Prefabs/Enemy/Spider_Normal");
        turretPrefab = Resources.Load<GameObject>("Prefabs/Enemy/Turret");
        for (int i = 0; i < 10; ++i)
        {
            GameObject gameObject = GameObject.Instantiate(spiderPrefab);
            gameObject.SetActive(false);
            _groundUnitList.Add(gameObject);

            gameObject = GameObject.Instantiate(turretPrefab);
            gameObject.SetActive(false);
            _rooftopUnitList.Add(gameObject);
        }
    }

    public void Spawn(int count, int mobTypes) // 코루틴 변경 필요
    {
        CreateCell();
        DetectSpawnPoint();
        Shuffle(_groundSpawnPoints);
        Shuffle(_rooftopSpawnPoints);

        if (count > _groundSpawnPoints.Count)
            count = _groundSpawnPoints.Count;
        
        if (count > _rooftopSpawnPoints.Count)
            count = _rooftopSpawnPoints.Count;

        //DestroyAllUnit();

        for (int i = 0; i < count; ++i)
        {
            // 이 부분은 나중에 풀링 사용해야 함. => 
            _groundUnitList[i].transform.position = _groundSpawnPoints[i];
            _groundUnitList[i].SetActive(true);

            _rooftopUnitList[i].transform.position = _rooftopSpawnPoints[i];
            _rooftopUnitList[i].SetActive(true);
        }
    }

    public void DestroyAllUnit() // TODO : 수정 필요 임시임
    {
        if (null == _groundUnitList)
            return;

        foreach (GameObject unit in _groundUnitList)
        {
            unit.SetActive(false);
        }

        if (null == _rooftopUnitList)
            return;

        foreach (GameObject unit in _rooftopUnitList)
        {
            unit.SetActive(false);
        }
    }

    public void CreateCell() // 그리드의 셀만 만듦
    {
        _groundCell.Clear();
        Vector3 wolrdBottomLeft = Vector3.zero - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2;

        for (int x = 0; x < gridSizeX; ++x)
        {
            for (int z = 0; z < gridSizeZ; ++z)
            {
                Vector3 groundSpawnPoint =
                    wolrdBottomLeft +
                    Vector3.right * (x * _cellDiameter + cellRadius) +
                    Vector3.up * cellRadius +
                    Vector3.forward * (z * _cellDiameter + cellRadius);

                Vector3 rooftopSpawnPoint =
                    wolrdBottomLeft +
                    Vector3.right * (x * _cellDiameter + cellRadius) +
                    Vector3.up * _rooftopDetectPoint +
                    Vector3.forward * (z * _cellDiameter + cellRadius);

                if (!Physics.CheckSphere(groundSpawnPoint, cellRadius / 2))
                {
                    _groundCell.Add(groundSpawnPoint);
                }

                _rooftopCell.Add(rooftopSpawnPoint);
            }
        }
    }

    public void DetectSpawnPoint() // 중복 없는 스폰 포인트 탐지
    {
        // 기존 포인트 지우기 작업
        _groundTempPoint.Clear();
        _rooftopTempPoint.Clear();

        _groundSpawnPoints.Clear();
        _rooftopSpawnPoints.Clear();

        RaycastHit hit;
        foreach (Vector3 cell in _groundCell)
        {
            if (Physics.Raycast(cell, Vector3.down, out hit, 10f))
                _groundTempPoint.Add(hit.point);
        }
        _groundSpawnPoints = new List<Vector3>(_groundTempPoint);

        foreach (Vector3 cell in _rooftopCell)
        {
            if (Physics.Raycast(cell, Vector3.down, out hit, _rooftopDetectDistance))
                _rooftopTempPoint.Add(hit.point);
        }
        _rooftopSpawnPoints = new List<Vector3>(_rooftopTempPoint);
    }

    private void Shuffle<T>(List<T> list) // 유틸에 넣기
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
