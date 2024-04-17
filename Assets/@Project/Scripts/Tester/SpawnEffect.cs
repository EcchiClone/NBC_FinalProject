using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField] Renderer[] _renderer;

    [SerializeField] Material _dissolve;

    [SerializeField] float _splitValue;
    [SerializeField] float _dissolveTime;
    [SerializeField] float _delay;

    [SerializeField] bool _inActive;
    [SerializeField] bool _isEnemy;

    private void Start()
    {
        foreach (var renderer in _renderer)
            renderer.material.DOFloat(_splitValue, "_Split", _dissolveTime).SetEase(Ease.OutQuad).SetDelay(_delay).OnComplete(() =>
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
