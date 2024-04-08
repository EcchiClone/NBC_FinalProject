using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class UI_Achievement : UI_Popup
{
    //private UI_Popup[] _partsMenus = new UI_Popup[4];
    private UnityAction _camAction;

    public GameObject achievementPrefab; // 업적 UI 프리팹
    public Transform contentPanel; // 스크롤 뷰의 Content 오브젝트

    public List<GameObject> activeAchievementList;

    //[SerializeField] TextMeshProUGUI[] _specTexts;

    enum Buttons
    {
        AllAchievements_Btn,
        ProgressAchievements_Btn,
        CompletedAchievements_Btn,
        Back_Btn,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Back_Btn).onClick.AddListener(BackToMain);

        GetButton((int)Buttons.AllAchievements_Btn).onClick.AddListener(() => PopulateAchievements(true, true));
        GetButton((int)Buttons.ProgressAchievements_Btn).onClick.AddListener(() => PopulateAchievements(true, false));
        GetButton((int)Buttons.CompletedAchievements_Btn).onClick.AddListener(() => PopulateAchievements(false, true));

    }
    private void OnEnable()
    {
        PopulateAchievements(true, true);
    }

    public void BindCamAction(UnityAction camAction)
    {
        _camAction = camAction;
    }
    private void BackToMain()
    {
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    void PopulateAchievements(bool viewActive, bool viewCompleted)
    {
        foreach(GameObject go in activeAchievementList)
        {
            Destroy(go);
        }

        var achievementsActive = viewActive ? AchievementSystem.Instance.ActiveAchievements : Enumerable.Empty<Achievement>();
        var achievementsCompleted = viewCompleted ? AchievementSystem.Instance.CompletedAchievements : Enumerable.Empty<Achievement>();
        var achievements = achievementsActive.Concat(achievementsCompleted);


        foreach (var achievement in achievements)
        {
            var instance = Instantiate(achievementPrefab, contentPanel);
            instance.GetComponent<UI_AchievementItem>().SetAchievementInfo(achievement);
            activeAchievementList.Add(instance);
        }
    }
}
