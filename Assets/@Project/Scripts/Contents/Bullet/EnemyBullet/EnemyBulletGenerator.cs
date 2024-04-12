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
    public static EnemyBulletGenerator instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // 탄막 생성 및 하위 패턴 실행
    public void StartPatternHierarchy(BulletGenerationSettings genSettings) // PatternHierarchy hierarchy, float cycleTime, GameObject rootObject, GameObject masterObject, Transform muzzleTransform = null, bool isOneTime = false
    {
        if (genSettings.patternHierarchy.patternSO != null)
        {
            StartCoroutine(Co_ExecutePatternForCycleTime(genSettings));
        }
    }

    // 코루틴을 실행. CycleTime마다 주어진 패턴구성을 반복
    private IEnumerator Co_ExecutePatternForCycleTime(BulletGenerationSettings genSettings) // PatternHierarchy hierarchy, float cycleTime, GameObject rootObject, GameObject masterObject, Transform muzzleTransform = null, bool isOneTime = false
    {
        while (true)
        {
            StartCoroutine(Co_ExecutePatternHierarchy(genSettings)); // 여러 패턴 대응 // hierarchy, hierarchy.cycleTime, rootObject, masterObject, muzzleTransform
            if (genSettings.isOneTime)
                break;
            //yield return new WaitForSeconds(genSettings.cycleTime);
            yield return Util.GetWaitSeconds(genSettings.cycleTime);
        }
        yield return null;
    }

    // startTime 만큼 기다린 후, 패턴 코루틴을 실행
    private IEnumerator Co_ExecutePatternHierarchy(BulletGenerationSettings genSettings) //PatternHierarchy hierarchy, float nextCycleTime, GameObject rootObject, GameObject masterObject, Transform muzzleTransform = null
    {
        //yield return new WaitForSeconds(genSettings.patternHierarchy.startTime);
        yield return Util.GetWaitSeconds(genSettings.patternHierarchy.startTime);
        if (genSettings.masterObject != null)
            ExecutePattern(genSettings); // hierarchy.patternSO, hierarchy.patternName, hierarchy.subPatterns, nextCycleTime, rootObject, masterObject, muzzleTransform
    }

    public void ExecutePattern(BulletGenerationSettings genSettings) // PatternSO patternSO, string patternName, List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject rootObject, GameObject masterObject, Transform muzzleTransform = null
    {
        //var patternData = patternSO.GetSpawnInfoByPatternName(patternName);
        //if (patternData != null)
        if (genSettings.patternHierarchy.patternSO.GetSpawnInfoByPatternName(genSettings.patternHierarchy.patternName) != null)
        {
            StartCoroutine(Co_ExecutePattern(genSettings)); // patternData.enemyBulletSettings, subPatterns, nextCycleTime, rootObject, masterObject, muzzleTransform
        }
    }

    IEnumerator Co_ExecutePattern(BulletGenerationSettings genSettings) // EnemyBulletSettings settings, List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject rootGo, GameObject masterGo, Transform muzzleTransform = null
    {
        EnemyBulletSettings settings = genSettings.patternHierarchy.patternSO.GetSpawnInfoByPatternName(genSettings.patternHierarchy.patternName).enemyBulletSettings;

        Vector3 playerPos = BulletMathUtils.GetPlayerPos(true);

        for (int setNum = 0; setNum < settings.numOfSet; ++setNum)
        {
            for (int shotNum = 0; shotNum < settings.shotPerSet; ++shotNum)
            {
                // masterObject의 상태 및 컴포넌트의 존재 여부 확인
                if (genSettings.masterObject == null || !genSettings.masterObject.activeInHierarchy)
                {
                    yield break; // masterObject가 비활성화되거나 파괴되면 코루틴 중단
                }
                var enemyPhaseStarter = genSettings.masterObject.GetComponent<EnemyBulletPatternStarter>();
                if (enemyPhaseStarter != null && enemyPhaseStarter.isShooting == false)
                {
                    yield break; // EnemyBulletPatternStarter 컴포넌트가 있으면서, isShooting이 false 면 코루틴 중단
                }

                // 1. 초기화 및 위치, 방향 일괄적용
                //List<GameObject> enemyBulletGoList = new List<GameObject>();
                List<(Vector3 Position, Quaternion Rotation)> enemyBulletTransformList = new List<(Vector3 Position, Quaternion Rotation)>();

                //SetupEnemyBulletGoList(settings, enemyBulletTransformList, playerTF, genSettings); // settings, enemyBulletTransformList, playerGo, rootGo, masterGo, muzzleTransform
                SetupEnemyBulletGoList(settings, enemyBulletTransformList, playerPos, genSettings); // settings, enemyBulletTransformList, playerGo, rootGo, masterGo, muzzleTransform
                // genSettings.rootObject, genSettings.masterObject, genSettings.muzzleTransform

                EnqueueEnemyBulletSpawnInfo(settings, enemyBulletTransformList, genSettings); // settings, enemyBulletTransformList, subPatterns, nextCycleTime, rootGo, masterGo.transform
                // genSettings.patternHierarchy.subPatterns, genSettings.patternHierarchy.cycleTime, genSettings.rootObject, genSettings.masterObject.transform


                if (settings.shotDelay > 0)
                    //yield return new WaitForSeconds(); // 각 발사 사이의 지연
                    yield return Util.GetWaitSeconds(settings.shotDelay);
            }
            if (settings.setDelay > 0)
                //yield return new WaitForSeconds(settings.setDelay); // 세트 사이의 지연
                yield return Util.GetWaitSeconds(settings.setDelay);
        }

        yield return null;
    }

    // 탄막의 생성 및 위치 초기화
    //private void SetupEnemyBulletGoList(EnemyBulletSettings settings, List<LightTransform> enemyBulletTransformList, Transform playerGo, BulletGenerationSettings genSettings) //GameObject rootGo, GameObject masterGo, Transform muzzleTransform = null
    private void SetupEnemyBulletGoList(EnemyBulletSettings settings, List<(Vector3 Position, Quaternion Rotation)> enemyBulletTransformList, Vector3 playerGo, BulletGenerationSettings genSettings) //GameObject rootGo, GameObject masterGo, Transform muzzleTransform = null
    {
        var muzzleTransform = genSettings.muzzleTransform;
        var rootGo = genSettings.rootObject;
        var masterGo = genSettings.masterObject;

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
                pivotDirection = muzzleTransform.forward;
                break;
            case PosDirection.ToPlayer:
                if (playerGo != null)
                {
                    Vector3 directionToPlayer = (playerGo - masterGo.transform.position).normalized;
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
            pivotDirection = BulletMathUtils.CalculateSpreadDirection(pivotDirection, settings.maxSpreadAngleA, settings.concentrationA);

        Quaternion rotationToPivotDirection = Quaternion.FromToRotation(Vector3.forward, pivotDirection.normalized);

        // 2. 위치 선정
        switch (settings.enemyBulletShape)
        {
            case(EnemyBulletShape.Linear): // 선형 발사
                for(int i = 0; i < settings.numPerShot; i++)
                    enemyBulletTransformList.Add((pivotPosition + pivotDirection * settings.initDistance, Quaternion.identity));
                break;

            case (EnemyBulletShape.Sphere):
                foreach (Vector3 spherePoint in BulletMathUtils.GenerateSpherePointsTypeA(settings.numPerShot, settings.shotVerticalNum, settings.initDistance))
                    enemyBulletTransformList.Add((pivotPosition + rotationToPivotDirection * spherePoint, Quaternion.identity));
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
                        expandedPoints.Add(Vector3.Lerp(start, end, t));
                    }
                }
                // 첫 번째와 마지막 점이 동일한 경우 마지막 점 추가 방지
                if (!isClosedShape)
                    expandedPoints.Add(settings.customBulletPosList[pointsCount - 1]);
                // 기준 방향으로 모든 점 회전 적용
                for (int i = 0; i < expandedPoints.Count; i++)
                    expandedPoints[i] = rotationToPivotDirection * expandedPoints[i];
                foreach (Vector3 point in expandedPoints)
                    enemyBulletTransformList.Add((pivotPosition + point * settings.initDistance, Quaternion.identity));
                break;

        }
        // Todo
        // 1-Add. 마스터기준 회전
        // 1-Add. 평행이동
        // 1-Add. 위치에 오차 주기
        // 1-Add. 점대칭/면대칭/스케일링과 오차 등등 떠오르는 건 많지만 일단 위의 세 개 구현이 된다면 고려

        // 2. 방향
        switch (settings.initDirectionType)
        {
            case EnemyBulletToDirection.Local: // 직접 지정한 회전치 사용. 전 탄막 일괄 적용
                for (int i = 0; i < enemyBulletTransformList.Count; i++)
                    enemyBulletTransformList[i] = (enemyBulletTransformList[i].Position, Quaternion.Euler(settings.initCustomDirection));
                break;

            case EnemyBulletToDirection.MasterOut: // 마스터(masterGo)와 반대되는 방향으로
                for (int i = 0; i < enemyBulletTransformList.Count; i++)
                {
                    Vector3 directionMasterToEnemyBullet = (enemyBulletTransformList[i].Position - masterGo.transform.position).normalized;
                    enemyBulletTransformList[i] = (enemyBulletTransformList[i].Position, Quaternion.LookRotation(directionMasterToEnemyBullet));
                }
                break;

            case EnemyBulletToDirection.MasterToPlayer: // 마스터가 플레이어를 바라보는 방향으로 설정
                for (int i = 0; i < enemyBulletTransformList.Count; i++)
                {
                    Quaternion newRotation;
                    if (playerGo != null)
                    {
                        Vector3 directionMasterToPlayer = (playerGo - masterGo.transform.position).normalized;
                        newRotation = Quaternion.LookRotation(directionMasterToPlayer);
                    }
                    else
                    {
                        // playerGo가 없을 경우, EnemyBulletToDirection.MasterOut와 같도록
                        Vector3 directionToMaster = (enemyBulletTransformList[i].Position - masterGo.transform.position).normalized;
                        newRotation = Quaternion.LookRotation(-directionToMaster);
                    }
                    enemyBulletTransformList[i] = (enemyBulletTransformList[i].Position, newRotation);
                }
                break;

            case EnemyBulletToDirection.MuzzleOut: // 생성위치와 반대되는 방향으로
                for (int i = 0; i < enemyBulletTransformList.Count; i++)
                {
                    Vector3 directionMuzzleToEnemyBullet;
                    if (muzzleTransform != null)
                        directionMuzzleToEnemyBullet = (enemyBulletTransformList[i].Position - muzzleTransform.position).normalized;
                    else
                        directionMuzzleToEnemyBullet = (enemyBulletTransformList[i].Position - masterGo.transform.position).normalized;

                    if (directionMuzzleToEnemyBullet != Vector3.zero)
                        enemyBulletTransformList[i] = (enemyBulletTransformList[i].Position, Quaternion.LookRotation(directionMuzzleToEnemyBullet));
                }
                break;

            case EnemyBulletToDirection.MuzzleToPlayer: // 생성위치에서 플레이어 방향으로
                for (int i = 0; i < enemyBulletTransformList.Count; i++)
                {
                    Quaternion newRotation;
                    if (playerGo != null)
                    {
                        Vector3 directionMuzzleToPlayer = (playerGo - muzzleTransform.position).normalized;
                        newRotation = Quaternion.LookRotation(directionMuzzleToPlayer);
                    }
                    else
                    {
                        // playerGo가 없을 경우, EnemyBulletToDirection.MuzzleOut와 같도록
                        Vector3 directionToMuzzle = (enemyBulletTransformList[i].Position - muzzleTransform.position).normalized;
                        newRotation = Quaternion.LookRotation(-directionToMuzzle);
                    }
                    enemyBulletTransformList[i] = (enemyBulletTransformList[i].Position, newRotation);
                }
                break;

            case EnemyBulletToDirection.ToPlayer: // 탄막이 플레이어를 바라보도록
                for (int i = 0; i < enemyBulletTransformList.Count; i++)
                {
                    if (playerGo != null)
                    {
                        Vector3 directionToPlayer = (playerGo - enemyBulletTransformList[i].Position).normalized;
                        enemyBulletTransformList[i] = (enemyBulletTransformList[i].Position, Quaternion.LookRotation(directionToPlayer));
                    }
                }
                break;

            case EnemyBulletToDirection.CompletelyRandom: // 모든 방향으로 랜덤
                for (int i = 0; i < enemyBulletTransformList.Count; i++)
                    enemyBulletTransformList[i] = (enemyBulletTransformList[i].Position, Random.rotation);
                break;

        }

        // 2. 방향에 일괄 오차 주기
        if (settings.spreadB == SpreadType.Spread)
        {
            for (int i = 0; i < enemyBulletTransformList.Count; i++)
            {
                Vector3 direction = enemyBulletTransformList[i].Rotation * Vector3.forward; // Quaternion에서 Vector3 방향으로 변환
                Vector3 newDirection = BulletMathUtils.CalculateSpreadDirection(direction, settings.maxSpreadAngleB, settings.concentrationB);
                enemyBulletTransformList[i] = (enemyBulletTransformList[i].Position, Quaternion.LookRotation(newDirection)); // 새 방향으로 Quaternion 업데이트
            }
        }

        // Todo
        // 2-Add. '1-Add'에서 행했던 것 또 넣어도 될 듯 함
    }

    // 배치 처리를 위한 큐 구조체 정의
    [System.Serializable]
    public struct EnemyBulletSpawnInfo
    {
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
        public EnemyBulletSettings settings;
        public float nextCycleTime;
        public GameObject rootGo;
        public Transform masterTf;
        public List<PatternHierarchy> subPatterns;
    }
    // 탄막 생성 정보를 담은 큐
    private Queue<EnemyBulletSpawnInfo> spawnQueue = new Queue<EnemyBulletSpawnInfo>();
    public int rentalBatchSize = 200;

    private void EnqueueEnemyBulletSpawnInfo(EnemyBulletSettings settings, List<(Vector3 Position, Quaternion Rotation)> enemyBulletTransformList, BulletGenerationSettings genSettings) // List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject rootGo, Transform masterTf
    {
        var subPatterns = genSettings.patternHierarchy.subPatterns;
        var nextCycleTime = genSettings.patternHierarchy.cycleTime;
        var rootGo = genSettings.rootObject;
        var masterGo = genSettings.masterObject;

        for (int i = 0; i < enemyBulletTransformList.Count; i++)
        {
            var (position, rotation) = enemyBulletTransformList[i];

            EnemyBulletParameters parameters = EnemyBulletParameters.FromSettings(settings);
            EnemyBulletSpawnInfo spawnInfo = new EnemyBulletSpawnInfo
            {
                prefabName = settings.enemyBulletPrefab.name,
                position = position,
                rotation = rotation,
                settings = settings,
                nextCycleTime = nextCycleTime,
                rootGo = rootGo,
                masterTf = masterGo.transform,
                subPatterns = subPatterns
            };
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
            EnemyBulletSpawnInfo spawnInfo = spawnQueue.Dequeue();

            GameObject enemyBulletGo = ObjectPooler.SpawnFromPool(spawnInfo.prefabName, Vector3.zero);

            enemyBulletGo.transform.position = spawnInfo.position;
            enemyBulletGo.transform.rotation = spawnInfo.rotation;

            EnemyBulletController enemyBulletController = enemyBulletGo.GetComponent<EnemyBulletController>();
            if (enemyBulletController != null)
            {
                enemyBulletController.Initialize(spawnInfo.settings, spawnInfo.nextCycleTime, spawnInfo.subPatterns, spawnInfo.rootGo, spawnInfo.masterTf);
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
