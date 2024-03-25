using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tier1PerkBehaviour : MonoBehaviour
{
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        LineToOrigin();
    }

    private void LineToOrigin()
    {
        _line.widthMultiplier = 50f;
        _line.SetPosition(0, Vector3.zero);
        _line.SetPosition(1, transform.position);
    }
}
