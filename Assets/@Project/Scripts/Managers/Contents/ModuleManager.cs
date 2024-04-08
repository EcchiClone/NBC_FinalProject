using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class ModuleManager
{
    // ToDo - 게임 시작 시 Resources(또는 다른 위치)폴더 내 모든 파츠 부위에 맞게 자료구조에 담기.
    // ToDo - 마지막으로 저장된 파츠 정보를 토대로 현재 Module 정보 생성에 사용.
    // ToDo - MainScene의 Module 창에서 파츠 변경 시 갱신된 정보를 저장.

    // Think - 굳이 Key로 Type을 쓸 필요가 있을까?
    private Dictionary<Type, List<BasePart>> _modules = new Dictionary<Type, List<BasePart>>();

    #region Events
    public event Action<PartData> OnUpperChange;
    public event Action<PartData> OnLowerChange;
    public event Action<PartData> OnLeftArmChange;
    public event Action<PartData> OnRightArmChange;
    public event Action<PartData> OnLeftShoulderChange;
    public event Action<PartData> OnRightSoulderChange;
    public event Action<string, string> OnInfoChange;

    public void CallUpperPartChange(PartData part) => OnUpperChange?.Invoke(part);
    public void CallLowerPartChange(PartData lower) => OnLowerChange?.Invoke(lower);
    public void CallLeftArmPartChange(PartData lower) => OnLeftArmChange?.Invoke(lower);
    public void CallRightArmPartChange(PartData lower) => OnRightArmChange?.Invoke(lower);
    public void CallLeftShoulderPartChange(PartData lower) => OnLeftShoulderChange?.Invoke(lower);
    public void CallRightShoulderPartChange(PartData lower) => OnRightSoulderChange?.Invoke(lower);
    public void CallInfoChange(string name, string desc) => OnInfoChange?.Invoke(name, desc);
    #endregion

    public Module CurrentModule { get; private set; }
    public LowerPart CurrentLowerPart { get; private set; }
    public UpperPart CurrentUpperPart { get; private set; }
    public ArmsPart CurrentLeftArmPart { get; private set; }
    public ArmsPart CurrentRightArmPart { get; private set; }
    public ShouldersPart CurrentLeftShoulderPart { get; private set; }
    public ShouldersPart CurrentRightShoulderPart { get; private set; }

    public int LowerPartsCount { get; private set; }
    public int UpperPartsCount { get; private set; }
    public int ArmWeaponPartsCount { get; private set; }
    public int ShoulderWeaponPartsCount { get; private set; }

    public int CurrentLowerIndex { get; private set; }
    public int CurrentUpperIndex { get; private set; }
    public int CurrentLeftArmIndex { get; private set; }
    public int CurrentRightArmIndex { get; private set; }
    public int CurrentLeftShoulderIndex { get; private set; }
    public int CurrentRightShoulderIndex { get; private set; }

    public void Init() // 게임 시작 시 Resources 폴더 내 초기 파츠 담기.
    {
        InitData initData = new InitData();
        
        List<BasePart> lowerParts = new List<BasePart>();
        List<BasePart> upperParts = new List<BasePart>();
        List<BasePart> armWeaponParts = new List<BasePart>();
        List<BasePart> shoulderWeaponParts = new List<BasePart>();

        InitAddDict<LowerPart>(initData.LowerPartId, lowerParts);
        InitAddDict<UpperPart>(initData.UpperPartId, upperParts);
        InitAddDict<ArmsPart>(initData.ArmWeaponPartId, armWeaponParts);
        InitAddDict<ShouldersPart>(initData.ShoulderWeaponPartId, shoulderWeaponParts);

        LowerPartsCount = lowerParts.Count;
        UpperPartsCount = upperParts.Count;
        ArmWeaponPartsCount = armWeaponParts.Count;
        ShoulderWeaponPartsCount = shoulderWeaponParts.Count;

        _modules.Add(typeof(LowerPart), lowerParts);
        _modules.Add(typeof(UpperPart), upperParts);
        _modules.Add(typeof(ArmsPart), armWeaponParts);
        _modules.Add(typeof(ShouldersPart), shoulderWeaponParts);
    }

    private void InitAddDict<T>(List<int> idList, List<BasePart> partList) where T : BasePart
    {
        foreach (var id in idList)
        {
            PartData data = Managers.Data.GetPartData(id);
            T part = Resources.Load<T>(data.Prefab_Path);
            part.SetID(id);

            partList.Add(part);
        }
    }

    // Player 또는 Selector 빈 모듈 생성 : Scene Script에서 처리하는게 맞다고 봄.
    public void CreatePlayerModule() => CreateEmptyModule("Prefabs/Parts/Player_Module");
    public void CreateSelectorModule() => CreateEmptyModule("Prefabs/Parts/Selector_Module");

    public void CreateEmptyModule(string path)
    {
        GameObject emptyModule = Resources.Load<GameObject>(path);
        CurrentModule = UnityEngine.Object.Instantiate(emptyModule).GetComponent<Module>();

        AssembleModule(CurrentModule.LowerPosition);
    }

    public void AssembleModule(Transform createPosition) // Lower - Upper 순차적 생성 및 Pivot 할당.
    {
        // Lower 생성
        CurrentLowerPart = CreatePart<LowerPart>(PartsType.Lower, createPosition, CurrentLowerIndex);

        // Upper 생성
        CurrentUpperPart = CreatePart<UpperPart>(PartsType.Upper, CurrentLowerPart.UpperPositions, CurrentUpperIndex);

        // Weapon 생성
        CurrentLeftArmPart = CreatePart<ArmsPart>(PartsType.Weapon_Arm_L, CurrentUpperPart.WeaponPositions[(int)UpperPart.WeaponType.LeftArm], CurrentLeftArmIndex);
        CurrentRightArmPart = CreatePart<ArmsPart>(PartsType.Weapon_Arm_R, CurrentUpperPart.WeaponPositions[(int)UpperPart.WeaponType.RightArm], CurrentRightArmIndex);
        CurrentLeftShoulderPart = CreatePart<ShouldersPart>(PartsType.Weapon_Shoulder_L, CurrentUpperPart.WeaponPositions[(int)UpperPart.WeaponType.LeftShoulder], CurrentLeftShoulderIndex);
        CurrentRightShoulderPart = CreatePart<ShouldersPart>(PartsType.Weapon_Shoulder_R, CurrentUpperPart.WeaponPositions[(int)UpperPart.WeaponType.RightShoulder], CurrentRightShoulderIndex);

        CurrentModule.Setup(CurrentLowerPart, CurrentUpperPart, CurrentLeftArmPart, CurrentRightArmPart, CurrentLeftShoulderPart, CurrentRightShoulderPart);
    }

    private T CreatePart<T>(PartsType type, Transform createPosition, int index = 0) where T : BasePart
    {
        List<BasePart> parts;
        if (_modules.TryGetValue(typeof(T), out parts) == false)
        {
            Debug.Log($"파츠 정보가 없습니다. {typeof(T).Name}");
            return null;
        }

        int id = parts[index].ID;

        GameObject go = UnityEngine.Object.Instantiate(parts[index].gameObject, createPosition);
        T part = go.GetComponent<T>();
        part.SetID(id);
        part.Setup(type,CurrentModule);

        return part;
    }

    public void ChangePart(int index, PartsType partsType) // 디자인 패턴을 사용해 만들어보자... 할 수 있으면 ㅠ
    {
        switch (partsType)
        {
            case PartsType.Lower:
                if (CurrentLowerIndex == index) return;

                CurrentUpperPart.transform.SetParent(CurrentModule.transform); // 상체 부모 오브젝트 변경
                UnityEngine.Object.DestroyImmediate(CurrentLowerPart.gameObject); // 즉시 파괴

                CurrentLowerPart = CreatePart<LowerPart>(partsType, CurrentModule.LowerPosition, index); // 하체 생성
                CurrentLowerIndex = index;

                CurrentUpperPart.transform.SetParent(CurrentLowerPart.UpperPositions); // 상체 부모 오브젝트 변경
                CurrentUpperPart.transform.localPosition = Vector3.zero; // 상체 위치 조정
                break;
            case PartsType.Upper:
                if (CurrentUpperIndex == index) return;

                UnityEngine.Object.DestroyImmediate(CurrentUpperPart.gameObject);

                CurrentUpperPart = CreatePart<UpperPart>(partsType, CurrentLowerPart.UpperPositions, index);                
                CurrentUpperIndex = index;
                break;
            case PartsType.Weapon_Arm_L:
                if (CurrentLeftArmIndex == index) return;

                UnityEngine.Object.DestroyImmediate(CurrentLeftArmPart.gameObject);

                CurrentLeftArmPart = CreatePart<ArmsPart>(partsType, CurrentUpperPart.WeaponPositions[(int)UpperPart.WeaponType.LeftArm], index);                
                CurrentLeftArmIndex = index;
                break;
            case PartsType.Weapon_Arm_R:
                if (CurrentRightArmIndex == index) return;

                UnityEngine.Object.DestroyImmediate(CurrentRightArmPart.gameObject);

                CurrentRightArmPart = CreatePart<ArmsPart>(partsType, CurrentUpperPart.WeaponPositions[(int)UpperPart.WeaponType.RightArm], index);                
                CurrentRightArmIndex = index;
                break;
            case PartsType.Weapon_Shoulder_L:
                if (CurrentLeftShoulderIndex == index) return;

                UnityEngine.Object.DestroyImmediate(CurrentLeftShoulderPart.gameObject);

                CurrentLeftShoulderPart = CreatePart<ShouldersPart>(partsType, CurrentUpperPart.WeaponPositions[(int)UpperPart.WeaponType.LeftShoulder], index);                
                CurrentLeftShoulderIndex = index;
                break;
            case PartsType.Weapon_Shoulder_R:
                if (CurrentRightShoulderIndex == index) return;

                UnityEngine.Object.DestroyImmediate(CurrentRightShoulderPart.gameObject);

                CurrentRightShoulderPart = CreatePart<ShouldersPart>(partsType, CurrentUpperPart.WeaponPositions[(int)UpperPart.WeaponType.RightShoulder], index);                
                CurrentRightShoulderIndex = index;
                break;
        }
    }

    public T GetPartOfIndex<T>(int index) where T : BasePart
    {
        if (!_modules.TryGetValue(typeof(T), out List<BasePart> parts))
        {
            Debug.Log($"type 잘못 입력{typeof(T)}");
            return null;
        }

        if (index >= parts.Count || parts[index] == null)
        {
            Debug.Log($"해당 번호의 파츠는 없어용{index}");
            return null;
        }

        return parts[index] as T;
    }

    public void Clear()
    {
        CurrentModule = null;
    }
}
