using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [field: SerializeField] public Transform LowerPosition { get; private set; }        

    public LowerPart CurrentLower { get; private set; }
    public UpperPart CurrentUpper { get; private set; }
    public WeaponPart CurrentLeftArm { get; private set; }
    public WeaponPart CurrentRightArm { get; private set; }
    public WeaponPart CurrentLeftShoulder { get; private set; }
    public WeaponPart CurrentRightShoulder {  get; private set; }

    public ModuleStatus ModuleStatus { get; private set; }
    public WeaponTiltController TiltController { get; private set; }
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    [field: SerializeField] public LockOnSystem LockOnSystem { get; private set; }

    public ISkill Skill { get; private set; }

    public bool IsPlayable { get; private set; }

    public void Setup(LowerPart lowerPart, UpperPart upperPart, WeaponPart leftArm, WeaponPart rightArm, WeaponPart leftShoulder, WeaponPart rightShoulder)
    {
        CurrentLower = lowerPart;
        CurrentUpper = upperPart;
        CurrentLeftArm = leftArm;
        CurrentRightArm = rightArm;
        CurrentLeftShoulder = leftShoulder;
        CurrentRightShoulder = rightShoulder;

        ModuleStatus = new ModuleStatus(lowerPart, upperPart, leftArm, rightArm, leftShoulder, rightShoulder);

        if (GetComponent<PlayerStateMachine>() == null)
            return;

        PlayerStateMachine = GetComponent<PlayerStateMachine>();
        transform.position = GameObject.Find("@PlayerSpawn").transform.position;

        IsPlayable = true;        
        TiltController = new WeaponTiltController(this);
        LockOnSystem.Setup(this);
        Skill = new RepairKit();
    }
}
