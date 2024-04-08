using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_DeadState : BaseState
{
    private Transform entity;

    public Ball_DeadState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
        entity = Context.Entity.transform;
    }

    public override void EnterState()
    {
        // 얘는 터지는게 맞을듯..
        Context.Entity.Controller.Stop();
        Context.Entity.StartCoroutine(Explosion());
    }

    public override void UpdateState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        // 얘는 되돌릴 필요 없음
    }

    public override void ExitState()
    {


    }

    public override void InitializeSubState()
    {
    }

    IEnumerator Explosion()
    {
        Context.Entity.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(2f);

        RaycastHit[] hits = Physics.SphereCastAll(Context.Entity.transform.position, 10, Vector3.up, 0f);

        foreach (RaycastHit hit in hits) 
        {
            Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                Vector3 other = hit.transform.position;
                Vector3 pushDirection = (other - entity.position).normalized;

                rigidbody.AddForce(pushDirection * 15, ForceMode.Impulse);
            }
            
        }

        // TODO : 사라지자

    }
}
