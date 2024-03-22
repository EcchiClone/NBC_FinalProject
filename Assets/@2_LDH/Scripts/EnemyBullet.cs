using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out PlayerStateMachine playerState) == true)
        {
            playerState.Player.GetDamage(5);

            EnemyBulletPoolManager.instance.OnReturnedToPool(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //Debug.Log(other.gameObject.layer);
            EnemyBulletPoolManager.instance.OnReturnedToPool(gameObject);
        }        
    }
}
