using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Option : MonoBehaviour
{
    [Header("Static")]
    public static Option option; //전역 참조 변수

    [Header("Option Panel")]
    public Canvas optionPanelCanvas; //OptionPanel의 Canvas 컴포넌트

    [Header("OptionTab Panel")]
    public GameObject[] optionTabPanels; //OptionTabPanel 오브젝트들
    public Image[] optionTabButtonImages; //OptionTabButton의 Image 컴포넌트들
    public GameObject[] optionTabButtonTints; //OptionTab의 Tint 오브젝트들
    private Color32 optionTabButtonActivationColor = new Color32(40, 40, 40, 255); //OptionTabButton의 활성화 색상
    private Color32 optionTabButtonDeactivationColor = new Color32(100, 100, 100, 255); //OptionTabButton의 비활성화 색상

    [Header("Graphic")]
    private int _quality;
    public int quality {
        get
        {
            return _quality; //품질 반환
        }
        set
        {
            _quality = value; //선택된 품질 지정

            for (int i = 0; i < 4; ++i) //품질 탭 개수만큼 반복
            {
                qualityButtonImages[i].color = qualityButtonDeactivationColors[i]; //모든 품질 버튼의 색상을 비활성화 색으로 변경
                qualityButtonTints[i].SetActive(false); //모든 품질 버튼의 Tint 오브젝트 비활성화
            }

            qualityButtonImages[value].color = qualityButtonActivationColors[value]; //선택된 품질 버튼의 색상을 활성화 색으로 변경
            qualityButtonTints[value].SetActive(true); //선택된 품질 버튼의 Tint 오브젝트 활성화

            /* 인덱스에 따라 품질 설정 */
            switch (value)
            {
                case 0:
                    universalRenderPipelineAsset.renderScale = (float)(1440 * 810) / (Display.main.systemWidth * Display.main.systemHeight);
                    break;
                case 1:
                    universalRenderPipelineAsset.renderScale = (float)(1600 * 900) / (Display.main.systemWidth * Display.main.systemHeight);
                    break;
                case 2:
                    universalRenderPipelineAsset.renderScale = (float)(1920 * 1080) / (Display.main.systemWidth * Display.main.systemHeight);
                    break;
                case 3:
                    universalRenderPipelineAsset.renderScale = (float)(2560 * 1440) / (Display.main.systemWidth * Display.main.systemHeight);
                    break;
            }
        }
    } //품질
    public Image[] qualityButtonImages; //품질 버튼의 Image 컴포넌트들
    public GameObject[] qualityButtonTints; //품질 버튼의 Tint 오브젝트들
    private Color32[] qualityButtonActivationColors = { new Color32(40, 40, 40, 255), new Color32(40, 40, 40, 255), new Color32(40, 40, 40, 255), new Color32(225, 0, 20, 255) }; //품질 버튼의 활성화 색상들
    private Color32[] qualityButtonDeactivationColors = { new Color32(100, 100, 100, 255), new Color32(100, 100, 100, 255), new Color32(100, 100, 100, 255), new Color32(100, 60, 60, 255) }; //품질 버튼의 비활성화 색상들
    public UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset universalRenderPipelineAsset; //UniversalRenderPipelineAsset

    private int _frame;
    public int frame
    {
        get
        {
            return _frame; //프레임 반환
        }
        set
        {
            _frame = value; //선택된 프레임 지정

            for (int i = 0; i < 3; ++i) //프레임 탭 개수만큼 반복
            {
                frameButtonImages[i].color = frameButtonDeactivationColors[i]; //모든 프레임 버튼의 색상을 비활성화 색으로 변경
                frameButtonTints[i].SetActive(false); //모든 프레임 버튼의 Tint 오브젝트 비활성화
            }

            frameButtonImages[value].color = frameButtonActivationColors[value]; //선택된 프레임 버튼의 색상을 활성화 색으로 변경
            frameButtonTints[value].SetActive(true); //선택된 프레임 버튼의 Tint 오브젝트 활성화

            /* 인덱스에 따라 프레임 설정 */
            switch (value)
            {
                case 0:
                    Application.targetFrameRate = 30;
                    break;
                case 1:
                    Application.targetFrameRate = 45;
                    break;
                case 2:
                    Application.targetFrameRate = 60;
                    break;
            }
        }
    } //프레임
    public Image[] frameButtonImages; //프레임 버튼의 Image 컴포넌트들
    public GameObject[] frameButtonTints; //프레임 버튼의 Tint 오브젝트들
    private Color32[] frameButtonActivationColors = { new Color32(40, 40, 40, 255), new Color32(40, 40, 40, 255), new Color32(40, 40, 40, 255) }; //프레임 버튼의 활성화 색상들
    private Color32[] frameButtonDeactivationColors = { new Color32(100, 100, 100, 255), new Color32(100, 100, 100, 255), new Color32(100, 100, 100, 255) }; //프레임 버튼의 비활성화 색상들

    [Header("Control")]
    public Slider cameraRotationSensitivitySlider; //CameraRotationSensitivity의 Slider 컴포넌트
    public TextMeshProUGUI cameraRotationSensitivitySliderValueTMP; //카메라 회전 민감도 Slider의 Value 오브젝트 TMP 컴포넌트
    public RectTransform cameraRotationSensitivitySliderRectTransform; //카메라 회전 민감도 Slider의 RectTransform 컴포넌트
    public RectTransform cameraRotationSensitivitySliderFillRectTransform; //카메라 회전 민감도 Slider Fill의 RectTransform 컴포넌트
    public RectTransform cameraRotationSensitivitySliderArrowRectTransform; //카메라 회전 민감도 Slider Arrow의 RectTransform 컴포넌트
    public RectTransform cameraRotationSensitivitySliderValueRectTransform; //카메라 회전 민감도 Slider Value의 RectTransform 컴포넌트

    public Slider cameraZoomSensitivitySlider; //CameraZoomSensitivity의 Slider 컴포넌트
    public TextMeshProUGUI cameraZoomSensitivitySliderValueTMP; //카메라 줌 민감도 Slider의 Value 오브젝트 TMP 컴포넌트
    public RectTransform cameraZoomSensitivitySliderRectTransform; //카메라 줌 민감도 Slider의 RectTransform 컴포넌트
    public RectTransform cameraZoomSensitivitySliderFillRectTransform; //카메라 줌 민감도 Slider Fill의 RectTransform 컴포넌트
    public RectTransform cameraZoomSensitivitySliderArrowRectTransform; //카메라 줌 민감도 Slider Arrow의 RectTransform 컴포넌트
    public RectTransform cameraZoomSensitivitySliderValueRectTransform; //카메라 줌 민감도 Slider Value의 RectTransform 컴포넌트

    [Header("Sound")]
    public Slider backgroundSoundSlider; //BackgroundSound의 Slider 컴포넌트
    public TextMeshProUGUI backgroundSoundSliderValueTMP; //배경음 Slider의 Value 오브젝트 TMP 컴포넌트
    public RectTransform backgroundSoundSliderRectTransform; //배경음 Slider의 RectTransform 컴포넌트
    public RectTransform backgroundSoundSliderFillRectTransform; //배경음 Slider Fill의 RectTransform 컴포넌트
    public RectTransform backgroundSoundSliderArrowRectTransform; //배경음 Slider Arrow의 RectTransform 컴포넌트
    public RectTransform backgroundSoundSliderValueRectTransform; //배경음 Slider Value의 RectTransform 컴포넌트

    public Slider effectSoundSlider; //EffectSound의 Slider 컴포넌트
    public TextMeshProUGUI effectSoundSliderValueTMP; //효과음 Slider의 Value 오브젝트 TMP 컴포넌트
    public RectTransform effectSoundSliderRectTransform; //효과음 Slider의 RectTransform 컴포넌트
    public RectTransform effectSoundSliderFillRectTransform; //효과음 Slider Fill의 RectTransform 컴포넌트
    public RectTransform effectSoundSliderArrowRectTransform; //효과음 Slider Arrow의 RectTransform 컴포넌트
    public RectTransform effectSoundSliderValueRectTransform; //효과음 Slider Value의 RectTransform 컴포넌트

    private void Awake()
    {
        option = this;
    }

    private void Start()
    {
        ActivateOptionTabCanvas(0);

        OnValueChangedCameraSensitivity();
        OnValueChangedSound();
    }

    /* OptionPanel을 활성화/비활성화하는 함수 */
    public void ActivateOptionPanel(bool state)
    {
        Home.home.ActivateHome(!state); //Home 활성화/비활성화
        optionPanelCanvas.enabled = state; //OptionPanel의 Canvas 컴포넌트 활성화/비활성화
    }

    /* Option에 있는 OptionTabCanvas를 활성화하는 함수 */
    public void ActivateOptionTabCanvas(int index)
    {
        for (int i = 0; i < 3; ++i) //OptionTab 개수만큼 반복
        {
            optionTabPanels[i].SetActive(false); //모든 Panel 비활성화
            optionTabButtonImages[i].color = optionTabButtonDeactivationColor; //모든 OptionTabButton 색상 비활성화 색상으로 변경
            optionTabButtonTints[i].SetActive(false); //모든 OptionTab의 Tint 오브젝트 비활성화
        }
        optionTabPanels[index].SetActive(true); //선택된  Panel 활성화
        optionTabButtonImages[index].color = optionTabButtonActivationColor; //선택된 OptionTabButton 색상 활성화 색상으로 변경
        optionTabButtonTints[index].SetActive(true); //선택된 OptionTab의 Tint 오브젝트 활성화
    }

    #region Graphic
    /* 품질 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickQualityButton(int index)
    {
        quality = index; //품질 설정
    }

    /* 프레임 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickFrameButton(int index)
    {
        frame = index; //프레임 설정
    }
    #endregion

    #region Control
    /* CameraSensitivity 값이 변경되면 실행되는 함수 */
    public void OnValueChangedCameraSensitivity()
    {
        Static.cameraRotationSensitivity = cameraRotationSensitivitySlider.value; //카메라 회전 민감도 적용
        cameraRotationSensitivitySliderValueTMP.text = string.Format("{0:0}", ((cameraRotationSensitivitySlider.value - cameraRotationSensitivitySlider.minValue) * (100f / (cameraRotationSensitivitySlider.maxValue - cameraRotationSensitivitySlider.minValue)))); //카메라 회전 민감도 Value TMP 갱신
        float cameraRotationSensitivitySliderFront = cameraRotationSensitivitySliderRectTransform.anchoredPosition.x - (cameraRotationSensitivitySliderRectTransform.sizeDelta.x * 0.5f); //카메라 회전 민감도 Slider의 앞 쪽 위치
        float cameraRotationSensitivitySliderRear = cameraRotationSensitivitySliderRectTransform.anchoredPosition.x + (cameraRotationSensitivitySliderRectTransform.sizeDelta.x * 0.5f); //카메라 회전 민감도 Slider의 뒤 쪽 위치
        cameraRotationSensitivitySliderValueRectTransform.anchoredPosition = new Vector2(cameraRotationSensitivitySliderFront + (cameraRotationSensitivitySliderRear - cameraRotationSensitivitySliderFront) * cameraRotationSensitivitySliderFillRectTransform.anchorMax.x, cameraRotationSensitivitySliderValueRectTransform.anchoredPosition.y); //Value 오브젝트 이동
        cameraRotationSensitivitySliderArrowRectTransform.anchoredPosition = new Vector2(cameraRotationSensitivitySliderFront + (cameraRotationSensitivitySliderRear - cameraRotationSensitivitySliderFront) * cameraRotationSensitivitySliderFillRectTransform.anchorMax.x, cameraRotationSensitivitySliderArrowRectTransform.anchoredPosition.y); //Arrow 오브젝트 이동

        Static.cameraZoomSensitivity = cameraZoomSensitivitySlider.value; //카메라 줌 민감도 적용
        cameraZoomSensitivitySliderValueTMP.text = string.Format("{0:0}", ((cameraZoomSensitivitySlider.value - cameraZoomSensitivitySlider.minValue) * (100f / (cameraZoomSensitivitySlider.maxValue - cameraZoomSensitivitySlider.minValue)))); //카메라 줌 민감도 Value TMP 갱신
        float cameraZoomSensitivitySliderFront = cameraZoomSensitivitySliderRectTransform.anchoredPosition.x - (cameraZoomSensitivitySliderRectTransform.sizeDelta.x * 0.5f); //카메라 줌 민감도 Slider의 앞 쪽 위치
        float cameraZoomSensitivitySliderRear = cameraZoomSensitivitySliderRectTransform.anchoredPosition.x + (cameraZoomSensitivitySliderRectTransform.sizeDelta.x * 0.5f); //카메라 줌 민감도 Slider의 뒤 쪽 위치
        cameraZoomSensitivitySliderValueRectTransform.anchoredPosition = new Vector2(cameraZoomSensitivitySliderFront + (cameraZoomSensitivitySliderRear - cameraZoomSensitivitySliderFront) * cameraZoomSensitivitySliderFillRectTransform.anchorMax.x, cameraZoomSensitivitySliderValueRectTransform.anchoredPosition.y); //Value 오브젝트 이동
        cameraZoomSensitivitySliderArrowRectTransform.anchoredPosition = new Vector2(cameraZoomSensitivitySliderFront + (cameraZoomSensitivitySliderRear - cameraZoomSensitivitySliderFront) * cameraZoomSensitivitySliderFillRectTransform.anchorMax.x, cameraZoomSensitivitySliderArrowRectTransform.anchoredPosition.y); //Arrow 오브젝트 이동
    }
    #endregion

    #region Sound
    /* 소리 음량이 변경되었을 때 실행되는 함수 */
    public void OnValueChangedSound()
    {
        backgroundSoundSliderValueTMP.text = string.Format("{0:0}", ((backgroundSoundSlider.value - backgroundSoundSlider.minValue) * (100f / (backgroundSoundSlider.maxValue - backgroundSoundSlider.minValue)))); //배경음 Value TMP 갱신
        float backgroundSoundSliderFront = backgroundSoundSliderRectTransform.anchoredPosition.x - (backgroundSoundSliderRectTransform.sizeDelta.x * 0.5f); //배경음 Slider의 앞 쪽 위치
        float backgroundSoundSliderRear = backgroundSoundSliderRectTransform.anchoredPosition.x + (backgroundSoundSliderRectTransform.sizeDelta.x * 0.5f); //배경음 Slider의 뒤 쪽 위치
        backgroundSoundSliderValueRectTransform.anchoredPosition = new Vector2(backgroundSoundSliderFront + (backgroundSoundSliderRear - backgroundSoundSliderFront) * backgroundSoundSliderFillRectTransform.anchorMax.x, backgroundSoundSliderValueRectTransform.anchoredPosition.y); //Value 오브젝트 이동
        backgroundSoundSliderArrowRectTransform.anchoredPosition = new Vector2(backgroundSoundSliderFront + (backgroundSoundSliderRear - backgroundSoundSliderFront) * backgroundSoundSliderFillRectTransform.anchorMax.x, backgroundSoundSliderArrowRectTransform.anchoredPosition.y); //Arrow 오브젝트 이동

        effectSoundSliderValueTMP.text = string.Format("{0:0}", ((effectSoundSlider.value - effectSoundSlider.minValue) * (100f / (effectSoundSlider.maxValue - effectSoundSlider.minValue)))); //효과음 Value TMP 갱신
        float effectSoundSliderFront = effectSoundSliderRectTransform.anchoredPosition.x - (effectSoundSliderRectTransform.sizeDelta.x * 0.5f); //효과음 Slider의 앞 쪽 위치
        float effectSoundSliderRear = effectSoundSliderRectTransform.anchoredPosition.x + (effectSoundSliderRectTransform.sizeDelta.x * 0.5f); //효과음 Slider의 뒤 쪽 위치
        effectSoundSliderValueRectTransform.anchoredPosition = new Vector2(effectSoundSliderFront + (effectSoundSliderRear - effectSoundSliderFront) * effectSoundSliderFillRectTransform.anchorMax.x, effectSoundSliderValueRectTransform.anchoredPosition.y); //Value 오브젝트 이동
        effectSoundSliderArrowRectTransform.anchoredPosition = new Vector2(effectSoundSliderFront + (effectSoundSliderRear - effectSoundSliderFront) * effectSoundSliderFillRectTransform.anchorMax.x, effectSoundSliderArrowRectTransform.anchoredPosition.y); //Arrow 오브젝트 이동
    }
    #endregion
}