using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [field: SerializeField] public Transform LowerPosition { get; private set; }        

    public LowerPart CurrentLower { get; private set; }
    public UpperPart CurrentUpper { get; private set; }
    public ArmsPart CurrentLeftArm { get; private set; }
    public ArmsPart CurrentRightArm { get; private set; }
    public ShouldersPart CurrentLeftShoulder { get; private set; }
    public ShouldersPart CurrentRightShoulder {  get; private set; }

    public ModuleStatus ModuleStatus { get; private set; }
    public WeaponTiltController TiltController { get; private set; }
    [field: SerializeField] public LockOnSystem LockOnSystem { get; private set; }

    public ISkill Skill { get; private set; }

    public bool IsPlayable { get; private set; }

    public void Setup(LowerPart lowerPart, UpperPart upperPart, ArmsPart leftArm, ArmsPart rightArm, ShouldersPart leftShoulder, ShouldersPart rightShoulder)
    {
        CurrentLower = lowerPart;
        CurrentUpper = upperPart;
        CurrentLeftArm = leftArm;
        CurrentRightArm = rightArm;
        CurrentLeftShoulder = leftShoulder;
        CurrentRightShoulder = rightShoulder;

        if (GetComponent<PlayerStateMachine>() == null)
            return;

        transform.position = GameObject.Find("@PlayerSpawn").transform.position;

        IsPlayable = true;
        ModuleStatus = new ModuleStatus(lowerPart, upperPart, leftArm, rightArm, leftShoulder, rightShoulder);
        TiltController = new WeaponTiltController(this);
        LockOnSystem.Setup(this);
        Skill = new RepairKit();
    }
}
