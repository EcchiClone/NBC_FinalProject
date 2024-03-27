using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum PartsType
    {
        Lower,
        Upper,
    }

    public class InitData
    {
        public readonly List<int> LowerPartId = new()
        {
            10001001,
            10001002,
        };

        public readonly List<int> UpperPartId = new()
        {
            10002001,
            10002002,
        };
    }
}