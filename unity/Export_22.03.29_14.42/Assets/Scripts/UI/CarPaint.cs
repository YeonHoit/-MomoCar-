using UnityEngine;
using UnityEngine.UI;

public class CarPaint : MonoBehaviour
{
    [Header("Static")]
    public static CarPaint carPaint; //���� ���� ����

    /* ���� ���� */
    public enum PaintPart
    {
        Entire, Bonnet, SideMirror, Roof, ChromeDelete, Wheel, BrakeCaliper, Window
    }

    [Header("Car Paint Panel")]
    public Canvas carPaintPanelCanvas; //CarPaintPanel�� Canvas ������Ʈ

    [Header("Select Paint Part")]
    private PaintPart _selectedPaintPart;
    public PaintPart selectedPaintPart //���õ� ���� ����
    {
        get
        {
            return _selectedPaintPart; //���õ� ���� ���� ��ȯ
        }
        set
        {
            _selectedPaintPart = value; //���õ� ���� ���� ����

            for (int i = 0; i < paintPartButtonImages.Length; ++i) paintPartButtonImages[i].color = paintPartButtonDeactivationColor; //��� ���� ���� ��ư ��Ȱ��ȭ �������� ����
            paintPartButtonImages[(int)_selectedPaintPart].color = paintPartButtonActivationColor; //���õ� ���� ���� ��ư Ȱ��ȭ �������� ����

            CarPaintColorPicker.carPaintColorPicker.ApplyColor(carSelectedPaintPartColor); //Color Picker ���� ����

            /* ���� �ʱ�ȭ */
            switch (_selectedPaintPart) //���õ� ���� ������ ����
            {
                case PaintPart.Entire:
                case PaintPart.Bonnet:
                case PaintPart.SideMirror:
                case PaintPart.Roof:
                case PaintPart.ChromeDelete:
                case PaintPart.Wheel:
                case PaintPart.BrakeCaliper:
                    for (int i = 0; i < carPaintSurfaceTextsTransform.childCount; ++i) carPaintSurfaceTextsTransform.GetChild(i).gameObject.SetActive(false); //��� Surface Text ��Ȱ��ȭ
                    carPaintSurfaceTextsTransform.GetChild((int)_selectedPaintPart).gameObject.SetActive(true); //���õ� ���� ���� ������ ���� Surface Text Ȱ��ȭ

                    carPaintSurfaceTextsTransform.gameObject.SetActive(true); //SurfaceTexts ������Ʈ Ȱ��ȭ
                    carPaintSurfacePreviousButton.SetActive(true); //SurfacePreviousButton ������Ʈ Ȱ��ȭ
                    carPaintSurfaceNextButton.SetActive(true); //SurfaceNextButton ������Ʈ Ȱ��ȭ
                    break;

                default:
                    carPaintSurfaceTextsTransform.gameObject.SetActive(false); //SurfaceTexts ������Ʈ ��Ȱ��ȭ
                    carPaintSurfacePreviousButton.SetActive(false); //SurfacePreviousButton ������Ʈ ��Ȱ��ȭ
                    carPaintSurfaceNextButton.SetActive(false); //SurfaceNextButton ������Ʈ ��Ȱ��ȭ
                    break;
            }
        }
    }
    public Image[] paintPartButtonImages; //���� ��ư�� Image ������Ʈ��
    private readonly Color32 paintPartButtonActivationColor = new Color32(25, 25, 25, 255); //���� ���� ��ư Ȱ��ȭ ����
    private readonly Color32 paintPartButtonDeactivationColor = new Color32(200, 200, 200, 255); //���� ���� ��ư ��Ȱ��ȭ ����

    [Header("Materials")]
    private const int carEntireMaterialLength = 3; //Entire Material ����
    public Material[] carEntireMaterials; //Entire Material ������Ʈ��
    private Material[] carEntireDefaultMaterials; //Entire�� �⺻ Material ������Ʈ��

    private const int carBonnetMaterialLength = 1; //Bonnet Material ����
    public Material[] carBonnetMaterials; //Bonnet Material ������Ʈ��
    private Material[] carBonnetDefaultMaterials; //Bonnet�� �⺻ Material ������Ʈ��

    private const int carSideMirrorMaterialLength = 1; //SideMirror Material ����
    public Material[] carSideMirrorMaterials; //SideMirror Material ������Ʈ��
    private Material[] carSideMirrorDefaultMaterials; //SideMirror�� �⺻ Material ������Ʈ��

    private const int carRoofMaterialLength = 1; //Roof Material ����
    public Material[] carRoofMaterials; //Roof Material ������Ʈ��
    private Material[] carRoofDefaultMaterials; //Roof�� �⺻ Material ������Ʈ��

    private const int carChromeDeleteMaterialLength = 12; //ChromeDelete Material ����
    public Material[] carChromeDeleteMaterials; //ChromeDelete Material ������Ʈ��
    private Material[] carChromeDeleteDefaultMaterials; //ChromeDelete�� �⺻ Material ������Ʈ��

    private const int carWheelMaterialLength = 8; //Wheel Material ����
    public Material[] carWheelMaterials; //Wheel Material ������Ʈ��
    private Material[] carWheelDefaultMaterials; //Wheel�� �⺻ Material ������Ʈ��

    private const int carBrakeCaliperMaterialLength = 4; //BrakeCaliper Material ����
    public Material[] carBrakeCaliperMaterials; //BrakeCaliper Material ������Ʈ��
    private Material[] carBrakeCaliperDefaultMaterials; //BrakeCaliper�� �⺻ Material ������Ʈ��

    private const int carWindowMaterialLength = 1; //Window Material ����
    public Material[] carWindowMaterials; //Window Material ������Ʈ��

