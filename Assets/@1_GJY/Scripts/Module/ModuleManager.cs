using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager
{
    // ToDo - 게임 시작 시 Resources(또는 다른 위치)폴더 내 모든 파츠 부위에 맞게 자료구조에 담기.
    // ToDo - 마지막으로 저장된 파츠 정보를 토대로 현재 Module 정보 생성에 사용.
    // ToDo - MainScene의 Module 창에서 파츠 변경 시 갱신된 정보를 저장.

    // Think - 굳이 Key로 Type을 쓸 필요가 있을까?
    private Dictionary<Type, BasePart[]> _modules = new Dictionary<Type, BasePart[]>();

    public Module CurrentModule { get; private set; }
    public LowerPart CurrentLowerPart { get; private set; }
    public UpperPart CurrentUpperPart { get; private set; }    

    public int LowerPartsCount { get; private set; }
    public int UpperPartsCount { get; private set; }

    public void Init() // 게임 시작 시 Resources 폴더 내 모든 파츠 담기.
    {        
        BasePart[] lowerParts = Resources.LoadAll<LowerPart>("TestPrefab/Parts/Lower");
        BasePart[] upperParts = Resources.LoadAll<UpperPart>("TestPrefab/Parts/Upper");

        LowerPartsCount = lowerParts.Length;
        UpperPartsCount = upperParts.Length;

        _modules.Add(typeof(LowerPart), lowerParts);
        _modules.Add(typeof(UpperPart), upperParts);
    }

    // Player 또는 Selector 빈 모듈 생성 : Scene Script에서 처리하는게 맞다고 봄.
    public void CreatePlayerModule() => CreateEmptyModule("TestPrefab/Parts/Player_Module"); 
    public void CreateSelectorModule() => CreateEmptyModule("TestPrefab/Parts/Selector_Module");

    public void CreateEmptyModule(string path)
    {
        GameObject emptyModule = Resources.Load<GameObject>(path);
        CurrentModule = emptyModule.GetComponent<Module>();

        UnityEngine.Object.Instantiate(emptyModule);
    }

    public void CreateModule(Transform createPosition, Module module) // Lower - Upper 순차적 생성 및 Pivot 할당.
    {
        CurrentModule = module;

        // Lower 생성 및 Upper Pivot 할당.
        CurrentLowerPart = CreatePart<LowerPart>(createPosition);        
        Transform upperPivot = FindPivot(CurrentLowerPart.transform);
        module.SetPivot(upperPivot);

        // Upper 생성 및 Weapon Pivot 할당.
        CurrentUpperPart = CreatePart<UpperPart>(upperPivot);
        Transform weaponPivot = FindPivot(CurrentUpperPart.transform);
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

    private T CreatePart<T>(Transform createPosition, int index = 0) where T : BasePart
    {
        BasePart[] parts;
        if (_modules.TryGetValue(typeof(T), out parts) == false)
        {
            Debug.Log("Upper 파츠 정보가 없습니다.");
            return null;
        }        

        GameObject go = UnityEngine.Object.Instantiate(parts[index].gameObject, createPosition);        
        T part = go.GetComponent<T>();
        part.Setup(CurrentModule);


        return part;
    }

    // 리팩토링요소 : UI에서 Generic 호출 구현이 가능하다면 ChangePart<T>와 RePositionUpperPart 메서드로 코드 간략화 가능... 하긴한데 Current 프로퍼티 쪽은 결국 남긴 하는데...

    public void ChangeLowerPart(int index)
    {
        if (CurrentLowerPart.ID == index) // 같은 파츠면 바꿀 필요X
            return;

        CurrentUpperPart.transform.SetParent(CurrentModule.transform); // 하체 파괴전 상체를 모듈 하위로 이동시켜 파괴방지.
        UnityEngine.Object.DestroyImmediate(CurrentLowerPart.gameObject); // 즉시 파괴로 Transform 참조를 이전 Lower가 아니게 방지.

        CurrentLowerPart = CreatePart<LowerPart>(CurrentModule.LowerPivot, index); // LowerPart를 새로 생성하여 CurrentLowerPart에 할당.
        CurrentModule.SetPivot(FindPivot(CurrentLowerPart.transform)); // 현재 Module의 Upper가 달릴 Pivot을 방금 생성한 Lower에서 찾아서 할당.

        CurrentUpperPart.transform.SetParent(CurrentModule.UpperPivot); // CurrentUpper는 남아있지만 부모Trasnform을 잃었으므로 현재 Module의 UpperPivot의 하위로 SetParent.
        CurrentUpperPart.transform.localPosition = Vector3.zero; // 로컬포지션을 Zero 세팅해 바뀐 하체에 장착되도록 한다.
    }

    public void ChangeUpperPart(int index)
    {
        if(CurrentUpperPart.ID == index) // 같은 파츠면 바꿀 필요X
            return;

        UnityEngine.Object.DestroyImmediate(CurrentUpperPart.gameObject); // 즉시 파괴로 Transform 참조를 이전 Upper가 아니게 방지.

        CurrentUpperPart = CreatePart<UpperPart>(CurrentModule.UpperPivot, index); 
        CurrentModule.SetPivot(FindPivot(CurrentUpperPart.transform));
    }

    public string GetPartName<T>(int index) where T : BasePart
    {
        BasePart[] parts;
        if(_modules.TryGetValue(typeof(T), out parts) == false)
        {
            Debug.LogWarning("파츠가 없는디?");
            return "없음";
        }

        return parts[index].name;
    }

    public void Clear()
    {
        CurrentModule = null;
    }
}
