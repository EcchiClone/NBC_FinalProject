using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnemyBulletPatternStarter;
using static PhaseSO;

public class EnemyBulletChainSpawn : MonoBehaviour
{
    // 일순간에 실행될 폭발형이나 연쇄형 패턴에 추천. 현재로썬 프리팹-패턴 일대일 대응이 한계.

    public List<PatternEntry> Patterns;    // (PatternSO,이름) 목록을 인덱스로 관리
    int patternMax;
    bool isPooled = false;

    void OnEnable()
    {
        if (isPooled == false)
        {
            isPooled = true;
        }
        else
        {
            patternMax = Patterns.Count();
            EnemyBulletController po = GetComponent<EnemyBulletController>();
            if (po != null)
                po.OnCol += Chain;
        }
    }
    public void Chain()
    {

        for (int i = 0; i < patternMax; i++)
        {
            var genSettings = new BulletGenerationSettings
            {
                muzzleTransform = gameObject.transform,
                rootObject = gameObject,
                masterObject = gameObject,
                cycleTime = 1f,
                isOneTime = true,
                patternHierarchy = new PatternHierarchy
                {
                    patternSO = Patterns[i].patternSO,
                    patternName = Patterns[i].patternName,
                    genCondition = GenCondition.Timer,
                    startTime = 0f,
                    cycleTime = 1f,
                    subPatterns = new List<PatternHierarchy>()
                }
            };
            EnemyBulletGenerator.instance.StartPatternHierarchy(genSettings);
        }
    }
}
