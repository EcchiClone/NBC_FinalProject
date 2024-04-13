using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Minion_Spider,
    Minion_Ball,
    Minion_Turret,

    Boss_SkyFire,
}


public class UnitSpawnInfo
{
    public UnitType unitType;
    public int count;
    public UnitSpawnInfo(UnitType unitType, int count)
    {
        this.unitType = unitType;
        this.count = count;
    }
}
