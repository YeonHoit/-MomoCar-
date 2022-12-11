using UnityEngine;
using UnityEngine.UI;

public class CarPaint : MonoBehaviour
{
    [Header("Static")]
    public static CarPaint carPaint; //전역 참조 변수

    /* 도색 부위 */
    public enum PaintPart
    {
        Entire, Bonnet, SideMirror, Roof, ChromeDelete, Wheel, BrakeCaliper, Window
    }

    [Header("Car Paint Panel")]
    public Canvas carPaintPanelCanvas; //CarPaintPanel의 Canvas 컴포넌트

    [Header("Select Paint Part")]
    private PaintPart _selectedPaintPart;
    public PaintPart selectedPaintPart //선택된 도색 부위
    {
        get
        {
            return _selectedPaintPart; //선택된 도색 부위 반환
        }
        set
        {
            _selectedPaintPart = value; //선택된 도색 부위 지정

            for (int i = 0; i < paintPartButtonImages.Length; ++i) paintPartButtonImages[i].color = paintPartButtonDeactivationColor; //모든 도색 부위 버튼 비활성화 색상으로 변경
            paintPartButtonImages[(int)_selectedPaintPart].color = paintPartButtonActivationColor; //선택된 도색 부위 버튼 활성화 색상으로 변경

            CarPaintColorPicker.carPaintColorPicker.ApplyColor(carSelectedPaintPartColor); //Color Picker 색상 갱신

            /* 재질 초기화 */
            switch (_selectedPaintPart) //선택된 도색 부위에 따라
            {
                case PaintPart.Entire:
                case PaintPart.Bonnet:
                case PaintPart.SideMirror:
                case PaintPart.Roof:
                case PaintPart.ChromeDelete:
                case PaintPart.Wheel:
                case PaintPart.BrakeCaliper:
                    for (int i = 0; i < carPaintSurfaceTextsTransform.childCount; ++i) carPaintSurfaceTextsTransform.GetChild(i).gameObject.SetActive(false); //모든 Surface Text 비활성화
                    carPaintSurfaceTextsTransform.GetChild((int)_selectedPaintPart).gameObject.SetActive(true); //선택된 도색 부위 재질에 따라 Surface Text 활성화

                    carPaintSurfaceTextsTransform.gameObject.SetActive(true); //SurfaceTexts 오브젝트 활성화
                    carPaintSurfacePreviousButton.SetActive(true); //SurfacePreviousButton 오브젝트 활성화
                    carPaintSurfaceNextButton.SetActive(true); //SurfaceNextButton 오브젝트 활성화
                    break;

                default:
                    carPaintSurfaceTextsTransform.gameObject.SetActive(false); //SurfaceTexts 오브젝트 비활성화
                    carPaintSurfacePreviousButton.SetActive(false); //SurfacePreviousButton 오브젝트 비활성화
                    carPaintSurfaceNextButton.SetActive(false); //SurfaceNextButton 오브젝트 비활성화
                    break;
            }
        }
    }
    public Image[] paintPartButtonImages; //도색 버튼의 Image 컴포넌트들
    private readonly Color32 paintPartButtonActivationColor = new Color32(25, 25, 25, 255); //도색 부위 버튼 활성화 색상
    private readonly Color32 paintPartButtonDeactivationColor = new Color32(200, 200, 200, 255); //도색 부위 버튼 비활성화 색상

    [Header("Materials")]
    private const int carEntireMaterialLength = 3; //Entire Material 개수
    public Material[] carEntireMaterials; //Entire Material 컴포넌트들
    private Material[] carEntireDefaultMaterials; //Entire의 기본 Material 컴포넌트들

    private const int carBonnetMaterialLength = 1; //Bonnet Material 개수
    public Material[] carBonnetMaterials; //Bonnet Material 컴포넌트들
    private Material[] carBonnetDefaultMaterials; //Bonnet의 기본 Material 컴포넌트들

    private const int carSideMirrorMaterialLength = 1; //SideMirror Material 개수
    public Material[] carSideMirrorMaterials; //SideMirror Material 컴포넌트들
    private Material[] carSideMirrorDefaultMaterials; //SideMirror의 기본 Material 컴포넌트들

    private const int carRoofMaterialLength = 1; //Roof Material 개수
    public Material[] carRoofMaterials; //Roof Material 컴포넌트들
    private Material[] carRoofDefaultMaterials; //Roof의 기본 Material 컴포넌트들

    private const int carChromeDeleteMaterialLength = 12; //ChromeDelete Material 개수
    public Material[] carChromeDeleteMaterials; //ChromeDelete Material 컴포넌트들
    private Material[] carChromeDeleteDefaultMaterials; //ChromeDelete의 기본 Material 컴포넌트들

