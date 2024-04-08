using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    public HexDecConverter _hexdec;
    private System.Random _random = new System.Random();

    private void Awake()
    {
        _hexdec = GetComponent<HexDecConverter>();
    }

    private void Start()
    {
        // 중복 없는 추첨 테스트
        List<int> list = RandomWithRangeNoRep(8, 8);
        foreach (int i in list)
            Debug.Log(i);
    }

    private int RandomWithRange(int range)
    {
        return _random.Next(range) + 1;
    }

    public string RandomSeedGenerator()
    {
        
        string tier1 = _hexdec.DecToHex(RandomWithRange(181), 2);
        string tier2 = _hexdec.DecToHex(RandomWithRange(51766), 4);
        string tier3 = _hexdec.DecToHex(RandomWithRange(14233963), 6);

        return tier1 + tier2 + tier3;
    }

    public List<int> RandomWithRangeNoRep(int range, int num)
    {
        if (num > range)
        {
            // 추첨수가 range를 넘어가면 예외 처리
            return new List<int>();
        }

        List<int> result = new List<int>(num);

        int count = 0;
        while (count < num)
        {
            int element = _random.Next(range);

            if (!result.Contains(element))
            {
                result.Add(element);
                count++;
            }
        }

        return result;
    }
}
