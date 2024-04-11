using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.UIElements.VisualElement;
using static PhaseSO;

public class EnemyPhaseStarter : MonoBehaviour
{
    // 여러 페이즈 넣어 관리 할 수 있도록 하기
    public PhaseSO[] Phases;    // PhaseSO 넣기
    public PatternSO[] Patterns;    // 단일 패턴 사용만을 위한 Patterns 넣기

    [SerializeField] private Transform[] muzzle = { }; // 총구 위치

    public bool isShooting = true;         // false가 되면 EnemyBulletGenerator에서 생성 작업을 멈춘다. 코루틴이 종료됨.
    //public bool[] isTriggerOn;      // 탄막 일괄 이벤트를 위한 트리거를 위해 추후 준비.

    [SerializeField] private List<TestPhaseStarter> onStartPhase = new List<TestPhaseStarter>();

    [System.Serializable]
    private struct TestPhaseStarter
    {
        public int phaseNum;
        public int muzzleNum;
        public bool isOneTime;
    }
    private void OnEnable()
    {
        // muzzle 0 번 인덱스에 gameobject.transform 끼워넣기
        Transform[] newMuzzle = new Transform[muzzle.Length + 1];
        newMuzzle[0] = this.transform;
        for (int i = 0; i < muzzle.Length; i++)
        {
            newMuzzle[i + 1] = muzzle[i];
        }
        muzzle = newMuzzle;

        isShooting = true;
        
        foreach (TestPhaseStarter t in onStartPhase) // TestPhaseStarter 만큼 사용
        {
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
        yield return Util.GetWaitSeconds(Phases[PhaseNum].startTime);

        if (PhaseNum < Phases.Length)
        {
            int _muzzleNum = (muzzle[muzzleNum] != null) ? muzzleNum : 0;

            // 각 패턴 계층을 위한 BulletGenerationSettings 인스턴스 생성
            foreach (var patternHierarchy in Phases[PhaseNum].hierarchicalPatterns)
            {
                var genSettings = new BulletGenerationSettings
                {
                    muzzleTransform = muzzle[_muzzleNum],
                    rootObject = gameObject,
                    masterObject = gameObject,
                    cycleTime = Phases[PhaseNum].cycleTime,
                    isOneTime = isOneTime,
                    patternHierarchy = patternHierarchy
                };
                EnemyBulletGenerator.instance.StartPatternHierarchy(genSettings);
            }
        }
    }

    public void StartPattern(int PatternNum, float cycleTime, int muzzleNum = 0, bool isOneTime = false)
    {
        if (PatternNum < Patterns.Length)
        {
            int _muzzleNum = (muzzle[muzzleNum] != null) ? muzzleNum : 0;

            var genSettings = new BulletGenerationSettings
            {
                muzzleTransform = muzzle[_muzzleNum],
                rootObject = gameObject,
                masterObject = gameObject,
                cycleTime = cycleTime,
                isOneTime = isOneTime,
                patternHierarchy = new PatternHierarchy
                {
                    patternSO = Patterns[PatternNum],
                    patternName = Patterns[PatternNum].name,
                    genCondition = GenCondition.Timer,
                    startTime = 0f,
                    cycleTime = cycleTime,
                    subPatterns = new List<PatternHierarchy>()
                }
            };
            EnemyBulletGenerator.instance.StartPatternHierarchy(genSettings);
        }
    }
}