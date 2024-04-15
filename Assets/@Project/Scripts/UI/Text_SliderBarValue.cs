using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Text_SliderBarValue : MonoBehaviour
{
    // 현재 미사용이지만, 슬라이더에 추가하여 가볍게 사용 가능한 리틀리틀 스크립트
    [SerializeField] TextMeshProUGUI TextMeshProUGUI;
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        TextMeshProUGUI.text = slider.value.ToString();
    }
    public void OnValueChangedSlider()
    {
        TextMeshProUGUI.text = slider.value.ToString();
    }
}
