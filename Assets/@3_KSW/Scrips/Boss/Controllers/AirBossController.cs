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

        AltitudeAdjustment();
        Look();
        CheckDistance();
        if (IsMoving)
        {
            Move();
        }
        
    }

    protected override void Move()
    {
        boss.transform.position = Vector3.Lerp(boss.transform.position, stopPoint, boss.Data.moveSpeed * Time.deltaTime);
        //AltitudeAdjustment();
    }
    protected override void Look()
    {
        // 타겟 방향 계산
        Vector3 targetDirection = (boss.Target.transform.position - boss.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

        // 쿼터니언의 오일러 각도를 제한해준다.
        Vector3 euler = lookRotation.eulerAngles;
        euler.x = Mathf.Min(euler.x, 40f);

        // 나온 오일러 각도를 다시 방향으로
        lookRotation = Quaternion.Euler(euler);
        boss.transform.rotation = Quaternion.RotateTowards(boss.transform.rotation, lookRotation, 30 * Time.deltaTime);
    }

    private void AltitudeAdjustment()
    {
        Vector3 nowPosition = boss.transform.position;

        boss.transform.position = Vector3.Lerp(nowPosition, new Vector3(nowPosition.x, TargetAltitude, nowPosition.z), Time.deltaTime);
    }

    public override void SetDestination(Vector3 target)
    {
        //float altitude = boss.transform.position.y;

        target.y = TargetAltitude;
        destination = target;

        //float distanceToTarget = Vector3.Distance(boss.transform.position, target);

        Vector3 stopDirection = boss.transform.position - target;
        stopDirection.y = 0f;
        stopDirection.Normalize();
        stopPoint = target + stopDirection * stopDistance;
    }

    protected override void CheckDistance()
    {
        Vector3 currentPosition = boss.transform.position;

        currentPosition.y = destination.y;

        float distanceToDestination = Vector3.Distance(currentPosition, destination);

        IsMoving = distanceToDestination > stopDistance + 5; // 선형보간 다 따라오면 절대 안멈춰서 임의의 숫자로 미리 멈추게 함
    }

    public override void Stop()
    {
        stopDistance = 0f;
        SetDestination(boss.transform.position);
    }

}
