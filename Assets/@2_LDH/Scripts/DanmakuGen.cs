using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuGen : MonoBehaviour
{
    // 보스오브젝트에 붙이기.
    // 탄막오브젝트에는, DanmakuMove와 DanmakuGen(재귀수에 따름)가 붙는다.
    // 재귀 수가 남아있다면, -1을 하여 자식 탄막에도 DanmakuGen을 부착.

    // 현재 보스 또는 탄막의 위치로부터, 다음 탄막의 생성 정보를 가지고 생성. {랜덤}은 혼자 쓰일수도, 다른 요소와 조합하여 쓰이는 것도 가능
    // 1. 사출 간격 : {세트 수} / {세트 간 딜레이} / {한 프레임에서 생성될 탄막 수}/ {한 세트 내부에서의 딜레이}
    // 2. 탄막 초기 transform : {2-1} + {2-2}
    // 2-1. 위치(방향 / 거리 / Fix여부) : {무방향(월드), 주체 방향, 주체Look플레이어 방향, {랜덤}} / {주체로부터, 플레이어로부터(심화){랜덤}} / {첫 위치 고정여부, {랜덤}} 
    // 2-2. 회전(회전 / Fix여부) : {월드기준, 주체기준, 탄막Look플레이어, 주체Look플레이어, {랜덤}} / {첫 회전 고정 여부, {랜덤}}
    // 3. 사출 속도 : {정속, 생성시간기준 변화, 특수 트리거로 변화}
    // +. 재귀 시 파괴 여부
    // +. 파괴 시간 또는 조건의 수동 지정

    // 컴포넌트 활성화 시 부터 바로 Gen 코루틴을 시작할지, 수동으로 Gen 할지
    //private bool _autoStart = true;

    // 생성 필수요소 : 1.

    private float _initDelay;   // 첫 탄막 생성까지의 지연
    private int _setNum;        // 총 세트 수
    private float _setDelay;    // 세트 사이의 지연
    private int _danmakuNum;    // 한 세트에서 탄막을 몇 차례 생성할지
    private float _inSetDelay;  // 탄막 생성 사이의 지연

    // Base 필수요소 : 2, 3, ...

    void OnEnable()
    {
        _initDelay = 1f;
        _setNum = 3;
        _setDelay = 2.0f;
        _danmakuNum = 5;
        _inSetDelay = 0.1f;
    }
    public void Gen()
    {
        StartCoroutine(Co_Gen());
    }

    IEnumerator Co_Gen()
    {
        yield return new WaitForSeconds(_initDelay); // 일시 중지

        // 테스트 : 1번 요소로만 탄막 생성.
        for (int _nowSet = 0; _nowSet < _setNum; ++_nowSet)
        {
            for(int _nowDanmaku = 0; _nowDanmaku < _danmakuNum;  ++_nowDanmaku)
            {
                Debug.Log("탄막생성");

                var danmakuGo = DanmakuPoolManager.instance.Pool.Get();

                danmakuGo.transform.position = transform.position;

                yield return new WaitForSeconds(_inSetDelay); // 일시 중지
            }
            yield return new WaitForSeconds(_setDelay); // 일시 중지
        }
    }
}
