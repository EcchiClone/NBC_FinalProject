using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PhaseSO;

[System.Serializable]
public class BulletGenerationSettings
{
    public Transform muzzleTransform;   // 총구 위치
    public GameObject rootObject;       // 탄막의 루트 객체
    public GameObject masterObject;     // 탄막을 제어하는 마스터 객체

    public float cycleTime; // 현재 패턴의 cycleTime
    public bool isOneTime; // CycleTime 관계없는 일회성 발사인지

    public PatternHierarchy patternHierarchy; // 현재 패턴 정보와 하위패턴목록
}