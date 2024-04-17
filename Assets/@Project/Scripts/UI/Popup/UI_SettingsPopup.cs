using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_SettingsPopup : UI_Popup
{
    enum Buttons
    {
        Button_BackToMain,
        //Btn_BackToMain2, // 더미 뒤로가기 버튼
        //Btn_BackToMain3,
        //Btn_BackToMain4,
        //Btn_BackToMain5,
        //Btn_BackToMain6,
        Background,

        //Button_Reset1,  // -> 데이터 초기화를 위한 버튼
        //Button_Reset2,  // -> 퍼크 초기화를 위한 버튼
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
            GetButton((int)Buttons.Button_BackToMain).onClick.AddListener(BackToMain);
            GetButton((int)Buttons.Background).onClick.AddListener(BackToMain);
            //GetButton((int)Buttons.Btn_BackToMain2).onClick.AddListener(BackToMain); // 더미 버튼, 정립 시 삭제
            //GetButton((int)Buttons.Btn_BackToMain3).onClick.AddListener(BackToMain);
            //GetButton((int)Buttons.Btn_BackToMain4).onClick.AddListener(BackToMain);
            //GetButton((int)Buttons.Btn_BackToMain5).onClick.AddListener(BackToMain);
            //GetButton((int)Buttons.Btn_BackToMain6).onClick.AddListener(BackToMain);
        }
        catch { }



    }
    private void SetElementEvents()
    {
        Slider slider; // 인스턴스 돌려쓰기

        // SOUND
        slider = Get<Slider>((int)Sliders.Slider_Sound1);
        slider.onValueChanged.AddListener(SliderEvent_Sound1);
        slider.value = AudioManager.Instance.masterVolume;
        slider = Get<Slider>((int)Sliders.Slider_Sound2);
        slider.onValueChanged.AddListener(SliderEvent_Sound2);
        slider.value = AudioManager.Instance.musicVolume;
        slider = Get<Slider>((int)Sliders.Slider_Sound3);
        slider.onValueChanged.AddListener(SliderEvent_Sound3);
        slider.value = AudioManager.Instance.ambienceVolume;
        slider = Get<Slider>((int)Sliders.Slider_Sound4);
        slider.onValueChanged.AddListener(SliderEvent_Sound4);
        slider.value = AudioManager.Instance.SFXVolume;

        // GRAPHICS
        slider = Get<Slider>((int)Sliders.Slider_Quality);
        slider.onValueChanged.AddListener(SliderEvent_Quality);
        slider.value = QualitySettings.GetQualityLevel();
        slider = Get<Slider>((int)Sliders.Slider_DPI);
        slider.onValueChanged.AddListener(SliderEvent_DPI);
        //slider.value =  // 기존 값 가져오는 로직

        // DATA
        //GetButton((int)Buttons.Button_Reset1).onClick.AddListener(Reset1Btn); // 게임 초기화, 미구현
    }

    public void SliderEvent_Sound1(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound1).text = ((int)value).ToString();
        AudioManager.Instance.masterVolume = value;

    }
    public void SliderEvent_Sound2(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound2).text = ((int)value).ToString();
        AudioManager.Instance.musicVolume = value;
    }
    public void SliderEvent_Sound3(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound3).text = ((int)value).ToString();
        AudioManager.Instance.ambienceVolume = value;
    }
    public void SliderEvent_Sound4(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound4).text = ((int)value).ToString();
        AudioManager.Instance.SFXVolume = value;
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
    //private void Reset1Btn() // 게임 초기화, 미구현
    //{
    //    // 팝업(()=>Data1 삭제 로직{});
    //}

    //private void Reset2Btn() // 퍼크 초기화, 미구현
    //{
    //    // 팝업(()=>Data2 삭제 로직{});
    //}
    private void BackToMain()
    {
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
