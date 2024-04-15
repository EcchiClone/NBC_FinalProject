using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_SettingsOnStagePopup : UI_Popup
{
    enum Buttons
    {
        Btn_BackToMain1,
        Btn_BackToMain2,
        Btn_BackToMain3,
        Btn_BackToMain4,
        Btn_BackToMain5,
        Btn_BackToMain6,
        Background,
    }
    enum Sliders
    {
        Slider_Sound1,
        Slider_Sound2,
        Slider_Sound3,
        Slider_Sound4,
        Slider_Quality,
        Slider_DPI,
    }
    enum TextMeshProUGUIs
    {
        Text_Sound1,
        Text_Sound2,
        Text_Sound3,
        Text_Sound4,
        Text_Quality,
        Text_DPI,
    }
    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));

        SetElementEvents();

        try
        {
            GetButton((int)Buttons.Btn_BackToMain1).onClick.AddListener(ClosePanel);
            GetButton((int)Buttons.Btn_BackToMain2).onClick.AddListener(ClosePanel);
            GetButton((int)Buttons.Btn_BackToMain3).onClick.AddListener(ClosePanel);
            GetButton((int)Buttons.Btn_BackToMain4).onClick.AddListener(ClosePanel);
            GetButton((int)Buttons.Btn_BackToMain5).onClick.AddListener(ClosePanel);
            GetButton((int)Buttons.Btn_BackToMain6).onClick.AddListener(ClosePanel);
        }
        catch { }
        GetButton((int)Buttons.Background).onClick.AddListener(ClosePanel);

    }
    private void SetElementEvents()
    {
        Slider slider; // 인스턴스 돌려쓰기

        // SOUND
        slider = Get<Slider>((int)Sliders.Slider_Sound1);
        slider.onValueChanged.AddListener(SliderEvent_Sound1);
        //slider.value = AudioManager.instance.masterVolume;
        slider = Get<Slider>((int)Sliders.Slider_Sound2);
        slider.onValueChanged.AddListener(SliderEvent_Sound2);
        //slider.value = AudioManager.instance.musicVolume;
        slider = Get<Slider>((int)Sliders.Slider_Sound3);
        slider.onValueChanged.AddListener(SliderEvent_Sound3);
        //slider.value = AudioManager.instance.ambienceVolume;
        slider = Get<Slider>((int)Sliders.Slider_Sound4);
        slider.onValueChanged.AddListener(SliderEvent_Sound4);
        //slider.value = AudioManager.instance.SFXVolume;

        // GRAPHICS
        slider = Get<Slider>((int)Sliders.Slider_Quality);
        slider.onValueChanged.AddListener(SliderEvent_Quality);
        slider.value = QualitySettings.GetQualityLevel();
        slider = Get<Slider>((int)Sliders.Slider_DPI);
        slider.onValueChanged.AddListener(SliderEvent_DPI);
        //slider.value =  // 기존 값 가져오는 로직
    }

    public void SliderEvent_Sound1(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound1).text = ((int)value).ToString();
        //AudioManager.instance.masterVolume = value;

    }
    public void SliderEvent_Sound2(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound2).text = ((int)value).ToString();
        //AudioManager.instance.musicVolume = value;
    }
    public void SliderEvent_Sound3(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound3).text = ((int)value).ToString();
        //AudioManager.instance.ambienceVolume = value;
    }
    public void SliderEvent_Sound4(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound4).text = ((int)value).ToString();
        //AudioManager.instance.SFXVolume = value;
    }
    public void SliderEvent_Quality(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Quality).text = ((int)value).ToString();
        QualitySettings.SetQualityLevel((int)value);
        Debug.Log("해상도 조정: " + QualitySettings.names[(int)value]);
    }
    public void SliderEvent_DPI(float value)
    {
        float dpiScale = Mathf.Lerp(0.5f, 2.0f, value / 100.0f); // value를 0에서 100 사이로 가정
        CanvasScaler canvasScaler = FindObjectOfType<CanvasScaler>();
        if (canvasScaler != null)
        {
            canvasScaler.scaleFactor = dpiScale;
        }
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_DPI).text = ((int)value).ToString();
        Debug.Log("DPI 조정: " + dpiScale);
    }
    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
