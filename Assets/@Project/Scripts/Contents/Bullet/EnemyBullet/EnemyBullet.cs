using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : Bullet
{
    public event Action OnCol;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Module module) == true)
        {
            DamageValue damageValue = GetComponent<DamageValue>();
            if (damageValue != null)
                module.ModuleStatus.GetDamage(damageValue.value);
            else
                module.ModuleStatus.GetDamage(1);
            Managers.Pool.GetPooler(PoolingType.Enemy).SpawnFromPool("Default_Explosion01_Effect", transform.position);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("Unwalkable") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            OnCol?.Invoke();
            Managers.Pool.GetPooler(PoolingType.Enemy).SpawnFromPool("Default_Explosion01_Effect", transform.position);
            if (OnCol != null)
            {
                OnCol = null;
                StartCoroutine(SetActiveFalseAfterFrames());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 태그를 가진 오브젝트에게 데미지를 주는 로직 대신 Debug Log를 실행하고 있습니다.");

            Managers.Pool.GetPooler(PoolingType.Enemy).SpawnFromPool("Default_Explosion01_Effect", transform.position);
            gameObject.SetActive(false);
        }
  
    }

    IEnumerator SetActiveFalseAfterFrames()
    {
        yield return null;
        yield return null;

        // 2프레임 후 메소드 실행
        SetActiveFalse();
    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
