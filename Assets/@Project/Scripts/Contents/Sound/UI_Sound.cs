using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Sound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private UISoundType _thisType;
    private EventReference _thisEnterEvent;
    private EventReference _thisClickEvent;

    private void Start()
    {
        FindSoundType();
    }

    private void FindSoundType()
    {
        switch (_thisType)
        {
            default:
                _thisEnterEvent = FMODEvents.Instance.UI_Entered;
                _thisClickEvent = FMODEvents.Instance.UI_Clicked;
                break;

        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(_thisEnterEvent, transform.position);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(_thisClickEvent, transform.position);
    }
}
