using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnemyDestroyer : MonoBehaviour
{
    void Start()
    {
        // 현재 객체를 2초 후에 파괴
        Destroy(gameObject, 2f);

        // 3초 후에 현재 객체의 새 인스턴스를 생성
        StartCoroutine(CreateAfterDelay(1f));
    }

    IEnumerator CreateAfterDelay(float delay)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 새 객체 인스턴스 생성
        Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
    }
}
