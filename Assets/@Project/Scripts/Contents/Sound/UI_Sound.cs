using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            case UISoundType.DENY:
                _thisEnterEvent = FMODEvents.Instance.UI_Entered;
                _thisClickEvent = FMODEvents.Instance.Perk_Denied;
                break;
            case UISoundType.MODULE:
                _thisEnterEvent = FMODEvents.Instance.UI_Entered;
                _thisClickEvent = FMODEvents.Instance.Weapon_Changed;
                break;
            case UISoundType.ACHIEVEMENT_REWARD:
                Button b = GetComponent<Button>();
                if (b!=null)
                {
                    if (b.interactable)
                    {
                        _thisEnterEvent = FMODEvents.Instance.UI_Entered;
                        _thisClickEvent = FMODEvents.Instance.Achivement_Success;
                    }
                }
                break;
            case UISoundType.ACHIEVEMENT_ALARM:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Achivement_Success, transform.position);
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
