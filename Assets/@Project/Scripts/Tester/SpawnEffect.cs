using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField] Renderer[] _renderer;

    [SerializeField] Material _dissolve;

    [Header("# Dissolve")]
    [SerializeField] float _startValue;
    [SerializeField] float _endValue;
    [SerializeField] float _dissolveTime;
    [SerializeField] float _delay;
    [SerializeField] Ease ease = Ease.OutQuad;

    [Header("# Setting")]
    [SerializeField] bool _inActive;
    [SerializeField] bool _isEnemy;

    private bool _isCreate = false;

    private void OnEnable()
    {
        if (!_isCreate)
        {
            _isCreate = true;
            return;
        }

        foreach (var renderer in _renderer)
        {
            renderer.material.SetFloat("_Split", _startValue);
            renderer.material.DOFloat(_endValue, "_Split", _dissolveTime).SetEase(ease).SetDelay(_delay).OnComplete(() =>
            {
                if (_inActive)
                    gameObject.SetActive(false);
                if (_isEnemy)
                {
                    Entity enemy = gameObject.GetComponent<Entity>();
                    enemy.Activate();
                }
            });
        }   
    }
}
