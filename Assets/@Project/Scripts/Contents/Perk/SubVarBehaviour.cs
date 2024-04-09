using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubVarBehaviour : MonoBehaviour
{
    private PerkTier _tier;
    private int _idx;
    private int _subIdx;

    private PerkInfo _perkInfo;
    private int _contentIdx;
    private bool _isActive;

    private ContentInfo _contentInfo;
    private string _name;
    private string _description;

    private SubPerkInfo _subInfo;

    private void Awake()
    {
        GetVarsFromManager();
    }

    private void Start()
    {
        GetInfosFromManager();
        GetSubInfos();
        GetContentsFromManager();
    }

    private void GetVarsFromManager()
    {
        _tier = PerkManager.Instance.PointerTier;
        _idx = PerkManager.Instance.PointerIdx;
        _subIdx = PerkManager.Instance.PointerSubIdx;
    }

    private void GetInfosFromManager()
    {
        _perkInfo = PerkManager.Instance.GetPerkInfo(_tier, _idx);
    }

    private void GetSubInfos()
    {
        int realIdx = 0;

        realIdx = _perkInfo.subPerks.FindIndex(info => info.PositionIdx.Equals(_subIdx));
        _subInfo = _perkInfo.subPerks[realIdx];
        _contentIdx = _subInfo.ContentIdx;
        _isActive = _subInfo.IsActive;
    }

    private void GetContentsFromManager()
    {
        _contentInfo = PerkManager.Instance.GetContentInfo(PerkTier.SUB, _contentIdx);
        _name = _contentInfo.name;
        _description = _contentInfo.description;
    }


    public PerkInfo ReturnPerkInfo()
    {
        return _perkInfo;
    }

    public ContentInfo ReturnContentInfo()
    {
        return _contentInfo;
    }

    public SubPerkInfo ReturnSubInfo()
    {
        return _subInfo;
    }
}
