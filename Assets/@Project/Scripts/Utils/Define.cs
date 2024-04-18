using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scenes
    {
        Unknown,
        TitleScene,
        TutorialScene,
        MainScene,
        DevScene,
        PerkViewerScene,
    }

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

    public enum PerkAdjustType
    {
        raisePercentage,
        RedusePercentage,
        plusValue,
        minusValue,
    }

    public enum WeaponType
    {
        Arm01,
        Arm02,
        Arm03,
        Arm04,
        Shoulder01,
        Shoulder02,
        Shoulder03,
    }
}