using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester_BulletShooter : MonoBehaviour
{
    public GameObject m_bullet;
    public Transform m_target;

    public float _standard;
    public float _delay;
    public float _fireRate;
    public float _bulletSpeed;
    public bool _isMultiple;
    public int _multiCount;
    public bool _isSplash;

    private float _time;    

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time > _delay)
        {
            _time = 0;
            if (!_isMultiple)
                ShotRepeater();
            else
                ShotMultiple();
        }
    }

    public void ShotRepeater()
    {
        RandomDirectionShot();
    }

    public void ShotMultiple()
    {
        StartCoroutine(CoMultiShot());
    }

    private IEnumerator CoMultiShot()
    {
        for (int i = 0; i < _multiCount; i++)
        {
            RandomDirectionShot();

            yield return Util.GetWaitSeconds(_fireRate);
        }
    }

    private void RandomDirectionShot()
    {
        float xError = SetShotErrorRange(_standard);
        float yError = SetShotErrorRange(_standard);

        GameObject bullet = Instantiate(m_bullet, transform);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;

        Quaternion rotation = Quaternion.Euler(yError, xError, 0f); // 각도 계산
        bullet.transform.rotation *= rotation; // 현재 방향에 추가 회전을 적용

        bullet.GetComponent<PlayerProjectile>().Setup(_bulletSpeed, 1f, _isSplash, Vector3.zero);
    }

    private float SetShotErrorRange(float standard = 0.5f)
    {
        float x1 = Random.Range(0f, 1f);
        float x2 = Random.Range(0f, 1f);

        return standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }
}
