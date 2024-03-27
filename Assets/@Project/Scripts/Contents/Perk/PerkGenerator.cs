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

    private SeedGenerator _seed;
    private BinaryCombineAlgorithm _algorithm;
    private HexDecConverter _hexdec;

    private string _currentSeed;

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
        _seed = GetComponent<SeedGenerator>();
        _algorithm = GetComponent<BinaryCombineAlgorithm>();
        _hexdec = GetComponent<HexDecConverter>();
        _tier1Perk = Resources.Load<GameObject>("Prefabs/Tier1Perk");
        _tier2Perk = Resources.Load<GameObject>("Prefabs/Tier2Perk");
        _tier3Perk = Resources.Load<GameObject>("Prefabs/Tier3Perk");
    }

    private void Start()
    {
        _currentSeed = _seed.RandomSeedGenerator();
        Debug.Log(_currentSeed);
        ParseSeed();
        ConvertSeedToLoc();
        InstantiateTier1Perks();
        InstantiateTier2Perks();
        InstantiateTier3Perks();
    }

    private void ParseSeed()
    {
        _tier1hex = _currentSeed.Substring(0, 2);
        _tier2hex = _currentSeed.Substring(2, 4);
        _tier3hex = _currentSeed.Substring(6, 6);
    }

    private void ConvertSeedToLoc()
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

    private void InstantiateTier1Perks()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_tier1arr[i])
            {
                int q = i / 2;
                int m = i % 2;
                float x = _tier1UI.position.x;
                float y = _tier1UI.position.y;

                switch (q)
                {
                    case 0:
                        x = 0f + 900f * m;
                        y = 900f;
                        break;
                    case 1:
                        x = 900f;
                        y = 0f - 900f * m;
                        break;
                    case 2:
                        x = 0f - 900f * m;
                        y = -900f;
                        break;
                    case 3:
                        x = -900f;
                        y = 0f + 900f * m;
                        break;
                }
                Instantiate(_tier1Perk, new Vector3(x, y, 0), Quaternion.identity, _tier1UI);
            }
        }
    }

    private void InstantiateTier2Perks()
    {
        for (int i = 0; i < 16; i++)
        {
            if (_tier2arr[i])
            {
                int q = i / 4;
                int m = i % 4;
                float x = _tier2UI.position.x;
                float y = _tier2UI.position.y;

                switch (q)
                {
                    case 0:
                        x = -900f + 900f * m;
                        y = 1800f;
                        break;
                    case 1:
                        x = 1800f;
                        y = 900f - 900f * m;
                        break;
                    case 2:
                        x = 900f - 900f * m;
                        y = -1800f;
                        break;
                    case 3:
                        x = -1800f;
                        y = -900f + 900f * m;
                        break;
                }
                Instantiate(_tier2Perk, new Vector3(x, y, 0), Quaternion.identity, _tier2UI);
            }
        }
    }

    private void InstantiateTier3Perks()
    {
        for (int i = 0; i < 24; i++)
        {
            if (_tier3arr[i])
            {
                int q = i / 6;
                int m = i % 6;
                float x = _tier2UI.position.x;
                float y = _tier2UI.position.y;

                switch (q)
                {
                    case 0:
                        x = -1800f + 900f * m;
                        y = 2700f;
                        break;
                    case 1:
                        x = 2700f;
                        y = 1800f - 900f * m;
                        break;
                    case 2:
                        x = 1800f - 900f * m;
                        y = -2700f;
                        break;
                    case 3:
                        x = -2700f;
                        y = -1800f + 900f * m;
                        break;
                }
                Instantiate(_tier3Perk, new Vector3(x, y, 0), Quaternion.identity, _tier3UI);
            }
        }
    }
}