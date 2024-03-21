using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletOwner
{
    Player,
    Enemy,
}

public class Bullet : PoolAble, IDamagable
{
    [SerializeField] BulletOwner owenr;

    private void OnTriggerEnter(Collider other)
    {
        switch (owenr)
        {
            case BulletOwner.Player:
                if (other.TryGetComponent(out Boss boss) == true && owenr == BulletOwner.Player)
                {
                    boss.GetDamaged(5);

                    EnemyBulletPoolManager.instance.OnReturnedToPool(gameObject);
                }
                break;
            case BulletOwner.Enemy:
                if (other.TryGetComponent(out PlayerStateMachine playerState) == true && owenr == BulletOwner.Enemy)
                {
                    playerState.Player.GetDamage(5);

                    EnemyBulletPoolManager.instance.OnReturnedToPool(gameObject);
                }
                break;
        }
    }

    public void HitTarget()
    {

    }
}
