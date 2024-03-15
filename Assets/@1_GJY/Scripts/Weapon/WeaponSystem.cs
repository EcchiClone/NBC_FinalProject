using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    // To - Do List
    // 기능 1. 락온시스템 : 마우스 휠 Started시, SphereCast로 주변 적 검색.
    // 기능 2. 타겟팅 시스템 : 다중 락온시스템에 의해 검색된 적들 중 가장 가까운 적 타겟팅
    // 기능 3. 카메라 추적 시스템 : 타겟팅 된 적에게 Cinemachine Cam - Look At Transform 이 해당 적으로 변경.
    // 기능 4. 다중 공격시스템 : 다중 락온에 의해 검색된 적들을 다중 공격이 가능한 무기로 공격 시 랜덤한 적에게 공격.

    [SerializeField] LayerMask _targetLayer;
    [SerializeField] private float _scanRange;

    public CinemachineFreeLook FollowCam { get; private set; }
    public CinemachineVirtualCamera LockOnCam { get; private set; }
    public CinemachineTargetGroup TargetGroup { get; private set; }

    private Transform _targetingEnemy;

    public void Setup()
    {
        // 시네머신 카메라 초기화
        FollowCam = GameObject.Find("@FollowCam").GetComponent<CinemachineFreeLook>();
        LockOnCam = GameObject.Find("@LockOnCam").GetComponent<CinemachineVirtualCamera>();
        TargetGroup = GameObject.Find("@TargetGroup").GetComponent<CinemachineTargetGroup>();

        FollowCam.Follow = transform;
        FollowCam.LookAt = transform;

        TargetGroup.AddMember(transform, 1, 0);

        LockOnCam.Follow = transform;
        LockOnCam.LookAt = TargetGroup.transform;
        LockOnCam.gameObject.SetActive(false);
    }

    // 다중 락온을 위해 모든 적을 Queue에 저장
    public bool IsThereEnemyScanned()
    {
        RaycastHit hit;
        if (!Physics.SphereCast(transform.position, _scanRange, Vector3.zero, out hit, 0, _targetLayer))
        {
            Debug.Log("현재 조준시스템에 포착된 적이 없습니다.");
            return false;
        }

        _targetingEnemy = hit.transform.GetComponent<Test_Enemy>().transform;
        return true;
    }

    public void LockOnTarget()
    {
        TargetGroup.AddMember(_targetingEnemy, 1, 0);
        LockOnCam.gameObject.SetActive(true);
        FollowCam.gameObject.SetActive(false);
    }

    public void ReleaseTarget()
    {
        TargetGroup.RemoveMember(_targetingEnemy);
        _targetingEnemy = null;
        FollowCam.gameObject.SetActive(true);
        LockOnCam.gameObject.SetActive(false);        
    }
}
