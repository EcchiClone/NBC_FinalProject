using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRainyReverse : MonoBehaviour
{
    public GameObject[] go;
    public float waitTime;
    public int numPerGen;
    GameObject parent;

    private void Start()
    {
        StartCoroutine(SpawnRandomObjects());
        parent = GameObject.Find("Rains");
    }

    private IEnumerator SpawnRandomObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            // 오브젝트 Instantiate
            for (int i=0; i< numPerGen; i++)
            {
                float x = Random.Range(-300.0f, 300.0f);
                float z = Random.Range(-300.0f, 300.0f);
                Vector3 spawnPosition = new Vector3(x, -100.0f, z);
                GameObject randomObject = go[Random.Range(0, go.Length)];
                var instGo = Instantiate(randomObject, spawnPosition, Quaternion.identity);
                instGo.transform.SetParent(parent.transform);
                instGo.GetComponent<TitleFalling>().acceleration *= -1f;
        }
        }
    }
}