using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Option : MonoBehaviour
{
    [Header("Static")]
    public static Option option; //���� ���� ����

    [Header("Option Panel")]
    public Canvas optionPanelCanvas; //OptionPanel�� Canvas ������Ʈ

    [Header("OptionTab Panel")]
    public GameObject[] optionTabPanels; //OptionTabPanel ������Ʈ��
    public Image[] optionTabButtonImages; //OptionTabButton�� Image ������Ʈ��
    public GameObject[] optionTabButtonTints; //OptionTab�� Tint ������Ʈ��
    private Color32 optionTabButtonActivationColor = new Color32(40, 40, 40, 255); //OptionTabButton�� Ȱ��ȭ ����
    private Color32 optionTabButtonDeactivationColor = new Color32(100, 100, 100, 255); //OptionTabButton�� ��Ȱ��ȭ ����

    [Header("Graphic")]
    private int _quality;
    public int quality {
        get
        {
            return _quality; //ǰ�� ��ȯ
        }
        set
        {
            _quality = value; //���õ� ǰ�� ����

            for (int i = 0; i < 4; ++i) //ǰ�� �� ������ŭ �ݺ�
            {
                qualityButtonImages[i].color = qualityButtonDeactivationColors[i]; //��� ǰ�� ��ư�� ������ ��Ȱ��ȭ ������ ����
                qualityButtonTints[i].SetActive(false); //��� ǰ�� ��ư�� Tint ������Ʈ ��Ȱ��ȭ
            }

            qualityButtonImages[value].color = qualityButtonActivationColors[value]; //���õ� ǰ�� ��ư�� ������ Ȱ��ȭ ������ ����
            qualityButtonTints[value].SetActive(true); //���õ� ǰ�� ��ư�� Tint ������Ʈ Ȱ��ȭ

            /* �ε����� ���� ǰ�� ���� */
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
    } //ǰ��
    public Image[] qualityButtonImages; //ǰ�� ��ư�� Image ������Ʈ��
    public GameObject[] qualityButtonTints; //ǰ�� ��ư�� Tint ������Ʈ��
    private Color32[] qualityButtonActivationColors = { new Color32(40, 40, 40, 255), new Color32(40, 40, 40, 255), new Color32(40, 40, 40, 255), new Color32(225, 0, 20, 255) }; //ǰ�� ��ư�� Ȱ��ȭ �����
    private Color32[] qualityButtonDeactivationColors = { new Color32(100, 100, 100, 255), new Color32(100, 100, 100, 255), new Color32(100, 100, 100, 255), new Color32(100, 60, 60, 255) }; //ǰ�� ��ư�� ��Ȱ��ȭ �����
    public UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset universalRenderPipelineAsset; //UniversalRenderPipelineAsset

    private int _frame;
    public int frame
    {
        get
        {
            return _frame; //������ ��ȯ
        }
        set
        {
            _frame = value; //���õ� ������ ����

            for (int i = 0; i < 3; ++i) //������ �� ������ŭ �ݺ�
            {
                frameButtonImages[i].color = frameButtonDeactivationColors[i]; //��� ������ ��ư�� ������ ��Ȱ��ȭ ������ ����
                frameButtonTints[i].SetActive(false); //��� ������ ��ư�� Tint ������Ʈ ��Ȱ��ȭ
            }

            frameButtonImages[value].color = frameButtonActivationColors[value]; //���õ� ������ ��ư�� ������ Ȱ��ȭ ������ ����
            frameButtonTints[value].SetActive(true); //���õ� ������ ��ư�� Tint ������Ʈ Ȱ��ȭ

            /* �ε����� ���� ������ ���� */
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
    } //������
    public Image[] frameButtonImages; //������ ��ư�� Image ������Ʈ��
    public GameObject[] frameButtonTints; //������ ��ư�� Tint ������Ʈ��
    private Color32[] frameButtonActivationColors = { new Color32(40, 40, 40, 255), new Color32(40, 40, 40, 255), new Color32(40, 40, 40, 255) }; //������ ��ư�� Ȱ��ȭ �����
    private Color32[] frameButtonDeactivationColors = { new Color32(100, 100, 100, 255), new Color32(100, 100, 100, 255), new Color32(100, 100, 100, 255) }; //������ ��ư�� ��Ȱ��ȭ �����

    [Header("Control")]
    public Slider cameraRotationSensitivitySlider; //CameraRotationSensitivity�� Slider ������Ʈ
    public TextMeshProUGUI cameraRotationSensitivitySliderValueTMP; //ī�޶� ȸ�� �ΰ��� Slider�� Value ������Ʈ TMP ������Ʈ
    public RectTransform cameraRotationSensitivitySliderRectTransform; //ī�޶� ȸ�� �ΰ��� Slider�� RectTransform ������Ʈ
    public RectTransform cameraRotationSensitivitySliderFillRectTransform; //ī�޶� ȸ�� �ΰ��� Slider Fill�� RectTransform ������Ʈ
    public RectTransform cameraRotationSensitivitySliderArrowRectTransform; //ī�޶� ȸ�� �ΰ��� Slider Arrow�� RectTransform ������Ʈ
    public RectTransform cameraRotationSensitivitySliderValueRectTransform; //ī�޶� ȸ�� �ΰ��� Slider Value�� RectTransform ������Ʈ

    public Slider cameraZoomSensitivitySlider; //CameraZoomSensitivity�� Slider ������Ʈ
    public TextMeshProUGUI cameraZoomSensitivitySliderValueTMP; //ī�޶� �� �ΰ��� Slider�� Value ������Ʈ TMP ������Ʈ
    public RectTransform cameraZoomSensitivitySliderRectTransform; //ī�޶� �� �ΰ��� Slider�� RectTransform ������Ʈ
    public RectTransform cameraZoomSensitivitySliderFillRectTransform; //ī�޶� �� �ΰ��� Slider Fill�� RectTransform ������Ʈ
    public RectTransform cameraZoomSensitivitySliderArrowRectTransform; //ī�޶� �� �ΰ��� Slider Arrow�� RectTransform ������Ʈ
    public RectTransform cameraZoomSensitivitySliderValueRectTransform; //ī�޶� �� �ΰ��� Slider Value�� RectTransform ������Ʈ

    [Header("Sound")]
    public Slider backgroundSoundSlider; //BackgroundSound�� Slider ������Ʈ
    public TextMeshProUGUI backgroundSoundSliderValueTMP; //����� Slider�� Value ������Ʈ TMP ������Ʈ
    public RectTransform backgroundSoundSliderRectTransform; //����� Slider�� RectTransform ������Ʈ
    public RectTransform backgroundSoundSliderFillRectTransform; //����� Slider Fill�� RectTransform ������Ʈ
    public RectTransform backgroundSoundSliderArrowRectTransform; //����� Slider Arrow�� RectTransform ������Ʈ
    public RectTransform backgroundSoundSliderValueRectTransform; //����� Slider Value�� RectTransform ������Ʈ

    public Slider effectSoundSlider; //EffectSound�� Slider ������Ʈ
    public TextMeshProUGUI effectSoundSliderValueTMP; //ȿ���� Slider�� Value ������Ʈ TMP ������Ʈ
    public RectTransform effectSoundSliderRectTransform; //ȿ���� Slider�� RectTransform ������Ʈ
    public RectTransform effectSoundSliderFillRectTransform; //ȿ���� Slider Fill�� RectTransform ������Ʈ
    public RectTransform effectSoundSliderArrowRectTransform; //ȿ���� Slider Arrow�� RectTransform ������Ʈ
    public RectTransform effectSoundSliderValueRectTransform; //ȿ���� Slider Value�� RectTransform ������Ʈ

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

    /* OptionPanel�� Ȱ��ȭ/��Ȱ��ȭ�ϴ� �Լ� */
    public void ActivateOptionPanel(bool state)
    {
        Home.home.ActivateHome(!state); //Home Ȱ��ȭ/��Ȱ��ȭ
        optionPanelCanvas.enabled = state; //OptionPanel�� Canvas ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
    }

    /* Option�� �ִ� OptionTabCanvas�� Ȱ��ȭ�ϴ� �Լ� */
    public void ActivateOptionTabCanvas(int index)
    {
        for (int i = 0; i < 3; ++i) //OptionTab ������ŭ �ݺ�
        {
            optionTabPanels[i].SetActive(false); //��� Panel ��Ȱ��ȭ
            optionTabButtonImages[i].color = optionTabButtonDeactivationColor; //��� OptionTabButton ���� ��Ȱ��ȭ �������� ����
            optionTabButtonTints[i].SetActive(false); //��� OptionTab�� Tint ������Ʈ ��Ȱ��ȭ
        }
        optionTabPanels[index].SetActive(true); //���õ�  Panel Ȱ��ȭ
        optionTabButtonImages[index].color = optionTabButtonActivationColor; //���õ� OptionTabButton ���� Ȱ��ȭ �������� ����
        optionTabButtonTints[index].SetActive(true); //���õ� OptionTab�� Tint ������Ʈ Ȱ��ȭ
    }

    #region Graphic
    /* ǰ�� ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickQualityButton(int index)
    {
        quality = index; //ǰ�� ����
    }

    /* ������ ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickFrameButton(int index)
    {
        frame = index; //������ ����
    }
    #endregion

    #region Control
    /* CameraSensitivity ���� ����Ǹ� ����Ǵ� �Լ� */
    public void OnValueChangedCameraSensitivity()
    {
        Static.cameraRotationSensitivity = cameraRotationSensitivitySlider.value; //ī�޶� ȸ�� �ΰ��� ����
        cameraRotationSensitivitySliderValueTMP.text = string.Format("{0:0}", ((cameraRotationSensitivitySlider.value - cameraRotationSensitivitySlider.minValue) * (100f / (cameraRotationSensitivitySlider.maxValue - cameraRotationSensitivitySlider.minValue)))); //ī�޶� ȸ�� �ΰ��� Value TMP ����
        float cameraRotationSensitivitySliderFront = cameraRotationSensitivitySliderRectTransform.anchoredPosition.x - (cameraRotationSensitivitySliderRectTransform.sizeDelta.x * 0.5f); //ī�޶� ȸ�� �ΰ��� Slider�� �� �� ��ġ
        float cameraRotationSensitivitySliderRear = cameraRotationSensitivitySliderRectTransform.anchoredPosition.x + (cameraRotationSensitivitySliderRectTransform.sizeDelta.x * 0.5f); //ī�޶� ȸ�� �ΰ��� Slider�� �� �� ��ġ
        cameraRotationSensitivitySliderValueRectTransform.anchoredPosition = new Vector2(cameraRotationSensitivitySliderFront + (cameraRotationSensitivitySliderRear - cameraRotationSensitivitySliderFront) * cameraRotationSensitivitySliderFillRectTransform.anchorMax.x, cameraRotationSensitivitySliderValueRectTransform.anchoredPosition.y); //Value ������Ʈ �̵�
        cameraRotationSensitivitySliderArrowRectTransform.anchoredPosition = new Vector2(cameraRotationSensitivitySliderFront + (cameraRotationSensitivitySliderRear - cameraRotationSensitivitySliderFront) * cameraRotationSensitivitySliderFillRectTransform.anchorMax.x, cameraRotationSensitivitySliderArrowRectTransform.anchoredPosition.y); //Arrow ������Ʈ �̵�

        Static.cameraZoomSensitivity = cameraZoomSensitivitySlider.value; //ī�޶� �� �ΰ��� ����
        cameraZoomSensitivitySliderValueTMP.text = string.Format("{0:0}", ((cameraZoomSensitivitySlider.value - cameraZoomSensitivitySlider.minValue) * (100f / (cameraZoomSensitivitySlider.maxValue - cameraZoomSensitivitySlider.minValue)))); //ī�޶� �� �ΰ��� Value TMP ����
        float cameraZoomSensitivitySliderFront = cameraZoomSensitivitySliderRectTransform.anchoredPosition.x - (cameraZoomSensitivitySliderRectTransform.sizeDelta.x * 0.5f); //ī�޶� �� �ΰ��� Slider�� �� �� ��ġ
        float cameraZoomSensitivitySliderRear = cameraZoomSensitivitySliderRectTransform.anchoredPosition.x + (cameraZoomSensitivitySliderRectTransform.sizeDelta.x * 0.5f); //ī�޶� �� �ΰ��� Slider�� �� �� ��ġ
        cameraZoomSensitivitySliderValueRectTransform.anchoredPosition = new Vector2(cameraZoomSensitivitySliderFront + (cameraZoomSensitivitySliderRear - cameraZoomSensitivitySliderFront) * cameraZoomSensitivitySliderFillRectTransform.anchorMax.x, cameraZoomSensitivitySliderValueRectTransform.anchoredPosition.y); //Value ������Ʈ �̵�
        cameraZoomSensitivitySliderArrowRectTransform.anchoredPosition = new Vector2(cameraZoomSensitivitySliderFront + (cameraZoomSensitivitySliderRear - cameraZoomSensitivitySliderFront) * cameraZoomSensitivitySliderFillRectTransform.anchorMax.x, cameraZoomSensitivitySliderArrowRectTransform.anchoredPosition.y); //Arrow ������Ʈ �̵�
    }
    #endregion

    #region Sound
    /* �Ҹ� ������ ����Ǿ��� �� ����Ǵ� �Լ� */
    public void OnValueChangedSound()
    {
        backgroundSoundSliderValueTMP.text = string.Format("{0:0}", ((backgroundSoundSlider.value - backgroundSoundSlider.minValue) * (100f / (backgroundSoundSlider.maxValue - backgroundSoundSlider.minValue)))); //����� Value TMP ����
        float backgroundSoundSliderFront = backgroundSoundSliderRectTransform.anchoredPosition.x - (backgroundSoundSliderRectTransform.sizeDelta.x * 0.5f); //����� Slider�� �� �� ��ġ
        float backgroundSoundSliderRear = backgroundSoundSliderRectTransform.anchoredPosition.x + (backgroundSoundSliderRectTransform.sizeDelta.x * 0.5f); //����� Slider�� �� �� ��ġ
        backgroundSoundSliderValueRectTransform.anchoredPosition = new Vector2(backgroundSoundSliderFront + (backgroundSoundSliderRear - backgroundSoundSliderFront) * backgroundSoundSliderFillRectTransform.anchorMax.x, backgroundSoundSliderValueRectTransform.anchoredPosition.y); //Value ������Ʈ �̵�
        backgroundSoundSliderArrowRectTransform.anchoredPosition = new Vector2(backgroundSoundSliderFront + (backgroundSoundSliderRear - backgroundSoundSliderFront) * backgroundSoundSliderFillRectTransform.anchorMax.x, backgroundSoundSliderArrowRectTransform.anchoredPosition.y); //Arrow ������Ʈ �̵�

        effectSoundSliderValueTMP.text = string.Format("{0:0}", ((effectSoundSlider.value - effectSoundSlider.minValue) * (100f / (effectSoundSlider.maxValue - effectSoundSlider.minValue)))); //ȿ���� Value TMP ����
        float effectSoundSliderFront = effectSoundSliderRectTransform.anchoredPosition.x - (effectSoundSliderRectTransform.sizeDelta.x * 0.5f); //ȿ���� Slider�� �� �� ��ġ
        float effectSoundSliderRear = effectSoundSliderRectTransform.anchoredPosition.x + (effectSoundSliderRectTransform.sizeDelta.x * 0.5f); //ȿ���� Slider�� �� �� ��ġ
        effectSoundSliderValueRectTransform.anchoredPosition = new Vector2(effectSoundSliderFront + (effectSoundSliderRear - effectSoundSliderFront) * effectSoundSliderFillRectTransform.anchorMax.x, effectSoundSliderValueRectTransform.anchoredPosition.y); //Value ������Ʈ �̵�
        effectSoundSliderArrowRectTransform.anchoredPosition = new Vector2(effectSoundSliderFront + (effectSoundSliderRear - effectSoundSliderFront) * effectSoundSliderFillRectTransform.anchorMax.x, effectSoundSliderArrowRectTransform.anchoredPosition.y); //Arrow ������Ʈ �̵�
    }
    #endregion
}