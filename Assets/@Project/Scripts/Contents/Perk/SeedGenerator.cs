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
            List<int> alter = RandomWithRangeNoRep(range, range);
            List<int> outRange = RandomWithRangeNoRep(range, num - range);
            foreach (int i in outRange)
            {
                alter.Add(i);
            }
            return alter;
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

    public List<int> RandomWithRangeReturnsList(int range, int num)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < num; i++)
        {
            result.Add(_random.Next(range));
        }

        return result;
    }
}
