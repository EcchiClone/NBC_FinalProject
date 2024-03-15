using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DanmakuParameters
{
    // 이동 관련 파라미터
    public float speed;
    public Vector3 moveDirection;
    public DanmakuMoveType danmakuMoveType;

    // 탄막 반환 관련 파라미터
    public ReleaseMethod releaseMethod;
    public float releaseTimer; // 타이머 방식일 경우 사용할 시간

    // 생성자
    public DanmakuParameters(
        float speed, 
        Vector3 moveDirection, 
        DanmakuMoveType danmakuMoveType,
        ReleaseMethod releaseMethod, 
        float releaseTimer
        )
    {
        this.speed = speed;
        this.moveDirection = moveDirection;
        this.danmakuMoveType = danmakuMoveType;
        this.releaseMethod = releaseMethod;
        this.releaseTimer = releaseTimer;
    }

    // DanmakuSettings에서 DanmakuParameters를 생성하기 위한 정적 메서드
    public static DanmakuParameters FromSettings(DanmakuSettings settings)
    {
        // 여기서 settings.initDirectionType을 처리할 수 있도록 수정
        return new DanmakuParameters(
            settings.initSpeed,
            settings.initCustomDirection,
            settings.danmakuMoveType,
            settings.releaseMethod,
            settings.releaseTimer
            );
    }
}
