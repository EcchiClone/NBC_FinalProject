using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBulletParameters
{
    // 이동 관련 파라미터
    public float speed;
    public Vector3 moveDirection;
    public EnemyBulletMoveType enemyBulletMoveType;

    // 탄막 반환 관련 파라미터
    public ReleaseMethod releaseMethod;
    public float releaseTimer; // 타이머 방식일 경우 사용할 시간

    // 생성자
    public EnemyBulletParameters(
        float speed, 
        Vector3 moveDirection, 
        EnemyBulletMoveType enemyBulletMoveType,
        ReleaseMethod releaseMethod, 
        float releaseTimer
        )
    {
        this.speed = speed;
        this.moveDirection = moveDirection;
        this.enemyBulletMoveType = enemyBulletMoveType;
        this.releaseMethod = releaseMethod;
        this.releaseTimer = releaseTimer;
    }

    // EnemyBulletSettings에서 EnemyBulletParameters를 생성하기 위한 정적 메서드
    public static EnemyBulletParameters FromSettings(EnemyBulletSettings settings)
    {
        // 여기서 settings.initDirectionType을 처리할 수 있도록 수정
        return new EnemyBulletParameters(
            settings.initSpeed,
            settings.initCustomDirection,
            settings.enemyBulletMoveType,
            settings.releaseMethod,
            settings.releaseTimer
            );
    }
}
