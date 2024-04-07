using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class AchievementCommonUpdater : MonoBehaviour
{
    public static AchievementCommonUpdater instance;

    public static AchievementCommonUpdater Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AchievementCommonUpdater>();
                if (instance == null)
                {
                    instance = new GameObject("@AchievementCommonUpdater").AddComponent<AchievementCommonUpdater>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // GameManager 오브젝트를 씬 전환 시 파괴되지 않도록 설정
        var _ = AchievementCommonUpdater.Instance; // AchievementCommonUpdater 인스턴스를 생성 및 초기화
    }
}
