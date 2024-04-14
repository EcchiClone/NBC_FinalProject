using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirUnitController : Controller
{
    public AirUnitController(Entity entity) : base(entity)
    {
        TargetAltitude = 20f;
    }

    public float TargetAltitude { get; set; }

    public override void Update()
    {
        if(!_entity.IsAlive)
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
        //boss.transform.position = Vector3.Lerp(boss.transform.position, stopPoint, boss.Data.moveSpeed * Time.deltaTime);
        //boss.transform.position = Vector3.MoveTowards(boss.transform.position, StopPoint, boss.Data.moveSpeed * Time.deltaTime);

        // 이동 방향 벡터 계산
        Vector3 moveDirection = (StopPoint - _entity.transform.position).normalized;

        float distanceToStopPoint = Vector3.Distance(_entity.transform.position, StopPoint);

        float forceMagnitude = distanceToStopPoint / _entity.Data.moveSpeed;

        forceMagnitude = Mathf.Clamp(forceMagnitude, 4, 10);

        // Rigidbody에 힘을 가해 이동
        _entity.GetComponent<Rigidbody>().AddForce(moveDirection * forceMagnitude);
    }
    protected override void Look()
    {
        // 타겟 방향 계산
        Vector3 targetDirection = (_entity.Target.transform.position - _entity.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

        // 쿼터니언의 오일러 각도를 제한해준다.
        Vector3 euler = lookRotation.eulerAngles;
        euler.x = Mathf.Min(euler.x, 40f);

        // 나온 오일러 각도를 다시 방향으로
        lookRotation = Quaternion.Euler(euler);
        _entity.transform.rotation = Quaternion.RotateTowards(_entity.transform.rotation, lookRotation, 30 * Time.deltaTime);
    }

    private void AltitudeAdjustment()
    {
        Vector3 nowPosition = _entity.transform.position;

        _entity.transform.position = Vector3.Lerp(nowPosition, new Vector3(nowPosition.x, TargetAltitude, nowPosition.z), Time.deltaTime);
    }

    public override void SetDestination(Vector3 target)
    {
        //float altitude = boss.transform.position.y;

        target.y = TargetAltitude;
        Destination = target;

        //float distanceToTarget = Vector3.Distance(boss.transform.position, target);

        Vector3 stopDirection = _entity.transform.position - target;
        stopDirection.y = 0f;
        stopDirection.Normalize();
        StopPoint = target + stopDirection * StopDistance;
    }

    public override void SetStopDistance(float stopDistance)
    {
        if (0 > stopDistance)
            StopDistance = _entity.Data.stopDistance;
        else
            StopDistance = stopDistance;
    }

    protected override void CheckDistance()
    {
        Vector3 currentPosition = _entity.transform.position;

        currentPosition.y = Destination.y;

        float distanceToDestination = Vector3.Distance(currentPosition, Destination);

        IsMoving = distanceToDestination > StopDistance;
    }

    public override void Stop()
    {
        StopDistance = 0f;
        SetDestination(_entity.transform.position);
    }

}
