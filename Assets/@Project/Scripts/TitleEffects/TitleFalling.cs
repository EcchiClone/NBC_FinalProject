using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFalling : MonoBehaviour
{
    public float acceleration;
    private float currentVelocity = 0;
    private float minY = -101f;
    private float maxY = 101f;
    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position; // 초기 위치 설정
    }

    void Update()
    {
        currentVelocity += acceleration * Time.deltaTime;

        // 이동할 위치 계산 및 업데이트
        Vector3 newPosition = transform.position - new Vector3(0, currentVelocity * Time.deltaTime, 0);
        transform.position = newPosition;

        // y < 100 에 도달했을 때 파괴
        if (transform.position.y < minY || transform.position.y > maxY)
        {
            Destroy(gameObject);
        }
        else
        {
            // 현재 이동하는 방향을 바라보게
            Vector3 moveDirection = newPosition - previousPosition;
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }

        // 위치갱신
        previousPosition = transform.position;
    }
}