using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scenes Scenes { get; protected set; } = Define.Scenes.Unknown;

    private void Awake()
    {
        Init();
    }

    public virtual void Init() { }

    public abstract void Clear();
}
