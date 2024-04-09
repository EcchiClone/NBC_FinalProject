using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tier3PerkLineDrawer : MonoBehaviour
{
    private LineRenderer _line;

    private GameObject[] _tier2Perks;
    private Vector3 _minPerk;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _tier2Perks = GameObject.FindGameObjectsWithTag("Tier2");
    }

    private void Start()
    {
        FindMinDistanceOfTier2Perks();
        LineToTier2Perk();
    }

    private void FindMinDistanceOfTier2Perks()
    {
        float min = 0f;
        float distance;

        foreach (GameObject perk in  _tier2Perks)
        {
            distance = Vector3.Distance(perk.transform.position, transform.position);

            if (min == 0f || distance < min)
            {
                min = distance;
                _minPerk = perk.transform.position;
            }
        }
    }

    private void LineToTier2Perk()
    {
        _line.widthMultiplier = 10f;
        _line.SetPosition(0, new Vector3(_minPerk.x, _minPerk.y, -1f));
        _line.SetPosition(1, new Vector3(transform.position.x, transform.position.y, -1f));
    }
}
