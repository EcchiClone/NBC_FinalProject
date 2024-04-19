using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirUnitController : Controller
{
    public AirUnitController(Entity entity) : base(entity)
    {
        TargetAltitude = Entity.Data.fixedAltitude;
    }

    public float TargetAltitude { get; set; }

    public override void Update()
    {
        if(!Entity.IsAlive)
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
        // 이동 방향 벡터 계산
        Vector3 moveDirection = (StopPoint - Entity.transform.position).normalized;

        float distanceToStopPoint = Vector3.Distance(Entity.transform.position, StopPoint);

        float forceMagnitude = distanceToStopPoint / Entity.Data.moveSpeed;

        forceMagnitude = Mathf.Clamp(forceMagnitude, 4, Speed);

        // Rigidbody에 힘을 가해 이동
        Rigidbody.AddForce(moveDirection * forceMagnitude);

        if (Rigidbody.velocity.magnitude > Speed)
            Rigidbody.velocity = Rigidbody.velocity.normalized * Speed;
    }
    protected override void Look()
    {
        // 타겟 방향 계산
        Vector3 targetDirection = (Entity.Target.transform.position - Entity.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

        // 쿼터니언의 오일러 각도를 제한해준다.
        Vector3 euler = lookRotation.eulerAngles;
        euler.x = Mathf.Min(euler.x, 40f);

        // 나온 오일러 각도를 다시 방향으로
        lookRotation = Quaternion.Euler(euler);
        Entity.transform.rotation = Quaternion.RotateTowards(Entity.transform.rotation, lookRotation, 100 * Time.deltaTime);
    }

    private void AltitudeAdjustment()
    {
        Vector3 nowPosition = Entity.transform.position;
        if(IsChasing)
        {
            Entity.transform.position = 
                Vector3.Lerp(
                    nowPosition, 
                    new Vector3(nowPosition.x, TargetAltitude + Entity.Target.position.y, nowPosition.z),
                    Time.deltaTime);
        }
        else
        {
            Entity.transform.position = 
                Vector3.Lerp(
                    nowPosition, 
                    new Vector3(nowPosition.x, TargetAltitude, nowPosition.z), 
                    Time.deltaTime);
        }
    }

    public override void SetDestination(Vector3 target)
    {
        target.y = TargetAltitude;
        Destination = target;

        Vector3 stopDirection = Entity.transform.position - target;
        stopDirection.y = 0f;
        stopDirection.Normalize();
        StopPoint = target + stopDirection * StopDistance;
    }

    public override void SetStopDistance(float stopDistance)
    {
        if (0 > stopDistance)
            StopDistance = Entity.Data.stopDistance;
        else
            StopDistance = stopDistance;
    }

    protected override void CheckDistance()
    {
        Vector3 currentPosition = Entity.transform.position;

        currentPosition.y = Destination.y;

        float distanceToDestination = Vector3.Distance(currentPosition, Destination);

        IsMoving = distanceToDestination > StopDistance;
    }

    public override void Stop()
    {
        StopDistance = 0f;
        SetDestination(Entity.transform.position);
    }
}
