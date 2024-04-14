using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private float _scanRange;

    public CinemachineFreeLook FollowCam { get; private set; }
    public CinemachineVirtualCamera LockOnCam { get; private set; }
    public CinemachineTargetGroup TargetGroup { get; private set; }

    public Transform TargetEnemy { get; private set; }

    private readonly float INIT_CAM_POS_Y = 0.5f;
    private readonly float INIT_CAM_POS_X = -8f;

    public void Setup(Module module)
    {
        _module = module;

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
        Vector3 origin = Camera.main.transform.position;
        RaycastHit[] hits = Physics.SphereCastAll(origin, _scanRange, Camera.main.transform.forward, 50f, _targetLayer);
        if (hits.Length == 0)
        {
            Debug.Log("현재 조준시스템에 포착된 적이 없습니다.");
            if (TargetEnemy != null)
                TargetEnemy = null;
            return false;
        }

        int closestIndex = GetClosestTargetIndex(hits);

        if (hits[closestIndex].transform.TryGetComponent(out Entity target) == false)
            return false;
        if (target == TargetEnemy)
            return false;

        TargetEnemy = hits[closestIndex].transform.GetComponent<Entity>().transform;
        return true;
    }

    private int GetClosestTargetIndex(RaycastHit[] hits)
    {
        float closestDist = float.MaxValue;
        int closestIndex = -1;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].distance < closestDist)
            {
                closestIndex = i;
                closestDist = hits[i].distance;
            }
        }

        return closestIndex;
    }

    public void LockOnTarget()
    {
        Managers.ActionManager.CallLockOn(TargetEnemy);
        LockOnCam.gameObject.SetActive(true);
        TargetGroup.AddMember(TargetEnemy, 1, 0);
    }

    public void ReleaseTarget()
    {
        FollowCam.m_XAxis.Value = LockOnCam.transform.rotation.eulerAngles.y;
        Debug.Log(LockOnCam.transform.rotation.eulerAngles.y);

        Managers.ActionManager.CallRelease();
        LockOnCam.gameObject.SetActive(false);
        TargetGroup.RemoveMember(TargetEnemy);
        TargetEnemy = null;
    }

    public void LockTargetChange(Test_Enemy prevTarget, UnityAction action)
    {
        if (TargetEnemy == null)
            return;

        if (!IsThereEnemyScanned())
        {
            ReleaseTarget();
            action.Invoke();
            return;
        }

        TargetGroup.RemoveMember(prevTarget.transform);
        LockOnTarget();
    }
}
