using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using static UnityEngine.Rendering.DebugUI;

public class UI_SettingsPopup : UI_Popup
{
    enum Buttons
    {
        Button_BackToMain,
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
        //Slider_DPI,
    }
    // 아래에 Dropdown 메뉴에 대한 enum 추가. 그 값은 오브젝트 이름과 같은 Dropdown_Resolution 으로 한다.
    enum TMP_Dropdowns
    {
        Dropdown_Resolution,
    }
    enum TextMeshProUGUIs
    {
        Text_Sound1,
        Text_Sound2,
        Text_Sound3,
        Text_Sound4,
        Text_Quality,
        //Text_DPI,
    }
    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<TMP_Dropdown>(typeof(TMP_Dropdowns));
        SetElementEvents();

        try
        {
            GetButton((int)Buttons.Button_BackToMain).onClick.AddListener(BackToMain);
            GetButton((int)Buttons.Background).onClick.AddListener(BackToMain);
        }
        catch { }



    }
    private void SetElementEvents()
    {
        Slider slider; // 인스턴스 돌려쓰기
        TMP_Dropdown dropdown;

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
        dropdown = Get<TMP_Dropdown>((int)TMP_Dropdowns.Dropdown_Resolution);
        InitDropdownList(dropdown);
        dropdown.onValueChanged.AddListener(DropdownEvent_Resolution);
        dropdown.value = PlayerPrefs.GetInt("selectedResolutionIndex", 0);
        slider = Get<Slider>((int)Sliders.Slider_Quality);
        slider.onValueChanged.AddListener(SliderEvent_Quality);
        slider.value = QualitySettings.GetQualityLevel();
        //slider = Get<Slider>((int)Sliders.Slider_DPI);
        //slider.onValueChanged.AddListener(SliderEvent_DPI);
        //slider.value = 
        //

        // DATA
        //GetButton((int)Buttons.Button_Reset1).onClick.AddListener(Reset1Btn); // 게임 초기화, 미구현
    }

    public void SliderEvent_Sound1(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound1).text = ((int)(value * 100)).ToString();
        AudioManager.Instance.masterVolume = value;

    }
    public void SliderEvent_Sound2(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound2).text = ((int)(value * 100)).ToString();
        AudioManager.Instance.musicVolume = value;
    }
    public void SliderEvent_Sound3(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound3).text = ((int)(value * 100)).ToString();
        AudioManager.Instance.ambienceVolume = value;
    }
    public void SliderEvent_Sound4(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Sound4).text = ((int)(value * 100)).ToString();
        AudioManager.Instance.SFXVolume = value;
    }
    public void InitDropdownList(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        List<string> resolutions = new List<string>
        {
            "1920 x 1080 FullScreen",
            "1920 x 1080 Windowed",
            "1280 x 720 FullScreen",
            "1280 x 720 Windowed",
            "1366 x 768 FullScreen",
            "1366 x 768 Windowed",
            "1600 x 900 FullScreen",
            "1600 x 900 Windowed",
            "2560 x 1440 FullScreen",
            "2560 x 1440 Windowed",
            "3840 x 2160 FullScreen",
            "3840 x 2160 Windowed",
        };

        foreach (string resolution in resolutions)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(resolution));
        }
        dropdown.RefreshShownValue(); // 보여주기
    }
    public void DropdownEvent_Resolution(int index)
    {
        PlayerPrefs.SetInt("selectedResolutionIndex", index);
        ApplyResolutionChange(index);
    }
    private void ApplyResolutionChange(int index)
    {
        TMP_Dropdown dropdown = Get<TMP_Dropdown>((int)TMP_Dropdowns.Dropdown_Resolution);
        string selectedOption = dropdown.options[index].text;
        string[] parts = selectedOption.Split(' ');
        int width = int.Parse(parts[0]);
        int height = int.Parse(parts[2]);
        FullScreenMode mode = parts[3] == "FullScreen" ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

        Screen.SetResolution(width, height, mode);
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
        PlayerPrefs.SetInt("FullscreenMode", (int)mode);
        PlayerPrefs.Save();
    }
    public void SliderEvent_Quality(float value)
    {
        Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_Quality).text = ((int)value).ToString();
        QualitySettings.SetQualityLevel((int)value);
        Debug.Log("품질 조정: " + QualitySettings.names[(int)value]);
    }
    //public void SliderEvent_DPI(float value)
    //{
    //    float dpiScale = Mathf.Lerp(0.5f, 2.0f, value / 100.0f); // value를 0에서 100 사이로 가정
    //    CanvasScaler canvasScaler = FindObjectOfType<CanvasScaler>();
    //    if (canvasScaler != null)
    //    {
    //        canvasScaler.scaleFactor = dpiScale;
    //    }
    //    Get<TextMeshProUGUI>((int)TextMeshProUGUIs.Text_DPI).text = ((int)value).ToString();
    //    Debug.Log("DPI 조정: " + dpiScale);
    //}
    private void BackToMain()
    {
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
