using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerOnEnemyBulletTestScene : MonoBehaviour
{
    public float distance = 5.0f; // 왕복 운동의 범위
    public float speed = 2.0f; // 왕복 운동의 속도

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // 초기 위치 저장
    }

    void Update()
    {
        Vector3 newPosition = startPosition;
        newPosition.x = startPosition.x + Mathf.PingPong(Time.time * speed, distance) - (distance / 2);
        transform.position = newPosition;
    }
}