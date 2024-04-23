using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using DG.Tweening;

public class UI_AchievementAlarmTable : MonoBehaviour
{
    /* Todo-List
    0. 유일성 보장하기 - AchievementSystem에서 관리
    1. 파라미터를 Item에 전달하기
    2. Item항목이 0개라면 삭제하기
    +. Item항목 생겨날 때, 지워질 때 Do애니메이션
    */

    float baseY = 0;
    float spacing = 90;

    // 위치 조절을 위한 리스트
    public List<GameObject> ItemLIst;
    public void AddItem(string desc) //  desc = $"[{achievement.DisplayName}] {achievement.Description}";
    {
        GameObject itemGo = Instantiate(Managers.AchievementSystem.Prefab_CompleteAlarmUI);
        itemGo.GetComponent<UI_AchievementAlarm>().Parent = gameObject;
        itemGo.GetComponent<UI_AchievementAlarm>().DescriptionMsg = desc;
        itemGo.transform.SetParent(gameObject.transform, false);
        itemGo.transform.localPosition = new Vector3(0, baseY - ItemLIst.Count * spacing, 0);

        ItemLIst.Add(itemGo);
        MoveAnimation();
    }
    public void RemoveItem(GameObject itemGo)
    {
        ItemLIst.Remove(itemGo);
        if(ItemLIst.Count <= 0)
        {
            Destroy(gameObject);
        }
        MoveAnimation();
    }
    private void MoveAnimation()
    {
        // 리스트 항목에 변경이 있을 때마다, DOTween 애니메이션으로 각 item 오브젝트에 대해 이동 애니메이션
        // 각 아이템의 transform 위치. (x = 0(부모기준 중앙정렬), y = (0 + ListNum*150)

        for (int i = 0; i < ItemLIst.Count; i++)
        {
            float targetY = baseY - i * spacing;

            ItemLIst[i].transform.DOLocalMoveY(targetY, 0.5f)
                    .SetEase(Ease.InOutQuad);
        }
    }
}
