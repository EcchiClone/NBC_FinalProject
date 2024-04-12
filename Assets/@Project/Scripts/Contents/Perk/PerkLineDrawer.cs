using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PerkLineDrawer : MonoBehaviour
{
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    public void LineToPrevPerk(Vector3 prevPosition)
    {
        _line.widthMultiplier = 10f;
        _line.SetPosition(0, new Vector3(prevPosition.x, prevPosition.y, -1f));
        _line.SetPosition(1, new Vector3(transform.position.x, transform.position.y, -1f));
    }
}
