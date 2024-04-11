using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPerkLineDrawer : MonoBehaviour
{
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    public void LineToMainPerk(Vector3 parent)
    {
        _line.widthMultiplier = 10f;
        _line.SetPosition(0, new Vector3(parent.x, parent.y, -1f));
        _line.SetPosition(1, new Vector3(transform.position.x, transform.position.y, -1f));
    }

}
