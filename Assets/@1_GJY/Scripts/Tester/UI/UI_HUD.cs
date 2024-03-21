using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUD : UI_Scene
{
    [Header("Aim")]
    [SerializeField] GameObject _crossHair;
    [SerializeField] GameObject _lockOnIndicator;

    [Header("AP")]
    [SerializeField] Image _apFill;
    [SerializeField] TextMeshProUGUI _apValueText;

    private Transform _target;    

    protected override void Init()
    {
        base.Init();

        LockOnSystem.OnLockOn += GetTargetedEnemy;
        LockOnSystem.OnRelease += ReleaseTarget;
        PlayerStatus.OnChangeArmorPoint += ChangeAPValue;
    }

    private void GetTargetedEnemy(Transform target)
    {
        _target = target;
        _crossHair.SetActive(false);
        _lockOnIndicator.SetActive(true);
    }

    private void ReleaseTarget()
    {
        _target = null;
        _crossHair.SetActive(true);
        _lockOnIndicator.SetActive(false);
    }

    private void ChangeAPValue(float totalAP, float remainAP)
    {
        _apFill.fillAmount = remainAP / totalAP;
        _apValueText.text = $"{(int)remainAP}";
    }

    private void Update()
    {
        if (!_lockOnIndicator.activeSelf || _target == null)
            return;

        _lockOnIndicator.transform.position = Camera.main.WorldToScreenPoint(_target.position);        
    }
}
