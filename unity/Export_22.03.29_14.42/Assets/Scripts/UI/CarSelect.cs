using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CarSelect : MonoBehaviour
{
    [Header("Static")]
    public static CarSelect carSelect; //���� ���� ����

    [Header("Car Select Panel")]
    public Canvas carSelectPanelCanvas; //CarSelectPanel�� Canvas ������Ʈ

    [Header("Expansion/Collapse")]
    public RectTransform backgroundRectTransform; //Background�� RectTransform ������Ʈ
    public float backgroundCollapseValue; //Background ������Ʈ ��� ��
    public float backgroundExpansionValue; //Background ������Ʈ Ȯ�� ��
    public GameObject carTypeScrollView; //CarTypeScrollView ������Ʈ
    public GameObject collapseButton; //CollapseButton ������Ʈ

    [Header("On Click Manufacturer Button")]
    public RectTransform manufacturerScrollViewContents; //ManufacturerScrollView Contents�� RectTransform ������Ʈ
    public RectTransform carTypeScrollViewContents; //CarTypeScrollView Contents�� RectTransform ������Ʈ
    public ScrollRect carTypeScrollViewScrollRect; //CarTypeScrollView�� ScrollRect ������Ʈ

    [Header("Tint")]
    [HideInInspector] public GameObject selectedManufacturerButtonTint; //���õ� ������ ��ư Tint
    [HideInInspector] public DynamicButton.CarTypeButton selectedCarTypeButtonScript; //���õ� ���� ���� ��ư Script
    [HideInInspector] public GameObject selectedYearTypeButtonTint; //���õ� ���� ��ư Tint

    [Header("Init")]
    public GameObject carTypeSample; //CarTypeSample ������Ʈ
    public GameObject yearTypeSampleButton; //YearTypeSampleButton ������Ʈ

    [Header("LoadCar")]
    public Transform[] carPartsTransforms; //�� ������ ��� ������Ʈ�� Transform ������

    private void Awake()
    {
        carSelect = this;
    }

    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) //���ͳ� ������ �Ǿ����� ������
        {
            Notification.notification.EnableLayout(Notification.Layout.InternetDisconnection);  //���̾ƿ� Ȱ��ȭ
        }
        else //���ͳ� ������ �Ǿ�������
        {
            StartCoroutine(InitCarSelectPanel());
        }
    }

    #region Init
    /* CarSelectPanel �ʱ�ȭ�ϴ� �ڷ�ƾ �Լ� */
    private IEnumerator InitCarSelectPanel()
    {
        yield return StartCoroutine(DBController.dbController.GetCarInfo()); //���� �������� DB�κ��� �ε�

        for (int i = 0; i < DBController.dbController.carInfos.Length; ++i) //���� ������ŭ �ݺ�
        {
            manufacturerScrollViewContents.Find(DBController.dbController.carInfos[i].manufacturer_eng).gameObject.SetActive(true); //�ش� ������ ��ư Ȱ��ȭ

            Transform manufacturerContent = carTypeScrollViewContents.Find(DBController.dbController.carInfos[i].manufacturer_eng); //������ Content ����
            Transform carTypeTransform = manufacturerContent.Find(DBController.dbController.carInfos[i].carType_eng); //���� ���� Transform Ž��

            if (carTypeTransform == null) //���� ���� Content�� ������
            {
                Transform carType = Instantiate(carTypeSample).transform; //���� ���� ����
                carType.name = DBController.dbController.carInfos[i].carType_eng; //�̸� ����
                carType.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.carInfos[i].carType_ko; //��ư Text ����
                carType.SetParent(manufacturerContent); //�θ� ����
                carType.localScale = Vector3.one; //ũ�� ������
                carType.gameObject.SetActive(true); //Ȱ��ȭ

                carTypeTransform = carType; //���� ���� Transform���� ����
            }

            Transform yearTypeTransform = carTypeTransform.GetChild(1); //���� Transform ����

            Transform yearTypeButton = Instantiate(yearTypeSampleButton).transform; //���� ��ư ����
            yearTypeButton.name = DBController.dbController.carInfos[i].yearType_eng; //�̸� ����
            yearTypeButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.carInfos[i].yearType_ko; //��ư Text ����
            yearTypeButton.SetParent(yearTypeTransform); //�θ� ����
            yearTypeButton.localScale = Vector3.one; //ũ�� ������
            yearTypeButton.GetComponent<DynamicButton.YearTypeButton>().yearType = DBController.dbController.carInfos[i].yearType_eng; //���� ����
            yearTypeButton.gameObject.SetActive(true); //Ȱ��ȭ
        }

        DBController.dbController.carInfos.Initialize(); //���� ������ �迭 �ʱ�ȭ
    }
    #endregion

    #region Interface
    /* CarSelectPanel�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �Լ� */
    public void ActivateCarSelectPanel(bool state)
    {
        Home.home.ActivateHome(!state); //Home Ȱ��ȭ/��Ȱ��ȭ
        carSelectPanelCanvas.enabled = state; //CarSelectPanel�� Canvas ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
    }

    /* ������ ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickManufacturerButton(string manufacturer)
    {
        if (selectedManufacturerButtonTint != null) selectedManufacturerButtonTint.SetActive(false); //������ Tint ��Ȱ��ȭ
        GameObject manufacturerButtonTint = manufacturerScrollViewContents.Find(manufacturer).GetChild(2).gameObject; //�ش� ������ ��ư�� Tint ������Ʈ ����
        selectedManufacturerButtonTint = manufacturerButtonTint; //���õ� ������ ��ư�� Tint ������Ʈ�� ����
        manufacturerButtonTint.SetActive(true); //Tint Ȱ��ȭ

        for (int i = 0; i < carTypeScrollViewContents.childCount; ++i) carTypeScrollViewContents.GetChild(i).gameObject.SetActive(false); //��� �������� Content ��Ȱ��ȭ

        GameObject manufacturerContent = carTypeScrollViewContents.Find(manufacturer).gameObject; //�ش� �������� Content Ž��
        manufacturerContent.SetActive(true); //�ش� �������� Content Ȱ��ȭ
        carTypeScrollViewScrollRect.content = manufacturerContent.GetComponent<RectTransform>(); //�ش� �������� RectTransform�� Content�� ����

        ExpandCarSelectPanel(true); //CarSelectPanel Ȯ��
    }

    /* CarSelectPanel�� Ȯ��/��� �ϴ� �Լ� */
    public void ExpandCarSelectPanel(bool state)
    {
        backgroundRectTransform.offsetMax = new Vector2(-(state ? backgroundExpansionValue : backgroundCollapseValue), backgroundRectTransform.offsetMax.y); //Right �� ����
        carTypeScrollView.SetActive(state); //CarTypeScrollView Ȱ��ȭ/��Ȱ��ȭ
        collapseButton.SetActive(state); //CollapseButton Ȱ��ȭ/��Ȱ��ȭ

        if (!state) selectedManufacturerButtonTint.SetActive(false); //������ ��ư�� Tint ��Ȱ��ȭ
    }
    #endregion

    #region Caching
    /* ��ü�� Transform���� ĳ���ϴ� �Լ� */
    private void CachingBodyTransforms()
    {
        Transform movePartsTransform = BundleController.bundleController.loadedBundles[11].transform.Find("Move").Find("PartsTransform"); //�����̴� PartsTransform ������Ʈ
        Transform stopPartsTransform = BundleController.bundleController.loadedBundles[11].transform.Find("PartsTransform"); //�����ִ� PartsTransform ������Ʈ

        /* ����Ʈ ���� */
        SuspensionController.suspensionController.carBodyPartsTransforms.frontBumper = movePartsTransform.Find("FrontBumper");

        /* ���� ���� */
        SuspensionController.suspensionController.carBodyPartsTransforms.rearBumper = movePartsTransform.Find("RearBumper");

        /* �׸� */
        SuspensionController.suspensionController.carBodyPartsTransforms.grill = movePartsTransform.Find("Grill");

        /* ���� */
        SuspensionController.suspensionController.carBodyPartsTransforms.bonnet = movePartsTransform.Find("Bonnet");

        /* ����Ʈ �Ӵ� */
        SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Left = movePartsTransform.Find("FrontFender_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Right = movePartsTransform.Find("FrontFender_Right");

        /* ���� �Ӵ� */
        SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Left = movePartsTransform.Find("RearFender_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Right = movePartsTransform.Find("RearFender_Right");

        /* �� */
        SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Left = stopPartsTransform.Find("Wheel_Front_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Right = stopPartsTransform.Find("Wheel_Front_Right");
        SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Left = stopPartsTransform.Find("Wheel_Rear_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Right = stopPartsTransform.Find("Wheel_Rear_Right");

        /* Ÿ�̾� */
        SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Left = stopPartsTransform.Find("Tire_Front_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Right = stopPartsTransform.Find("Tire_Front_Right");
        SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Left = stopPartsTransform.Find("Tire_Rear_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Right = stopPartsTransform.Find("Tire_Rear_Right");

        /* �극��ũ */
        SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Left = stopPartsTransform.Find("Brake_Front_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Right = stopPartsTransform.Find("Brake_Front_Right");
        SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Left = stopPartsTransform.Find("Brake_Rear_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Right = stopPartsTransform.Find("Brake_Rear_Right");

        /* �����Ϸ� */
        SuspensionController.suspensionController.carBodyPartsTransforms.spoiler = movePartsTransform.Find("Spoiler");

        /* �� �Ͽ콺 */
        SuspensionController.suspensionController.carBodyWheelHouseTransform = BundleController.bundleController.loadedBundles[11].transform.Find("Move").Find("WheelHouse");
    }

    /* ������ Transform���� ĳ���ϴ� �Լ� */
    public void CachingMoveTransforms(int index)
    {
        switch (index)
        {
            case (int)TypeOfParts.FrontBumper: //����Ʈ ����
                if (carPartsTransforms[(int)TypeOfParts.FrontBumper].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.frontBumper = carPartsTransforms[(int)TypeOfParts.FrontBumper].GetChild(0);
                }
                break;
            case (int)TypeOfParts.RearBumper: //���� ����
                if (carPartsTransforms[(int)TypeOfParts.RearBumper].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.rearBumper = carPartsTransforms[(int)TypeOfParts.RearBumper].GetChild(0);
                }
                break;
            case (int)TypeOfParts.Grill: //�׸�
                if (carPartsTransforms[(int)TypeOfParts.Grill].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.grill = carPartsTransforms[(int)TypeOfParts.Grill].GetChild(0);
                }
                break;
            case (int)TypeOfParts.Bonnet: //����
                if (carPartsTransforms[(int)TypeOfParts.Bonnet].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.bonnet = carPartsTransforms[(int)TypeOfParts.Bonnet].GetChild(0);
                }
                break;
            case (int)TypeOfParts.FrontFender: //����Ʈ �Ӵ�
                if (carPartsTransforms[(int)TypeOfParts.FrontFender].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.frontFender_Left = carPartsTransforms[(int)TypeOfParts.FrontFender].GetChild(0);
                    SuspensionController.suspensionController.movePartsTransforms.frontFender_Right = carPartsTransforms[(int)TypeOfParts.FrontFender].GetChild(1);
                }
                break;
            case (int)TypeOfParts.RearFender: //���� �Ӵ�
                if (carPartsTransforms[(int)TypeOfParts.RearFender].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.rearFender_Left = carPartsTransforms[(int)TypeOfParts.RearFender].GetChild(0);
                    SuspensionController.suspensionController.movePartsTransforms.rearFender_Right = carPartsTransforms[(int)TypeOfParts.RearFender].GetChild(1);
                }
                break;
            case (int)TypeOfParts.Wheel: //��
                if (carPartsTransforms[(int)TypeOfParts.Wheel].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left = carPartsTransforms[(int)TypeOfParts.Wheel].GetChild(0);
                    SuspensionController.suspensionController.carWheelWheelCapPosTransforms[0] = SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left.Find("WheelCapPos"); //WheelCapPos Transform ĳ��

                    SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right = carPartsTransforms[(int)TypeOfParts.Wheel].GetChild(1);
                    SuspensionController.suspensionController.carWheelWheelCapPosTransforms[1] = SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right.Find("WheelCapPos"); //WheelCapPos Transform ĳ��

                    SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left = carPartsTransforms[(int)TypeOfParts.Wheel].GetChild(2);
                    SuspensionController.suspensionController.carWheelWheelCapPosTransforms[2] = SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left.Find("WheelCapPos"); //WheelCapPos Transform ĳ��

                    SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right = carPartsTransforms[(int)TypeOfParts.Wheel].GetChild(3);
                    SuspensionController.suspensionController.carWheelWheelCapPosTransforms[3] = SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right.Find("WheelCapPos"); //WheelCapPos Transform ĳ��
                }
                break;
            case (int)TypeOfParts.Tire: //Ÿ�̾�
                if (carPartsTransforms[(int)TypeOfParts.Tire].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.tire_Front_Left = carPartsTransforms[(int)TypeOfParts.Tire].GetChild(0);
                    SuspensionController.suspensionController.movePartsTransforms.tire_Front_Right = carPartsTransforms[(int)TypeOfParts.Tire].GetChild(1);
                    SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Left = carPartsTransforms[(int)TypeOfParts.Tire].GetChild(2);
                    SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Right = carPartsTransforms[(int)TypeOfParts.Tire].GetChild(3);
                }
                break;
            case (int)TypeOfParts.Brake: //�극��ũ
                if (carPartsTransforms[(int)TypeOfParts.Brake].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.brake_Front_Left = carPartsTransforms[(int)TypeOfParts.Brake].GetChild(0);
                    SuspensionController.suspensionController.movePartsTransforms.brake_Front_Right = carPartsTransforms[(int)TypeOfParts.Brake].GetChild(1);
                    SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Left = carPartsTransforms[(int)TypeOfParts.Brake].GetChild(2);
                    SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Right = carPartsTransforms[(int)TypeOfParts.Brake].GetChild(3);
                }
                break;
            case (int)TypeOfParts.Spoiler: //�����Ϸ�
                if (carPartsTransforms[(int)TypeOfParts.Spoiler].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.spoiler = carPartsTransforms[(int)TypeOfParts.Spoiler].GetChild(0);
                }
                break;
            default:
                SuspensionController.suspensionController.moveBodyTransform = BundleController.bundleController.loadedBundles[11].transform.Find("Move"); //��ü

                SuspensionController.suspensionController.moveBrakeDiscRotorTransform = BundleController.bundleController.loadedBundles[11].transform.Find("BrakeDiscRotor"); //BrakeDiscRotor

                Transform wheelCapTransform = BundleController.bundleController.loadedBundles[11].transform.Find("WheelCap"); //WheelCap
                SuspensionController.suspensionController.moveWheelCapTransforms[0] = wheelCapTransform.GetChild(0);
                SuspensionController.suspensionController.moveWheelCapTransforms[1] = wheelCapTransform.GetChild(1);
                SuspensionController.suspensionController.moveWheelCapTransforms[2] = wheelCapTransform.GetChild(2);
                SuspensionController.suspensionController.moveWheelCapTransforms[3] = wheelCapTransform.GetChild(3);
                break;
        }
    }
    #endregion

    /* ������ �ҷ����� �ڷ�ƾ �Լ� */
    public IEnumerator LoadCar(string carInfo)
    {
        if (carInfo != null && carInfo.Length != 0) //���� ������ �����ϸ�
        {
            Notification.notification.EnableLayout(Notification.Layout.LoadCar);  //���̾ƿ� Ȱ��ȭ

            Static.selectedCarName = null; //���õ� ���� �̸� ����

            yield return StartCoroutine(DBController.dbController.GetCarBundleInfo(carInfo)); //���� ������ ���� ���� ���� ������ ȹ��

            yield return StartCoroutine(DBController.dbController.GetSelectedPartsName(carInfo)); //���� ������ ���� ���õ� ��ǰ ������ ȹ��

            yield return StartCoroutine(DBController.dbController.GetPaintPartSurfaceInfo(carInfo)); //���� ������ ���� ���� ���� ������ ȹ��

            if (BundleController.bundleController.loadedBundles[11]) Destroy(BundleController.bundleController.loadedBundles[11]); //��ü�� �����ϸ� ����
            CarPaint.carPaint.carEntireMaterials[0] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carSideMirrorMaterials[0] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carRoofMaterials[0] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[0] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[1] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[2] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carWindowMaterials[0] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(DBController.dbController.carBundleInfo, Vector3.zero, Vector3.zero, carPartsTransforms[11], 11)); //��ü ���� �ε�

            if (BundleController.bundleController.loadedBundles[11]) //��ü ������ �ε�Ǹ�
            {
                CachingBodyTransforms(); //��ü�� Transform ������Ʈ���� ĳ��
                MainCamera.mainCamera.CachingCameraPosTransforms(); //��ü�� CameraPos Transform ������Ʈ���� ĳ��

                CachingMoveTransforms(11); //������ ��ü Transform ĳ�� 
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.FrontBumper, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.FrontBumper], false, false)); //����Ʈ ���� �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.RearBumper, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.RearBumper], false, false)); //���� ���� �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Grill, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Grill], false, false)); //�׸� �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Bonnet, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Bonnet], false, false)); //���� �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.FrontFender, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.FrontFender], false, false)); //����Ʈ �Ӵ� �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.RearFender, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.RearFender], false, false)); //���� �Ӵ� �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Wheel, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Wheel], false, false)); //�� �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Tire, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Tire], false, false)); //Ÿ�̾� �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Brake, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Brake], false, false)); //�극��ũ �ҷ�����
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Spoiler, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Spoiler], false, false)); //�����Ϸ� �ҷ�����

                CachingMoveTransforms((int)TypeOfParts.FrontBumper); //������ FrontBumper ĳ��
                CachingMoveTransforms((int)TypeOfParts.RearBumper); //������ RearBumper ĳ��
                CachingMoveTransforms((int)TypeOfParts.Grill); //������ Grill ĳ��
                CachingMoveTransforms((int)TypeOfParts.Bonnet); //������ Bonnet ĳ��
                CachingMoveTransforms((int)TypeOfParts.FrontFender); //������ FrontFender ĳ��
                CachingMoveTransforms((int)TypeOfParts.RearFender); //������ RearFender ĳ��
                CachingMoveTransforms((int)TypeOfParts.Wheel); //������ Wheel ĳ��
                CachingMoveTransforms((int)TypeOfParts.Tire); //������ Tire ĳ��
                CachingMoveTransforms((int)TypeOfParts.Brake); //������ Brake ĳ��
                CachingMoveTransforms((int)TypeOfParts.Spoiler); //������ Spoiler ĳ��

                SuspensionController.suspensionController.SyncMovePartsTransforms(); //������ ������ Transform���� ����ȭ

                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Entire); //���� Entire Material ������Ʈ�� ĳ��
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Bonnet); //���� Bonnet Material ������Ʈ�� ĳ��
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.SideMirror); //���� SideMirror Material ������Ʈ�� ĳ��
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Roof); //���� Roof Material ������Ʈ�� ĳ��
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.ChromeDelete); //���� ChromeDelete Material ������Ʈ�� ĳ��
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Wheel); //���� Wheel Material ������Ʈ�� ĳ��
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.BrakeCaliper); //���� BrakeCaliper Material ������Ʈ�� ĳ��
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Window); //���� Window Material ������Ʈ�� ĳ��
                CarLight.carLight.CachingCarLightMaterials(); //���� Light Material ������Ʈ�� ĳ��

                CarPaint.carPaint.carEntireColor = CarPaint.carPaint.carEntireColor; //Entire ���� ����
                CarPaint.carPaint.carBonnetColor = CarPaint.carPaint.carBonnetColor; //Bonnet ���� ����
                CarPaint.carPaint.carSideMirrorColor = CarPaint.carPaint.carSideMirrorColor; //SideMirror ���� ����
                CarPaint.carPaint.carRoofColor = CarPaint.carPaint.carRoofColor; //Roof ���� ����
                CarPaint.carPaint.carChromeDeleteColor = CarPaint.carPaint.carChromeDeleteColor; //ChromeDelete ���� ����
                CarPaint.carPaint.carWheelColor = CarPaint.carPaint.carWheelColor; //Wheel ���� ����
                CarPaint.carPaint.carBrakeCaliperColor = CarPaint.carPaint.carBrakeCaliperColor; //BrakeCaliper ���� ����
                CarPaint.carPaint.carWindowColor = CarPaint.carPaint.carWindowColor; //Window ���� ����

                CarPaint.carPaint.carEntireSurface = CarPaint.carPaint.carEntireSurface; //Entire ���� ����
                CarPaint.carPaint.carBonnetSurface = CarPaint.carPaint.carBonnetSurface; //Bonnet ���� ����
                CarPaint.carPaint.carSideMirrorSurface = CarPaint.carPaint.carSideMirrorSurface; //SideMirror ���� ����
                CarPaint.carPaint.carRoofSurface = CarPaint.carPaint.carRoofSurface; //Roof ���� ����
                CarPaint.carPaint.carChromeDeleteSurface = CarPaint.carPaint.carChromeDeleteSurface; //ChromeDelete ���� ����
                CarPaint.carPaint.carWheelSurface = CarPaint.carPaint.carWheelSurface; //Wheel ���� ����
                CarPaint.carPaint.carBrakeCaliperSurface = CarPaint.carPaint.carBrakeCaliperSurface; //BrakeCaliper ���� ����

                CarLight.carLight.activateLight = CarLight.carLight.activateLight; //Light Ȱ��ȭ ���� ����

                Static.selectedCarName = carInfo; //���õ� ���� �̸� ����

                yield return StartCoroutine(DBController.dbController.GetMountableParts(carInfo)); //���� ������ ���� ������ ȹ��
                yield return StartCoroutine(PartsSelect.partsSelect.CreateFrontBumperButtons()); //����Ʈ ���� ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateRearBumperButtons()); //���� ���� ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateGrillButtons()); //�׸� ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateBonnetButtons()); //���� ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateFrontFenderButtons()); //����Ʈ �Ӵ� ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateRearFenderButtons()); //���� �Ӵ� ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateWheelButtons()); //�� ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateBrakeButtons()); //�극��ũ ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateExhaustPipeButtons()); //��ⱸ ��ư ����
                yield return StartCoroutine(PartsSelect.partsSelect.CreateSpoilerButtons()); //�����Ϸ� ��ư ����

                Resources.UnloadUnusedAssets(); //�޸� ���� ����
            }
            else //��ü ������ �ε���� ������
            {
                Notification.notification.EnableLayout(Notification.Layout.FailureLoadCar);  //���̾ƿ� Ȱ��ȭ
            }

            Notification.notification.DisableLayout(Notification.Layout.LoadCar);  //���̾ƿ� ��Ȱ��ȭ
        }
    }

    /* �ѱ۷� ������ �ҷ����� �ڷ�ƾ �Լ� */
    public IEnumerator LoadCarFromKo(string carInfo)
    {
        yield return DBController.dbController.GetYearTypeEngFromKo(carInfo); //���� �ҷ�����
        StartCoroutine(LoadCar(DBController.dbController.yearTypeEng)); //���� �ҷ�����
    }
}