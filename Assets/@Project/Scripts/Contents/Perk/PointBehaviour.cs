using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBehaviour : MonoBehaviour
{
    // 포인트의 구조 (1 + 2 + 3)
    // 1. 티어 별 기본 포인트: SUB = 3, TIER1 = 5, TIER2 = 7, TIER3 = 10
    // 2. 거리 비례값: SUB, TIER1 = 0, TIER2, TIER3 = 거리에 따라 상이
    // 3. 선형적 해금 비용 증가치: 최초 = +0, 1회 = +1, 2회 = +2, ...

    private int _requirePoint;
    private int _unlockCount;
    private int _distancePoint;

    private void Awake()
    {
        _requirePoint = 0;
        _unlockCount = 0;
        _distancePoint = 0;
    }

    private void Start()
    {
        PerkManager.Instance.OnPerkClicked += OnPerkClicked;
        PerkManager.Instance.OnUnlockBtnClicked += OnUnlockBtnClicked;
    }

    private void UpdateRequirePoint()
    {
        PerkInfo perkInfo = PerkManager.Instance.SelectedPerkInfo;
        SubPerkInfo subInfo = PerkManager.Instance.SelectedSubInfo;
        
        PerkTier tier = subInfo == null ? perkInfo.Tier : PerkTier.SUB;
        int basePoint;

        switch (tier)
        {
            case PerkTier.TIER3: basePoint = 10; break;
            case PerkTier.TIER2: basePoint = 7; break;
            case PerkTier.TIER1: basePoint = 5; break;
            default: basePoint = 3; break;
        }

        _unlockCount = PerkManager.Instance.UnlockCount;

        float distance = PerkManager.Instance.SelectedPerkDistance;
        _distancePoint = (int) (distance / 100 * 2 / 3);

        _requirePoint = basePoint + _unlockCount + _distancePoint;

        PerkManager.Instance.RequirePoint = _requirePoint;
        
    }

    private void PointSubtraction()
    {
        int playerPoint = PerkManager.Instance.PlayerPoint;

        if (playerPoint >= _requirePoint)
        {
            PerkManager.Instance.UnlockCount++;
            PerkManager.Instance.PlayerPoint -= _requirePoint;
            PerkManager.Instance.SetPerkIsActive();

            ContentInfo contentInfo = PerkManager.Instance.SelectedContentInfo;
            PerkType type = contentInfo.type;
            float value = contentInfo.value;

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Perk_Released, transform.position);
            PerkManager.Instance.perkData.SetActivedPerk(type, value);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Perk_Denied, transform.position);
            Debug.Log("요구되는 포인트보다 가지고 있는 포인트가 적습니다.");
        }
    }

    private void OnPerkClicked(object sender, EventArgs eventArgs)
    {
        UpdateRequirePoint();
    }

    private void OnUnlockBtnClicked(object sender, EventArgs eventArgs)
    {
        PointSubtraction();
    }

}
