using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    private JsonSaveNLoader _json; // Json 저장 및 불러오기
    private PerkGenerator _gen; // 퍼크 생성기
    private SeedGenerator _seed; // 랜덤 시드 생성기
    private PointBehaviour _point; // 포인트 관리 스크립트

    public static PerkManager Instance { get; private set; } // 싱글톤 인스턴스

    public int PlayerPoint { get; set; } // 현재 포인트
    public int UnlockCount { get; set; } // 기본 요구 포인트
    public string CurrentSeed { get; set; } // 현재 시드

    private PerkList _tier1Perks = new PerkList(); // Tier1 퍼크 집합
    private PerkList _tier2Perks = new PerkList(); // Tier2 퍼크 집합
    private PerkList _tier3Perks = new PerkList(); // Tier3 퍼크 집합

    private ContentList _tier1Contents = new ContentList(); // Tier1 컨텐츠 집합
    private ContentList _tier2Contents = new ContentList(); // Tier2 컨텐츠 집합
    private ContentList _tier3Contents = new ContentList(); // Tier3 컨텐츠 집합
    private ContentList _subPerkContents = new ContentList(); // Sub Perk 컨텐츠 집합

    public PerkTier PointerTier { get; set; } // 퍼크 생성 시 현재 생성하는 퍼크의 티어
    public int PointerIdx { get; set; } // 퍼크 생성 시 현재 생성하는 퍼크의 인덱스
    public int PointerSubIdx { get; set; } // 서브 퍼크 생성 시 현재 생성하는 서브 퍼크의 인덱스

    public PerkInfo SelectedPerkInfo { get; set; } // 클릭한 퍼크의 기본 정보
    public SubPerkInfo SelectedSubInfo { get; set; } // 클릭한 서브 퍼크의 기본 정보
    public ContentInfo SelectedContentInfo { get; set; } // 클릭한 퍼크의 내장 컨텐츠 정보
    public float SelectedPerkDistance { get; set; } // 클릭한 퍼크와 전 퍼크의 거리 정보

    public int RequirePoint { get; set; } // 선택한 퍼크의 요구되는 포인트

    public event EventHandler OnPerkClicked; // 퍼크를 클릭했을 때 호출되는 이벤트
    public event EventHandler OnUnlockBtnClicked; // 'Unlock' 버튼을 클릭했을 때 호출되는 이벤트

    public PerkData perkData { get; private set; }

    private void Awake()
    {
        perkData = Managers.GameManager.PerkData;

        // 스크립트 가져오기
        _json = GetComponent<JsonSaveNLoader>();
        _gen = GetComponent<PerkGenerator>();
        _seed = GetComponent<SeedGenerator>();
        _point = GetComponent<PointBehaviour>();

        // 싱글톤 선언
        if (Instance == null)
        {
            Instance = this;
        }

        // 클래스 내 데이터 초기화
        _tier1Perks.data = new List<PerkInfo> ();
        _tier2Perks.data = new List<PerkInfo> ();
        _tier3Perks.data = new List<PerkInfo> ();

        _tier1Contents.data = new List<ContentInfo> ();
        _tier2Contents.data = new List<ContentInfo> ();
        _tier3Contents.data = new List<ContentInfo> ();
        _subPerkContents.data = new List<ContentInfo> ();

        InitVars();
    }

    private void Start()
    {
        // 변수 초기화
        PlayerPoint = 9999;
        UnlockCount = 0;
        CurrentSeed = _seed.RandomSeedGenerator();

        // 컨텐츠 json 파일 먼저 불러오기
        _json.LoadContentData(ref _tier1Contents, "tier1ContentData");
        _json.LoadContentData(ref _tier2Contents, "tier2ContentData");
        _json.LoadContentData(ref _tier3Contents, "tier3ContentData");
        _json.LoadContentData(ref _subPerkContents, "subPerkContentData");

        // 퍼크 데이터 존재 여부 확인
        CheckDataExists();
    }

    private void OnApplicationQuit()
    {
        SavePerkSequence();
    }

    private void InitVars()
    {
        // 변수 초기화
        PointerTier = PerkTier.TIER1;
        PointerIdx = 0;
        PointerSubIdx = 0;

        SelectedPerkInfo = new PerkInfo(PerkTier.TIER1, 0, 0, false);
        SelectedSubInfo = null;

        SelectedContentInfo = new ContentInfo();
        SelectedContentInfo.contentIdx = 0;
        SelectedContentInfo.name = "";
        SelectedContentInfo.description = "";

        SelectedPerkDistance = 0;
    }

    private void CheckDataExists()
    {
        // 저장된 퍼크 파일이 전부 존재하는지 확인
        if (_json.IsExist("tier1PerkData") && _json.IsExist("tier2PerkData") && _json.IsExist("tier3PerkData"))
        {
            Debug.Log("데이터 있음");
        }
        else
        {
            Debug.Log("데이터 없음");
            CreateNewPerkSequence();
        }

        LoadPerkSequence();
    }

    private void CreateNewPerkSequence()
    {
        // 퍼크를 새로 생성하는 시퀀스
        _gen.ParseSeed(CurrentSeed);
        _gen.ConvertSeedToLoc();
        _gen.SendLocToPerkManager();

        // json 변수에 할당
        _tier1Perks.point = PlayerPoint;
        _tier1Perks.unlockCount = UnlockCount;
        _tier1Perks.currentSeed = CurrentSeed;

        _json.tier1PerkData = _tier1Perks;
        _json.tier2PerkData = _tier2Perks;
        _json.tier3PerkData = _tier3Perks;

        // json 저장 테스트
        _json.SavePerkData(_json.tier1PerkData, "tier1PerkData");
        _json.SavePerkData(_json.tier2PerkData, "tier2PerkData");
        _json.SavePerkData(_json.tier3PerkData, "tier3PerkData");

    }

    private void LoadPerkSequence()
    {
        // 기존 퍼크를 불러오는 시퀀스
        _json.LoadPerkData(ref _tier1Perks, "tier1PerkData");
        _json.LoadPerkData(ref _tier2Perks, "tier2PerkData");
        _json.LoadPerkData(ref _tier3Perks, "tier3PerkData");

        // 기존 변수 할당
        PlayerPoint = _tier1Perks.point;
        UnlockCount = _tier1Perks.unlockCount;
        CurrentSeed = _tier1Perks.currentSeed;

        // 퍼크 생성
        GeneratePerks();
    }

    public void SavePerkSequence()
    {
        // 현재 포인트 저장
        _tier1Perks.point = PlayerPoint;
        _tier1Perks.unlockCount = UnlockCount;

        // PerkManager -> JsonSaveNLoader
        _json.tier1PerkData = _tier1Perks;
        _json.tier2PerkData = _tier2Perks;
        _json.tier3PerkData = _tier3Perks;

        // json으로 현재 정보 저장
        _json.SavePerkData(_json.tier1PerkData, "tier1PerkData");
        _json.SavePerkData(_json.tier2PerkData, "tier2PerkData");
        _json.SavePerkData(_json.tier3PerkData, "tier3PerkData");
    }

    public void ResetPerkSequence()
    {
        // 생성된 퍼크 제거
        GameObject[] tier1Perks = GameObject.FindGameObjectsWithTag("Tier1");
        GameObject[] tier2Perks = GameObject.FindGameObjectsWithTag("Tier2");
        GameObject[] tier3Perks = GameObject.FindGameObjectsWithTag("Tier3");

        foreach (GameObject perk in tier1Perks)
            Destroy(perk);

        foreach (GameObject perk in tier2Perks)
            Destroy(perk);

        foreach (GameObject perk in tier3Perks)
            Destroy(perk);

        // 현재 데이터 초기화
        _tier1Perks.data.Clear();
        _tier2Perks.data.Clear();
        _tier3Perks.data.Clear();

        _json.tier1PerkData.data.Clear();
        _json.tier2PerkData.data.Clear();
        _json.tier3PerkData.data.Clear();

        Invoke("RerollSequence", 3f);
    }

    private void RerollSequence()
    {
        // 새로 생성
        CurrentSeed = _seed.RandomSeedGenerator();

        InitVars();
        CreateNewPerkSequence();
        GeneratePerks();
    }

    private void GeneratePerks()
    {
        // 퍼크 생성
        _gen.InstantiatePerks(_tier1Perks.data);
        _gen.InstantiatePerks(_tier2Perks.data);
        _gen.InstantiatePerks(_tier3Perks.data);
    }

    public void ConvertLocToList(bool[] binaryData, PerkTier tier)
    {
        int count = 1;
        foreach (bool b in  binaryData)
        {
            if (b)
            {
                count++;
            }
        }

        List<int> contentIdxs = new List<int>();
        MakeContentIdxs(tier, ref contentIdxs, count);

        int idx = 0;
        for (int i = 0; i < binaryData.Length; i++)
        {
            PerkInfo perkInfo = new PerkInfo(tier, i, contentIdxs[idx], false);
            MakeRandomSubPerks(ref perkInfo);

            if (binaryData[i])
            {
                if (tier == PerkTier.TIER1)
                {
                    _tier1Perks.data.Add(perkInfo);
                }
                else if (tier == PerkTier.TIER2)
                {
                    _tier2Perks.data.Add(perkInfo);
                }
                else
                {
                    _tier3Perks.data.Add(perkInfo);
                }
                idx++;
            }
        }
    }

    private void MakeContentIdxs(PerkTier tier, ref List<int> contentIdxs, int count)
    {
        if (tier == PerkTier.TIER1)
        {
            contentIdxs = _seed.RandomWithRangeNoRep(_tier1Contents.data.Count, count);
        }
        else if (tier == PerkTier.TIER2)
        {
            contentIdxs = _seed.RandomWithRangeNoRep(_tier2Contents.data.Count, count);
        }
        else
        {
            contentIdxs = _seed.RandomWithRangeNoRep(_tier3Contents.data.Count, count);
        }
    }

    private void MakeRandomSubPerks(ref PerkInfo perkInfo)
    {
        perkInfo.subPerks = new List<SubPerkInfo>();
        List<int> positionIdxs = _seed.RandomWithRangeNoRep(8, 3);
        List<int> contentIdxs = _seed.RandomWithRangeReturnsList(_subPerkContents.data.Count, 3);
        positionIdxs.Sort();

        for (int i = 0; i < positionIdxs.Count; i++)
        {
            perkInfo.subPerks.Add(new SubPerkInfo(positionIdxs[i], contentIdxs[i], false));
        }
    }

    public PerkInfo GetPerkInfo(PerkTier tier, int idx)
    {
        PerkInfo perkInfo;
        int realIdx = ReturnRealIndex(tier, idx);

        if (tier == PerkTier.TIER1)
        {
            perkInfo = _tier1Perks.data[realIdx];
        }
        else if (tier == PerkTier.TIER2)
        {
            perkInfo = _tier2Perks.data[realIdx];
        }
        else
        {
            perkInfo = _tier3Perks.data[realIdx];
        }

        return perkInfo;
    }

    public ContentInfo GetContentInfo(PerkTier tier, int idx)
    {
        ContentInfo contentInfo;
        
        if (tier == PerkTier.TIER1)
        {
            contentInfo = _tier1Contents.data[idx];
        }
        else if (tier == PerkTier.TIER2)
        {
            contentInfo = _tier2Contents.data[idx];
        }
        else if (tier == PerkTier.TIER3)
        {
            contentInfo = _tier3Contents.data[idx];
        }
        else
        {
            contentInfo = _subPerkContents.data[idx];
        }

        return contentInfo;
    }

    public void SetPerkIsActive()
    {
        PerkTier tier = SelectedPerkInfo.Tier;
        int idx = SelectedPerkInfo.PositionIdx;

        int realIdx = ReturnRealIndex(tier, idx);

        if (SelectedSubInfo == null)
        {
            if (tier == PerkTier.TIER1)
            {
                _tier1Perks.data[realIdx].IsActive = true;
            }
            else if (tier == PerkTier.TIER2)
            {
                _tier2Perks.data[realIdx].IsActive = true;
            }
            else
            {
                _tier3Perks.data[realIdx].IsActive = true;
            }
        }
        else
        {
            int subIdx = SelectedSubInfo.PositionIdx;
            int realSubIdx = ReturnRealSubIndex(tier, realIdx, subIdx);

            if (tier == PerkTier.TIER1)
            {
                _tier1Perks.data[realIdx].subPerks[realSubIdx].IsActive = true;
            }
            else if (tier == PerkTier.TIER2)
            {
                _tier2Perks.data[realIdx].subPerks[realSubIdx].IsActive = true;
            }
            else
            {
                _tier3Perks.data[realIdx].subPerks[realSubIdx].IsActive = true;
            }
        }
    }

    public int ReturnRealIndex(PerkTier tier, int idx)
    {
        int realIdx;

        switch (tier)
        {
            case PerkTier.TIER3:
                realIdx = _tier3Perks.data.FindIndex(info => info.PositionIdx.Equals(idx));
                break;
            case PerkTier.TIER2:
                realIdx = _tier2Perks.data.FindIndex(info => info.PositionIdx.Equals(idx));
                break;
            case PerkTier.TIER1:
                realIdx = _tier1Perks.data.FindIndex(info => info.PositionIdx.Equals(idx));
                break;
            default:
                realIdx = 0;
                break;
        }

        return realIdx;
    }

    public int ReturnRealSubIndex(PerkTier tier, int idx, int subIdx)
    {
        int realIdx;

        switch (tier)
        {
            case PerkTier.TIER3:
                realIdx = _tier3Perks.data[idx].subPerks.FindIndex(info => info.PositionIdx.Equals(subIdx));
                break;
            case PerkTier.TIER2:
                realIdx = _tier2Perks.data[idx].subPerks.FindIndex(info => info.PositionIdx.Equals(subIdx));
                break;
            case PerkTier.TIER1:
                realIdx = _tier1Perks.data[idx].subPerks.FindIndex(info => info.PositionIdx.Equals(subIdx));
                break;
            default:
                realIdx = 0;
                break;
        }

        return realIdx;
    }

    public void CallOnPerkClicked()
    {
        OnPerkClicked?.Invoke(this, EventArgs.Empty);
    }

    public void CallOnUnlockBtnClicked()
    {
        OnUnlockBtnClicked?.Invoke(this, EventArgs.Empty);
    }
}

