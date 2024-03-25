using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.UIElements.VisualElement;

public class EnemyPhaseStarter : MonoBehaviour
{
    // 여러 페이즈 넣어 관리 할 수 있도록 하기
    public PhaseSO[] Phases;    // PhaseSO 넣기

    [SerializeField] private Transform[] muzzle = { }; // 총구 위치

    public bool isShooting;         // false가 되면 EnemyBulletGenerator에서 생성 작업을 멈춘다. 코루틴이 종료됨.
    //public bool[] isTriggerOn;      // 탄막 일괄 이벤트를 위한 트리거를 위해 추후 준비.

    [SerializeField] private List<TestPhaseStarter> onStartPhase = new List<TestPhaseStarter>();

    [System.Serializable]
    private struct TestPhaseStarter
    {
        public int phaseNum;
        public int muzzleNum;
        public bool isOneTime;
    }
    private void Start()
    {
        // muzzle 0 번 인덱스에 gameobject.transform 끼워넣기
        Transform[] newMuzzle = new Transform[muzzle.Length + 1];
        newMuzzle[0] = this.transform;
        for (int i = 0; i < muzzle.Length; i++)
        {
            newMuzzle[i + 1] = muzzle[i];
        }
        muzzle = newMuzzle;

        // TestPhaseStarter 만큼 사용
        foreach (TestPhaseStarter t in onStartPhase)
        {
            isShooting = true; // 테스트 중일 경우, On한 후 사용. 실 사용시에는 testStartList 지우고, StartPhase 직접 사용하는 걸로.
            StartPhase(t.phaseNum, t.muzzleNum, t.isOneTime);
        }
    }

    public void StartPhase(int PhaseNum, int muzzleNum = 0, bool isOneTime = false) // 페이즈 인덱스 및 총구 인덱스 전달받아 실행
    {
        StartCoroutine(Co_StartPhase(PhaseNum, muzzleNum, isOneTime));
    }

    public void StopPhase()
    {
        isShooting = false;
    }

    private IEnumerator Co_StartPhase(int PhaseNum, int muzzleNum, bool isOneTime)
    {
        yield return new WaitForSeconds(Phases[PhaseNum].startTime);

        if (PhaseNum < Phases.Length)
        {
            int _muzzleNum = (muzzle[muzzleNum] != null) ? muzzleNum : 0;
            foreach (var patternHierarchy in Phases[PhaseNum].hierarchicalPatterns)
            {
                EnemyBulletGenerator.instance.StartPatternHierarchy(patternHierarchy, Phases[PhaseNum].cycleTime, gameObject, gameObject, muzzle[_muzzleNum], isOneTime);
            }
        }
    }

}