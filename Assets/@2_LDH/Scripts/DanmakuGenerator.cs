using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PhaseSO;

public class DanmakuGenerator : MonoBehaviour
{
    public static DanmakuGenerator instance;
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
            StartCoroutine(ExecutePatternForCycleTime(hierarchy, cycleTime, masterObject));
        }
    }
    private IEnumerator ExecutePatternForCycleTime(PatternHierarchy hierarchy, float cycleTime, GameObject masterObject)
    {
        while (true)
        {
            StartCoroutine(ExecutePatternHierarchy(hierarchy, hierarchy.cycleTime, masterObject));
            yield return new WaitForSeconds(cycleTime);
        }
    }

    private IEnumerator ExecutePatternHierarchy(PatternHierarchy hierarchy, float nextCycleTime, GameObject masterObject)
    {
        yield return new WaitForSeconds(hierarchy.startTime);
        if(masterObject != null)
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

    IEnumerator Co_ExecutePattern(DanmakuSettings settings, List<PatternHierarchy> subPatterns, float nextCycleTime, GameObject masterObject)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 오브젝트 찾기
        Transform masterTransform = masterObject.transform;

        for (int setNum = 0; setNum < settings.numOfSet; ++setNum)
        {
            for (int shotNum = 0; shotNum < settings.shotPerSet; ++shotNum)
            {
                var danmakuGo = DanmakuPoolManager.instance.GetGo(settings.danmakuPrefab.name);
                if (danmakuGo != null)
                {
                    Vector3 initPosition = masterTransform.position; // 마스터의 위치를 기본값으로

                    // 초기 위치 설정
                    switch (settings.posDirection)
                    {
                        case PosDirection.World:
                            initPosition += settings.customPosDirection * settings.initDistance;
                            break;
                        case PosDirection.Look:
                            initPosition += masterTransform.forward * settings.initDistance;
                            break;
                        case PosDirection.LookPlayer:
                            if (player != null)
                            {
                                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                                initPosition += directionToPlayer * settings.initDistance;
                            }
                            break;
                        case PosDirection.CompletelyRandom:
                            initPosition += Random.onUnitSphere * settings.initDistance;
                            break;
                    }
                    danmakuGo.transform.position = initPosition;

                    // 초기 방향 설정
                    switch (settings.initDirectionType)
                    {
                        case DanmakuToDirection.World:
                            // 직접 지정한 회전치 사용 (구현 필요)
                            break;
                        case DanmakuToDirection.Outer:
                            danmakuGo.transform.rotation = Quaternion.LookRotation(-transform.forward);
                            break;
                        case DanmakuToDirection.MasterLookPlayer:
                            // 주체가 플레이어를 바라보도록 설정
                            if (player != null)
                            {
                                transform.LookAt(player.transform);
                                danmakuGo.transform.rotation = transform.rotation;
                            }
                            break;
                        case DanmakuToDirection.LookPlayer:
                            // 탄막이 플레이어를 바라보도록 설정
                            if (player != null)
                            {
                                danmakuGo.transform.LookAt(player.transform);
                            }
                            break;
                        case DanmakuToDirection.CompletelyRandom:
                            danmakuGo.transform.rotation = Random.rotation;
                            break;
                    }

                    DanmakuController danmakuController = danmakuGo.GetComponent<DanmakuController>();
                    if (danmakuController != null)
                    {
                        DanmakuParameters parameters = DanmakuParameters.FromSettings(settings);
                        danmakuController.Initialize(parameters, nextCycleTime, subPatterns);
                    }
                }
                yield return new WaitForSeconds(settings.shotDelay); // 각 발사 사이의 지연
            }
            yield return new WaitForSeconds(settings.setDelay); // 세트 사이의 지연
        }
    }
}
