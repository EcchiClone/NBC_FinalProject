using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUnitController : Controller
{
    // Must Fix This.
    Transform head;
    Transform armPivot;

    public TurretUnitController(Entity entity) : base(entity)
    {
        // Must Fix This.
        head = entity.transform.Find("Turret_Upper");
        armPivot = entity.transform.Find("Turret_Upper/Turret_Weapon");
    }

    public override void Update() 
    {
        Look();
    }

    protected override void Look() 
    {
        //머리는 Y만
        Quaternion currentLocalRotation = head.localRotation;
        head.localRotation = Quaternion.identity;

        Vector3 targetWorldLookDir = Target.position - head.position;
        Vector3 targetLocalLookDir = head.InverseTransformDirection(targetWorldLookDir);

        Quaternion targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

        // 쿼터니언 -> 오일러 각도로 변경
        Vector3 euler = targetLocalRotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;

        // 오일러 -> 쿼터니언으로 변경
        targetLocalRotation = Quaternion.Euler(euler);

        head.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1 - Mathf.Exp(-Entity.Data.rotationSpeed * Time.deltaTime)
            );


        //========================================================================== 동일한 내용인데 어떻게 할지 고민!!
        //ArmPivot은 X만
        currentLocalRotation = armPivot.localRotation;
        armPivot.localRotation = Quaternion.identity;

        targetWorldLookDir = Target.position - armPivot.position;
        targetLocalLookDir = armPivot.InverseTransformDirection(targetWorldLookDir);

        targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

        euler = targetLocalRotation.eulerAngles;
        euler.y = 0;
        euler.z = 0;

        targetLocalRotation = Quaternion.Euler(euler);

        armPivot.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1 - Mathf.Exp(-Entity.Data.rotationSpeed * Time.deltaTime)
            );
    }

    private void Standby()
    {

    }

    public override void SetDestination(Vector3 target) { }
}
