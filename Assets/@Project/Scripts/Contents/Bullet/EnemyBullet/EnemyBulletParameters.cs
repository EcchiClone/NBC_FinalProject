using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBulletParameters
{

    // 이동 관련 파라미터
    public float Speed { get; set; }
    public float MinSpeed { get; set; }
    public float MaxSpeed { get; set; }
    public float AccelMultiple { get; set; }
    public float AccelPlus { get; set; }
    public float RotationSpeed { get; set; }
    public float LocalYRotationSpeed { get; set; }
    public Vector3 MoveDirection { get; set; }
    public Vector3 MoveDirectionAim { get; set; }
    public EnemyBulletMoveType _EnemyBulletMoveType { get; set; }
    public List<EnemyBulletChangePropertys> _EnemyBulletChangeMoveProperty { get; set; }

    // 이동 관련 파라미터
    //public float Speed;
    //public float MinSpeed;
    //public float MaxSpeed;
    //public float AccelMultiple;
    //public float AccelPlus;
    //public float RotationSpeed;
    //public float LocalYRotationSpeed;
    //public Vector3 MoveDirection;
    //public Vector3 MoveDirectionAim;
    //public EnemyBulletMoveType _EnemyBulletMoveType;
    //public List<EnemyBulletChangePropertys> _EnemyBulletChangeMoveProperty;

    // 탄막 반환 관련 파라미터
    public ReleaseMethod releaseMethod;
    public float releaseTimer; // 타이머 방식일 경우 사용할 시간

    // 생성자
    public EnemyBulletParameters(
        float speed,
        float minSpeed,
        float maxSpeed,
        float accelMultiple,
        float accelPlus,
        float rotationSpeed,
        float localYRotationSpeed,
        Vector3 moveDirection,
        Vector3 moveDirectionAim,
        EnemyBulletMoveType enemyBulletMoveType,
        List<EnemyBulletChangePropertys> enemyBulletChangeMoveProperty,
        ReleaseMethod releaseMethod, 
        float releaseTimer
        )
    {
        this.Speed = speed;
        this.MinSpeed = minSpeed;
        this.MaxSpeed = maxSpeed;
        this.AccelMultiple = accelMultiple;
        this.AccelPlus = accelPlus;
        this.RotationSpeed = rotationSpeed;
        this.LocalYRotationSpeed = localYRotationSpeed;
        this.MoveDirection = moveDirection;
        this.MoveDirectionAim = moveDirectionAim;
        this._EnemyBulletMoveType = enemyBulletMoveType;
        this._EnemyBulletChangeMoveProperty = enemyBulletChangeMoveProperty;
        this.releaseMethod = releaseMethod;
        this.releaseTimer = releaseTimer;
    }

    // EnemyBulletSettings에서 EnemyBulletParameters를 생성하기 위한 정적 메서드
    public static EnemyBulletParameters FromSettings(EnemyBulletSettings settings)
    {
        // 여기서 settings.initDirectionType을 처리할 수 있도록 수정
        return new EnemyBulletParameters(
            settings.initSpeed,
            settings.minSpeed,
            settings.maxSpeed,
            settings.initAccelMultiple,
            settings.initAccelPlus,
            settings.initRotationSpeed,
            settings.initLocalYRotationSpeed,
            settings.initCustomDirection,
            settings.initCustomDirection,
            settings.enemyBulletMoveType,
            settings.enemyBulletChangeMoveProperty,
            settings.releaseMethod,
            settings.releaseTimer
            );
    }
}
