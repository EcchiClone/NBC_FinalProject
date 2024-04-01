using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPerkBehaviour : MonoBehaviour
{
    private LineRenderer _line;
    private Transform _parent;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _parent = transform.parent;
    }

    private void Start()
    {
        LineToMainPerk();
    }

    private void LineToMainPerk()
    {
        _line.widthMultiplier = 10f;
        _line.SetPosition(0, new Vector3(_parent.position.x, _parent.position.y, -1f));
        _line.SetPosition(1, new Vector3(transform.position.x, transform.position.y, -1f));
    }

}
