using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CarSelect : MonoBehaviour
{
    [Header("Static")]
    public static CarSelect carSelect; //전역 참조 변수

    [Header("Car Select Panel")]
    public Canvas carSelectPanelCanvas; //CarSelectPanel의 Canvas 컴포넌트

    [Header("Expansion/Collapse")]
    public RectTransform backgroundRectTransform; //Background의 RectTransform 컴포넌트
    public float backgroundCollapseValue; //Background 오브젝트 축소 값
    public float backgroundExpansionValue; //Background 오브젝트 확대 값
    public GameObject carTypeScrollView; //CarTypeScrollView 오브젝트
    public GameObject collapseButton; //CollapseButton 오브젝트

    [Header("On Click Manufacturer Button")]
    public RectTransform manufacturerScrollViewContents; //ManufacturerScrollView Contents의 RectTransform 컴포넌트
    public RectTransform carTypeScrollViewContents; //CarTypeScrollView Contents의 RectTransform 컴포넌트
    public ScrollRect carTypeScrollViewScrollRect; //CarTypeScrollView의 ScrollRect 컴포넌트

    [Header("Tint")]
    [HideInInspector] public GameObject selectedManufacturerButtonTint; //선택된 제조사 버튼 Tint
    [HideInInspector] public DynamicButton.CarTypeButton selectedCarTypeButtonScript; //선택된 차량 유형 버튼 Script
    [HideInInspector] public GameObject selectedYearTypeButtonTint; //선택된 연식 버튼 Tint

    [Header("Init")]
    public GameObject carTypeSample; //CarTypeSample 오브젝트
    public GameObject yearTypeSampleButton; //YearTypeSampleButton 오브젝트

    [Header("LoadCar")]
    public Transform[] carPartsTransforms; //각 파츠를 담는 오브젝트의 Transform 정보들

    private void Awake()
    {
        carSelect = this;
    }

    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) //인터넷 연결이 되어있지 않으면
        {
            Notification.notification.EnableLayout(Notification.Layout.InternetDisconnection);  //레이아웃 활성화
        }
        else //인터넷 연결이 되어있으면
        {
            StartCoroutine(InitCarSelectPanel());
        }
    }

    #region Init
    /* CarSelectPanel 초기화하는 코루틴 함수 */
    private IEnumerator InitCarSelectPanel()
    {
        yield return StartCoroutine(DBController.dbController.GetCarInfo()); //차량 정보들을 DB로부터 로드

        for (int i = 0; i < DBController.dbController.carInfos.Length; ++i) //차량 개수만큼 반복
        {
            manufacturerScrollViewContents.Find(DBController.dbController.carInfos[i].manufacturer_eng).gameObject.SetActive(true); //해당 제조사 버튼 활성화

            Transform manufacturerContent = carTypeScrollViewContents.Find(DBController.dbController.carInfos[i].manufacturer_eng); //제조사 Content 저장
            Transform carTypeTransform = manufacturerContent.Find(DBController.dbController.carInfos[i].carType_eng); //차량 유형 Transform 탐색

            if (carTypeTransform == null) //차량 유형 Content가 없으면
            {
                Transform carType = Instantiate(carTypeSample).transform; //차량 유형 복제
                carType.name = DBController.dbController.carInfos[i].carType_eng; //이름 변경
                carType.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.carInfos[i].carType_ko; //버튼 Text 변경
                carType.SetParent(manufacturerContent); //부모 변경
                carType.localScale = Vector3.one; //크기 재조절
                carType.gameObject.SetActive(true); //활성화

                carTypeTransform = carType; //차량 유형 Transform으로 저장
            }

            Transform yearTypeTransform = carTypeTransform.GetChild(1); //연식 Transform 저장

            Transform yearTypeButton = Instantiate(yearTypeSampleButton).transform; //연식 버튼 복제
            yearTypeButton.name = DBController.dbController.carInfos[i].yearType_eng; //이름 변경
            yearTypeButton.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.carInfos[i].yearType_ko; //버튼 Text 변경
            yearTypeButton.SetParent(yearTypeTransform); //부모 변경
            yearTypeButton.localScale = Vector3.one; //크기 재조절
            yearTypeButton.GetComponent<DynamicButton.YearTypeButton>().yearType = DBController.dbController.carInfos[i].yearType_eng; //연식 지정
            yearTypeButton.gameObject.SetActive(true); //활성화
        }

        DBController.dbController.carInfos.Initialize(); //차량 정보들 배열 초기화
    }
    #endregion

    #region Interface
    /* CarSelectPanel을 활성화/비활성화 하는 함수 */
    public void ActivateCarSelectPanel(bool state)
    {
        Home.home.ActivateHome(!state); //Home 활성화/비활성화
        carSelectPanelCanvas.enabled = state; //CarSelectPanel의 Canvas 컴포넌트 활성화/비활성화
    }

    /* 제조사 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickManufacturerButton(string manufacturer)
    {
        if (selectedManufacturerButtonTint != null) selectedManufacturerButtonTint.SetActive(false); //기존의 Tint 비활성화
        GameObject manufacturerButtonTint = manufacturerScrollViewContents.Find(manufacturer).GetChild(2).gameObject; //해당 제조사 버튼의 Tint 오브젝트 저장
        selectedManufacturerButtonTint = manufacturerButtonTint; //선택된 제조사 버튼의 Tint 오브젝트로 설정
        manufacturerButtonTint.SetActive(true); //Tint 활성화

        for (int i = 0; i < carTypeScrollViewContents.childCount; ++i) carTypeScrollViewContents.GetChild(i).gameObject.SetActive(false); //모든 제조사의 Content 비활성화

        GameObject manufacturerContent = carTypeScrollViewContents.Find(manufacturer).gameObject; //해당 제조사의 Content 탐색
        manufacturerContent.SetActive(true); //해당 제조사의 Content 활성화
        carTypeScrollViewScrollRect.content = manufacturerContent.GetComponent<RectTransform>(); //해당 제조사의 RectTransform을 Content로 지정

        ExpandCarSelectPanel(true); //CarSelectPanel 확대
    }

    /* CarSelectPanel을 확대/축소 하는 함수 */
    public void ExpandCarSelectPanel(bool state)
    {
        backgroundRectTransform.offsetMax = new Vector2(-(state ? backgroundExpansionValue : backgroundCollapseValue), backgroundRectTransform.offsetMax.y); //Right 값 설정
        carTypeScrollView.SetActive(state); //CarTypeScrollView 활성화/비활성화
        collapseButton.SetActive(state); //CollapseButton 활성화/비활성화

        if (!state) selectedManufacturerButtonTint.SetActive(false); //제조사 버튼의 Tint 비활성화
    }
    #endregion

    #region Caching
    /* 본체의 Transform들을 캐싱하는 함수 */
    private void CachingBodyTransforms()
    {
        Transform movePartsTransform = BundleController.bundleController.loadedBundles[11].transform.Find("Move").Find("PartsTransform"); //움직이는 PartsTransform 컴포넌트
        Transform stopPartsTransform = BundleController.bundleController.loadedBundles[11].transform.Find("PartsTransform"); //멈춰있는 PartsTransform 컴포넌트

        /* 프론트 범퍼 */
        SuspensionController.suspensionController.carBodyPartsTransforms.frontBumper = movePartsTransform.Find("FrontBumper");

        /* 리어 범퍼 */
        SuspensionController.suspensionController.carBodyPartsTransforms.rearBumper = movePartsTransform.Find("RearBumper");

        /* 그릴 */
        SuspensionController.suspensionController.carBodyPartsTransforms.grill = movePartsTransform.Find("Grill");

        /* 본넷 */
        SuspensionController.suspensionController.carBodyPartsTransforms.bonnet = movePartsTransform.Find("Bonnet");

        /* 프론트 휀다 */
        SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Left = movePartsTransform.Find("FrontFender_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Right = movePartsTransform.Find("FrontFender_Right");

        /* 리어 휀다 */
        SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Left = movePartsTransform.Find("RearFender_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Right = movePartsTransform.Find("RearFender_Right");

        /* 휠 */
        SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Left = stopPartsTransform.Find("Wheel_Front_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Right = stopPartsTransform.Find("Wheel_Front_Right");
        SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Left = stopPartsTransform.Find("Wheel_Rear_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Right = stopPartsTransform.Find("Wheel_Rear_Right");

        /* 타이어 */
        SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Left = stopPartsTransform.Find("Tire_Front_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Right = stopPartsTransform.Find("Tire_Front_Right");
        SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Left = stopPartsTransform.Find("Tire_Rear_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Right = stopPartsTransform.Find("Tire_Rear_Right");

        /* 브레이크 */
        SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Left = stopPartsTransform.Find("Brake_Front_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Right = stopPartsTransform.Find("Brake_Front_Right");
        SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Left = stopPartsTransform.Find("Brake_Rear_Left");
        SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Right = stopPartsTransform.Find("Brake_Rear_Right");

        /* 스포일러 */
        SuspensionController.suspensionController.carBodyPartsTransforms.spoiler = movePartsTransform.Find("Spoiler");

        /* 휠 하우스 */
        SuspensionController.suspensionController.carBodyWheelHouseTransform = BundleController.bundleController.loadedBundles[11].transform.Find("Move").Find("WheelHouse");
    }

    /* 움직일 Transform들을 캐싱하는 함수 */
    public void CachingMoveTransforms(int index)
    {
        switch (index)
        {
            case (int)TypeOfParts.FrontBumper: //프론트 범퍼
                if (carPartsTransforms[(int)TypeOfParts.FrontBumper].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.frontBumper = carPartsTransforms[(int)TypeOfParts.FrontBumper].GetChild(0);
                }
                break;
            case (int)TypeOfParts.RearBumper: //리어 범퍼
                if (carPartsTransforms[(int)TypeOfParts.RearBumper].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.rearBumper = carPartsTransforms[(int)TypeOfParts.RearBumper].GetChild(0);
                }
                break;
            case (int)TypeOfParts.Grill: //그릴
                if (carPartsTransforms[(int)TypeOfParts.Grill].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.grill = carPartsTransforms[(int)TypeOfParts.Grill].GetChild(0);
                }
                break;
            case (int)TypeOfParts.Bonnet: //본넷
                if (carPartsTransforms[(int)TypeOfParts.Bonnet].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.bonnet = carPartsTransforms[(int)TypeOfParts.Bonnet].GetChild(0);
                }
                break;
            case (int)TypeOfParts.FrontFender: //프론트 휀다
                if (carPartsTransforms[(int)TypeOfParts.FrontFender].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.frontFender_Left = carPartsTransforms[(int)TypeOfParts.FrontFender].GetChild(0);
                    SuspensionController.suspensionController.movePartsTransforms.frontFender_Right = carPartsTransforms[(int)TypeOfParts.FrontFender].GetChild(1);
                }
                break;
            case (int)TypeOfParts.RearFender: //리어 휀다
                if (carPartsTransforms[(int)TypeOfParts.RearFender].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.rearFender_Left = carPartsTransforms[(int)TypeOfParts.RearFender].GetChild(0);
                    SuspensionController.suspensionController.movePartsTransforms.rearFender_Right = carPartsTransforms[(int)TypeOfParts.RearFender].GetChild(1);
                }
                break;
            case (int)TypeOfParts.Wheel: //휠
                if (carPartsTransforms[(int)TypeOfParts.Wheel].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left = carPartsTransforms[(int)TypeOfParts.Wheel].GetChild(0);
                    SuspensionController.suspensionController.carWheelWheelCapPosTransforms[0] = SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left.Find("WheelCapPos"); //WheelCapPos Transform 캐싱

                    SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right = carPartsTransforms[(int)TypeOfParts.Wheel].GetChild(1);
                    SuspensionController.suspensionController.carWheelWheelCapPosTransforms[1] = SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right.Find("WheelCapPos"); //WheelCapPos Transform 캐싱

                    SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left = carPartsTransforms[(int)TypeOfParts.Wheel].GetChild(2);
                    SuspensionController.suspensionController.carWheelWheelCapPosTransforms[2] = SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left.Find("WheelCapPos"); //WheelCapPos Transform 캐싱

                    SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right = carPartsTransforms[(int)TypeOfParts.Wheel].GetChild(3);
                    SuspensionController.suspensionController.carWheelWheelCapPosTransforms[3] = SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right.Find("WheelCapPos"); //WheelCapPos Transform 캐싱
                }
                break;
            case (int)TypeOfParts.Tire: //타이어
                if (carPartsTransforms[(int)TypeOfParts.Tire].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.tire_Front_Left = carPartsTransforms[(int)TypeOfParts.Tire].GetChild(0);
                    SuspensionController.suspensionController.movePartsTransforms.tire_Front_Right = carPartsTransforms[(int)TypeOfParts.Tire].GetChild(1);
                    SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Left = carPartsTransforms[(int)TypeOfParts.Tire].GetChild(2);
                    SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Right = carPartsTransforms[(int)TypeOfParts.Tire].GetChild(3);
                }
                break;
            case (int)TypeOfParts.Brake: //브레이크
                if (carPartsTransforms[(int)TypeOfParts.Brake].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.brake_Front_Left = carPartsTransforms[(int)TypeOfParts.Brake].GetChild(0);
                    SuspensionController.suspensionController.movePartsTransforms.brake_Front_Right = carPartsTransforms[(int)TypeOfParts.Brake].GetChild(1);
                    SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Left = carPartsTransforms[(int)TypeOfParts.Brake].GetChild(2);
                    SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Right = carPartsTransforms[(int)TypeOfParts.Brake].GetChild(3);
                }
                break;
            case (int)TypeOfParts.Spoiler: //스포일러
                if (carPartsTransforms[(int)TypeOfParts.Spoiler].childCount > 0)
                {
                    SuspensionController.suspensionController.movePartsTransforms.spoiler = carPartsTransforms[(int)TypeOfParts.Spoiler].GetChild(0);
                }
                break;
            default:
                SuspensionController.suspensionController.moveBodyTransform = BundleController.bundleController.loadedBundles[11].transform.Find("Move"); //본체

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

    /* 차량을 불러오는 코루틴 함수 */
    public IEnumerator LoadCar(string carInfo)
    {
        if (carInfo != null && carInfo.Length != 0) //차량 정보가 존재하면
        {
            Notification.notification.EnableLayout(Notification.Layout.LoadCar);  //레이아웃 활성화

            Static.selectedCarName = null; //선택된 차량 이름 제거

            yield return StartCoroutine(DBController.dbController.GetCarBundleInfo(carInfo)); //차량 정보를 통해 차량 번들 정보를 획득

            yield return StartCoroutine(DBController.dbController.GetSelectedPartsName(carInfo)); //차량 정보를 통해 선택된 부품 정보를 획득

            yield return StartCoroutine(DBController.dbController.GetPaintPartSurfaceInfo(carInfo)); //차량 정보를 통해 도색 부위 정보를 획득

            if (BundleController.bundleController.loadedBundles[11]) Destroy(BundleController.bundleController.loadedBundles[11]); //본체가 존재하면 제거
            CarPaint.carPaint.carEntireMaterials[0] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carSideMirrorMaterials[0] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carRoofMaterials[0] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[0] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[1] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[2] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carWindowMaterials[0] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(DBController.dbController.carBundleInfo, Vector3.zero, Vector3.zero, carPartsTransforms[11], 11)); //본체 번들 로드

            if (BundleController.bundleController.loadedBundles[11]) //본체 번들이 로드되면
            {
                CachingBodyTransforms(); //본체의 Transform 컴포넌트들을 캐싱
                MainCamera.mainCamera.CachingCameraPosTransforms(); //본체의 CameraPos Transform 컴포넌트들을 캐싱

                CachingMoveTransforms(11); //움직일 본체 Transform 캐싱 
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.FrontBumper, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.FrontBumper], false, false)); //프론트 범퍼 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.RearBumper, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.RearBumper], false, false)); //리어 범퍼 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Grill, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Grill], false, false)); //그릴 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Bonnet, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Bonnet], false, false)); //본넷 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.FrontFender, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.FrontFender], false, false)); //프론트 휀다 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.RearFender, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.RearFender], false, false)); //리어 휀다 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Wheel, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Wheel], false, false)); //휠 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Tire, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Tire], false, false)); //타이어 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Brake, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Brake], false, false)); //브레이크 불러오기
                yield return StartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Spoiler, DBController.dbController.selectedPartsInfos[(int)TypeOfParts.Spoiler], false, false)); //스포일러 불러오기

                CachingMoveTransforms((int)TypeOfParts.FrontBumper); //움직일 FrontBumper 캐싱
                CachingMoveTransforms((int)TypeOfParts.RearBumper); //움직일 RearBumper 캐싱
                CachingMoveTransforms((int)TypeOfParts.Grill); //움직일 Grill 캐싱
                CachingMoveTransforms((int)TypeOfParts.Bonnet); //움직일 Bonnet 캐싱
                CachingMoveTransforms((int)TypeOfParts.FrontFender); //움직일 FrontFender 캐싱
                CachingMoveTransforms((int)TypeOfParts.RearFender); //움직일 RearFender 캐싱
                CachingMoveTransforms((int)TypeOfParts.Wheel); //움직일 Wheel 캐싱
                CachingMoveTransforms((int)TypeOfParts.Tire); //움직일 Tire 캐싱
                CachingMoveTransforms((int)TypeOfParts.Brake); //움직일 Brake 캐싱
                CachingMoveTransforms((int)TypeOfParts.Spoiler); //움직일 Spoiler 캐싱

                SuspensionController.suspensionController.SyncMovePartsTransforms(); //움직일 파츠의 Transform들을 동기화

                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Entire); //차량 Entire Material 컴포넌트들 캐싱
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Bonnet); //차량 Bonnet Material 컴포넌트들 캐싱
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.SideMirror); //차량 SideMirror Material 컴포넌트들 캐싱
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Roof); //차량 Roof Material 컴포넌트들 캐싱
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.ChromeDelete); //차량 ChromeDelete Material 컴포넌트들 캐싱
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Wheel); //차량 Wheel Material 컴포넌트들 캐싱
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.BrakeCaliper); //차량 BrakeCaliper Material 컴포넌트들 캐싱
                CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Window); //차량 Window Material 컴포넌트들 캐싱
                CarLight.carLight.CachingCarLightMaterials(); //차량 Light Material 컴포넌트들 캐싱

                CarPaint.carPaint.carEntireColor = CarPaint.carPaint.carEntireColor; //Entire 색상 변경
                CarPaint.carPaint.carBonnetColor = CarPaint.carPaint.carBonnetColor; //Bonnet 색상 변경
                CarPaint.carPaint.carSideMirrorColor = CarPaint.carPaint.carSideMirrorColor; //SideMirror 색상 변경
                CarPaint.carPaint.carRoofColor = CarPaint.carPaint.carRoofColor; //Roof 색상 변경
                CarPaint.carPaint.carChromeDeleteColor = CarPaint.carPaint.carChromeDeleteColor; //ChromeDelete 색상 변경
                CarPaint.carPaint.carWheelColor = CarPaint.carPaint.carWheelColor; //Wheel 색상 변경
                CarPaint.carPaint.carBrakeCaliperColor = CarPaint.carPaint.carBrakeCaliperColor; //BrakeCaliper 색상 변경
                CarPaint.carPaint.carWindowColor = CarPaint.carPaint.carWindowColor; //Window 색상 변경

                CarPaint.carPaint.carEntireSurface = CarPaint.carPaint.carEntireSurface; //Entire 재질 변경
                CarPaint.carPaint.carBonnetSurface = CarPaint.carPaint.carBonnetSurface; //Bonnet 재질 변경
                CarPaint.carPaint.carSideMirrorSurface = CarPaint.carPaint.carSideMirrorSurface; //SideMirror 재질 변경
                CarPaint.carPaint.carRoofSurface = CarPaint.carPaint.carRoofSurface; //Roof 재질 변경
                CarPaint.carPaint.carChromeDeleteSurface = CarPaint.carPaint.carChromeDeleteSurface; //ChromeDelete 재질 변경
                CarPaint.carPaint.carWheelSurface = CarPaint.carPaint.carWheelSurface; //Wheel 재질 변경
                CarPaint.carPaint.carBrakeCaliperSurface = CarPaint.carPaint.carBrakeCaliperSurface; //BrakeCaliper 재질 변경

                CarLight.carLight.activateLight = CarLight.carLight.activateLight; //Light 활성화 여부 변경

                Static.selectedCarName = carInfo; //선택된 차량 이름 지정

                yield return StartCoroutine(DBController.dbController.GetMountableParts(carInfo)); //장착 가능한 파츠 정보를 획득
                yield return StartCoroutine(PartsSelect.partsSelect.CreateFrontBumperButtons()); //프론트 범퍼 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateRearBumperButtons()); //리어 범퍼 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateGrillButtons()); //그릴 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateBonnetButtons()); //본넷 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateFrontFenderButtons()); //프론트 휀다 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateRearFenderButtons()); //리어 휀다 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateWheelButtons()); //휠 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateBrakeButtons()); //브레이크 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateExhaustPipeButtons()); //배기구 버튼 생성
                yield return StartCoroutine(PartsSelect.partsSelect.CreateSpoilerButtons()); //스포일러 버튼 생성

                Resources.UnloadUnusedAssets(); //메모리 누수 방지
            }
            else //본체 번들이 로드되지 않으면
            {
                Notification.notification.EnableLayout(Notification.Layout.FailureLoadCar);  //레이아웃 활성화
            }

            Notification.notification.DisableLayout(Notification.Layout.LoadCar);  //레이아웃 비활성화
        }
    }

    /* 한글로 차량을 불러오는 코루틴 함수 */
    public IEnumerator LoadCarFromKo(string carInfo)
    {
        yield return DBController.dbController.GetYearTypeEngFromKo(carInfo); //연식 불러오기
        StartCoroutine(LoadCar(DBController.dbController.yearTypeEng)); //차량 불러오기
    }
}