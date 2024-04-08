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
}
