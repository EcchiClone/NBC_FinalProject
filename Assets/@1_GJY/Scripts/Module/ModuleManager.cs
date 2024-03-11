using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager
{
    // ToDo - 게임 시작 시 Resources(또는 다른 위치)폴더 내 모든 파츠 부위에 맞게 자료구조에 담기.
    // ToDo - 마지막으로 저장된 파츠 정보를 토대로 현재 Module 정보 생성에 사용.
    // ToDo - MainScene의 Module 창에서 파츠 변경 시 갱신된 정보를 저장.

    private Dictionary<Type, BasePart[]> _modules = new Dictionary<Type, BasePart[]>();

    public LowerPart CurrentLowerPart { get; private set; }
    public UpperPart CurrentUpperPart { get; private set; }

    public void Init() // 게임 시작 시 Resources 폴더 내 모든 파츠 담기.
    {        
        BasePart[] lowerParts = Resources.LoadAll<LowerPart>("TestPrefab/Parts/Lower");
        BasePart[] upperParts = Resources.LoadAll<UpperPart>("TestPrefab/Parts/Upper");

        _modules.Add(typeof(LowerPart), lowerParts);
        _modules.Add(typeof(UpperPart), upperParts);
    }

    public void CreateEmptyModule() // PlayScene 진입 시 플레이어가 사용할 빈 모듈 생성 : Scene Script에서 처리하는게 맞다고 봄.
    {
        GameObject emptyModule = Resources.Load<GameObject>("TestPrefab/Parts/Player_Module");

        UnityEngine.Object.Instantiate(emptyModule);
    }

    public void CreateModule(Transform createPosition, Module module) // Lower - Upper 순차적 생성 및 Pivot 할당.
    {
        // Lower 생성 및 Upper Pivot 할당.
        LowerPart lower = CreateAndSetupPart<LowerPart>(createPosition);        
        Transform upperPivot = FindPivot(lower.transform);
        module.SetPivot(upperPivot);

        // Upper 생성 및 Weapon Pivot 할당.
        UpperPart upper = CreateAndSetupPart<UpperPart>(upperPivot);
        Transform weaponPivot = FindPivot(upper.transform);
        module.SetPivot(weaponPivot);
    }

    private Transform FindPivot(Transform part)
    {
        Pivot pivot = part.GetComponentInChildren<Pivot>();
        if(pivot == null)
        {
            Debug.Log($"Pivot이 존재하지 않습니다. : {part.name}");
            return null;
        }

        return pivot.transform;
    }

    private T CreateAndSetupPart<T>(Transform createPosition , int index = 0) where T : BasePart
    {
        BasePart[] parts;
        if (_modules.TryGetValue(typeof(T), out parts) == false)
        {
            Debug.Log("Upper 파츠 정보가 없습니다.");
            return null;
        }
        Debug.Log(parts.Length);

        GameObject go = UnityEngine.Object.Instantiate(parts[index].gameObject, createPosition);
        T part = go.GetComponent<T>();
        part.Setup();

        return part;
    }

    public void Clear()
    {

    }
}