    [Header("Car PaintPart Surface")]
    public Transform carPaintSurfaceTextsTransform; //CarPaintPanel > Surface > Texts�� Transform ������Ʈ
    public GameObject carPaintSurfacePreviousButton; //CarPaintPanel > Surface > PreviousButton ������Ʈ
    public GameObject carPaintSurfaceNextButton; //CarPaintPanel > Surface > NextButton ������Ʈ

    #region CarPaintPartColor
    public Color32 carSelectedPaintPartColor //���õ� ���� ���� ����
    {
        get
        {
            switch (selectedPaintPart) //���õ� ���� ������ ���� ���� ��ȯ
            {
                case PaintPart.Entire:
                    return carEntireColor;
                case PaintPart.Bonnet:
                    return carBonnetColor;
                case PaintPart.SideMirror:
                    return carSideMirrorColor;
                case PaintPart.Roof:
                    return carRoofColor;
                case PaintPart.ChromeDelete:
                    return carChromeDeleteColor;
                case PaintPart.Wheel:
                    return carWheelColor;
                case PaintPart.BrakeCaliper:
                    return carBrakeCaliperColor;
                case PaintPart.Window:
                    return carWindowColor;
                default:
                    return Color.black;
            }
        }
        set
        {
            switch (selectedPaintPart) //���õ� ���� ������ ���� ���� ����
            {
                case PaintPart.Entire:
                    carEntireColor = value;
                    break;
                case PaintPart.Bonnet:
                    carBonnetColor = value;
                    break;
                case PaintPart.SideMirror:
                    carSideMirrorColor = value;
                    break;
                case PaintPart.Roof:
                    carRoofColor = value;
                    break;
                case PaintPart.ChromeDelete:
                    carChromeDeleteColor = value;
                    break;
                case PaintPart.Wheel:
                    carWheelColor = value;
                    break;
                case PaintPart.BrakeCaliper:
                    carBrakeCaliperColor = value;
                    break;
                case PaintPart.Window:
                    carWindowColor = value;
                    break;
                default:
                    break;
            }
        }
    }

    private Color32 _carEntireColor;
    public Color32 carEntireColor //Entire Color ������Ʈ
    {
        get
        {
            return _carEntireColor; //���� ����
        }
        set
        {
            if (_carEntireColor.r == carBonnetColor.r && _carEntireColor.g == carBonnetColor.g && _carEntireColor.b == carBonnetColor.b && _carEntireSurface == carBonnetSurface) //Entire�� Bonnet�� ������ ������
            {
                carBonnetColor = value; //���� ����
            }
            if (_carEntireColor.r == carSideMirrorColor.r && _carEntireColor.g == carSideMirrorColor.g && _carEntireColor.b == carSideMirrorColor.b && _carEntireSurface == carSideMirrorSurface) //Entire�� SideMirror�� ������ ������
            {
                carSideMirrorColor = value; //���� ����
            }
            if (_carEntireColor.r == carRoofColor.r && _carEntireColor.g == carRoofColor.g && _carEntireColor.b == carRoofColor.b && _carEntireSurface == carRoofSurface) //Entire�� Roof�� ������ ������
            {
                carRoofColor = value; //���� ����
            }

            _carEntireColor = value; //���� ����
            Utility.SetCarMaterialsProperty(ref carEntireMaterials, "_Main_Color", value); //���� ���� ���� ����
        }
    }

    private Color32 _carBonnetColor;
    public Color32 carBonnetColor  //Bonnet Color ������Ʈ
    {
        get
        {
            return _carBonnetColor; //���� ����
        }
        set
        {
            _carBonnetColor = value; //���� ����
            Utility.SetCarMaterialsProperty(ref carBonnetMaterials, "_Main_Color", value); //���� ���� ���� ����
        }
    }

    private Color32 _carSideMirrorColor;
    public Color32 carSideMirrorColor  //SideMirror Color ������Ʈ
    {
        get
        {
            return _carSideMirrorColor; //���� ����
        }
        set
        {
            _carSideMirrorColor = value; //���� ����
            Utility.SetCarMaterialsProperty(ref carSideMirrorMaterials, "_Main_Color", value); //���� ���� ���� ����
        }
    }

    private Color32 _carRoofColor;
    public Color32 carRoofColor  //Roof Color ������Ʈ
    {
        get
        {
            return _carRoofColor; //���� ����
        }
        set
        {
            _carRoofColor = value; //���� ����
            Utility.SetCarMaterialsProperty(ref carRoofMaterials, "_Main_Color", value); //���� ���� ���� ����
        }
    }

    private Color32 _carChromeDeleteColor; //ChromeDelete Color ������Ʈ
    public Color32 carChromeDeleteColor  //ChromeDelete Color ������Ʈ
    {
        get
        {
            return _carChromeDeleteColor; //���� ����
        }
        set
        {
            _carChromeDeleteColor = value; //���� ����
            Utility.SetCarMaterialsProperty(ref carChromeDeleteMaterials, "_Main_Color", value); //���� ���� ���� ����
        }
    }

    private Color32 _carWheelColor;
    public Color32 carWheelColor  //Wheel Color ������Ʈ
    {
        get
        {
            return _carWheelColor; //���� ����
        }
        set
        {
            _carWheelColor = value; //���� ����
            Utility.SetCarMaterialsProperty(ref carWheelMaterials, "_Main_Color", value); //���� ���� ���� ����
        }
    }

    private Color32 _carBrakeCaliperColor;
    public Color32 carBrakeCaliperColor  //BrakeCaliper Color ������Ʈ
    {
        get
        {
            return _carBrakeCaliperColor; //���� ����
        }
        set
        {
            _carBrakeCaliperColor = value; //���� ����
            Utility.SetCarMaterialsProperty(ref carBrakeCaliperMaterials, "_Main_Color", value); //���� ���� ���� ����
        }
    }