    private const int carWheelMaterialLength = 8; //Wheel Material 개수
    public Material[] carWheelMaterials; //Wheel Material 컴포넌트들
    private Material[] carWheelDefaultMaterials; //Wheel의 기본 Material 컴포넌트들

    private const int carBrakeCaliperMaterialLength = 4; //BrakeCaliper Material 개수
    public Material[] carBrakeCaliperMaterials; //BrakeCaliper Material 컴포넌트들
    private Material[] carBrakeCaliperDefaultMaterials; //BrakeCaliper의 기본 Material 컴포넌트들

    private const int carWindowMaterialLength = 1; //Window Material 개수
    public Material[] carWindowMaterials; //Window Material 컴포넌트들

    [Header("Car PaintPart Surface")]
    public Transform carPaintSurfaceTextsTransform; //CarPaintPanel > Surface > Texts의 Transform 컴포넌트
    public GameObject carPaintSurfacePreviousButton; //CarPaintPanel > Surface > PreviousButton 오브젝트
    public GameObject carPaintSurfaceNextButton; //CarPaintPanel > Surface > NextButton 오브젝트

    #region CarPaintPartColor
    public Color32 carSelectedPaintPartColor //선택된 도색 부위 색상
    {
        get
        {
            switch (selectedPaintPart) //선택된 도색 부위에 따라 색상 반환
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
            switch (selectedPaintPart) //선택된 도색 부위에 따라 색상 지정
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
    public Color32 carEntireColor //Entire Color 컴포넌트
    {
        get
        {
            return _carEntireColor; //색상 지정
        }
        set
        {
            if (_carEntireColor.r == carBonnetColor.r && _carEntireColor.g == carBonnetColor.g && _carEntireColor.b == carBonnetColor.b && _carEntireSurface == carBonnetSurface) //Entire와 Bonnet의 색상이 같으면
            {
                carBonnetColor = value; //색상 변경
            }
            if (_carEntireColor.r == carSideMirrorColor.r && _carEntireColor.g == carSideMirrorColor.g && _carEntireColor.b == carSideMirrorColor.b && _carEntireSurface == carSideMirrorSurface) //Entire와 SideMirror의 색상이 같으면
            {
                carSideMirrorColor = value; //색상 변경
            }
            if (_carEntireColor.r == carRoofColor.r && _carEntireColor.g == carRoofColor.g && _carEntireColor.b == carRoofColor.b && _carEntireSurface == carRoofSurface) //Entire와 Roof의 색상이 같으면
            {
                carRoofColor = value; //색상 변경
            }

            _carEntireColor = value; //색상 변경
            Utility.SetCarMaterialsProperty(ref carEntireMaterials, "_Main_Color", value); //도색 부위 색상 변경
        }
    }

    private Color32 _carBonnetColor;
    public Color32 carBonnetColor  //Bonnet Color 컴포넌트
    {
        get
        {
            return _carBonnetColor; //색상 지정
        }
        set
        {
            _carBonnetColor = value; //색상 변경
            Utility.SetCarMaterialsProperty(ref carBonnetMaterials, "_Main_Color", value); //도색 부위 색상 변경
        }
    }

    private Color32 _carSideMirrorColor;
    public Color32 carSideMirrorColor  //SideMirror Color 컴포넌트
    {
        get
        {
            return _carSideMirrorColor; //색상 지정
        }
        set
        {
            _carSideMirrorColor = value; //색상 변경
            Utility.SetCarMaterialsProperty(ref carSideMirrorMaterials, "_Main_Color", value); //도색 부위 색상 변경
        }
    }

    private Color32 _carRoofColor;
    public Color32 carRoofColor  //Roof Color 컴포넌트
    {
        get
        {
            return _carRoofColor; //색상 지정
        }
        set
        {
            _carRoofColor = value; //색상 변경
            Utility.SetCarMaterialsProperty(ref carRoofMaterials, "_Main_Color", value); //도색 부위 색상 변경
        }
    }

    private Color32 _carChromeDeleteColor; //ChromeDelete Color 컴포넌트
    public Color32 carChromeDeleteColor  //ChromeDelete Color 컴포넌트
    {
        get
        {
            return _carChromeDeleteColor; //색상 지정
        }
        set
        {
            _carChromeDeleteColor = value; //색상 변경
            Utility.SetCarMaterialsProperty(ref carChromeDeleteMaterials, "_Main_Color", value); //도색 부위 색상 변경
        }
    }

    private Color32 _carWheelColor;
    public Color32 carWheelColor  //Wheel Color 컴포넌트
    {
        get
        {
            return _carWheelColor; //색상 지정
        }
        set
        {
            _carWheelColor = value; //색상 변경
            Utility.SetCarMaterialsProperty(ref carWheelMaterials, "_Main_Color", value); //도색 부위 색상 변경
        }
    }

    private Color32 _carBrakeCaliperColor;
    public Color32 carBrakeCaliperColor  //BrakeCaliper Color 컴포넌트
    {
        get
        {
            return _carBrakeCaliperColor; //색상 지정
        }
        set
        {
            _carBrakeCaliperColor = value; //색상 변경
            Utility.SetCarMaterialsProperty(ref carBrakeCaliperMaterials, "_Main_Color", value); //도색 부위 색상 변경
        }
    }

    private Color32 _carWindowColor;
    public Color32 carWindowColor  //Window Color 컴포넌트
    {
        get
        {
            return _carWindowColor; //색상 지정
        }
        set
        {
            _carWindowColor = value; //색상 변경
            Utility.SetCarMaterialsProperty(ref carWindowMaterials, "_Main_Color", _carWindowColor); //도색 부위 색상 변경
        }
    }
    #endregion

    #region CarPaintPartSurface
    public int carSelectedPaintPartSurface
    {
        get
        {
            switch (selectedPaintPart) //선택된 도색 부위에 따라 재질 반환
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
            switch (selectedPaintPart) //선택된 도색 부위에 따라 재질 지정
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
    } //선택된 도색 부위 재질
    public int carSelectedPaintPartSurfacesLength //선택된 도색 부위 재질들 길이
    {
        get
        {
            switch (selectedPaintPart) //선택된 도색 부위에 따라 반환
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
    public int carEntireSurface //Entire 재질
    {
        get
        {
            return _carEntireSurface; //재질 반환
        }
        set
        {
            if (_carEntireSurface == carBonnetSurface && _carEntireColor.r == carBonnetColor.r && _carEntireColor.g == carBonnetColor.g && _carEntireColor.b == carBonnetColor.b) //Entire와 Bonnet의 재질이 같으면
            {
                carBonnetSurface = value; //재질 변경
            }
            if (_carEntireSurface == carSideMirrorSurface && _carEntireColor.r == carSideMirrorColor.r && _carEntireColor.g == carSideMirrorColor.g && _carEntireColor.b == carSideMirrorColor.b) //Entire와 SideMirror의 재질이 같으면
            {
                carSideMirrorSurface = value; //재질 변경
            }
            if (_carEntireSurface == carRoofSurface && _carEntireColor.r == carRoofColor.r && _carEntireColor.g == carRoofColor.g && _carEntireColor.b == carRoofColor.b) //Entire와 Roof의 재질이 같으면
            {
                carRoofSurface = value; //재질 변경
            }

            _carEntireSurface = value; //재질 변경

            if (_carEntireSurface == 0 && carEntireMaterials != null && carEntireDefaultMaterials != null) //기본 재질이면
            {
                for (int i = 0; i < carEntireMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carEntireMaterials[i] != null && carEntireDefaultMaterials[i] != null)
                    {
                        carEntireMaterials[i].shader = carEntireDefaultMaterials[i].shader; //기본 재질 셰이더 지정
                        carEntireMaterials[i].CopyPropertiesFromMaterial(carEntireDefaultMaterials[i]); //기본 재질 Property 복사
                    }
                }
            }
            else if (_carEntireSurface != 0 && carEntireMaterials != null && DBController.dbController.carEntireSurfaceProperties != null) //기본 재질이 아니면
            {
                for (int i = 0; i < carEntireMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carEntireMaterials[i] != null && DBController.dbController.carEntireSurfaceProperties[_carEntireSurface - 1] != null)
                    {
                        carEntireMaterials[i].shader = DBController.dbController.carEntireSurfaceProperties[_carEntireSurface - 1].shader; //재질 셰이더 지정
                        carEntireMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carEntireSurfaceProperties[_carEntireSurface - 1]); //기본 재질 Property 복사
                        carEntireColor = carEntireColor; //색상 지정
                    }
                }
            }

            Transform carEntireSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.Entire); //도색 부위 재질의 Text들의 부모를 저장
            for (int i = 0; i < carEntireSurfaces.Length + 1; ++i) carEntireSurfaceTexts.GetChild(i).gameObject.SetActive(false); //모든 도색 부위 재질의 Text를 비활성화
            carEntireSurfaceTexts.GetChild(_carEntireSurface).gameObject.SetActive(true); //선택된 도색 부위 재질의 Text를 활성화
        }
    }
    public Shader[] carEntireSurfaces; //Entire 재질들

    private int _carBonnetSurface;
    public int carBonnetSurface //Bonnet 재질
    {
        get
        {
            return _carBonnetSurface; //재질 반환
        }
        set
        {
            _carBonnetSurface = value; //재질 변경

            if (_carBonnetSurface == 0 && carBonnetMaterials != null && carBonnetDefaultMaterials != null) //기본 재질이면
            {
                for (int i = 0; i < carBonnetMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carBonnetMaterials[i] != null && carBonnetDefaultMaterials[i] != null)
                    {
                        carBonnetMaterials[i].shader = carBonnetDefaultMaterials[i].shader; //기본 재질 셰이더 지정
                        carBonnetMaterials[i].CopyPropertiesFromMaterial(carBonnetDefaultMaterials[i]); //기본 재질 Property 복사
                    }
                }
            }
            else if (_carBonnetSurface != 0 && carBonnetMaterials != null && DBController.dbController.carBonnetSurfaceProperties != null) //기본 재질이 아니면
            {
                for (int i = 0; i < carBonnetMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carBonnetMaterials[i] != null && DBController.dbController.carBonnetSurfaceProperties[_carBonnetSurface - 1] != null)
                    {
                        carBonnetMaterials[i].shader = DBController.dbController.carBonnetSurfaceProperties[_carBonnetSurface - 1].shader; //재질 셰이더 지정
                        carBonnetMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carBonnetSurfaceProperties[_carBonnetSurface - 1]); //기본 재질 Property 복사
                        carBonnetColor = carBonnetColor; //색상 지정
                    }
                }
            }

