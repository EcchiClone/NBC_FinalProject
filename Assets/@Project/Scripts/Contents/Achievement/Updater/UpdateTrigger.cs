using System;
using UnityEngine;

public class UpdateTrigger : AchievementUpdater
{
    // OnDisable에서 Invoke하여, 몬스터가 파괴될 때 실행될 내용들을 구독
    private event Action onTriggerEnter; // 이벤트 선언

    private void Awake()
    {
        // onEnemyDestroied += 에 GameManager에서 Kill Counter 등의 역할을 할 메서드 달기

        onTriggerEnter += Report;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Module module) == true)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        onTriggerEnter?.Invoke();
    }
}