    private Color32 _carWindowColor;
    public Color32 carWindowColor  //Window Color ������Ʈ
    {
        get
        {
            return _carWindowColor; //���� ����
        }
        set
        {
            _carWindowColor = value; //���� ����
            Utility.SetCarMaterialsProperty(ref carWindowMaterials, "_Main_Color", _carWindowColor); //���� ���� ���� ����
        }
    }
    #endregion

    #region CarPaintPartSurface
    public int carSelectedPaintPartSurface
    {
        get
        {
            switch (selectedPaintPart) //���õ� ���� ������ ���� ���� ��ȯ
            {
                case PaintPart.Entire:
                    return carEntireSurface;
                case PaintPart.Bonnet:
                    return carBonnetSurface;
                case PaintPart.SideMirror:
                    return carSideMirrorSurface;
                case PaintPart.Roof:
                    return carRoofSurface;
                case PaintPart.ChromeDelete:
                    return carChromeDeleteSurface;
                case PaintPart.Wheel:
                    return carWheelSurface;
                case PaintPart.BrakeCaliper:
                    return carBrakeCaliperSurface;
                default:
                    return 0;
            }
        }
        set
        {
            switch (selectedPaintPart) //���õ� ���� ������ ���� ���� ����
            {
                case PaintPart.Entire:
                    carEntireSurface = value;
                    break;
                case PaintPart.Bonnet:
                    carBonnetSurface = value;
                    break;
                case PaintPart.SideMirror:
                    carSideMirrorSurface = value;
                    break;
                case PaintPart.Roof:
                    carRoofSurface = value;
                    break;
                case PaintPart.ChromeDelete:
                    carChromeDeleteSurface = value;
                    break;
                case PaintPart.Wheel:
                    carWheelSurface = value;
                    break;
                case PaintPart.BrakeCaliper:
                    carBrakeCaliperSurface = value;
                    break;
                default:
                    break;
            }
        }
    } //���õ� ���� ���� ����
    public int carSelectedPaintPartSurfacesLength //���õ� ���� ���� ������ ����
    {
        get
        {
            switch (selectedPaintPart) //���õ� ���� ������ ���� ��ȯ
            {
                case PaintPart.Entire:
                    return carEntireSurfaces.Length;
                case PaintPart.Bonnet:
                    return carBonnetSurfaces.Length;
                case PaintPart.SideMirror:
                    return carSideMirrorSurfaces.Length;
                case PaintPart.Roof:
                    return carRoofSurfaces.Length;
                case PaintPart.ChromeDelete:
                    return carChromeDeleteSurfaces.Length;
                case PaintPart.Wheel:
                    return carWheelSurfaces.Length;
                case PaintPart.BrakeCaliper:
                    return carBrakeCaliperSurfaces.Length;
                default:
                    return 0;
            }
        }
    }

