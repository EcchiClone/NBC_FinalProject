using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    #region Save Path
    private const string kSaveRootPath = "achievementSystem";
    private const string kActiveAchievementsSavePath = "activeAchievements";
    private const string kCompletedAchievementsSavePath = "completedAchievements";
    #endregion

    #region Events
    public delegate void AchievementRegisteredHandler(Achievement newAchievement);
    public delegate void AchievementIsReadyToCompleteHandler(Achievement achievement);
    public delegate void AchievementCompletedHandler(Achievement achievement);
    public delegate void AchievementCanceledHandler(Achievement achievement);
    #endregion

    public static AchievementSystem instance;
    private static bool isApplicationQuitting;

    public static AchievementSystem Instance
    {
        get
        {
            if (!isApplicationQuitting && instance == null)
            {
                instance = FindObjectOfType<AchievementSystem>();
                if (instance == null)
                {
                    instance = new GameObject("@AchievementSystem").AddComponent<AchievementSystem>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    private GameObject CompleteAlarmUI;

    private List<Achievement> activeAchievements = new List<Achievement>();
    private List<Achievement> completedAchievements = new List<Achievement>();

    private AchievementDatabase achievementDatabase;

    public event AchievementRegisteredHandler onAchievementRegistered;
    public event AchievementIsReadyToCompleteHandler onAchievementIsReadyToComplete;
    public event AchievementCompletedHandler onAchievementCompleted;
    public event AchievementCanceledHandler onAchievementCanceled;

    public IReadOnlyList<Achievement> ActiveAchievements => activeAchievements;
    public IReadOnlyList<Achievement> CompletedAchievements => completedAchievements;

    private void Awake()
    {
        achievementDatabase = Resources.Load<AchievementDatabase>("Data/AchievementDatabase");
        CompleteAlarmUI = Resources.Load<GameObject>("Prefabs/UI/Others/UI_AchievementAlarm");

        if (!Load())
        {
            foreach (var achievement in achievementDatabase.Achievements)
                Register(achievement);
        }
    }

    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
        Save();
    }

    public Achievement Register(Achievement achievement)
    {
        var newAchievement = achievement.Clone();

        if (newAchievement is Achievement)
        {
            newAchievement.onCompleted += OnAchievementCompleted;
            newAchievement.onWaitForComplete += OnAchievementIsReadyToComplete;

            activeAchievements.Add(newAchievement);

            newAchievement.OnRegister();
            onAchievementRegistered?.Invoke(newAchievement);
        }
        else
        {
            newAchievement.onCompleted += OnAchievementCompleted;
            newAchievement.onWaitForComplete += OnAchievementIsReadyToComplete;
            newAchievement.onCanceled += OnAchievementCanceled;

            activeAchievements.Add(newAchievement);

            newAchievement.OnRegister();
            onAchievementRegistered?.Invoke(newAchievement);
        }

        return newAchievement;
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(activeAchievements, category, target, successCount);
    }

    public void ReceiveReport(TaskCategory category, TaskTarget target, int successCount)
        => ReceiveReport(category.CodeName, target.Value, successCount);
    public void ReceiveReport(TaskCategory category, string target, int successCount)
        => ReceiveReport(category.CodeName, target, successCount);
    public void ReceiveReport(string category, TaskTarget target, int successCount)
        => ReceiveReport(category, target.Value, successCount);

    private void ReceiveReport(List<Achievement> achievements, string category, object target, int successCount)
    {
        foreach (var Achievement in achievements.ToArray())
            Achievement.ReceiveReport(category, target, successCount);
    }

    public void ReceiveRewardsAndCompleteAchievement(string codeName)
    {
        var achievement = activeAchievements.FirstOrDefault(a => a.CodeName == codeName && a.IsWaitingForCompletion);
        if (achievement != null)
        {
            achievement.ReceiveRewardsAndComplete();
        }
        else
        {
            Debug.LogWarning("지정된 코드 이름의 업적을 찾을 수 없거나, 업적이 완료 대기 상태가 아님.");
        }
    }

    public void CompleteWaitingAchievements() // 완료대기 일괄수령 버튼
    {
        foreach (var achievement in activeAchievements.ToList())
        {
            if (achievement.IsWaitingForCompletion)
                achievement.Complete();
        }
    }
    public bool ContainsInActiveAchievements(Achievement achievement) => activeAchievements.Any(x => x.CodeName == achievement.CodeName);

    public bool ContainsInCompletedAchievements(Achievement achievement) => completedAchievements.Any(x => x.CodeName == achievement.CodeName);

    private void Save()
    {
        var root = new JObject();
        root.Add(kActiveAchievementsSavePath, CreateSaveDatas(activeAchievements));
        root.Add(kCompletedAchievementsSavePath, CreateSaveDatas(completedAchievements));

        PlayerPrefs.SetString(kSaveRootPath, root.ToString());
        PlayerPrefs.Save();
    }

    private bool Load()
    {
        if (PlayerPrefs.HasKey(kSaveRootPath))
        {
            var root = JObject.Parse(PlayerPrefs.GetString(kSaveRootPath));

            LoadSaveDatas(root[kActiveAchievementsSavePath], achievementDatabase, LoadActiveAchievement);
            LoadSaveDatas(root[kCompletedAchievementsSavePath], achievementDatabase, LoadCompletedAchievement);

            return true;
        }
        else
            return false;
    }

    private JArray CreateSaveDatas(IReadOnlyList<Achievement> achievements)
    {
        var saveDatas = new JArray();
        foreach (var achievement in achievements)
        {
            //if (achievement.IsSavable)
                saveDatas.Add(JObject.FromObject(achievement.ToSaveData()));
        }
        return saveDatas;
    }

    private void LoadSaveDatas(JToken datasToken, AchievementDatabase database, System.Action<AchievementSaveData, Achievement> onSuccess)
    {
        try
        {
            var datas = datasToken as JArray;
            foreach (var data in datas)
            {
                var saveData = data.ToObject<AchievementSaveData>();
                var achievement = database.FindAchievementBy(saveData.codeName);
                onSuccess.Invoke(saveData, achievement);
            }
        }
        catch
        {
            Debug.LogError("업적데이터 로드 실패(\"AchievementDatabase\" SO파일 내용의 누락 예상)");
        }
    }

    private void LoadActiveAchievement(AchievementSaveData saveData, Achievement achievement)
    {
        var newAchievement = Register(achievement);
        newAchievement.LoadFrom(saveData);
    }

    private void LoadCompletedAchievement(AchievementSaveData saveData, Achievement achievement)
    {
        var newAchievement = achievement.Clone();
        newAchievement.LoadFrom(saveData);

        if (newAchievement is Achievement)
            completedAchievements.Add(newAchievement);
        else
            completedAchievements.Add(newAchievement);
    }

    #region Callback
    private void OnAchievementCompleted(Achievement achievement)
    {
        activeAchievements.Remove(achievement);
        completedAchievements.Add(achievement);

        onAchievementCompleted?.Invoke(achievement);
    }
    private void OnAchievementIsReadyToComplete(Achievement achievement)
    {
        DisplayCompleteAlarm(achievement);

        onAchievementIsReadyToComplete?.Invoke(achievement);
    }

    private void OnAchievementCanceled(Achievement achievement)
    {
        activeAchievements.Remove(achievement);
        onAchievementCanceled?.Invoke(achievement);

        Destroy(achievement, Time.deltaTime);
    }
    #endregion
    private void DisplayCompleteAlarm(Achievement achievement)
    {
        string desc = $"[{achievement.DisplayName}] {achievement.Description}";
        GameObject go = Instantiate(CompleteAlarmUI);
        go.transform.SetParent(GameObject.Find("@UI_Root").transform,false);
        go.GetComponent<UI_AchievementAlarm>().DescriptionMsg = desc;
    }
}
