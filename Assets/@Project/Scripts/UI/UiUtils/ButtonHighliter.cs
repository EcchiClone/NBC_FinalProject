using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHighlighter : MonoBehaviour
{
    public Button[] buttons;
    public Sprite normalSprite;
    public Sprite selectedSprite;
    private Button selectedButton;

    void Awake()
    {
        foreach (var button in buttons)
        {
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

            // Pointer Down 이벤트
            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
            trigger.triggers.Add(pointerDown);

            // Pointer Up 이벤트
            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data, button); });
            trigger.triggers.Add(pointerUp);
        }
    }

    private void OnPointerDown(PointerEventData eventData)
    {
    }

    private void OnPointerUp(PointerEventData eventData, Button button)
    {
        if (eventData.pointerPress == button.gameObject)
        {
            if (selectedButton != button) 
            {
                SelectButton(button);
            }
        }
    }

    void SelectButton(Button button)
    {
        if (selectedButton != null)
        {
            ChangeButtonSprite(selectedButton, normalSprite);
        }

        ChangeButtonSprite(button, selectedSprite);
        selectedButton = button; 
    }

    private void ChangeButtonSprite(Button button, Sprite newSprite)
    {
        button.image.sprite = newSprite;
        button.image.DOFade(1, 0.5f);
    }

    void OnDisable()
    {
        DOTween.KillAll();
    }
}
