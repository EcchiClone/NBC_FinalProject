using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tier3PerkBehaviour : MonoBehaviour
{
    private System.Random _random = new System.Random();
    private BinaryCombineAlgorithm _algorithm;

    private LineRenderer _line;
    private GameObject _subPerk;
    private string _subBin;
    private bool[] _subArr = new bool[8];

    private GameObject[] _tier2Perks;
    private Vector3 _minPerk;

    private void Awake()
    {
        _algorithm = GetComponent<BinaryCombineAlgorithm>();
        _line = GetComponent<LineRenderer>();
        _subPerk = Resources.Load<GameObject>("Prefabs/Perk/SubPerk");
        _tier2Perks = GameObject.FindGameObjectsWithTag("Tier2");
    }

    private void Start()
    {
        FindMinDistanceOfTier2Perks();
        LineToTier2Perk();
        _subBin = _algorithm.ConvertStructureToBinary(8, 3, RandomWithRange(56));
        BinaryStrToBoolArr(8, ref _subArr, ref _subBin);
        GenerateSubPerks();
    }

    private void FindMinDistanceOfTier2Perks()
    {
        float min = 0f;
        float distance;

        foreach (GameObject perk in _tier2Perks)
        {
            distance = Vector3.Distance(perk.transform.position, transform.position);

            if (min == 0f || distance < min)
            {
                min = distance;
                _minPerk = perk.transform.position;
            }
        }
    }

    private void LineToTier2Perk()
    {
        _line.widthMultiplier = 10f;
        _line.SetPosition(0, new Vector3(_minPerk.x, _minPerk.y, -1f));
        _line.SetPosition(1, new Vector3(transform.position.x, transform.position.y, -1f));
    }

    private int RandomWithRange(int range)
    {
        return _random.Next(range) + 1;
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

    private void GenerateSubPerks()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_subArr[i])
            {
                int q = i / 2;
                int m = i % 2;
                float x = transform.position.x;
                float y = transform.position.y;

                switch (q)
                {
                    case 0:
                        x += 0f + 225f * m;
                        y += 225f;
                        break;
                    case 1:
                        x += 225f;
                        y += 0f - 225f * m;
                        break;
                    case 2:
                        x += 0f - 225f * m;
                        y += -225f;
                        break;
                    case 3:
                        x += -225f;
                        y += 0f + 225f * m;
                        break;
                }
                Instantiate(_subPerk, new Vector3(x, y, -2), Quaternion.identity, transform);
            }
        }
    }
}
