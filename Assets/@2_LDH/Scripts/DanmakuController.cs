using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//public class DanmakuController : MonoBehaviour
//{
//    // 탄막의 이동과 반환에 관한 클래스가 될 듯.

//    public IObjectPool<GameObject> Pool { get; set; }

//    private float _speed = 5f;
//    private float _releaseTime = 2f;

//    private Vector3 _randOnSpherePos;

//    private void OnEnable() // Active 상태가 될 때 실행
//    {
//        StartCoroutine(Co_Release()); // 특정 시간 뒤 오브젝트 반환
//        _randOnSpherePos = Random.onUnitSphere;
//    }
//    void Update()
//    {
//        // 탄막 오브젝트 이동 로직
//        // 예시패턴1. 랜덤한 방향으로 직선이동
//        this.transform.Translate(_randOnSpherePos * this._speed * Time.deltaTime); 
//    }
//    IEnumerator Co_Release()
//    {
//        yield return new WaitForSeconds(_releaseTime);
//        Pool.Release(this.gameObject);
//    }
//}
public class DanmakuController : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    private DanmakuParameters _currentParameters; // 현재 탄막의 파라미터를 저장

    private void OnEnable()
    {
        InitializeParameters(); // _currentParameters를 초기화
        StartCoroutine(UpdateMoveParameter()); // 이동에 관해 파라미터를 조정하는 내용을 처리
        StartCoroutine(ProcessBehavior()); // 하위 탄막 생성에 관한 로직을 처리
        StartCoroutine(Co_Release()); // Pool 반환에 관한 로직을 처리
    }
    void InitializeParameters()
    {

    }

    void Update()
    {
        Move();
    }

    void Move()
    {

    }

    IEnumerator UpdateMoveParameter()
    {
        // 이동 파라미터 업데이트
        yield return null;
    }

    IEnumerator ProcessBehavior()
    {
        // 하위 탄막 생성
        yield return null;
    }
    IEnumerator Co_Release()
    {
        // 반환 관리
        yield return new WaitForSeconds(_currentParameters.releaseTimer);
        Pool.Release(this.gameObject);
    }


}
