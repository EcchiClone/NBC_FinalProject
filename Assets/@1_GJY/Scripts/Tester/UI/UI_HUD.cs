using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HUD : UI_Scene
{
    [SerializeField] GameObject _crossHair;
    [SerializeField] GameObject _lockOnIndicator;

    private Transform _target;

    protected override void Init()
    {
        base.Init();

        LockOnSystem.OnLockOn += GetTargetedEnemy;
        LockOnSystem.OnRelease += ReleaseTarget;
    }

    private void GetTargetedEnemy(Transform target)
    {
        _target = target;
        _crossHair.SetActive(false);
        _lockOnIndicator.SetActive(true);
    }

    private void ReleaseTarget()
    {
        _target = null;
        _crossHair.SetActive(true);
        _lockOnIndicator.SetActive(false);
    }

    private void Update()
    {
        if (!_lockOnIndicator.activeSelf || _target == null)
            return;

        _lockOnIndicator.transform.position = Camera.main.WorldToScreenPoint(_target.position);
        Debug.Log(Camera.main.WorldToScreenPoint(_lockOnIndicator.transform.position));
    }
}
