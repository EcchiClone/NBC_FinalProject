using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    private JsonSaveNLoader _json; // Json 저장 및 불러오기
    private PerkGenerator _gen; // 퍼크 생성기
    private SeedGenerator _seed; // 랜덤 시드 생성기

    public static PerkManager Instance { get; private set; } // 싱글톤 선언

    [Header ("Player Info")]
    [SerializeField] private int _point; // 현재 포인트
    [SerializeField] private string _currentSeed; // 현재 시드

    private PerkList _tier1Perks = new PerkList(); // Tier1 퍼크 집합
    private PerkList _tier2Perks = new PerkList(); // Tier2 퍼크 집합
    private PerkList _tier3Perks = new PerkList(); // Tier3 퍼크 집합

    private void Awake()
    {
        // 스크립트 가져오기
        _json = GetComponent<JsonSaveNLoader>();
        _gen = GetComponent<PerkGenerator>();
        _seed = GetComponent<SeedGenerator>();

        if (Instance == null)
        {
            Instance = this;
        }

        // 클래스 내 데이터 초기화
        _tier1Perks.data = new List<PerkInfo> ();
        _tier2Perks.data = new List<PerkInfo> ();
        _tier3Perks.data = new List<PerkInfo> ();

    }

    private void Start()
    {
        // TODO:
        // 1. 최초 실행 시 퍼크 데이터 존재 유무 확인
        // 2. 없으면: 새로 생성되는 시퀀스, 있으면: 기존 퍼크를 불러오기

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
        _currentSeed = _seed.RandomSeedGenerator();
        _gen.ParseSeed(_currentSeed);
        _gen.ConvertSeedToLoc();
        _gen.SendLocToPerkManager();

        // json 변수에 할당
        _json.tier1PerkData = _tier1Perks;
        _json.tier2PerkData = _tier2Perks;
        _json.tier3PerkData = _tier3Perks;

        // json 저장 테스트
        _json.SaveData(_json.tier1PerkData, "tier1PerkData");
        _json.SaveData(_json.tier2PerkData, "tier2PerkData");
        _json.SaveData(_json.tier3PerkData, "tier3PerkData");

    }

    private void LoadPerkSequence()
    {
        // 기존 퍼크를 불러오는 시퀀스
        _json.LoadData(ref _tier1Perks, "tier1PerkData");
        _json.LoadData(ref _tier2Perks, "tier2PerkData");
        _json.LoadData(ref _tier3Perks, "tier3PerkData");

    }

    public void ConvertLocToList(bool[] binaryData, PerkTier tier)
    {

        for (int i = 0; i < binaryData.Length; i++)
        {
            PerkInfo perkInfo = new PerkInfo(tier, i, 0, false);

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

    private void DebugList(List<PerkInfo> perks)
    {
        // 리스트 안에 뭔가 저장되어 있는지 디버깅하는 용도
        foreach (PerkInfo perkInfo in perks)
        {
            Debug.Log(perkInfo.PositionIdx);
        }
    }
}

