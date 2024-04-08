using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tier1PerkBehaviour : MonoBehaviour
{
    // 지금 얘한테 있는 기능: 라인렌더러 만들어서 가까운 저티어대 퍼크 연결
    // 그리고 서브 퍼크 자체적으로 만드는 거 (퍼크 매니저로 기능 통합해야 함)
    private System.Random _random = new System.Random();
    private BinaryCombineAlgorithm _algorithm;

    private LineRenderer _line;
    private GameObject _subPerk;
    private string _subBin;
    private bool[] _subArr = new bool[8];

    private void Awake()
    {
        _algorithm = GetComponent<BinaryCombineAlgorithm>();
        _line = GetComponent<LineRenderer>();
        _subPerk = Resources.Load<GameObject>("Prefabs/Perk/SubPerk");
    }

    private void Start()
    {
        LineToOrigin();
        _subBin = _algorithm.ConvertStructureToBinary(8, 3, RandomWithRange(56));
        BinaryStrToBoolArr(8, ref _subArr, ref _subBin);
        GenerateSubPerks();
    }

    private void LineToOrigin()
    {
        _line.widthMultiplier = 10f;
        _line.SetPosition(0, new Vector3(0f, 0f, -1f));
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
