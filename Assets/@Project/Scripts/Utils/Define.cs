using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum CamType
    {
        Main,
        Module,
        Lower,
        Upper,
        Arm_Holder,
        Shoulder_Holder,
        Arm_Left,
        Arm_Right,
        Shoulder_Left,
        Shoulder_Right,
    }

    public enum Parts_Type
    {
        Lower,
        Upper,
        Arm,
        Shoulder,
    }

    public enum Parts_Location
    {
        Lower,
        Upper,
        Weapon_Arm_L,
        Weapon_Arm_R,
        Weapon_Shoulder_L,
        Weapon_Shoulder_R,
    }

    public enum Weapon_Location
    {
        LeftArm,
        RightArm,
        LeftShoulder,
        RightShoulder,
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
            10001002,
        };

        public readonly List<int> UpperPartId = new()
        {
            10002001,
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
            10004003,
        };
    }
}