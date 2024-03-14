using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static PhaseSO;
using static UnityEngine.UIElements.VisualElement;

public class DanmakuGenerator : MonoBehaviour
{
    // 싱글톤
    public static DanmakuGenerator instance;
    public IObjectPool<GameObject> Pool { get; set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // 탄막 생성 및 하위 패턴 실행
    public void StartPatternHierarchy(PatternHierarchy hierarchy, float cycleTime, GameObject masterObject)
    {
        if (hierarchy.patternSO != null)
        {
            StartCoroutine(Co_ExecutePatternForCycleTime(hierarchy, cycleTime, masterObject));
        }
    }
    // 코루틴을 실행. CycleTime마다 주어진 패턴구성을 반복
    private IEnumerator Co_ExecutePatternForCycleTime(PatternHierarchy hierarchy, float cycleTime, GameObject masterObject)
    {
        while (true)
        {
            StartCoroutine(Co_ExecutePatternHierarchy(hierarchy, hierarchy.cycleTime, masterObject)); // 여러 패턴 대응, 여기에 넣으면 괜찮을듯? foreach로. 추후 수정.
            yield return new WaitForSeconds(cycleTime);
        }
    }
    // startTime 만큼 기다린 후, 패턴 코루틴을 실행
    private IEnumerator Co_ExecutePatternHierarchy(PatternHierarchy hierarchy, float nextCycleTime, GameObject masterObject)
    {
        yield return new WaitForSeconds(hierarchy.startTime);
        if (masterObject != null)
            ExecutePattern(hierarchy.patternSO, hierarchy.patternName, hierarchy.subPatterns, nextCycleTime, masterObject);
    }

    private void ExecutePattern(PatternSO patternSO, string patternName, List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject masterObject)
    {
        var patternData = patternSO.GetSpawnInfoByPatternName(patternName);
        if (patternData != null)
        {
            StartCoroutine(Co_ExecutePattern(patternData.danmakuSettings, subPatterns, nextCycleTime, masterObject));
        }
    }

    IEnumerator Co_ExecutePattern(DanmakuSettings settings, List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject masterGo)
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
                var enemyDanmakuTrigger = masterGo.GetComponent<EnemyDanmakuTrigger>();
                if (enemyDanmakuTrigger != null && enemyDanmakuTrigger.isShooting == false)
                {
                    yield break; // EnemyDanmakuTrigger 컴포넌트가 있으면서, isShooting이 false 면 코루틴 중단
                }

                // 1. 초기화 및 위치, 방향 일괄적용
                List<GameObject> danmakuGoList = new List<GameObject>();
                SetupDanmakuGoList(settings, danmakuGoList, playerGo, masterGo);

                // 2. 그 외 세팅 파라미터 등 하위 탄막의 DanmakuController에 전달
                foreach (GameObject danmakuGo in danmakuGoList)
                {
                    DanmakuController danmakuController = danmakuGo.GetComponent<DanmakuController>();
                    if (danmakuController != null)
                    {
                        DanmakuParameters parameters = DanmakuParameters.FromSettings(settings);
                        danmakuController.Initialize(parameters, nextCycleTime, subPatterns);
                    }
                    // else{} // danmakuController가 null일 경우, 해당 오브젝트는 다른 몬스터일 가능성이 큼.
                    //           풀을 사용하지 않는 방향으로, 그냥 Instantiate만 사용하면 될 것으로 예상됨.
                }

                if (settings.shotDelay > 0)
                    yield return new WaitForSeconds(settings.shotDelay); // 각 발사 사이의 지연
            }
            if (settings.setDelay > 0)
                yield return new WaitForSeconds(settings.setDelay); // 세트 사이의 지연
        }

        yield return null;
    }

    // 탄막의 생성 및 위치 초기화
    private void SetupDanmakuGoList(DanmakuSettings settings, List<GameObject> danmakuGoList, GameObject playerGo, GameObject masterGo)
    {
        Vector3 pivotPosition = masterGo.transform.position; // 마스터의 위치를 기본값으로

        // 1. 위치
        switch (settings.danmakuShape)
        {
            case(DanmakuShape.Linear):
                // 몇 개의 탄막씩 발사할지 : settings.numPerShot
                // 생성(현재 1개만 대응)
                for(int i = 0; i < 1; i++)
                {
                    GameObject danmakuGo = DanmakuPoolManager.instance.GetGo(settings.danmakuPrefab.name);
                    Vector3 initPosition = pivotPosition;
                    // 위치 설정
                    switch (settings.posDirection)
                    {
                        case PosDirection.World:
                            initPosition += settings.customPosDirection * settings.initDistance;
                            break;
                        case PosDirection.Look:
                            initPosition += masterGo.transform.forward * settings.initDistance;
                            break;
                        case PosDirection.LookPlayer:
                            if (playerGo != null)
                            {
                                Vector3 directionToPlayer = (playerGo.transform.position - transform.position).normalized;
                                initPosition += directionToPlayer * settings.initDistance;
                            }
                            break;
                        case PosDirection.CompletelyRandom:
                            initPosition += Random.onUnitSphere * settings.initDistance;
                            break;
                    }
                    danmakuGo.transform.position = initPosition;
                    danmakuGoList.Add(danmakuGo);
                }
                break;

            case (DanmakuShape.Sphere):
                // 한 층의 둘레에 생기게 할 탄막 수 : settings.numPerShot
                // 몇 층으로 쌓을지에 대한 수 : settings.shotVerticalNum

                foreach (Vector3 newGoPos in MathUtils.GenerateSpherePointsTypeA(settings.numPerShot, settings.shotVerticalNum, settings.initDistance))
                {
                    GameObject danmakuGo = DanmakuPoolManager.instance.GetGo(settings.danmakuPrefab.name);
                    danmakuGo.transform.position = pivotPosition + newGoPos;
                    danmakuGoList.Add(danmakuGo);
                }
                break;

        }
        // 1>?. 마스터기준 회전
        // 1>?. 평행이동
        // 1>?. 위치에 오차 주기
        // 1>?. 점대칭/면대칭/스케일링과 오차 등등 떠오르는 건 많지만 일단 위의 세 개 구현이 된다면 고려

        // 2. 방향
        switch (settings.initDirectionType)
        {
            case DanmakuToDirection.World: // 직접 지정한 회전치 사용. 전 탄막 일괄 적용
                foreach (GameObject danmakuGo in danmakuGoList)
                {
                    danmakuGo.transform.rotation = Quaternion.Euler(settings.initCustomDirection);
                }
                break;

            case DanmakuToDirection.Outer: // 마스터(masterGo)와 반대되는 방향으로
                foreach (GameObject danmakuGo in danmakuGoList)
                {
                    Vector3 directionMasterToDanmaku = (danmakuGo.transform.position - masterGo.transform.position).normalized;
                    danmakuGo.transform.rotation = Quaternion.LookRotation(directionMasterToDanmaku);
                }
                break;

            case DanmakuToDirection.MasterLookPlayer: // 마스터가 플레이어를 바라보는 방향으로 설정

                foreach (GameObject danmakuGo in danmakuGoList)
                {
                    if (playerGo != null)
                    {
                        Vector3 directionMasterToPlayer = (playerGo.transform.position - masterGo.transform.position).normalized;
                        danmakuGo.transform.rotation = Quaternion.LookRotation(directionMasterToPlayer);
                    }
                    else
                    {
                        // playerGo가 없을 경우, DanmakuToDirection.Outer 와 같도록
                        Vector3 directionToMaster = (danmakuGo.transform.position - masterGo.transform.position).normalized;
                        danmakuGo.transform.rotation = Quaternion.LookRotation(-directionToMaster);
                    }
                }
                break;

            case DanmakuToDirection.LookPlayer: // 탄막이 플레이어를 바라보도록
                foreach (GameObject danmakuGo in danmakuGoList)
                {
                    if (playerGo != null)
                    {
                        danmakuGo.transform.LookAt(playerGo.transform);
                    }
                }
                break;

            case DanmakuToDirection.CompletelyRandom: // 모든 방향으로 랜덤
                foreach (GameObject danmakuGo in danmakuGoList)
                {
                    danmakuGo.transform.rotation = Random.rotation;
                }
                break;
        }
        // 2>?. 방향에 일괄 오차 주기
        // 1>?. 에서 행했던 것 또 넣어도 될 듯 함



    }

}
