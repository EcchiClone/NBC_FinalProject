using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AirBossController : BossController
{
    public AirBossController(Boss boss)
    {
        this.boss = boss;
        TargetAltitude = 20f;
    }

    public float TargetAltitude { get; set; }

    public override void Update()
    {
        if(!boss.IsAlive)
        {
            return;
        }

        CheckDistance();
        if (isMoving)
        {
            Move();
        }
    }

    public override void Move()
    {
        boss.transform.position = Vector3.Lerp(boss.transform.position, destination, boss.data.moveSpeed * Time.deltaTime);
        //AltitudeAdjustment();
    }

    public override void SetDestination(Vector3 target)
    {
        float altitude = boss.transform.position.y;

        target.y = TargetAltitude;

        float distanceToTarget = Vector3.Distance(boss.transform.position, target);

        Vector3 stopDirection = boss.transform.position - target;
        stopDirection.y = 0f;
        stopDirection.Normalize();
        destination = target + stopDirection * stopDistance;
    }

    protected override void CheckDistance()
    {
        Vector3 currentPosition = boss.transform.position;
        Vector3 targetPosition = boss.Target.position;

        //currentPosition.y = 0f;
        targetPosition.y = currentPosition.y;


        float distanceToDestination = Vector3.Distance(currentPosition, targetPosition);

        isMoving = distanceToDestination > stopDistance + 5; // 선형보간 다 따라오면 절대 안멈춰서 임의의 숫자로 미리 멈추게 함
    }

    public override void Stop()
    {
        stopDistance = 0f;
        SetDestination(boss.transform.position);
    }
}
