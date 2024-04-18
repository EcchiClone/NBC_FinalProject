using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    // 아래와 같은 방식으로 fmod 내 이벤트를 가져와 사용
    //[field: Header("Player SFX")]
    //[field: SerializeField] public EventReference footstepsWalk { get; private set; }

    [field: Header("Test SFX")]
    [field: SerializeField] public EventReference testEvent { get; private set; }


    [field: Header("BGM(2D)")]
    [field: SerializeField] public EventReference Field_BGM { get; private set; }


    [field: Header("Ambience(2D)")]
    [field: SerializeField] public EventReference Main_Ambience { get; private set; }
    [field: SerializeField] public EventReference Perk_Ambience { get; private set; }


    [field: Header("Perk/UI SFX(2D)")]
    [field: SerializeField] public EventReference UI_Clicked { get; private set; }
    [field: SerializeField] public EventReference UI_Entered { get; private set; }
    [field: SerializeField] public EventReference Perk_Denied { get; private set; }
    [field: SerializeField] public EventReference Perk_Released { get; private set; }
    [field: SerializeField] public EventReference Perk_Reroll { get; private set; }
    [field: SerializeField] public EventReference Weapon_Changed { get; private set; }
    [field: SerializeField] public EventReference Achivement_Success { get; private set; }
    [field: SerializeField] public EventReference Achivement_Notification { get; private set; }
    [field: SerializeField] public EventReference Tutorial_Dialogue { get; private set; }


    [field: Header("Player SFX(2D)")]
    [field: SerializeField] public EventReference Player_Footsteps { get; private set; }
    [field: SerializeField] public EventReference Player_BoosterLoop { get; private set; }
    [field: SerializeField] public EventReference Player_Repair { get; private set; }
    [field: SerializeField] public EventReference Player_LockOn { get; private set; }


    [field: Header("Player Weapons")]
    [field: SerializeField] public EventReference Player_Weapon1_Shot { get; private set; } // Arm01
    [field: SerializeField] public EventReference Player_Weapon1_Reload { get; private set; }
    [field: SerializeField] public EventReference Player_Weapon2_Shot { get; private set; } // Arm02
    [field: SerializeField] public EventReference Player_Weapon2_Reload { get; private set; }
    [field: SerializeField] public EventReference Player_Laser_Shot { get; private set; } // Arm03
    [field: SerializeField] public EventReference Player_Laser_Charge { get; private set; }
    [field: SerializeField] public EventReference Player_Laser_Reload { get; private set; }
    [field: SerializeField] public EventReference Player_Cannon_Shot { get; private set; } // Arm04
    [field: SerializeField] public EventReference Player_Cannon_Reload { get; private set; } 
    [field: SerializeField] public EventReference Player_LaunchMissile { get; private set; } // Shoulder01
    [field: SerializeField] public EventReference Player_LaunchRocket { get; private set; } // Shoulder02
    [field: SerializeField] public EventReference Player_LaserCannon_Shot { get; private set; } // Shoulder03


    [field: Header("Others SFX(3D)")]
    [field: SerializeField] public EventReference Others_Appear { get; private set; }
    [field: SerializeField] public EventReference Others_Disappear { get; private set; }
    [field: SerializeField] public EventReference Boom_Distance { get; private set; }


    [field: Header("Enemies SFX(3D)")]
    [field: SerializeField] public EventReference Ball_Drag { get; private set; }
    [field: SerializeField] public EventReference Ball_Explode { get; private set; }
    [field: SerializeField] public EventReference Drone_Idle { get; private set; }
    [field: SerializeField] public EventReference Drone_Shot { get; private set; }
    [field: SerializeField] public EventReference Spider_Shot { get; private set; }
    [field: SerializeField] public EventReference Spider_Footsteps { get; private set; }
    [field: SerializeField] public EventReference Boss_Appear { get; private set; }
    [field: SerializeField] public EventReference Boss_Detect { get; private set; }
    [field: SerializeField] public EventReference Boss_Phase_1 { get; private set; }
    [field: SerializeField] public EventReference Boss_Phase_2 { get; private set; }
    [field: SerializeField] public EventReference Boss_Phase_3 { get; private set; }
    [field: SerializeField] public EventReference Enemy_Hits { get; private set; }
    [field: SerializeField] public EventReference Enemy_Down { get; private set; }


    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }
}
