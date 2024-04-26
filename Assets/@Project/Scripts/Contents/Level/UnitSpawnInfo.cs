using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    None,

    Minion_Spider,
    Minion_Ball,
    Minion_Drone,
    Minion_WarMachine,

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
