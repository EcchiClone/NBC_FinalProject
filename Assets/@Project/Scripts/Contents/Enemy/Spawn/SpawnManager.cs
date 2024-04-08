using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MobType
{

}



public class SpawnManager : MonoBehaviour
{
    private Queue<Vector3> TurretSpawnPoints = new Queue<Vector3>();
    private Queue<Vector3> NormalSpawnPoints = new Queue<Vector3>();


    void Start()
    {
        TurretSpawnPoints.Enqueue(new Vector3(7.5f, 3.5f, -28.5f));
        TurretSpawnPoints.Enqueue(new Vector3(16.0f, 3.5f, -28.5f));
        TurretSpawnPoints.Enqueue(new Vector3(24.0f, 3.5f, -28.5f));
    }

    private Vector3 GetSpawnPoint()
    {
        Vector3 spawnPoint = TurretSpawnPoints.Dequeue();
        TurretSpawnPoints.Enqueue(spawnPoint);

        return spawnPoint;
    }

    public void SpawnTurret()
    {

    }
}
