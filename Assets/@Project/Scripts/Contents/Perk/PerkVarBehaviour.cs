using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkVarBehaviour : MonoBehaviour
{
    private PerkLineDrawer _drawer;

    private PerkTier _tier;
    private int _idx;

    private PerkInfo _perkInfo;
    private int _contentIdx;

    private ContentInfo _contentInfo;

    private float _distance;
    private Vector3 _prevPosition;

    private void Awake()
    {
        _drawer = GetComponent<PerkLineDrawer>();
        GetVarsFromManager();
    }

    private void Start()
    {
        FindPrevPerks(_perkInfo.Tier, ref _distance, ref _prevPosition);
        DrawLineToPrevPerk();
    }

    private void GetVarsFromManager()
    {
        _tier = PerkManager.Instance.PointerTier;
        _idx = PerkManager.Instance.PointerIdx;
        GetInfosFromManager();
        GetContentsFromManager();
    }

    private void GetInfosFromManager()
    {
        _perkInfo = PerkManager.Instance.GetPerkInfo(_tier, _idx);
        _contentIdx = _perkInfo.ContentIdx;
    }

    private void GetContentsFromManager()
    {
        _contentInfo = PerkManager.Instance.GetContentInfo(_tier, _contentIdx);
    }

    private void FindPrevPerks(PerkTier tier, ref float distance, ref Vector3 prevPosition)
    {
        GameObject[] prevPerks;

        switch (tier)
        {
            case PerkTier.TIER3: 
                prevPerks = GameObject.FindGameObjectsWithTag("Tier2");
                break;
            case PerkTier.TIER2:
                prevPerks = GameObject.FindGameObjectsWithTag("Tier1");
                break;
            default:
                prevPerks = null;
                break;
        }

        if (prevPerks != null)
        {
            float min = 0f;

            foreach (GameObject perk in prevPerks)
            {
                distance = Vector3.Distance(perk.transform.position, transform.position);

                if (min == 0f || distance < min)
                {
                    min = distance;
                    prevPosition = perk.transform.position;
                }
            }
        }
        else
        {
            distance = 0f;
            prevPosition = Vector3.zero;
        }

    }

    private void DrawLineToPrevPerk()
    {
        _drawer.LineToPrevPerk(_prevPosition);
    }

    public PerkInfo ReturnPerkInfo()
    {
        return _perkInfo;
    }

    public ContentInfo ReturnContentInfo()
    {
        return _contentInfo;
    }

    public float ReturnPrevDistance()
    {
        return _distance;
    }
}
