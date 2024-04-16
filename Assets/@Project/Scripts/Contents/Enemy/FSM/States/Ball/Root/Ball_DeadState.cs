using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_DeadState : BaseState
{
    private Transform entity;

    bool _isExplodeFinish = false;

    public Ball_DeadState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
        entity = Context.Entity.transform;
    }

    public override void EnterState()
    {
        Context.Entity.Controller.Stop();
        
        Context.Entity.StartCoroutine(Explosion());        
    }

    public override void UpdateState()
    {
        if(_isExplodeFinish)
        {
            Context.Entity.gameObject.SetActive(false);
        }
    }

    public override void CheckSwitchStates()
    {
    }

    public override void ExitState()
    {


    }

    public override void InitializeSubState()
    {
    }

    IEnumerator Explosion()
    {
        //Context.Entity.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(1f);

        RaycastHit[] hits = Physics.SphereCastAll(Context.Entity.transform.position, 10, Vector3.up, 0f);

        foreach (RaycastHit hit in hits) 
        {
            Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                Vector3 other = hit.transform.position;
                Vector3 pushDirection = (other - entity.position).normalized;

                rigidbody.AddForce(pushDirection * 10, ForceMode.Impulse);
            }

            if(hit.transform.gameObject.TryGetComponent(out Module module))
            {
                module.ModuleStatus.GetDamage(20); // TODO 데미지 조절 필요
            }
        }
        // TODO : 사라지자
        ObjectPooler.SpawnFromPool("EnemyExplosion01", _entityTransform.position);
        _isExplodeFinish = true;
    }
}
