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


    [field: Header("Perk/UI SFX")]
    [field: SerializeField] public EventReference UI_Clicked { get; private set; }
    [field: SerializeField] public EventReference UI_Entered { get; private set; }
    [field: SerializeField] public EventReference Perk_Denied { get; private set; }
    [field: SerializeField] public EventReference Perk_Released { get; private set; }


    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Scene에 적어도 하나의 FMODEvents가 존재하는 지 확인하세요.");
        }
        Instance = this;
    }
}
