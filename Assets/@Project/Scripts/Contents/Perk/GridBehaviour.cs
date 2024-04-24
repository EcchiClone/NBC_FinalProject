using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridBehaviour : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Transform _cameraHolder;

    private Vector3 _defaultPos;
    private Vector2 _startingPos;
    private Vector2 _moveOffset;

    // Grid 클릭 시
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _defaultPos = new Vector3(-_cameraHolder.position.x, -_cameraHolder.position.y, -1000f);
        _startingPos = eventData.position;
    }

    // 드래그 중일 때
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _moveOffset = eventData.position - _startingPos;
        _cameraHolder.position = new Vector3(- (_defaultPos.x + _moveOffset.x), - (_defaultPos.y + _moveOffset.y), -1000f);
    }

}
