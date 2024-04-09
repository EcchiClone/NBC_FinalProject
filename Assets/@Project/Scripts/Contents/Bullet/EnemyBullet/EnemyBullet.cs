using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Module module) == true)
        {
            module.ModuleStatus.GetDamage(5);

            //EnemyBulletPoolManager.instance.OnReturnedToPool(gameObject);
            ObjectPooler.SpawnFromPool("Test_Copied_Explosion01_Effect", transform.position);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //Debug.Log(other.gameObject.layer);
            //EnemyBulletPoolManager.instance.OnReturnedToPool(gameObject);
            ObjectPooler.SpawnFromPool("Test_Copied_Explosion01_Effect", transform.position);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 태그를 가진 오브젝트에게 데미지를 주는 로직 대신 Debug Log를 실행하고 있습니다.");

            //EnemyBulletPoolManager.instance.OnReturnedToPool(gameObject);
            ObjectPooler.SpawnFromPool("Test_Copied_Explosion01_Effect", transform.position);
            gameObject.SetActive(false);
        }
  
    }
}
