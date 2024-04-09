using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tier2PerkLineDrawer : MonoBehaviour
{
    private LineRenderer _line;
    private PerkVarBehaviour _var;

    private GameObject[] _tier1Perks;
    private Vector3 _minPerk;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _var = GetComponent<PerkVarBehaviour>();
        _tier1Perks = GameObject.FindGameObjectsWithTag("Tier1");
    }

    private void Start()
    {
        FindMinDistanceOfTier1Perks();
        LineToTier1Perk();
        SetDistance();
    }

    private void FindMinDistanceOfTier1Perks()
    {
        float min = 0f;
        float distance;

        foreach (GameObject perk in  _tier1Perks)
        {
            distance = Vector3.Distance(perk.transform.position, transform.position);

            if (min == 0f || distance < min)
            {
                min = distance;
                _minPerk = perk.transform.position;
            }
        }
    }

    private void LineToTier1Perk()
    {
        _line.widthMultiplier = 10f;
        _line.SetPosition(0, new Vector3(_minPerk.x, _minPerk.y, -1f));
        _line.SetPosition(1, new Vector3(transform.position.x, transform.position.y, -1f));
    }

    private void SetDistance()
    {
        _var.distance = Vector3.Distance(_minPerk, transform.position);
    }
}
