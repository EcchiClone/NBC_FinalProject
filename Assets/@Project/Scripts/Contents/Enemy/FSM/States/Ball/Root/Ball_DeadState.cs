using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ball_DeadState : BaseState
{
    float explodeDelay = 1f;
    float passedTime;

    public Ball_DeadState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        passedTime = 0f;
        
    }

    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if(passedTime >= explodeDelay)
        {
            Explosion();
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

    private void Explosion()
    {
        float damage = Context.Entity.Data.damage;

        RaycastHit[] hits = Physics.SphereCastAll(Context.Entity.transform.position, 10, Vector3.up, 0f);

        foreach (RaycastHit hit in hits) 
        {           
            if (hit.transform.gameObject.TryGetComponent(out Entity entity))
            {
                Rigidbody rigidbody = entity.transform.GetComponent<Rigidbody>();

                if (rigidbody != null)
                {
                    Vector3 other = hit.transform.position;
                    Vector3 pushDirection = (other - _entityTransform.position).normalized;

                    rigidbody.AddForce(pushDirection * 14, ForceMode.Impulse);
                }

                entity.GetDamaged(damage);
            }

            if (hit.transform.gameObject.TryGetComponent(out Module module))
            {
                module.ModuleStatus.GetDamage(damage);
            }
        }
        ObjectPooler.SpawnFromPool("EnemyExplosion01", _entityTransform.position);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Ball_Explode, _entityTransform.position);
        Context.Entity.gameObject.SetActive(false);
    }
}
