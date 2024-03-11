using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootTest : MonoBehaviour
{
    public Transform bulletSpawnPoint;

    void Update()
    {
        DanmakuGen Dg = GetComponent<DanmakuGen>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dg.Gen();
        }

    }
}