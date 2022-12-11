using UnityEngine;
using UnityEngine.UI;

public class CarLight : MonoBehaviour
{
    [Header("Static")]
    public static CarLight carLight; //���� ���� ����

    [Header("Light")]
    private bool _activateLight;
    public bool activateLight //Light Ȱ��ȭ ����
    {
        get
        {
            return _activateLight; //Light Ȱ��ȭ ���� ��ȯ
        }
        set
        {
            _activateLight = value; //Light Ȱ��ȭ ���� ����
            lightButtonImage.color = value ? buttonActivationColor : buttonDeactivationColor; //LightButton�� Ȱ��ȭ/��Ȱ��ȭ �������� ����
            Utility.SetCarMaterialsProperty(ref carLightMaterials, "_Enable_Light", value ? 1 : 0); //���� ���� ���� ����
        }
    }
    public Image lightButtonImage; //LightButton�� Image ������Ʈ

    [Header("Materials")]
    public Material[] carLightMaterials; //Light�� Material ������Ʈ��

    [Header("Cache")]
    private readonly Color32 buttonActivationColor = new Color32(255, 28, 86, 255); //��ư Ȱ��ȭ ����
    private readonly Color32 buttonDeactivationColor = new Color32(100, 100, 100, 255); //��ư ��Ȱ��ȭ ����

    private void Awake()
    {
        carLight = this; //���� ���� ���� ����
    }

    private void Start()
    {
        activateLight = true;
    }

    /* ���� Light Material���� ĳ���ϴ� �Լ� */
    public void CachingCarLightMaterials()
    {
        /* ������Ʈ�� Material ������Ʈ���� ���� ĳ�� */
        Material[] carBodyMaterials = SuspensionController.suspensionController.moveBodyTransform.GetChild(0).Find("Body").GetComponent<MeshRenderer>().sharedMaterials; //���� Body�� Material ������Ʈ��

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

    /* Light�� �����ϴ� �Լ� */
    public void SetCarLight()
    {
        activateLight = !activateLight; //Light Ȱ��ȭ/��Ȱ��ȭ
    }
}