    private int _carEntireSurface;
    public int carEntireSurface //Entire ����
    {
        get
        {
            return _carEntireSurface; //���� ��ȯ
        }
        set
        {
            if (_carEntireSurface == carBonnetSurface && _carEntireColor.r == carBonnetColor.r && _carEntireColor.g == carBonnetColor.g && _carEntireColor.b == carBonnetColor.b) //Entire�� Bonnet�� ������ ������
            {
                carBonnetSurface = value; //���� ����
            }
            if (_carEntireSurface == carSideMirrorSurface && _carEntireColor.r == carSideMirrorColor.r && _carEntireColor.g == carSideMirrorColor.g && _carEntireColor.b == carSideMirrorColor.b) //Entire�� SideMirror�� ������ ������
            {
                carSideMirrorSurface = value; //���� ����
            }
            if (_carEntireSurface == carRoofSurface && _carEntireColor.r == carRoofColor.r && _carEntireColor.g == carRoofColor.g && _carEntireColor.b == carRoofColor.b) //Entire�� Roof�� ������ ������
            {
                carRoofSurface = value; //���� ����
            }

            _carEntireSurface = value; //���� ����

            if (_carEntireSurface == 0 && carEntireMaterials != null && carEntireDefaultMaterials != null) //�⺻ �����̸�
            {
                for (int i = 0; i < carEntireMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carEntireMaterials[i] != null && carEntireDefaultMaterials[i] != null)
                    {
                        carEntireMaterials[i].shader = carEntireDefaultMaterials[i].shader; //�⺻ ���� ���̴� ����
                        carEntireMaterials[i].CopyPropertiesFromMaterial(carEntireDefaultMaterials[i]); //�⺻ ���� Property ����
                    }
                }
            }
            else if (_carEntireSurface != 0 && carEntireMaterials != null && DBController.dbController.carEntireSurfaceProperties != null) //�⺻ ������ �ƴϸ�
            {
                for (int i = 0; i < carEntireMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carEntireMaterials[i] != null && DBController.dbController.carEntireSurfaceProperties[_carEntireSurface - 1] != null)
                    {
                        carEntireMaterials[i].shader = DBController.dbController.carEntireSurfaceProperties[_carEntireSurface - 1].shader; //���� ���̴� ����
                        carEntireMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carEntireSurfaceProperties[_carEntireSurface - 1]); //�⺻ ���� Property ����
                        carEntireColor = carEntireColor; //���� ����
                    }
                }
            }

            Transform carEntireSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.Entire); //���� ���� ������ Text���� �θ� ����
            for (int i = 0; i < carEntireSurfaces.Length + 1; ++i) carEntireSurfaceTexts.GetChild(i).gameObject.SetActive(false); //��� ���� ���� ������ Text�� ��Ȱ��ȭ
            carEntireSurfaceTexts.GetChild(_carEntireSurface).gameObject.SetActive(true); //���õ� ���� ���� ������ Text�� Ȱ��ȭ
        }
    }
    public Shader[] carEntireSurfaces; //Entire ������

    private int _carBonnetSurface;
    public int carBonnetSurface //Bonnet ����
    {
        get
        {
            return _carBonnetSurface; //���� ��ȯ
        }
        set
        {
            _carBonnetSurface = value; //���� ����

            if (_carBonnetSurface == 0 && carBonnetMaterials != null && carBonnetDefaultMaterials != null) //�⺻ �����̸�
            {
                for (int i = 0; i < carBonnetMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carBonnetMaterials[i] != null && carBonnetDefaultMaterials[i] != null)
                    {
                        carBonnetMaterials[i].shader = carBonnetDefaultMaterials[i].shader; //�⺻ ���� ���̴� ����
                        carBonnetMaterials[i].CopyPropertiesFromMaterial(carBonnetDefaultMaterials[i]); //�⺻ ���� Property ����
                    }
                }
            }
            else if (_carBonnetSurface != 0 && carBonnetMaterials != null && DBController.dbController.carBonnetSurfaceProperties != null) //�⺻ ������ �ƴϸ�
            {
                for (int i = 0; i < carBonnetMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carBonnetMaterials[i] != null && DBController.dbController.carBonnetSurfaceProperties[_carBonnetSurface - 1] != null)
                    {
                        carBonnetMaterials[i].shader = DBController.dbController.carBonnetSurfaceProperties[_carBonnetSurface - 1].shader; //���� ���̴� ����
                        carBonnetMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carBonnetSurfaceProperties[_carBonnetSurface - 1]); //�⺻ ���� Property ����
                        carBonnetColor = carBonnetColor; //���� ����
                    }
                }
            }

            Transform carBonnetSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.Bonnet); //���� ���� ������ Text���� �θ� ����
            for (int i = 0; i < carBonnetSurfaces.Length + 1; ++i) carBonnetSurfaceTexts.GetChild(i).gameObject.SetActive(false); //��� ���� ���� ������ Text�� ��Ȱ��ȭ
            carBonnetSurfaceTexts.GetChild(_carBonnetSurface).gameObject.SetActive(true); //���õ� ���� ���� ������ Text�� Ȱ��ȭ
        }
    }
    public Shader[] carBonnetSurfaces; //Bonnet ������

    private int _carSideMirrorSurface;
    public int carSideMirrorSurface //SideMirror ����
    {
        get
        {
            return _carSideMirrorSurface; //���� ��ȯ
        }
        set
        {
            _carSideMirrorSurface = value; //���� ����

            if (_carSideMirrorSurface == 0 && carSideMirrorMaterials != null && carSideMirrorDefaultMaterials != null) //�⺻ �����̸�
            {
                for (int i = 0; i < carSideMirrorMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carSideMirrorMaterials[i] != null && carSideMirrorDefaultMaterials[i] != null)
                    {
                        carSideMirrorMaterials[i].shader = carSideMirrorDefaultMaterials[i].shader; //�⺻ ���� ���̴� ����
                        carSideMirrorMaterials[i].CopyPropertiesFromMaterial(carSideMirrorDefaultMaterials[i]); //�⺻ ���� Property ����
                    }
                }
            }
            else if (_carSideMirrorSurface != 0 && carSideMirrorMaterials != null && DBController.dbController.carSideMirrorSurfaceProperties != null) //�⺻ ������ �ƴϸ�
            {
                for (int i = 0; i < carSideMirrorMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carSideMirrorMaterials[i] != null && DBController.dbController.carSideMirrorSurfaceProperties[_carSideMirrorSurface - 1] != null)
                    {
                        carSideMirrorMaterials[i].shader = DBController.dbController.carSideMirrorSurfaceProperties[_carSideMirrorSurface - 1].shader; //���� ���̴� ����
                        carSideMirrorMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carSideMirrorSurfaceProperties[_carSideMirrorSurface - 1]); //�⺻ ���� Property ����
                        carSideMirrorColor = carSideMirrorColor; //���� ����
                    }
                }
            }

            Transform carSideMirrorSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.SideMirror); //���� ���� ������ Text���� �θ� ����
            for (int i = 0; i < carSideMirrorSurfaces.Length + 1; ++i) carSideMirrorSurfaceTexts.GetChild(i).gameObject.SetActive(false); //��� ���� ���� ������ Text�� ��Ȱ��ȭ
            carSideMirrorSurfaceTexts.GetChild(_carSideMirrorSurface).gameObject.SetActive(true); //���õ� ���� ���� ������ Text�� Ȱ��ȭ
        }
    }
    public Shader[] carSideMirrorSurfaces; //SideMirror ������

    private int _carRoofSurface;
    public int carRoofSurface //Roof ����
    {
        get
        {
            return _carRoofSurface; //���� ��ȯ
        }
        set
        {
            _carRoofSurface = value; //���� ����

            if (_carRoofSurface == 0 && carRoofMaterials != null && carRoofDefaultMaterials != null) //�⺻ �����̸�
            {
                for (int i = 0; i < carRoofMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carRoofMaterials[i] != null && carRoofDefaultMaterials[i] != null)
                    {
                        carRoofMaterials[i].shader = carRoofDefaultMaterials[i].shader; //�⺻ ���� ���̴� ����
                        carRoofMaterials[i].CopyPropertiesFromMaterial(carRoofDefaultMaterials[i]); //�⺻ ���� Property ����
                    }
                }
            }
            else if (_carRoofSurface != 0 && carRoofMaterials != null && DBController.dbController.carRoofSurfaceProperties != null) //�⺻ ������ �ƴϸ�
            {
                for (int i = 0; i < carRoofMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carRoofMaterials[i] != null && DBController.dbController.carRoofSurfaceProperties[_carRoofSurface - 1] != null)
                    {
                        carRoofMaterials[i].shader = DBController.dbController.carRoofSurfaceProperties[_carRoofSurface - 1].shader; //���� ���̴� ����
                        carRoofMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carRoofSurfaceProperties[_carRoofSurface - 1]); //�⺻ ���� Property ����
                        carRoofColor = carRoofColor; //���� ����
                    }
                }
            }

            Transform carRoofSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.Roof); //���� ���� ������ Text���� �θ� ����
            for (int i = 0; i < carRoofSurfaces.Length + 1; ++i) carRoofSurfaceTexts.GetChild(i).gameObject.SetActive(false); //��� ���� ���� ������ Text�� ��Ȱ��ȭ
            carRoofSurfaceTexts.GetChild(_carRoofSurface).gameObject.SetActive(true); //���õ� ���� ���� ������ Text�� Ȱ��ȭ
        }
    }
    public Shader[] carRoofSurfaces; //Roof ������

    private int _carChromeDeleteSurface;
    public int carChromeDeleteSurface //ChromeDelete ����
    {
        get
        {
            return _carChromeDeleteSurface; //���� ��ȯ
        }
        set
        {
            _carChromeDeleteSurface = value; //���� ����

            if (_carChromeDeleteSurface == 0 && carChromeDeleteMaterials != null && carChromeDeleteDefaultMaterials != null) //�⺻ �����̸�
            {
                for (int i = 0; i < carChromeDeleteMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carChromeDeleteMaterials[i] != null && carChromeDeleteDefaultMaterials[i] != null)
                    {
                        carChromeDeleteMaterials[i].shader = carChromeDeleteDefaultMaterials[i].shader; //�⺻ ���� ���̴� ����
                        carChromeDeleteMaterials[i].CopyPropertiesFromMaterial(carChromeDeleteDefaultMaterials[i]); //�⺻ ���� Property ����
                    }
                }
            }
            else if (_carChromeDeleteSurface != 0 && carChromeDeleteMaterials != null && DBController.dbController.carChromeDeleteSurfaceProperties != null) //�⺻ ������ �ƴϸ�
            {
                for (int i = 0; i < carChromeDeleteMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carChromeDeleteMaterials[i] != null && DBController.dbController.carChromeDeleteSurfaceProperties[_carChromeDeleteSurface - 1] != null)
                    {
                        carChromeDeleteMaterials[i].shader = DBController.dbController.carChromeDeleteSurfaceProperties[_carChromeDeleteSurface - 1].shader; //���� ���̴� ����
                        carChromeDeleteMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carChromeDeleteSurfaceProperties[_carChromeDeleteSurface - 1]); //�⺻ ���� Property ����
                        carChromeDeleteColor = carChromeDeleteColor; //���� ����
                    }
                }
            }

            Transform carChromeDeleteSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.ChromeDelete); //���� ���� ������ Text���� �θ� ����
            for (int i = 0; i < carChromeDeleteSurfaces.Length + 1; ++i) carChromeDeleteSurfaceTexts.GetChild(i).gameObject.SetActive(false); //��� ���� ���� ������ Text�� ��Ȱ��ȭ
            carChromeDeleteSurfaceTexts.GetChild(_carChromeDeleteSurface).gameObject.SetActive(true); //���õ� ���� ���� ������ Text�� Ȱ��ȭ
        }
    }
    public Shader[] carChromeDeleteSurfaces; //ChromeDelete ������

    private int _carWheelSurface;
    public int carWheelSurface //Wheel ����
    {
        get
        {
            return _carWheelSurface; //���� ��ȯ
        }
        set
        {
            _carWheelSurface = value; //���� ����

            if (_carWheelSurface == 0 && carWheelMaterials != null && carWheelDefaultMaterials != null) //�⺻ �����̸�
            {
                for (int i = 0; i < carWheelMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carWheelMaterials[i] != null && carWheelDefaultMaterials[i] != null)
                    {
                        carWheelMaterials[i].shader = carWheelDefaultMaterials[i].shader; //�⺻ ���� ���̴� ����
                        carWheelMaterials[i].CopyPropertiesFromMaterial(carWheelDefaultMaterials[i]); //�⺻ ���� Property ����
                    }
                }
            }
            else if (_carWheelSurface != 0 && carWheelMaterials != null && DBController.dbController.carWheelSurfaceProperties != null) //�⺻ ������ �ƴϸ�
            {
                for (int i = 0; i < carWheelMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carWheelMaterials[i] != null && DBController.dbController.carWheelSurfaceProperties[_carWheelSurface - 1] != null)
                    {
                        carWheelMaterials[i].shader = DBController.dbController.carWheelSurfaceProperties[_carWheelSurface - 1].shader; //���� ���̴� ����
                        carWheelMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carWheelSurfaceProperties[_carWheelSurface - 1]); //�⺻ ���� Property ����
                        carWheelColor = carWheelColor; //���� ����
                    }
                }
            }

            Transform carWheelSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.Wheel); //���� ���� ������ Text���� �θ� ����
            for (int i = 0; i < carWheelSurfaces.Length + 1; ++i) carWheelSurfaceTexts.GetChild(i).gameObject.SetActive(false); //��� ���� ���� ������ Text�� ��Ȱ��ȭ
            carWheelSurfaceTexts.GetChild(_carWheelSurface).gameObject.SetActive(true); //���õ� ���� ���� ������ Text�� Ȱ��ȭ
        }
    }
    public Shader[] carWheelSurfaces; //Wheel ������

    private int _carBrakeCaliperSurface;
    public int carBrakeCaliperSurface //BrakeCaliper ����
    {
        get
        {
            return _carBrakeCaliperSurface; //���� ��ȯ
        }
        set
        {
            _carBrakeCaliperSurface = value; //���� ����

            if (_carBrakeCaliperSurface == 0 && carBrakeCaliperMaterials != null && carBrakeCaliperDefaultMaterials != null) //�⺻ �����̸�
            {
                for (int i = 0; i < carBrakeCaliperMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carBrakeCaliperMaterials[i] != null && carBrakeCaliperDefaultMaterials[i] != null)
                    {
                        carBrakeCaliperMaterials[i].shader = carBrakeCaliperDefaultMaterials[i].shader; //�⺻ ���� ���̴� ����
                        carBrakeCaliperMaterials[i].CopyPropertiesFromMaterial(carBrakeCaliperDefaultMaterials[i]); //�⺻ ���� Property ����
                    }
                }
            }
            else if (_carBrakeCaliperSurface != 0 && carBrakeCaliperMaterials != null && DBController.dbController.carBrakeCaliperSurfaceProperties != null) //�⺻ ������ �ƴϸ�
            {
                for (int i = 0; i < carBrakeCaliperMaterials.Length; ++i) //���� ������ŭ �ݺ�
                {
                    if (carBrakeCaliperMaterials[i] != null && DBController.dbController.carBrakeCaliperSurfaceProperties[_carBrakeCaliperSurface - 1] != null)
                    {
                        carBrakeCaliperMaterials[i].shader = DBController.dbController.carBrakeCaliperSurfaceProperties[_carBrakeCaliperSurface - 1].shader; //���� ���̴� ����
                        carBrakeCaliperMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carBrakeCaliperSurfaceProperties[_carBrakeCaliperSurface - 1]); //�⺻ ���� Property ����
                        carBrakeCaliperColor = carBrakeCaliperColor; //���� ����
                    }
                }
            }

            Transform carBrakeCaliperSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.BrakeCaliper); //���� ���� ������ Text���� �θ� ����
            for (int i = 0; i < carBrakeCaliperSurfaces.Length + 1; ++i) carBrakeCaliperSurfaceTexts.GetChild(i).gameObject.SetActive(false); //��� ���� ���� ������ Text�� ��Ȱ��ȭ
            carBrakeCaliperSurfaceTexts.GetChild(_carBrakeCaliperSurface).gameObject.SetActive(true); //���õ� ���� ���� ������ Text�� Ȱ��ȭ
        }
    }
    public Shader[] carBrakeCaliperSurfaces; //BrakeCaliper ������
    #endregion

    private void Awake()
    {
        carEntireMaterials = new Material[carEntireMaterialLength];
        carBonnetMaterials = new Material[carBonnetMaterialLength];
        carSideMirrorMaterials = new Material[carSideMirrorMaterialLength];
        carRoofMaterials = new Material[carRoofMaterialLength];
        carChromeDeleteMaterials = new Material[carChromeDeleteMaterialLength];
        carWheelMaterials = new Material[carWheelMaterialLength];
        carBrakeCaliperMaterials = new Material[carBrakeCaliperMaterialLength];
        carWindowMaterials = new Material[carWindowMaterialLength];

        carEntireDefaultMaterials = new Material[carEntireMaterialLength];
        carBonnetDefaultMaterials = new Material[carBonnetMaterialLength];
        carSideMirrorDefaultMaterials = new Material[carSideMirrorMaterialLength];
        carRoofDefaultMaterials = new Material[carRoofMaterialLength];
        carChromeDeleteDefaultMaterials = new Material[carChromeDeleteMaterialLength];
        carWheelDefaultMaterials = new Material[carWheelMaterialLength];
        carBrakeCaliperDefaultMaterials = new Material[carBrakeCaliperMaterialLength];

        carPaint = this; //���� ���� ���� ����
    }

    private void Start()
    {
        OnClickPaintResetButton();
        OnClickPaintPartButton(0);
    }

    #region Interface
    /* CarPaintPanel�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �Լ� */
    public void ActivateCarPaintPanel(bool state)
    {
        Home.home.ActivateHome(!state); //Home Ȱ��ȭ/��Ȱ��ȭ
        carPaintPanelCanvas.enabled = state; //CarPaintPanel�� Canvas ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
        MainCamera.mainCamera.SetCurrentPos(state ? CameraPosition.CarPaint : CameraPosition.Center); //ī�޶� ��ġ ����
    }

    /* ���� ���� ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickPaintPartButton(int paintPart)
    {
        selectedPaintPart = (PaintPart)paintPart; //���õ� ���� ���� ����
    }

    /* Surface Button�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickSurfaceButton(bool isNext)
    {
        if (isNext) //�����̸�
        {
            carSelectedPaintPartSurface = carSelectedPaintPartSurface == carSelectedPaintPartSurfacesLength ? 0 : carSelectedPaintPartSurface + 1; //���� ����
        }
        else //�����̸�
        {
            carSelectedPaintPartSurface = carSelectedPaintPartSurface == 0 ? carSelectedPaintPartSurfacesLength : carSelectedPaintPartSurface - 1; //���� ����
        }
    }

    /* PaintResetButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickPaintResetButton()
    {
        carEntireColor = Color.black;
        carBonnetColor = Color.black;
        carSideMirrorColor = Color.black;
        carRoofColor = Color.black;
        carChromeDeleteColor = Color.white;
        carWheelColor = Color.gray;
        carBrakeCaliperColor = Color.gray;
        carWindowColor = Color.black;

        carEntireSurface = 0;
        carBonnetSurface = 0;
        carSideMirrorSurface = 0;
        carRoofSurface = 0;
        carChromeDeleteSurface = 0;
        carWheelSurface = 0;
        carBrakeCaliperSurface = 0;

        CarPaintColorPicker.carPaintColorPicker.ApplyColor(carSelectedPaintPartColor); //Color Picker ���� ����
    }
    #endregion

    #region Caching
    /* ���� ���� ���� Material���� ĳ���ϴ� �Լ� */
    public void CachingCarPaintPartMaterials(PaintPart paintPart)
    {
        if (PaintPart.Entire == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //���� Body�� Transform ������Ʈ�� �����ϸ�
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //���� Body�� Material ������Ʈ��

            Material[] frontBumperMaterials = null;
            if (SuspensionController.suspensionController.movePartsTransforms.frontBumper != null) //���� FrontBumper�� Transform ������Ʈ�� �����ϸ�
                frontBumperMaterials = SuspensionController.suspensionController.movePartsTransforms.frontBumper.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //���� FrontBumper�� Material ������Ʈ��

            Material[] rearBumperMaterials = null; //���� RearBumper�� Transform ������Ʈ�� �����ϸ�
            if (SuspensionController.suspensionController.movePartsTransforms.rearBumper != null)
                rearBumperMaterials = SuspensionController.suspensionController.movePartsTransforms.rearBumper.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //���� RearBumper�� Material ������Ʈ��

            Material[] materials = new Material[carEntireMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "Body"),
                Utility.FindMaterialWithName(ref frontBumperMaterials, "Body"),
                Utility.FindMaterialWithName(ref rearBumperMaterials, "Body")
            };

            /* �⺻ ���� */
            for (int i = 0; i < carEntireMaterialLength; ++i) //��Ƽ���� ������ŭ �ݺ�
            {
                if (materials[i] == null) carEntireDefaultMaterials[i] = null; //��Ƽ������ �������� ������
                else carEntireDefaultMaterials[i] = carEntireMaterials[i] == null ? Instantiate(materials[i]) : carEntireDefaultMaterials[i]; //��Ƽ������ �����ϸ�
            }

            carEntireMaterials = materials; //ĳ��
        }
        else if (PaintPart.Bonnet == paintPart)
        {
            Material[] bonnetMaterials = null;
            if (SuspensionController.suspensionController.movePartsTransforms.bonnet != null) //���� Bonnet�� Transform ������Ʈ�� �����ϸ�
                bonnetMaterials = SuspensionController.suspensionController.movePartsTransforms.bonnet.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //���� Bonnet�� Material ������Ʈ��

            Material[] materials = new Material[carBonnetMaterialLength]
            {
                Utility.FindMaterialWithName(ref bonnetMaterials, "Body"),
            };

            /* �⺻ ���� */
            for (int i = 0; i < carBonnetMaterialLength; ++i) //��Ƽ���� ������ŭ �ݺ�
            {
                if (materials[i] == null) carBonnetDefaultMaterials[i] = null; //��Ƽ������ �������� ������
                else carBonnetDefaultMaterials[i] = carBonnetMaterials[i] == null ? Instantiate(materials[i]) : carBonnetDefaultMaterials[i]; //��Ƽ������ �����ϸ�
            }

            carBonnetMaterials = materials; //ĳ��
        }
        else if (PaintPart.SideMirror == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //���� Body�� Transform ������Ʈ�� �����ϸ�
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //���� Body�� Material ������Ʈ��

            Material[] materials = new Material[carSideMirrorMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "SideMirror"),
            };

            /* �⺻ ���� */
            for (int i = 0; i < carSideMirrorMaterialLength; ++i) //��Ƽ���� ������ŭ �ݺ�
            {
                if (materials[i] == null) carSideMirrorDefaultMaterials[i] = null; //��Ƽ������ �������� ������
                else carSideMirrorDefaultMaterials[i] = carSideMirrorMaterials[i] == null ? Instantiate(materials[i]) : carSideMirrorDefaultMaterials[i]; //��Ƽ������ �����ϸ�
            }

            carSideMirrorMaterials = materials; //ĳ��
        }
        else if (PaintPart.Roof == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //���� Body�� Transform ������Ʈ�� �����ϸ�
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //���� Body�� Material ������Ʈ��

            Material[] materials = new Material[carRoofMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "Roof"),
            };

            /* �⺻ ���� */
            for (int i = 0; i < carRoofMaterialLength; ++i) //��Ƽ���� ������ŭ �ݺ�
            {
                if (materials[i] == null) carRoofDefaultMaterials[i] = null; //��Ƽ������ �������� ������
                else carRoofDefaultMaterials[i] = carRoofMaterials[i] == null ? Instantiate(materials[i]) : carRoofDefaultMaterials[i]; //��Ƽ������ �����ϸ�
            }

            carRoofMaterials = materials; //ĳ��
        }
        else if (PaintPart.ChromeDelete == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //���� Body�� Transform ������Ʈ�� �����ϸ�
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //���� Body�� Material ������Ʈ��

            Material[] grillMaterials = null; //���� Grill�� Material ������Ʈ��
            if (SuspensionController.suspensionController.movePartsTransforms.grill != null) //���� Grill�� Transform ������Ʈ�� �����ϸ�
                grillMaterials = SuspensionController.suspensionController.movePartsTransforms.grill.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //���� Grill�� Material ������Ʈ��

            Material[] bonnetMaterials = null;
            if (SuspensionController.suspensionController.movePartsTransforms.bonnet != null) //���� Bonnet�� Transform ������Ʈ�� �����ϸ�
                bonnetMaterials = SuspensionController.suspensionController.movePartsTransforms.bonnet.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //���� Bonnet�� Material ������Ʈ��

            Material[] frontBumperMaterials = null;
            if (SuspensionController.suspensionController.movePartsTransforms.frontBumper != null) //���� FrontBumper�� Transform ������Ʈ�� �����ϸ�
                frontBumperMaterials = SuspensionController.suspensionController.movePartsTransforms.frontBumper.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //���� FrontBumper�� Material ������Ʈ��

            Material[] materials = new Material[carChromeDeleteMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "ChromeDelete"),
                Utility.FindMaterialWithName(ref bodyMaterials, "ChromeDelete2"),
                Utility.FindMaterialWithName(ref bodyMaterials, "ChromeDelete2"),
                Utility.FindMaterialWithName(ref grillMaterials, "ChromeDelete"),
                Utility.FindMaterialWithName(ref grillMaterials, "ChromeDelete2"),
                Utility.FindMaterialWithName(ref grillMaterials, "ChromeDelete3"),
                Utility.FindMaterialWithName(ref bonnetMaterials, "ChromeDelete"),
                Utility.FindMaterialWithName(ref bonnetMaterials, "ChromeDelete2"),
                Utility.FindMaterialWithName(ref bonnetMaterials, "ChromeDelete3"),
                Utility.FindMaterialWithName(ref frontBumperMaterials, "ChromeDelete"),
                Utility.FindMaterialWithName(ref frontBumperMaterials, "ChromeDelete2"),
                Utility.FindMaterialWithName(ref frontBumperMaterials, "ChromeDelete3")
            };

            /* �⺻ ���� */
            for (int i = 0; i < carChromeDeleteMaterialLength; ++i) //��Ƽ���� ������ŭ �ݺ�
            {
                if (materials[i] == null) carChromeDeleteDefaultMaterials[i] = null; //��Ƽ������ �������� ������
                else carChromeDeleteDefaultMaterials[i] = carChromeDeleteMaterials[i] == null ? Instantiate(materials[i]) : carChromeDeleteDefaultMaterials[i]; //��Ƽ������ �����ϸ�
            }

            carChromeDeleteMaterials = materials; //ĳ��
        }
        else if (PaintPart.Wheel == paintPart)
        {
            Material[][] wheelsMaterials = new Material[4][] //���� Wheel���� Material ������Ʈ��
            {
                SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials,
                SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials,
                SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials,
                SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials
            };

            Material[] materials = new Material[carWheelMaterialLength]
            {
                Utility.FindMaterialWithName(ref wheelsMaterials[0], "WheelFace"),
                Utility.FindMaterialWithName(ref wheelsMaterials[1], "WheelFace"),
                Utility.FindMaterialWithName(ref wheelsMaterials[2], "WheelFace"),
                Utility.FindMaterialWithName(ref wheelsMaterials[3], "WheelFace"),
                Utility.FindMaterialWithName(ref wheelsMaterials[0], "WheelWindow"),
                Utility.FindMaterialWithName(ref wheelsMaterials[1], "WheelWindow"),
                Utility.FindMaterialWithName(ref wheelsMaterials[2], "WheelWindow"),
                Utility.FindMaterialWithName(ref wheelsMaterials[3], "WheelWindow")
            };

            /* �⺻ ���� */
            for (int i = 0; i < carWheelMaterialLength; ++i) //��Ƽ���� ������ŭ �ݺ�
            {
                if (materials[i] == null) carWheelDefaultMaterials[i] = null; //��Ƽ������ �������� ������
                else carWheelDefaultMaterials[i] = carWheelMaterials[i] == null ? Instantiate(materials[i]) : carWheelDefaultMaterials[i]; //��Ƽ������ �����ϸ�
            }

            carWheelMaterials = materials; //ĳ��
        }
        else if (PaintPart.BrakeCaliper == paintPart)
        {
            Material[][] brakesMaterials = new Material[4][] //���� Brake���� Material ������Ʈ��
            {
                SuspensionController.suspensionController.movePartsTransforms.brake_Front_Left.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials,
                SuspensionController.suspensionController.movePartsTransforms.brake_Front_Right.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials,
                SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Left.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials,
                SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Right.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials
            };

            Material[] materials = new Material[carBrakeCaliperMaterialLength]
            {
                Utility.FindMaterialWithName(ref brakesMaterials[0], "BrakeCaliper"),
                Utility.FindMaterialWithName(ref brakesMaterials[1], "BrakeCaliper"),
                Utility.FindMaterialWithName(ref brakesMaterials[2], "BrakeCaliper"),
                Utility.FindMaterialWithName(ref brakesMaterials[3], "BrakeCaliper")
            };

            /* �⺻ ���� */
            for (int i = 0; i < carBrakeCaliperMaterialLength; ++i) //��Ƽ���� ������ŭ �ݺ�
            {
                if (materials[i] == null) carBrakeCaliperDefaultMaterials[i] = null; //��Ƽ������ �������� ������
                else carBrakeCaliperDefaultMaterials[i] = carBrakeCaliperMaterials[i] == null ? Instantiate(materials[i]) : carBrakeCaliperDefaultMaterials[i]; //��Ƽ������ �����ϸ�
            }

            carBrakeCaliperMaterials = materials; //ĳ��
        }
        else if (PaintPart.Window == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //���� Body�� Transform ������Ʈ�� �����ϸ�
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //���� Body�� Material ������Ʈ��

            carWindowMaterials = new Material[carWindowMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "Window")
            };
        }
    }
    #endregion
}