            Transform carBonnetSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.Bonnet); //도색 부위 재질의 Text들의 부모를 저장
            for (int i = 0; i < carBonnetSurfaces.Length + 1; ++i) carBonnetSurfaceTexts.GetChild(i).gameObject.SetActive(false); //모든 도색 부위 재질의 Text를 비활성화
            carBonnetSurfaceTexts.GetChild(_carBonnetSurface).gameObject.SetActive(true); //선택된 도색 부위 재질의 Text를 활성화
        }
    }
    public Shader[] carBonnetSurfaces; //Bonnet 재질들

    private int _carSideMirrorSurface;
    public int carSideMirrorSurface //SideMirror 재질
    {
        get
        {
            return _carSideMirrorSurface; //재질 반환
        }
        set
        {
            _carSideMirrorSurface = value; //재질 변경

            if (_carSideMirrorSurface == 0 && carSideMirrorMaterials != null && carSideMirrorDefaultMaterials != null) //기본 재질이면
            {
                for (int i = 0; i < carSideMirrorMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carSideMirrorMaterials[i] != null && carSideMirrorDefaultMaterials[i] != null)
                    {
                        carSideMirrorMaterials[i].shader = carSideMirrorDefaultMaterials[i].shader; //기본 재질 셰이더 지정
                        carSideMirrorMaterials[i].CopyPropertiesFromMaterial(carSideMirrorDefaultMaterials[i]); //기본 재질 Property 복사
                    }
                }
            }
            else if (_carSideMirrorSurface != 0 && carSideMirrorMaterials != null && DBController.dbController.carSideMirrorSurfaceProperties != null) //기본 재질이 아니면
            {
                for (int i = 0; i < carSideMirrorMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carSideMirrorMaterials[i] != null && DBController.dbController.carSideMirrorSurfaceProperties[_carSideMirrorSurface - 1] != null)
                    {
                        carSideMirrorMaterials[i].shader = DBController.dbController.carSideMirrorSurfaceProperties[_carSideMirrorSurface - 1].shader; //재질 셰이더 지정
                        carSideMirrorMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carSideMirrorSurfaceProperties[_carSideMirrorSurface - 1]); //기본 재질 Property 복사
                        carSideMirrorColor = carSideMirrorColor; //색상 지정
                    }
                }
            }

            Transform carSideMirrorSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.SideMirror); //도색 부위 재질의 Text들의 부모를 저장
            for (int i = 0; i < carSideMirrorSurfaces.Length + 1; ++i) carSideMirrorSurfaceTexts.GetChild(i).gameObject.SetActive(false); //모든 도색 부위 재질의 Text를 비활성화
            carSideMirrorSurfaceTexts.GetChild(_carSideMirrorSurface).gameObject.SetActive(true); //선택된 도색 부위 재질의 Text를 활성화
        }
    }
    public Shader[] carSideMirrorSurfaces; //SideMirror 재질들

    private int _carRoofSurface;
    public int carRoofSurface //Roof 재질
    {
        get
        {
            return _carRoofSurface; //재질 반환
        }
        set
        {
            _carRoofSurface = value; //재질 변경

            if (_carRoofSurface == 0 && carRoofMaterials != null && carRoofDefaultMaterials != null) //기본 재질이면
            {
                for (int i = 0; i < carRoofMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carRoofMaterials[i] != null && carRoofDefaultMaterials[i] != null)
                    {
                        carRoofMaterials[i].shader = carRoofDefaultMaterials[i].shader; //기본 재질 셰이더 지정
                        carRoofMaterials[i].CopyPropertiesFromMaterial(carRoofDefaultMaterials[i]); //기본 재질 Property 복사
                    }
                }
            }
            else if (_carRoofSurface != 0 && carRoofMaterials != null && DBController.dbController.carRoofSurfaceProperties != null) //기본 재질이 아니면
            {
                for (int i = 0; i < carRoofMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carRoofMaterials[i] != null && DBController.dbController.carRoofSurfaceProperties[_carRoofSurface - 1] != null)
                    {
                        carRoofMaterials[i].shader = DBController.dbController.carRoofSurfaceProperties[_carRoofSurface - 1].shader; //재질 셰이더 지정
                        carRoofMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carRoofSurfaceProperties[_carRoofSurface - 1]); //기본 재질 Property 복사
                        carRoofColor = carRoofColor; //색상 지정
                    }
                }
            }

            Transform carRoofSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.Roof); //도색 부위 재질의 Text들의 부모를 저장
            for (int i = 0; i < carRoofSurfaces.Length + 1; ++i) carRoofSurfaceTexts.GetChild(i).gameObject.SetActive(false); //모든 도색 부위 재질의 Text를 비활성화
            carRoofSurfaceTexts.GetChild(_carRoofSurface).gameObject.SetActive(true); //선택된 도색 부위 재질의 Text를 활성화
        }
    }
    public Shader[] carRoofSurfaces; //Roof 재질들

    private int _carChromeDeleteSurface;
    public int carChromeDeleteSurface //ChromeDelete 재질
    {
        get
        {
            return _carChromeDeleteSurface; //재질 반환
        }
        set
        {
            _carChromeDeleteSurface = value; //재질 변경

            if (_carChromeDeleteSurface == 0 && carChromeDeleteMaterials != null && carChromeDeleteDefaultMaterials != null) //기본 재질이면
            {
                for (int i = 0; i < carChromeDeleteMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carChromeDeleteMaterials[i] != null && carChromeDeleteDefaultMaterials[i] != null)
                    {
                        carChromeDeleteMaterials[i].shader = carChromeDeleteDefaultMaterials[i].shader; //기본 재질 셰이더 지정
                        carChromeDeleteMaterials[i].CopyPropertiesFromMaterial(carChromeDeleteDefaultMaterials[i]); //기본 재질 Property 복사
                    }
                }
            }
            else if (_carChromeDeleteSurface != 0 && carChromeDeleteMaterials != null && DBController.dbController.carChromeDeleteSurfaceProperties != null) //기본 재질이 아니면
            {
                for (int i = 0; i < carChromeDeleteMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carChromeDeleteMaterials[i] != null && DBController.dbController.carChromeDeleteSurfaceProperties[_carChromeDeleteSurface - 1] != null)
                    {
                        carChromeDeleteMaterials[i].shader = DBController.dbController.carChromeDeleteSurfaceProperties[_carChromeDeleteSurface - 1].shader; //재질 셰이더 지정
                        carChromeDeleteMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carChromeDeleteSurfaceProperties[_carChromeDeleteSurface - 1]); //기본 재질 Property 복사
                        carChromeDeleteColor = carChromeDeleteColor; //색상 지정
                    }
                }
            }

            Transform carChromeDeleteSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.ChromeDelete); //도색 부위 재질의 Text들의 부모를 저장
            for (int i = 0; i < carChromeDeleteSurfaces.Length + 1; ++i) carChromeDeleteSurfaceTexts.GetChild(i).gameObject.SetActive(false); //모든 도색 부위 재질의 Text를 비활성화
            carChromeDeleteSurfaceTexts.GetChild(_carChromeDeleteSurface).gameObject.SetActive(true); //선택된 도색 부위 재질의 Text를 활성화
        }
    }
    public Shader[] carChromeDeleteSurfaces; //ChromeDelete 재질들

    private int _carWheelSurface;
    public int carWheelSurface //Wheel 재질
    {
        get
        {
            return _carWheelSurface; //재질 반환
        }
        set
        {
            _carWheelSurface = value; //재질 변경

            if (_carWheelSurface == 0 && carWheelMaterials != null && carWheelDefaultMaterials != null) //기본 재질이면
            {
                for (int i = 0; i < carWheelMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carWheelMaterials[i] != null && carWheelDefaultMaterials[i] != null)
                    {
                        carWheelMaterials[i].shader = carWheelDefaultMaterials[i].shader; //기본 재질 셰이더 지정
                        carWheelMaterials[i].CopyPropertiesFromMaterial(carWheelDefaultMaterials[i]); //기본 재질 Property 복사
                    }
                }
            }
            else if (_carWheelSurface != 0 && carWheelMaterials != null && DBController.dbController.carWheelSurfaceProperties != null) //기본 재질이 아니면
            {
                for (int i = 0; i < carWheelMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carWheelMaterials[i] != null && DBController.dbController.carWheelSurfaceProperties[_carWheelSurface - 1] != null)
                    {
                        carWheelMaterials[i].shader = DBController.dbController.carWheelSurfaceProperties[_carWheelSurface - 1].shader; //재질 셰이더 지정
                        carWheelMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carWheelSurfaceProperties[_carWheelSurface - 1]); //기본 재질 Property 복사
                        carWheelColor = carWheelColor; //색상 지정
                    }
                }
            }

            Transform carWheelSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.Wheel); //도색 부위 재질의 Text들의 부모를 저장
            for (int i = 0; i < carWheelSurfaces.Length + 1; ++i) carWheelSurfaceTexts.GetChild(i).gameObject.SetActive(false); //모든 도색 부위 재질의 Text를 비활성화
            carWheelSurfaceTexts.GetChild(_carWheelSurface).gameObject.SetActive(true); //선택된 도색 부위 재질의 Text를 활성화
        }
    }
    public Shader[] carWheelSurfaces; //Wheel 재질들

    private int _carBrakeCaliperSurface;
    public int carBrakeCaliperSurface //BrakeCaliper 재질
    {
        get
        {
            return _carBrakeCaliperSurface; //재질 반환
        }
        set
        {
            _carBrakeCaliperSurface = value; //재질 변경

            if (_carBrakeCaliperSurface == 0 && carBrakeCaliperMaterials != null && carBrakeCaliperDefaultMaterials != null) //기본 재질이면
            {
                for (int i = 0; i < carBrakeCaliperMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carBrakeCaliperMaterials[i] != null && carBrakeCaliperDefaultMaterials[i] != null)
                    {
                        carBrakeCaliperMaterials[i].shader = carBrakeCaliperDefaultMaterials[i].shader; //기본 재질 셰이더 지정
                        carBrakeCaliperMaterials[i].CopyPropertiesFromMaterial(carBrakeCaliperDefaultMaterials[i]); //기본 재질 Property 복사
                    }
                }
            }
            else if (_carBrakeCaliperSurface != 0 && carBrakeCaliperMaterials != null && DBController.dbController.carBrakeCaliperSurfaceProperties != null) //기본 재질이 아니면
            {
                for (int i = 0; i < carBrakeCaliperMaterials.Length; ++i) //재질 개수만큼 반복
                {
                    if (carBrakeCaliperMaterials[i] != null && DBController.dbController.carBrakeCaliperSurfaceProperties[_carBrakeCaliperSurface - 1] != null)
                    {
                        carBrakeCaliperMaterials[i].shader = DBController.dbController.carBrakeCaliperSurfaceProperties[_carBrakeCaliperSurface - 1].shader; //재질 셰이더 지정
                        carBrakeCaliperMaterials[i].CopyPropertiesFromMaterial(DBController.dbController.carBrakeCaliperSurfaceProperties[_carBrakeCaliperSurface - 1]); //기본 재질 Property 복사
                        carBrakeCaliperColor = carBrakeCaliperColor; //색상 지정
                    }
                }
            }

            Transform carBrakeCaliperSurfaceTexts = carPaintSurfaceTextsTransform.GetChild((int)PaintPart.BrakeCaliper); //도색 부위 재질의 Text들의 부모를 저장
            for (int i = 0; i < carBrakeCaliperSurfaces.Length + 1; ++i) carBrakeCaliperSurfaceTexts.GetChild(i).gameObject.SetActive(false); //모든 도색 부위 재질의 Text를 비활성화
            carBrakeCaliperSurfaceTexts.GetChild(_carBrakeCaliperSurface).gameObject.SetActive(true); //선택된 도색 부위 재질의 Text를 활성화
        }
    }
    public Shader[] carBrakeCaliperSurfaces; //BrakeCaliper 재질들
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

        carPaint = this; //전역 참조 변수 지정
    }

    private void Start()
    {
        OnClickPaintResetButton();
        OnClickPaintPartButton(0);
    }

    #region Interface
    /* CarPaintPanel을 활성화/비활성화 하는 함수 */
    public void ActivateCarPaintPanel(bool state)
    {
        Home.home.ActivateHome(!state); //Home 활성화/비활성화
        carPaintPanelCanvas.enabled = state; //CarPaintPanel의 Canvas 컴포넌트 활성화/비활성화
        MainCamera.mainCamera.SetCurrentPos(state ? CameraPosition.CarPaint : CameraPosition.Center); //카메라 위치 변경
    }

    /* 도색 부위 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickPaintPartButton(int paintPart)
    {
        selectedPaintPart = (PaintPart)paintPart; //선택된 도색 부위 변경
    }

    /* Surface Button을 클릭했을 때 실행되는 함수 */
    public void OnClickSurfaceButton(bool isNext)
    {
        if (isNext) //다음이면
        {
            carSelectedPaintPartSurface = carSelectedPaintPartSurface == carSelectedPaintPartSurfacesLength ? 0 : carSelectedPaintPartSurface + 1; //재질 변경
        }
        else //이전이면
        {
            carSelectedPaintPartSurface = carSelectedPaintPartSurface == 0 ? carSelectedPaintPartSurfacesLength : carSelectedPaintPartSurface - 1; //재질 변경
        }
    }

    /* PaintResetButton을 클릭했을 때 실행되는 함수 */
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

        CarPaintColorPicker.carPaintColorPicker.ApplyColor(carSelectedPaintPartColor); //Color Picker 색상 갱신
    }
    #endregion

    #region Caching
    /* 차량 도색 부위 Material들을 캐싱하는 함수 */
    public void CachingCarPaintPartMaterials(PaintPart paintPart)
    {
        if (PaintPart.Entire == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //차량 Body의 Transform 컴포넌트가 존재하면
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //차량 Body의 Material 컴포넌트들

            Material[] frontBumperMaterials = null;
            if (SuspensionController.suspensionController.movePartsTransforms.frontBumper != null) //차량 FrontBumper의 Transform 컴포넌트가 존재하면
                frontBumperMaterials = SuspensionController.suspensionController.movePartsTransforms.frontBumper.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //차량 FrontBumper의 Material 컴포넌트들

            Material[] rearBumperMaterials = null; //차량 RearBumper의 Transform 컴포넌트가 존재하면
            if (SuspensionController.suspensionController.movePartsTransforms.rearBumper != null)
                rearBumperMaterials = SuspensionController.suspensionController.movePartsTransforms.rearBumper.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //차량 RearBumper의 Material 컴포넌트들

            Material[] materials = new Material[carEntireMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "Body"),
                Utility.FindMaterialWithName(ref frontBumperMaterials, "Body"),
                Utility.FindMaterialWithName(ref rearBumperMaterials, "Body")
            };

            /* 기본 재질 */
            for (int i = 0; i < carEntireMaterialLength; ++i) //머티리얼 개수만큼 반복
            {
                if (materials[i] == null) carEntireDefaultMaterials[i] = null; //머티리얼이 존재하지 않으면
                else carEntireDefaultMaterials[i] = carEntireMaterials[i] == null ? Instantiate(materials[i]) : carEntireDefaultMaterials[i]; //머티리얼이 존재하면
            }

            carEntireMaterials = materials; //캐싱
        }
        else if (PaintPart.Bonnet == paintPart)
        {
            Material[] bonnetMaterials = null;
            if (SuspensionController.suspensionController.movePartsTransforms.bonnet != null) //차량 Bonnet의 Transform 컴포넌트가 존재하면
                bonnetMaterials = SuspensionController.suspensionController.movePartsTransforms.bonnet.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //차량 Bonnet의 Material 컴포넌트들

            Material[] materials = new Material[carBonnetMaterialLength]
            {
                Utility.FindMaterialWithName(ref bonnetMaterials, "Body"),
            };

            /* 기본 재질 */
            for (int i = 0; i < carBonnetMaterialLength; ++i) //머티리얼 개수만큼 반복
            {
                if (materials[i] == null) carBonnetDefaultMaterials[i] = null; //머티리얼이 존재하지 않으면
                else carBonnetDefaultMaterials[i] = carBonnetMaterials[i] == null ? Instantiate(materials[i]) : carBonnetDefaultMaterials[i]; //머티리얼이 존재하면
            }

            carBonnetMaterials = materials; //캐싱
        }
        else if (PaintPart.SideMirror == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //차량 Body의 Transform 컴포넌트가 존재하면
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //차량 Body의 Material 컴포넌트들

            Material[] materials = new Material[carSideMirrorMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "SideMirror"),
            };

            /* 기본 재질 */
            for (int i = 0; i < carSideMirrorMaterialLength; ++i) //머티리얼 개수만큼 반복
            {
                if (materials[i] == null) carSideMirrorDefaultMaterials[i] = null; //머티리얼이 존재하지 않으면
                else carSideMirrorDefaultMaterials[i] = carSideMirrorMaterials[i] == null ? Instantiate(materials[i]) : carSideMirrorDefaultMaterials[i]; //머티리얼이 존재하면
            }

            carSideMirrorMaterials = materials; //캐싱
        }
        else if (PaintPart.Roof == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //차량 Body의 Transform 컴포넌트가 존재하면
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //차량 Body의 Material 컴포넌트들

            Material[] materials = new Material[carRoofMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "Roof"),
            };

            /* 기본 재질 */
            for (int i = 0; i < carRoofMaterialLength; ++i) //머티리얼 개수만큼 반복
            {
                if (materials[i] == null) carRoofDefaultMaterials[i] = null; //머티리얼이 존재하지 않으면
                else carRoofDefaultMaterials[i] = carRoofMaterials[i] == null ? Instantiate(materials[i]) : carRoofDefaultMaterials[i]; //머티리얼이 존재하면
            }

            carRoofMaterials = materials; //캐싱
        }
        else if (PaintPart.ChromeDelete == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //차량 Body의 Transform 컴포넌트가 존재하면
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //차량 Body의 Material 컴포넌트들

            Material[] grillMaterials = null; //차량 Grill의 Material 컴포넌트들
            if (SuspensionController.suspensionController.movePartsTransforms.grill != null) //차량 Grill의 Transform 컴포넌트가 존재하면
                grillMaterials = SuspensionController.suspensionController.movePartsTransforms.grill.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //차량 Grill의 Material 컴포넌트들

            Material[] bonnetMaterials = null;
            if (SuspensionController.suspensionController.movePartsTransforms.bonnet != null) //차량 Bonnet의 Transform 컴포넌트가 존재하면
                bonnetMaterials = SuspensionController.suspensionController.movePartsTransforms.bonnet.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //차량 Bonnet의 Material 컴포넌트들

            Material[] frontBumperMaterials = null;
            if (SuspensionController.suspensionController.movePartsTransforms.frontBumper != null) //차량 FrontBumper의 Transform 컴포넌트가 존재하면
                frontBumperMaterials = SuspensionController.suspensionController.movePartsTransforms.frontBumper.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials; //차량 FrontBumper의 Material 컴포넌트들

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

            /* 기본 재질 */
            for (int i = 0; i < carChromeDeleteMaterialLength; ++i) //머티리얼 개수만큼 반복
            {
                if (materials[i] == null) carChromeDeleteDefaultMaterials[i] = null; //머티리얼이 존재하지 않으면
                else carChromeDeleteDefaultMaterials[i] = carChromeDeleteMaterials[i] == null ? Instantiate(materials[i]) : carChromeDeleteDefaultMaterials[i]; //머티리얼이 존재하면
            }

            carChromeDeleteMaterials = materials; //캐싱
        }
        else if (PaintPart.Wheel == paintPart)
        {
            Material[][] wheelsMaterials = new Material[4][] //차량 Wheel들의 Material 컴포넌트들
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

            /* 기본 재질 */
            for (int i = 0; i < carWheelMaterialLength; ++i) //머티리얼 개수만큼 반복
            {
                if (materials[i] == null) carWheelDefaultMaterials[i] = null; //머티리얼이 존재하지 않으면
                else carWheelDefaultMaterials[i] = carWheelMaterials[i] == null ? Instantiate(materials[i]) : carWheelDefaultMaterials[i]; //머티리얼이 존재하면
            }

            carWheelMaterials = materials; //캐싱
        }
        else if (PaintPart.BrakeCaliper == paintPart)
        {
            Material[][] brakesMaterials = new Material[4][] //차량 Brake들의 Material 컴포넌트들
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

            /* 기본 재질 */
            for (int i = 0; i < carBrakeCaliperMaterialLength; ++i) //머티리얼 개수만큼 반복
            {
                if (materials[i] == null) carBrakeCaliperDefaultMaterials[i] = null; //머티리얼이 존재하지 않으면
                else carBrakeCaliperDefaultMaterials[i] = carBrakeCaliperMaterials[i] == null ? Instantiate(materials[i]) : carBrakeCaliperDefaultMaterials[i]; //머티리얼이 존재하면
            }

            carBrakeCaliperMaterials = materials; //캐싱
        }
        else if (PaintPart.Window == paintPart)
        {
            Material[] bodyMaterials = null;
            if (SuspensionController.suspensionController.moveBodyTransform != null) //차량 Body의 Transform 컴포넌트가 존재하면
                bodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //차량 Body의 Material 컴포넌트들

            carWindowMaterials = new Material[carWindowMaterialLength]
            {
                Utility.FindMaterialWithName(ref bodyMaterials, "Window")
            };
        }
    }
    #endregion
}