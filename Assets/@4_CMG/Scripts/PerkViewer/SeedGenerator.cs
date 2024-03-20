using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : HexDecConverter
{
    System.Random _random = new System.Random();

    // TODO: 정규분포를 따르는 확률로 무작위 시드 생성기 제작
    private void Start()
    {
        Debug.Log(RandomSeedGenerator());
    }

    private int RandomWithRange(int range)
    {
        return _random.Next(range) + 1;
    }

    private string RandomSeedGenerator()
    {
        string tier1 = DecToHex(RandomWithRange(181), 2);
        string tier2 = DecToHex(RandomWithRange(51766), 4);
        string tier3 = DecToHex(RandomWithRange(14233963), 6);

        return tier1 + tier2 + tier3;
    }
}
