using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class LockOnSystem
{
    // To - Do List
    // 기능 1. 락온시스템 : 마우스 휠 Started시, SphereCast로 주변 적 검색.
    // 기능 2. 타겟팅 시스템 : 다중 락온시스템에 의해 검색된 적들 중 가장 가까운 적 타겟팅
    // 기능 3. 카메라 추적 시스템 : 타겟팅 된 적에게 Cinemachine Cam - Look At Transform 이 해당 적으로 변경.    
    private Module _module;

    [SerializeField] Transform _followOnTargetMode;
    [SerializeField] LayerMask _targetLayer;
    [field: SerializeField] public float ScanRange;
    [field: SerializeField] public float ScanRadius;

    public CinemachineFreeLook FollowCam { get; private set; }
    public CinemachineVirtualCamera LockOnCam { get; private set; }
    public CinemachineTargetGroup TargetGroup { get; private set; }

    public bool IsLockon { get; private set; }
    public ITarget TargetEnemy { get; private set; }

    private readonly float INIT_CAM_POS_Y = 0.5f;
    private readonly float INIT_CAM_POS_X = -8f;

    public void Setup(Module module)
    {
        _module = module;
        ScanRange *= module.ModuleStatus.ScanRangeAdjust;

        // 시네머신 카메라 초기화
        FollowCam = GameObject.Find("@FollowCam").GetComponent<CinemachineFreeLook>();
        LockOnCam = GameObject.Find("@LockOnCam").GetComponent<CinemachineVirtualCamera>();
        TargetGroup = GameObject.Find("@TargetGroup").GetComponent<CinemachineTargetGroup>();

        FollowCam.Follow = _module.transform;
        FollowCam.LookAt = _module.transform;

        LockOnCam.Follow = _followOnTargetMode;
        LockOnCam.LookAt = TargetGroup.transform;
        LockOnCam.gameObject.SetActive(false);

        TargetGroup.AddMember(_module.transform, 1, 0);
        TargetGroup.AddMember(_followOnTargetMode, 1, 0);

        FollowCam.m_YAxis.Value = INIT_CAM_POS_Y;
        FollowCam.m_XAxis.Value = INIT_CAM_POS_X;
    }

    public bool IsThereEnemyScanned()
    {
        Vector3 origin;
        Vector3 dir;
        RaycastHit[] hitsRay = null;
        Collider[] hitsColl = null;

        bool isLockon;

        if (IsLockon)
        {
            origin = TargetEnemy.Transform.position;
            hitsColl = Physics.OverlapSphere(origin, ScanRadius, _targetLayer);
            isLockon = true;
        }
        else
        {
            origin = _module.transform.position + Camera.main.transform.forward * ScanRadius + Camera.main.transform.up * ScanRadius * 0.5f;
            dir = Camera.main.transform.forward;
            hitsRay = Physics.SphereCastAll(origin, ScanRadius, dir, ScanRange, _targetLayer);
            isLockon = false;
        }

        ITarget newTarget = isLockon ? GetClosestTarget(hitsColl) : GetClosestTarget(hitsRay);
        if (newTarget == null)
            return false;

        TargetEnemy = newTarget;
        return true;
    }

    private ITarget GetClosestTarget(RaycastHit[] hits)
    {
        if (hits == null)
        {
            Debug.Log("현재 조준시스템에 포착된 적이 없습니다.");
            return null;
        }

        float closestDist = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.TryGetComponent(out ITarget target) == false)
                continue;
            if (target == TargetEnemy || !target.IsAlive)
                continue;
            if (target.EnemyType == Define.EnemyType.Boss)
                return target;
            if (hits[i].distance < closestDist)
            {
                closestIndex = i;
                closestDist = hits[i].distance;
            }
        }
        if (closestIndex == -1)
            return null;
        
        return hits[closestIndex].transform.GetComponent<ITarget>();
    }

    private ITarget GetClosestTarget(Collider[] hits)
    {
        if (hits == null)
        {
            Debug.Log("현재 조준시스템에 포착된 적이 없습니다.");
            return null;
        }

        float closestDist = float.MaxValue;
        ITarget ClosestTarget = null;

        foreach (Collider hit in hits)
        {
            if (hit.transform.TryGetComponent(out ITarget target) == false)
                continue;
            if (target == TargetEnemy || !target.IsAlive)
                continue;
            if (target.EnemyType == Define.EnemyType.Boss)
                return target;

            float dist = Vector3.Distance(TargetEnemy.Transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                ClosestTarget = target;
                closestDist = dist;
            }
        }
        return ClosestTarget;
    }

    public void LockOnTarget()
    {
        IsLockon = true;

        float percent = TargetEnemy.AP / TargetEnemy.MaxAP;
        Managers.ActionManager.CallLockOn(TargetEnemy, percent);
        LockOnCam.gameObject.SetActive(true);
        TargetGroup.AddMember(TargetEnemy.Transform, 1, 0);

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_LockOn, Vector3.zero);
    }

    public void ReleaseTarget()
    {
        IsLockon = false;
        FollowCam.m_XAxis.Value = LockOnCam.transform.rotation.eulerAngles.y;

        Managers.ActionManager.CallRelease();
        LockOnCam.gameObject.SetActive(false);
        TargetGroup.RemoveMember(TargetEnemy.Transform);
        TargetEnemy = null;
    }

    public void LockTargetChange(ITarget prevTarget)
    {
        if (TargetEnemy == null || TargetEnemy != prevTarget)
        {
            Debug.Log("콜 한 애가 락온 한 애가 아님");
            return;
        }

        if (!IsThereEnemyScanned())
        {
            Debug.Log("락 해제");
            ReleaseTarget();
            return;
        }

        Debug.Log("이전꺼 없애고 새로운 락온");
        TargetGroup.RemoveMember(prevTarget.Transform);
        LockOnTarget();
    }
}
