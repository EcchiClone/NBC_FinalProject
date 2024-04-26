using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static PhaseSO;

public class EnemyBulletPatternStarter : MonoBehaviour
{
    public delegate void BulletStoper();
    public event BulletStoper onStopBullet;
    public void StopBullet()
    {
        Debug.Log("이전 패턴 멈추기");
        onStopBullet?.Invoke();
    }

    [System.Serializable]
    private struct TestPhaseStarter
    {
        public int phaseNum;
        public int muzzleNum;
        public bool isOneTime;
    }
    [System.Serializable]
    private struct TestPatternStarter
    {
        public int patternNum;
        public int muzzleNum;
        public int cycleTime;
        public bool isOneTime;
    }
    [System.Serializable]
    public struct PatternEntry
    {
        public PatternSO patternSO;
        public string patternName;
    }

    public PhaseSO[] Phases;    // PhaseSO 목록을 인덱스로 관리
    public PatternEntry[] Patterns;    // (PatternSO,이름) 목록을 인덱스로 관리

    public bool isShooting = true;

    [SerializeField] private Transform[] muzzle = { }; // 총구 위치

    [SerializeField] private List<TestPhaseStarter> onStartPhase = new List<TestPhaseStarter>();
    [SerializeField] private List<TestPatternStarter> onStartPattern = new List<TestPatternStarter>();

    private List<IEnumerator> activeCoroutine = new List<IEnumerator>();

    private void Awake()
    {
        // muzzle 0 번 인덱스에 gameobject.transform 끼워넣기
        Transform[] newMuzzle = new Transform[muzzle.Length + 1];
        newMuzzle[0] = this.transform;
        for (int i = 0; i < muzzle.Length; i++)
        {
            newMuzzle[i + 1] = muzzle[i];
        }
        muzzle = newMuzzle;
    }

    private void OnEnable()
    {
        onStopBullet += EnemyBulletGenerator.instance.StopAllCoroutinesPattern;

        foreach (TestPhaseStarter t in onStartPhase)
        {
            StartPhase(t.phaseNum, t.muzzleNum, t.isOneTime);
        }
        foreach (TestPatternStarter t in onStartPattern)
        {
            StartPattern(t.patternNum, t.cycleTime, t.muzzleNum, t.isOneTime);
        }
    }
    private void OnDisable()
    {
        onStopBullet -= EnemyBulletGenerator.instance.StopAllCoroutinesPattern;
    }

    public void StartPhase(int PhaseNum, int muzzleNum = 0, bool isOneTime = false) // 페이즈 인덱스 및 총구 인덱스 전달받아 실행
    {
        IEnumerator myCoroutine = Co_StartPhase(PhaseNum, muzzleNum, isOneTime);
        StartCoroutine(myCoroutine);
        activeCoroutine.Add(myCoroutine);
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
                EnemyBulletGenerator.instance.StartPatternHierarchy(genSettings, onStopBullet);
            }
        }
    }
    public void StartPattern(int PatternNum, float cycleTime, int muzzleNum = 0, bool isOneTime = false) // 페이즈 인덱스 및 총구 인덱스 전달받아 실행
    {
        IEnumerator myCoroutine = Co_StartPattern(PatternNum, cycleTime, muzzleNum, isOneTime);
        StartCoroutine(myCoroutine);
        activeCoroutine.Add(myCoroutine);
    }
    
    public void StopPattern()
    {
        foreach (IEnumerator co in activeCoroutine)
        {
            if (co != null)
            {
                StopAllCoroutines();
            }
        }
        activeCoroutine.Clear();
    }

    public IEnumerator Co_StartPattern(int PatternNum, float cycleTime, int muzzleNum = 0, bool isOneTime = false)
    {
        
        yield return Util.GetWaitSeconds(0.01f);
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
                    patternSO = Patterns[PatternNum].patternSO,
                    patternName = Patterns[PatternNum].patternName,
                    genCondition = GenCondition.Timer,
                    startTime = 0f,
                    cycleTime = cycleTime,
                    subPatterns = new List<PatternHierarchy>()
                }
            };
            EnemyBulletGenerator.instance.StartPatternHierarchy(genSettings, onStopBullet);
        }
    }
}