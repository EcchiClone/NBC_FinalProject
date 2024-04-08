using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    private JsonSaveNLoader _json; // Json 저장 및 불러오기
    private PerkGenerator _gen; // 퍼크 생성기
    private SeedGenerator _seed; // 랜덤 시드 생성기

    public static PerkManager Instance { get; private set; } // 싱글톤 인스턴스

    [Header ("Player Info")]
    [SerializeField] private int _point; // 현재 포인트
    [SerializeField] private string _currentSeed; // 현재 시드

    private PerkList _tier1Perks = new PerkList(); // Tier1 퍼크 집합
    private PerkList _tier2Perks = new PerkList(); // Tier2 퍼크 집합
    private PerkList _tier3Perks = new PerkList(); // Tier3 퍼크 집합

    private ContentList _tier1Contents = new ContentList(); // Tier1 컨텐츠 집합
    private ContentList _tier2Contents = new ContentList(); // Tier2 컨텐츠 집합
    private ContentList _tier3Contents = new ContentList(); // Tier3 컨텐츠 집합

    public PerkTier PointerTier { get; set; } // 퍼크 생성 시 현재 생성하는 퍼크의 티어
    public int PointerIdx { get; set; } // 퍼크 생성 시 현재 생성하는 퍼크의 인덱스

    private void Awake()
    {
        // 스크립트 가져오기
        _json = GetComponent<JsonSaveNLoader>();
        _gen = GetComponent<PerkGenerator>();
        _seed = GetComponent<SeedGenerator>();

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

    }

    private void Start()
    {
        // 변수 초기화
        _point = 100;
        _currentSeed = _seed.RandomSeedGenerator();

        // 컨텐츠 json 파일 먼저 불러오기
        _json.LoadContentData(ref _tier1Contents, "tier1ContentData");
        _json.LoadContentData(ref _tier2Contents, "tier2ContentData");
        _json.LoadContentData(ref _tier3Contents, "tier3ContentData");

        // 퍼크 데이터 존재 여부 확인
        CheckDataExists();
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
        _gen.ParseSeed(_currentSeed);
        _gen.ConvertSeedToLoc();
        _gen.SendLocToPerkManager();

        // json 변수에 할당
        _tier1Perks.point = _point;
        _tier1Perks.currentSeed = _currentSeed;

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
        _point = _tier1Perks.point;
        _currentSeed = _tier1Perks.currentSeed;

        // 퍼크 생성
        _gen.InstantiatePerks(_tier1Perks.data);
        _gen.InstantiatePerks(_tier2Perks.data);
        _gen.InstantiatePerks(_tier3Perks.data);
    }

    public void ConvertLocToList(bool[] binaryData, PerkTier tier)
    {
        List<int> contentIdxs = new List<int>();
        MakeContentIdxs(tier, ref contentIdxs);

        for (int i = 0; i < binaryData.Length; i++)
        {
            PerkInfo perkInfo = new PerkInfo(tier, i, contentIdxs[i], false);

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
            }
        }
    }

    private void MakeContentIdxs(PerkTier tier, ref List<int> contentIdxs)
    {
        if (tier == PerkTier.TIER1)
        {
            contentIdxs = _seed.RandomWithRangeNoRep(_tier1Contents.data.Count, 8);
        }
        else if (tier == PerkTier.TIER2)
        {
            contentIdxs = _seed.RandomWithRangeNoRep(_tier2Contents.data.Count, 16);
        }
        else
        {
            contentIdxs = _seed.RandomWithRangeNoRep(_tier3Contents.data.Count, 24);
        }
    }

    public PerkInfo GetPerkInfo(PerkTier tier, int idx)
    {
        PerkInfo perkInfo = null;
        int realIdx = 0;

        if (tier == PerkTier.TIER1)
        {
            realIdx = _tier1Perks.data.FindIndex(info => info.PositionIdx.Equals(idx));
            perkInfo = _tier1Perks.data[realIdx];
        }
        else if (tier == PerkTier.TIER2)
        {
            realIdx = _tier2Perks.data.FindIndex(info => info.PositionIdx.Equals(idx));
            perkInfo = _tier2Perks.data[realIdx];
        }
        else
        {
            realIdx = _tier3Perks.data.FindIndex(info => info.PositionIdx.Equals(idx));
            perkInfo = _tier3Perks.data[realIdx];
        }

        return perkInfo;
    }

    public ContentInfo GetContentInfo(PerkTier tier, int idx)
    {
        ContentInfo contentInfo = null;
        
        if (tier == PerkTier.TIER1)
        {
            contentInfo = _tier1Contents.data[idx];
        }
        else if (tier == PerkTier.TIER2)
        {
            contentInfo = _tier2Contents.data[idx];
        }
        else
        {
            contentInfo = _tier3Contents.data[idx];
        }

        return contentInfo;
    }
}

