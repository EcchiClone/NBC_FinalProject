using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DanmakuParameters
{
    // 이동 관련 파라미터
    public float speed;
    public Vector3 direction;

    // 다음 단계 탄막 생성 관련 파라미터
    public DanmakuPatternData nextPatternData; // 다음 단계 탄막에 대한 데이터

    // 탄막 반환 관련 파라미터
    public ReleaseMethod releaseMethod;
    public float releaseTimer; // 타이머 방식일 경우 사용할 시간

    // Todo. 수많은 파라미터들 아직 미작성
    // 작성해서 바로 테스트 들어갈것.

    public DanmakuParameters(float speed, Vector3 direction, DanmakuPatternData nextPatternData, ReleaseMethod releaseMethod, float releaseTimer)
    {
        this.speed = speed;
        this.direction = direction;
        this.nextPatternData = nextPatternData; // PhaseSO에서 패킹한 데이터를 언팩하면서 하위 탄막에 계속 전달할 것.
        this.releaseMethod = releaseMethod;
        this.releaseTimer = releaseTimer;
    }

}
