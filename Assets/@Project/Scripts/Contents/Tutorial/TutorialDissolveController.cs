using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDissolveController : MonoBehaviour
{
    [SerializeField] Renderer[] _renderers;
    [SerializeField] Material _mat;
    [SerializeField] float _value;
    [SerializeField] float _time;
    [SerializeField] float _delay;
    [SerializeField] bool _inActive;
    [SerializeField] bool _outQuad;

    private void OnEnable()
    {
        if (!_inActive)
            Dissolve();
    }

    public void Dissolve()
    {
        foreach (var renderer in _renderers)
            renderer.material.DOFloat(_value, "_Split", _time).SetEase(_outQuad ? Ease.OutQuad : Ease.InQuad).OnComplete(() => { if (_inActive) gameObject.SetActive(false); });
    }
}
