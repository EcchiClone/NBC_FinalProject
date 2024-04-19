using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    [SerializeField] private float _hp;
    
    private SpawnEffect _spawnEffect;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _spawnEffect = GetComponent<SpawnEffect>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void GetDamaged(float damage, Vector3 bulletPos)
    {
        if (_hp <= 0)
            return;

        Vector3 forceDir = (transform.position - bulletPos);

        _hp -= damage;
        _rigidbody.AddForce(forceDir * damage, ForceMode.Impulse);

        if (_hp <= 0)
            _spawnEffect.DisableEffect();
    }
}
