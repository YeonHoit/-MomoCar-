using UnityEngine;
using UnityEngine.UI;

public class CarLight : MonoBehaviour
{
    [Header("Static")]
    public static CarLight carLight; //전역 참조 변수

    [Header("Light")]
    private bool _activateLight;
    public bool activateLight //Light 활성화 여부
    {
        get
        {
            return _activateLight; //Light 활성화 여부 반환
        }
        set
        {
            _activateLight = value; //Light 활성화 여부 지정
            lightButtonImage.color = value ? buttonActivationColor : buttonDeactivationColor; //LightButton을 활성화/비활성화 색상으로 변경
            Utility.SetCarMaterialsProperty(ref carLightMaterials, "_Enable_Light", value ? 1 : 0); //도색 부위 색상 변경
        }
    }
    public Image lightButtonImage; //LightButton의 Image 컴포넌트

    [Header("Materials")]
    public Material[] carLightMaterials; //Light의 Material 컴포넌트들

    [Header("Cache")]
    private readonly Color32 buttonActivationColor = new Color32(255, 28, 86, 255); //버튼 활성화 색상
    private readonly Color32 buttonDeactivationColor = new Color32(100, 100, 100, 255); //버튼 비활성화 색상

    private void Awake()
    {
        carLight = this; //전역 참조 변수 지정
    }

    private void Start()
    {
        activateLight = true;
    }

    /* 차량 Light Material들을 캐싱하는 함수 */
    public void CachingCarLightMaterials()
    {
        /* 오브젝트의 Material 컴포넌트들을 먼저 캐싱 */
        Material[] carBodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //차량 Body의 Material 컴포넌트들

        /* Light */
        carLightMaterials = new Material[9]
        {
            Utility.FindMaterialWithName(ref carBodyMaterials, "HeadLight"),
            Utility.FindMaterialWithName(ref carBodyMaterials, "HeadLight2"),
            Utility.FindMaterialWithName(ref carBodyMaterials, "TailLight"),
            Utility.FindMaterialWithName(ref carBodyMaterials, "TurnLight"),
            Utility.FindMaterialWithName(ref carBodyMaterials, "TurnLight2"),
            Utility.FindMaterialWithName(ref carBodyMaterials, "TurnLight3"),
            Utility.FindMaterialWithName(ref carBodyMaterials, "TurnLight4"),
            Utility.FindMaterialWithName(ref carBodyMaterials, "TurnLight5"),
            Utility.FindMaterialWithName(ref carBodyMaterials, "TurnLight6")
        };
    }

    /* Light를 적용하는 함수 */
    public void SetCarLight()
    {
        activateLight = !activateLight; //Light 활성화/비활성화
    }
}