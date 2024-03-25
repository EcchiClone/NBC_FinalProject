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


    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Scene에 적어도 하나의 FMODEvents가 존재하는 지 확인하세요.");
        }
        instance = this;
    }
}
