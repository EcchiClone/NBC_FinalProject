using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRainy : MonoBehaviour
{
    public GameObject[] go;
    public float waitTime;
    public int numPerGen;

    private void Start()
    {
        StartCoroutine(SpawnRandomObjects());
    }

    private IEnumerator SpawnRandomObjects()
    {
        while (true)
        {
            // 0.5초 대기
            yield return new WaitForSeconds(waitTime);
            // 오브젝트 Instantiate
            for (int i=0; i< numPerGen; i++)
            {
                float x = Random.Range(-100.0f, 100.0f);
                float z = Random.Range(-100.0f, 100.0f);
                Vector3 spawnPosition = new Vector3(x, 100.0f, z);
                GameObject randomObject = go[Random.Range(0, go.Length)];
                Instantiate(randomObject, spawnPosition, Quaternion.identity);
            }
        }
    }
}