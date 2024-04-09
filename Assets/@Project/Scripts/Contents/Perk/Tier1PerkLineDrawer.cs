using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tier1PerkLineDrawer : MonoBehaviour
{
    private LineRenderer _line;
    private PerkVarBehaviour _var;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _var = GetComponent<PerkVarBehaviour>();
    }

    private void Start()
    {
        LineToOrigin();
        SetDistance();
    }

    private void LineToOrigin()
    {
        _line.widthMultiplier = 10f;
        _line.SetPosition(0, new Vector3(0f, 0f, -1f));
        _line.SetPosition(1, new Vector3(transform.position.x, transform.position.y, -1f));
    }

    private void SetDistance()
    {
        _var.distance = 0f;
    }

}
