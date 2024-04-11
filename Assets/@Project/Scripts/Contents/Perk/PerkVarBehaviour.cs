using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkVarBehaviour : MonoBehaviour
{
    private PerkLineDrawer _drawer;
    private Transform _transform;

    private PerkTier _tier;
    private int _idx;

    private PerkInfo _perkInfo;
    private int _contentIdx;

    private ContentInfo _contentInfo;

    private float _distance;
    private Vector3 _prevPosition;
    private PerkInfo _prevInfo;
    private bool _isPrevActive;

    private void Awake()
    {
        _drawer = GetComponent<PerkLineDrawer>();
        _transform = GetComponent<Transform>();
        GetVarsFromManager();
        ChangeSignOfTransformZ();

        _isPrevActive = false;
    }

    private void Start()
    {
        FindPrevPerks(_perkInfo.Tier, ref _distance, ref _prevPosition, ref _prevInfo);
    }

    private void FixedUpdate()
    {
        CheckPrevPerkIsActive(_prevInfo);
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

    private void FindPrevPerks(PerkTier tier, ref float distance, ref Vector3 prevPosition, ref PerkInfo prevInfo)
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
            default: // PerkTier.TIER1 & PerkTier.SUB 일 때
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
                    prevInfo = perk.GetComponent<PerkVarBehaviour>().ReturnPerkInfo();
                }
            }
        }
        else
        {
            distance = 0f;
            prevPosition = Vector3.zero;
            prevInfo = null;
        }

    }

    private void CheckPrevPerkIsActive(PerkInfo prevInfo)
    {
        if (!_isPrevActive)
        {
            if (prevInfo != null)
            {
                PerkInfo updatedInfo = PerkManager.Instance.GetPerkInfo(prevInfo.Tier, prevInfo.PositionIdx);

                if (updatedInfo.IsActive)
                {
                    SetCurrentPerkActive();
                    _isPrevActive = true;
                }
                else
                {
                    _isPrevActive = false;
                }
            }
            else
            {
                SetCurrentPerkActive();
                _isPrevActive = true;
            }
        }
    }

    private void SetCurrentPerkActive()
    {
        ChangeSignOfTransformZ();
        DrawLineToPrevPerk();
    }

    private void DrawLineToPrevPerk()
    {
        _drawer.LineToPrevPerk(_prevPosition);
    }

    private void ChangeSignOfTransformZ()
    {
        float newZ = _transform.position.z * -1f;
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, newZ);
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
