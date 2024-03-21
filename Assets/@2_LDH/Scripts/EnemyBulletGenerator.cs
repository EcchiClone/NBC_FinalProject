using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;
using static PhaseSO;
using static UnityEngine.UIElements.VisualElement;

public class EnemyBulletGenerator : MonoBehaviour
{
    // 싱글톤
    public static EnemyBulletGenerator instance;
    public IObjectPool<GameObject> Pool { get; set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // 탄막 생성 및 하위 패턴 실행
    public void StartPatternHierarchy(PatternHierarchy hierarchy, float cycleTime, GameObject rootObject, GameObject masterObject, Transform muzzleTransform = null, bool isOneTime = false)
    {
        if (hierarchy.patternSO != null)
        {
            StartCoroutine(Co_ExecutePatternForCycleTime(hierarchy, cycleTime, rootObject, masterObject, muzzleTransform, isOneTime));
        }
    }
    // 코루틴을 실행. CycleTime마다 주어진 패턴구성을 반복
    private IEnumerator Co_ExecutePatternForCycleTime(PatternHierarchy hierarchy, float cycleTime, GameObject rootObject, GameObject masterObject, Transform muzzleTransform = null, bool isOneTime = false)
    {
        while (true)
        {
            StartCoroutine(Co_ExecutePatternHierarchy(hierarchy, hierarchy.cycleTime, rootObject, masterObject, muzzleTransform)); // 여러 패턴 대응, 여기에 넣으면 괜찮을듯? foreach로. 추후 수정.
            if (isOneTime)
                break;
            yield return new WaitForSeconds(cycleTime);
        }
        yield return null;
    }
    // startTime 만큼 기다린 후, 패턴 코루틴을 실행
    private IEnumerator Co_ExecutePatternHierarchy(PatternHierarchy hierarchy, float nextCycleTime, GameObject rootObject, GameObject masterObject, Transform muzzleTransform = null)
    {
        yield return new WaitForSeconds(hierarchy.startTime);
        if (masterObject != null)
            ExecutePattern(hierarchy.patternSO, hierarchy.patternName, hierarchy.subPatterns, nextCycleTime, rootObject, masterObject, muzzleTransform);
    }

    private void ExecutePattern(PatternSO patternSO, string patternName, List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject rootObject, GameObject masterObject, Transform muzzleTransform = null)
    {
        var patternData = patternSO.GetSpawnInfoByPatternName(patternName);
        if (patternData != null)
        {
            StartCoroutine(Co_ExecutePattern(patternData.enemyBulletSettings, subPatterns, nextCycleTime, rootObject, masterObject, muzzleTransform));
        }
    }

    IEnumerator Co_ExecutePattern(EnemyBulletSettings settings, List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject rootGo, GameObject masterGo, Transform muzzleTransform = null)
    {
        GameObject playerGo = GameObject.FindGameObjectWithTag("Player"); // 플레이어 오브젝트 찾기

        for (int setNum = 0; setNum < settings.numOfSet; ++setNum)
        {
            for (int shotNum = 0; shotNum < settings.shotPerSet; ++shotNum)
            {
                // masterObject의 상태 및 컴포넌트의 존재 여부 확인
                if (masterGo == null || !masterGo.activeInHierarchy)
                {
                    yield break; // masterObject가 비활성화되거나 파괴되면 코루틴 중단
                }
                var enemyPhaseStarter = masterGo.GetComponent<EnemyPhaseStarter>();
                if (enemyPhaseStarter != null && enemyPhaseStarter.isShooting == false)
                {
                    yield break; // EnemyPhaseStarter 컴포넌트가 있으면서, isShooting이 false 면 코루틴 중단
                }

                // 1. 초기화 및 위치, 방향 일괄적용
                //List<GameObject> enemyBulletGoList = new List<GameObject>();
                List<LightTransform> enemyBulletTransformList = new List<LightTransform>();

                SetupEnemyBulletGoList(settings, enemyBulletTransformList, playerGo, rootGo, masterGo, muzzleTransform);

                EnqueueEnemyBulletSpawnInfo(settings, enemyBulletTransformList, subPatterns, nextCycleTime, rootGo, masterGo.transform);



                if (settings.shotDelay > 0)
                    yield return new WaitForSeconds(settings.shotDelay); // 각 발사 사이의 지연
            }
            if (settings.setDelay > 0)
                yield return new WaitForSeconds(settings.setDelay); // 세트 사이의 지연
        }

        yield return null;
    }
    public class LightTransform {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
    }

    // 탄막의 생성 및 위치 초기화
    private void SetupEnemyBulletGoList(EnemyBulletSettings settings, List<LightTransform> enemyBulletTransformList, GameObject playerGo, GameObject rootGo, GameObject masterGo, Transform muzzleTransform = null)
    {
        // 1. 패턴을 생성할 기준 위치 설정
        Vector3 pivotPosition;
        if (muzzleTransform != null)
            pivotPosition = muzzleTransform.position;
        else
            pivotPosition = masterGo.transform.position;

        // 1. 패턴을 생성할 기준 방향 벡터 설정
        Vector3 pivotDirection;

        switch (settings.posDirection)
        {
            case PosDirection.Forward:
                pivotDirection = masterGo.transform.forward;
                break;
            case PosDirection.ToPlayer:
                if (playerGo != null)
                {
                    Vector3 directionToPlayer = (playerGo.transform.position - masterGo.transform.position).normalized;
                    pivotDirection = directionToPlayer;
                }
                else pivotDirection = masterGo.transform.forward; // Player 없을 시, Forward를 사용
                break;
            case PosDirection.CompletelyRandom:
                pivotDirection = Random.onUnitSphere;
                break;
            case PosDirection.CustomWorld:
                pivotDirection = settings.customPosDirection.normalized;
                break;
            case PosDirection.CustomLocal:
                pivotDirection = masterGo.transform.TransformDirection(settings.customPosDirection).normalized;
                break;
            default:
                pivotDirection = masterGo.transform.forward; // 예외 발생 시, Forward를 사용
                break;
        }

        // 1. 기준 방향 벡터 오차 주기
        if (settings.spreadA == SpreadType.Spread)
            pivotDirection = GameMathUtils.CalculateSpreadDirection(pivotDirection, settings.maxSpreadAngleA, settings.concentrationA);

        Quaternion rotationToPivotDirection = Quaternion.FromToRotation(Vector3.forward, pivotDirection.normalized);

        // 2. 위치 선정
        switch (settings.enemyBulletShape)
        {
            case(EnemyBulletShape.Linear): // 선형 발사
                for(int i = 0; i < settings.numPerShot; i++)
                {
                    LightTransform enemyBulletTransform = new LightTransform(); // 위치와 방향을 담을 클래스

                    Vector3 initPosition = pivotPosition + pivotDirection * settings.initDistance;

                    enemyBulletTransform.position = initPosition;
                    enemyBulletTransformList.Add(enemyBulletTransform);
                }
                break;

            case (EnemyBulletShape.Sphere):

                foreach (Vector3 spherePoint in GameMathUtils.GenerateSpherePointsTypeA(settings.numPerShot, settings.shotVerticalNum, settings.initDistance))
                {
                    LightTransform enemyBulletTransform = new LightTransform();

                    // 스피어 포인트를 기준 방향으로 회전
                    Vector3 rotatedSpherePoint = rotationToPivotDirection * spherePoint;

                    enemyBulletTransform.position = pivotPosition + rotatedSpherePoint; // 회전된 포인트를 기준 위치에 추가
                    enemyBulletTransformList.Add(enemyBulletTransform);
                }
                break;
            case (EnemyBulletShape.Custom):
                List<Vector3> expandedPoints = new List<Vector3>();
                int pointsCount = settings.customBulletPosList.Length;
                bool isClosedShape = settings.customBulletPosList[0] == settings.customBulletPosList[pointsCount - 1];

                for (int i = 0; i < (isClosedShape ? pointsCount - 1 : pointsCount); i++)
                {
                    Vector3 start = settings.customBulletPosList[i];
                    Vector3 end = settings.customBulletPosList[(i + 1) % pointsCount];

                    expandedPoints.Add(start);

                    for (int j = 1; j <= settings.divisionPointsPerEdge; j++)
                    {
                        float t = (float)j / (settings.divisionPointsPerEdge + 1);
                        Vector3 additionalPoint = Vector3.Lerp(start, end, t);
                        expandedPoints.Add(additionalPoint);
                    }
                }

                // 첫 번째와 마지막 점이 동일한 경우 마지막 점 추가 방지
                if (!isClosedShape)
                {
                    expandedPoints.Add(settings.customBulletPosList[pointsCount - 1]);
                }

                // 기준 방향으로 모든 점 회전 적용
                for (int i = 0; i < expandedPoints.Count; i++)
                {
                    Vector3 rotatedPoint = rotationToPivotDirection * expandedPoints[i];
                    expandedPoints[i] = rotatedPoint;
                }

                foreach (Vector3 point in expandedPoints)
                {
                    LightTransform enemyBulletTransform = new LightTransform();
                    // 기준 위치와 회전된 점을 사용하여 위치 설정
                    enemyBulletTransform.position = pivotPosition + point * settings.initDistance;
                    enemyBulletTransformList.Add(enemyBulletTransform);
                }
                break;

        }

        // 2. 


        // 1>?. 마스터기준 회전
        // 1>?. 평행이동
        // 1>?. 위치에 오차 주기
        // 1>?. 점대칭/면대칭/스케일링과 오차 등등 떠오르는 건 많지만 일단 위의 세 개 구현이 된다면 고려

        // 2. 방향
        switch (settings.initDirectionType)
        {
            case EnemyBulletToDirection.Local: // 직접 지정한 회전치 사용. 전 탄막 일괄 적용
                foreach (LightTransform enemyBulletTransform in enemyBulletTransformList)
                {
                    enemyBulletTransform.rotation = Quaternion.Euler(settings.initCustomDirection);
                }
                break;

            case EnemyBulletToDirection.MasterOut: // 마스터(masterGo)와 반대되는 방향으로
                foreach (LightTransform enemyBulletTransform in enemyBulletTransformList)
                {
                    Vector3 directionMasterToEnemyBullet = (enemyBulletTransform.position - masterGo.transform.position).normalized;
                    enemyBulletTransform.rotation = Quaternion.LookRotation(directionMasterToEnemyBullet);
                }
                break;

            case EnemyBulletToDirection.MasterToPlayer: // 마스터가 플레이어를 바라보는 방향으로 설정

                foreach (LightTransform enemyBulletTransform in enemyBulletTransformList)
                {
                    if (playerGo != null)
                    {
                        Vector3 directionMasterToPlayer = (playerGo.transform.position - masterGo.transform.position).normalized;
                        enemyBulletTransform.rotation = Quaternion.LookRotation(directionMasterToPlayer);
                    }
                    else
                    {
                        // playerGo가 없을 경우, EnemyBulletToDirection.MasterOut 와 같도록
                        Vector3 directionToMaster = (enemyBulletTransform.position - masterGo.transform.position).normalized;
                        enemyBulletTransform.rotation = Quaternion.LookRotation(-directionToMaster);
                    }
                }
                break;

            case EnemyBulletToDirection.MuzzleOut: // 생성위치와 반대되는 방향으로
                foreach (LightTransform enemyBulletTransform in enemyBulletTransformList)
                {
                    Vector3 directionMuzzleToEnemyBullet;
                    if (muzzleTransform != null)
                        directionMuzzleToEnemyBullet = (enemyBulletTransform.position - muzzleTransform.position).normalized;
                    else
                        directionMuzzleToEnemyBullet = (enemyBulletTransform.position - masterGo.transform.position).normalized;
                    if(directionMuzzleToEnemyBullet!=Vector3.zero)
                        enemyBulletTransform.rotation = Quaternion.LookRotation(directionMuzzleToEnemyBullet);
                }
                break;
            case EnemyBulletToDirection.ToPlayer: // 탄막이 플레이어를 바라보도록
                foreach (LightTransform enemyBulletTransform in enemyBulletTransformList)
                {
                    if (playerGo != null)
                    {
                        Vector3 directionToPlayer = (playerGo.transform.position - enemyBulletTransform.position).normalized;
                        enemyBulletTransform.rotation = Quaternion.LookRotation(directionToPlayer);
                    }
                }
                break;

            case EnemyBulletToDirection.CompletelyRandom: // 모든 방향으로 랜덤
                foreach (LightTransform enemyBulletTransform in enemyBulletTransformList)
                {
                    enemyBulletTransform.rotation = Random.rotation;
                }
                break;
        }

        // 2. 방향에 일괄 오차 주기

        if (settings.spreadB == SpreadType.Spread)
        {
            foreach (LightTransform enemyBulletTransform in enemyBulletTransformList)
            {
                Vector3 direction = enemyBulletTransform.rotation * Vector3.forward; // Q to V3
                Vector3 newDirection = GameMathUtils.CalculateSpreadDirection(direction, settings.maxSpreadAngleB, settings.concentrationB);
                enemyBulletTransform.rotation = Quaternion.LookRotation(newDirection); // V3 to Q
            }
        }
        // 1>?. 에서 행했던 것 또 넣어도 될 듯 함

    }

    // 배치 처리를 위한 큐 구조체 정의
    [System.Serializable]
    public class EnemyBulletSpawnInfo
    {
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
        public EnemyBulletParameters parameters;
        public float nextCycleTime;
        public GameObject rootGo;
        public Transform masterTf;
        public List<PatternHierarchy> subPatterns;

        public EnemyBulletSpawnInfo(string prefabName, Vector3 position, Quaternion rotation, EnemyBulletParameters parameters, float nextCycleTime, GameObject rootGo, Transform masterTf, List<PatternHierarchy> subPatterns)
        {
            this.prefabName = prefabName;
            this.position = position;
            this.rotation = rotation;
            this.parameters = parameters;
            this.nextCycleTime = nextCycleTime;
            this.rootGo = rootGo;
            this.masterTf = masterTf;
            this.subPatterns = subPatterns;
        }
    }
    // 탄막 생성 정보를 담은 큐
    private Queue<EnemyBulletSpawnInfo> spawnQueue = new Queue<EnemyBulletSpawnInfo>();
    public int rentalBatchSize = 200;

    private void EnqueueEnemyBulletSpawnInfo(EnemyBulletSettings settings, List<LightTransform> enemyBulletTransformList, List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject rootGo, Transform masterTf)
    {
        foreach (LightTransform enemyBulletTransform in enemyBulletTransformList)
        {
            EnemyBulletParameters parameters = EnemyBulletParameters.FromSettings(settings);
            EnemyBulletSpawnInfo spawnInfo = new EnemyBulletSpawnInfo(
                settings.enemyBulletPrefab.name,
                enemyBulletTransform.position,
                enemyBulletTransform.rotation,
                parameters,
                nextCycleTime,
                rootGo,
                masterTf,
                subPatterns);

            spawnQueue.Enqueue(spawnInfo);
        }
    }
    //큐로 관리하는 탄막 생성 함수
    private void Update()
    {
        ProcessSpawnQueue();
    }

    private void ProcessSpawnQueue()
    {
        int spawnCountThisFrame = 0;
        while (spawnQueue.Count > 0 && spawnCountThisFrame < rentalBatchSize)
        {
            //Debug.Log($"{spawnQueue.Count}, {spawnCountThisFrame}");
            EnemyBulletSpawnInfo spawnInfo = spawnQueue.Dequeue();

            //Debug.Log(spawnInfo.prefabName);
            GameObject enemyBulletGo = EnemyBulletPoolManager.instance.GetGo(spawnInfo.prefabName);
            //Debug.Log(enemyBulletGo.gameObject.ToString());

            enemyBulletGo.transform.position = spawnInfo.position;
            enemyBulletGo.transform.rotation = spawnInfo.rotation;

            EnemyBulletController enemyBulletController = enemyBulletGo.GetComponent<EnemyBulletController>();
            if (enemyBulletController != null)
            {
                enemyBulletController.Initialize(spawnInfo.parameters, spawnInfo.nextCycleTime, spawnInfo.subPatterns, spawnInfo.rootGo, spawnInfo.masterTf);
            }
            else
            {
                // enemyBulletController가 null일 경우, 해당 오브젝트는 다른 몬스터일 가능성이 큼.
                // Pool을 사용하지 않는 방향으로, 그냥 Instantiate만 사용하면 될 것으로 예상됨.
            }

            spawnCountThisFrame++;
        }
    }
    //GameObject enemyBulletGo = EnemyBulletPoolManager.instance.GetGo(settings.enemyBulletPrefab.name);
    /*
                // 2. 그 외 세팅 파라미터 등 하위 탄막의 EnemyBulletController에 전달
                foreach (GameObject enemyBulletGo in enemyBulletGoList)
                {
                    EnemyBulletController enemyBulletController = enemyBulletGo.GetComponent<EnemyBulletController>();
                    if (enemyBulletController != null)
                    {
                        EnemyBulletParameters parameters = EnemyBulletParameters.FromSettings(settings);
                        enemyBulletController.Initialize(parameters, nextCycleTime, subPatterns);
                    }
                    // else{} // enemyBulletController가 null일 경우, 해당 오브젝트는 다른 몬스터일 가능성이 큼.
                    //           풀을 사용하지 않는 방향으로, 그냥 Instantiate만 사용하면 될 것으로 예상됨.
                }
    */
}
