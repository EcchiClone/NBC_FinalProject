using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnManager : MonoBehaviour
{
    private List<Vector3> spawnPoints = new List<Vector3>();

    public int CurrentLevel { get; private set; }



    // TEMP
    [SerializeField] private GameObject spawnPrefab;

    void Start()
    {
        for(int i = -49; i < 49; )// x
        {
            for(int j = -49; j < 49; )// z
            {
                spawnPoints.Add(new Vector3(i, 2f, j));
                j += 5;
            }
            i += 5;
        }

        Spawn();
    }

    private void Update()
    {
        
    }

    /*private Vector3 GetSpawnPoint()
    {
        Vector3 spawnPoint = NormalSpawnPoints.Dequeue();
        NormalSpawnPoints.Enqueue(spawnPoint);

        return spawnPoint;
    }*/


    public void LoadLevel()
    {

    }


    public void Spawn()
    {
        for(int i = 0; i < spawnPoints.Count; ++i)
        {
            Instantiate(spawnPrefab, spawnPoints[i], Quaternion.identity);
        }
    }
}
