using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum PartsType
    {
        Lower,
        Upper,
        Weapon_Arm,        
        Weapon_Shoulder,        
    }

    public enum ChangePartsType
    {
        Lower,
        Upper,
        Weapon_Arm_L,
        Weapon_Arm_R,
        Weapon_Shoulder_L,
        Weapon_Shoulder_R,
    }

    public enum BulletType
    {
        Gun,
        Cannon,
        Missile
    }

    public class InitData
    {
        public readonly List<int> LowerPartId = new()
        {
            10001001,            
        };

        public readonly List<int> UpperPartId = new()
        {
            10002002,            
        };

        public readonly List<int> ArmWeaponPartId = new()
        {
            10003001,
            10003002,
            10003003,
            10003004,
        };

        public readonly List<int> ShoulderWeaponPartId = new()
        {
            10004001,
            10004002,
        };
    }
}