using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkVarBehaviour : MonoBehaviour
{
    private PerkTier _tier;
    private int _idx;

    private PerkInfo _perkInfo;
    private int _contentIdx;
    private bool _isActive;

    private ContentInfo _contentInfo;
    private string _name;
    private string _description;

    public float distance;

    private void Awake()
    {
        GetVarsFromManager();
    }

    private void Start()
    {
        GetInfosFromManager();
        GetContentsFromManager();
    }

    private void GetVarsFromManager()
    {
        _tier = PerkManager.Instance.PointerTier;
        _idx = PerkManager.Instance.PointerIdx;
    }

    private void GetInfosFromManager()
    {
        _perkInfo = PerkManager.Instance.GetPerkInfo(_tier, _idx);
        _contentIdx = _perkInfo.ContentIdx;
        _isActive = _perkInfo.IsActive;
    }

    private void GetContentsFromManager()
    {
        _contentInfo = PerkManager.Instance.GetContentInfo(_tier, _contentIdx);
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
}
