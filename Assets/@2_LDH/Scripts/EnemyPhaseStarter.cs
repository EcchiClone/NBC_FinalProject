using UnityEngine;
using System.Collections;

public class EnemyPhaseStarter : MonoBehaviour
{
    // 여러 페이즈 넣어 관리 할 수 있도록 하기
    public PhaseSO[] Phases;    // PhaseSO 넣기

    [SerializeField] private Transform[] muzzle; // 총구 위치

    public bool isShooting;         // false가 되면 EnemyBulletGenerator에서 생성 작업을 멈춘다. 코루틴이 종료됨.
    //public bool[] isTriggerOn;      // 탄막 일괄 이벤트를 위한 트리거를 위해 추후 준비.
    private void Start()
    {
        // for test;
        isShooting = true;
        for(int i = 0; i < Phases.Length; i++)
        {
            startPhase(i,0);
        }
    }

    public void startPhase(int PhaseNum, int muzzleNum)
    {
        // Todo : 총구 트랜스폼도 전달해야함.
        foreach (var patternHierarchy in Phases[PhaseNum].hierarchicalPatterns)
        {
            EnemyBulletGenerator.instance.StartPatternHierarchy(patternHierarchy, Phases[PhaseNum].cycleTime, gameObject);
        }
    }

    public void stopPhase()
    {
        isShooting = false;
    }

}