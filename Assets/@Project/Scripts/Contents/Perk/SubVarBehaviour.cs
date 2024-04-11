using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SubVarBehaviour : MonoBehaviour
{
    private SubPerkLineDrawer _drawer;
    private Transform _parent;
    private Transform _transform;

    private PerkTier _tier;
    private int _idx;
    private int _subIdx;

    private PerkInfo _perkInfo;
    private int _contentIdx;

    private ContentInfo _contentInfo;
    private SubPerkInfo _subInfo;

    private PerkInfo _parentInfo;
    private bool _isParentActive;

    private void Awake()
    {
        _drawer = GetComponent<SubPerkLineDrawer>();
        _parent = transform.parent;
        _transform = transform;
        _parentInfo = transform.parent.GetComponent<PerkVarBehaviour>().ReturnPerkInfo();

        GetVarsFromManager();
        ChangeSignOfTransformZ();

        _isParentActive = false;
    }

    private void FixedUpdate()
    {
        CheckParentIsActive();
    }

    private void GetVarsFromManager()
    {
        _tier = PerkManager.Instance.PointerTier;
        _idx = PerkManager.Instance.PointerIdx;
        _subIdx = PerkManager.Instance.PointerSubIdx;
        GetInfosFromManager();
        GetSubInfos();
        GetContentsFromManager();
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
    }

    private void GetContentsFromManager()
    {
        _contentInfo = PerkManager.Instance.GetContentInfo(PerkTier.SUB, _contentIdx);
    }

    private void CheckParentIsActive()
    {
        if (!_isParentActive)
        {
            if (_parentInfo.IsActive)
            {
                SetCurrentPerkActive();
                _isParentActive = true;
            }
            else
            {
                _isParentActive = false;
            }
        }
    }

    private void SetCurrentPerkActive()
    {
        ChangeSignOfTransformZ();
        _drawer.LineToMainPerk(_parent.position);
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

    public SubPerkInfo ReturnSubInfo()
    {
        return _subInfo;
    }
}
