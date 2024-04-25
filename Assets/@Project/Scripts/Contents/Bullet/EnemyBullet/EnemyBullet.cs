using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Module module) == true)
        {
            DamageValue damageValue = GetComponent<DamageValue>();
            if (damageValue != null)
                module.ModuleStatus.GetDamage(damageValue.value);
            else
                module.ModuleStatus.GetDamage(1);
            Util.GetPooler(PoolingType.Enemy).SpawnFromPool("Default_Explosion01_Effect", transform.position);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("Unwalkable") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Util.GetPooler(PoolingType.Enemy).SpawnFromPool("Default_Explosion01_Effect", transform.position);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 태그를 가진 오브젝트에게 데미지를 주는 로직 대신 Debug Log를 실행하고 있습니다.");

            Util.GetPooler(PoolingType.Enemy).SpawnFromPool("Default_Explosion01_Effect", transform.position);
            gameObject.SetActive(false);
        }
  
    }
}
