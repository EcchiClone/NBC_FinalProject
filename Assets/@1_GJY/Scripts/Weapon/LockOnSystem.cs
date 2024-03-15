using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
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

    public Transform TargetEnemy { get; private set; }    

    public static event Action<Transform> OnLockOn;
    public static event Action OnRelease;

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

    public bool IsThereEnemyScanned()
    {
        Vector3 origin = Camera.main.transform.position;
        RaycastHit[] hits = Physics.SphereCastAll(origin, _scanRange, Camera.main.transform.forward, 50f, _targetLayer);
        if (hits.Length == 0)
        {
            Debug.Log("현재 조준시스템에 포착된 적이 없습니다.");
            return false;
        }

        int closestIndex = GetClosestTargetIndex(hits);
        TargetEnemy = hits[closestIndex].transform.GetComponent<Test_Enemy>().transform;
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
        OnLockOn.Invoke(TargetEnemy);
        TargetGroup.AddMember(TargetEnemy, 1, 0);
        LockOnCam.gameObject.SetActive(true);        
    }

    public void ReleaseTarget()
    {
        OnRelease.Invoke();
        TargetGroup.RemoveMember(TargetEnemy);
        TargetEnemy = null;
        LockOnCam.gameObject.SetActive(false);        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // 레이캐스트 색상 설정
        Gizmos.DrawWireSphere(Camera.main.transform.position, _scanRange); // 구체 형태의 레이캐스트를 그립니다.
    }
}
