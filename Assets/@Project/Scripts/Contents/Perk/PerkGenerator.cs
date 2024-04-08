using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PerkGenerator : MonoBehaviour
{
    [SerializeField] private Transform _tier1UI;
    [SerializeField] private Transform _tier2UI;
    [SerializeField] private Transform _tier3UI;

    private BinaryCombineAlgorithm _algorithm;
    private HexDecConverter _hexdec;

    private GameObject _tier1Perk;
    private GameObject _tier2Perk;
    private GameObject _tier3Perk;

    private bool[] _tier1arr = new bool[8];
    private bool[] _tier2arr = new bool[16];
    private bool[] _tier3arr = new bool[24];

    private string _tier1hex;
    private string _tier2hex;
    private string _tier3hex;


    private void Awake()
    {
        _algorithm = GetComponent<BinaryCombineAlgorithm>();
        _hexdec = GetComponent<HexDecConverter>();
        _tier1Perk = Resources.Load<GameObject>("Prefabs/Perk/Tier1Perk");
        _tier2Perk = Resources.Load<GameObject>("Prefabs/Perk/Tier2Perk");
        _tier3Perk = Resources.Load<GameObject>("Prefabs/Perk/Tier3Perk");
    }

    public void ParseSeed(string seed)
    {
        _tier1hex = seed.Substring(0, 2);
        _tier2hex = seed.Substring(2, 4);
        _tier3hex = seed.Substring(6, 6);
    }

    public void ConvertSeedToLoc()
    {
        int tier1idx = _hexdec.HexToDec(_tier1hex);
        int tier2idx = _hexdec.HexToDec(_tier2hex);
        int tier3idx = _hexdec.HexToDec(_tier3hex);

        CheckTier1Range(out int tier1num, ref tier1idx);
        CheckTier2Range(out int tier2num, ref tier2idx);
        CheckTier3Range(out int tier3num, ref tier3idx);

        string tier1bin = _algorithm.ConvertStructureToBinary(8, tier1num, tier1idx);
        string tier2bin = _algorithm.ConvertStructureToBinary(16, tier2num, tier2idx);
        string tier3bin = _algorithm.ConvertStructureToBinary(24, tier3num, tier3idx);

        BinaryStrToBoolArr(8, ref _tier1arr, ref tier1bin);
        BinaryStrToBoolArr(16, ref _tier2arr, ref tier2bin);
        BinaryStrToBoolArr(24, ref _tier3arr, ref tier3bin);

        Debug.Log(tier1bin);
        Debug.Log(tier2bin);
        Debug.Log(tier3bin);
    }

    public void SendLocToPerkManager()
    {
        PerkManager.Instance.ConvertLocToList(_tier1arr, PerkTier.TIER1);
        PerkManager.Instance.ConvertLocToList(_tier2arr, PerkTier.TIER2);
        PerkManager.Instance.ConvertLocToList(_tier3arr, PerkTier.TIER3);
    }

    private void CheckTier1Range(out int num1, ref int idx1)
    {
        if (idx1 < 56 + 1)
        {
            num1 = 3;
        }
        else if (idx1 < 56 + 70 + 1)
        {
            num1 = 4;
            idx1 -= 56;
        }
        else
        {
            num1 = 5;
            idx1 -= 56 + 70;
        }
    }

    private void CheckTier2Range(out int num2, ref int idx2)
    {
        if (idx2 < 8008 + 1)
        {
            num2 = 6;
        }
        else if (idx2 < 8008 + 11440 + 1)
        {
            num2 = 7;
            idx2 -= 8008;
        }
        else if (idx2 < 8008 + 11440 + 12870 + 1)
        {
            num2 = 8;
            idx2 -= 8008 + 11440;
        }
        else if (idx2 < 8008 + 11440 + 12870 + 11440 + 1)
        {
            num2 = 9;
            idx2 -= 8008 + 11440 + 12870;
        }
        else
        {
            num2 = 10;
            idx2 -= 8008 + 11440 + 12870 + 11440;
        }
    }

    private void CheckTier3Range(out int num3, ref int idx3)
    {
        if (idx3 < 1307504 + 1)
        {
            num3 = 9;
        }
        else if (idx3 < 1307504 + 1961256 + 1)
        {
            num3 = 10;
            idx3 -= 1307504;
        }
        else if (idx3 < 1307504 + 1961256 + 2496144 + 1)
        {
            num3 = 11;
            idx3 -= 1307504 + 1961256;
        }
        else if (idx3 < 1307504 + 1961256 + 2496144 + 2704156 + 1)
        {
            num3 = 12;
            idx3 -= 1307504 + 1961256 + 2496144;
        }
        else if (idx3 < 1307504 + 1961256 + 2496144 + 2704156 + 2496144 + 1)
        {
            num3 = 13;
            idx3 -= 1307504 + 1961256 + 2496144 + 2704156;
        }
        else if (idx3 < 1307504 + 1961256 + 2496144 + 2704156 + 2496144 + 1961256 + 1)
        {
            num3 = 14;
            idx3 -= 1307504 + 1961256 + 2496144 + 2704156 + 2496144;
        }
        else
        {
            num3 = 15;
            idx3 -= 1307504 + 1961256 + 2496144 + 2704156 + 2496144 + 1961256;
        }
    }

    private void BinaryStrToBoolArr(int length, ref bool[] tierArr, ref string tierbin)
    {
        for (int i = 0; i < length; i++)
        {
            string s = tierbin.Substring(i, 1);

            if (s == "1")
                tierArr[i] = true;
            else
                tierArr[i] = false;
        }
    }

    public void InstantiatePerks(List<PerkInfo> perkList)
    {
        foreach (PerkInfo perkInfo in perkList)
        {
            int idx = perkInfo.PositionIdx;
            int tier = (int)perkInfo.Tier;

            int q = idx / (2 * tier);
            int m = idx % (2 * tier);

            float x = 0;
            float y = 0;

            switch (q)
            {
                case 0:
                    x = -900f * (tier - 1) + 900f * m;
                    y = 900f * tier;
                    break;
                case 1:
                    x = 900f * tier;
                    y = 900f * (tier - 1) - 900f * m;
                    break;
                case 2:
                    x = 900f * (tier - 1) - 900f * m;
                    y = -900f * tier;
                    break;
                case 3:
                    x = -900f * tier;
                    y = -900f * (tier - 1) + 900f * m;
                    break;
            }

            if (tier == 1)
            {
                Instantiate(_tier1Perk, new Vector3(x, y, -2), Quaternion.identity, _tier1UI);
            }
            else if (tier == 2)
            {
                Instantiate(_tier2Perk, new Vector3(x, y, -2), Quaternion.identity, _tier2UI);
            }
            else
            {
                Instantiate(_tier3Perk, new Vector3(x, y, -2), Quaternion.identity, _tier3UI);
            }
        }
    }
}
