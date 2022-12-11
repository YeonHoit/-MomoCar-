using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Home : MonoBehaviour
{
    [Header("Static")]
    public static Home home; //전역 참조 변수

    [Header("Variable")]
    private bool activateMenuButtons = true; //MenuButton들의 활성화 여부

    [Header("Component")]
    public Image menuButtonImage; //MenuButton의 Image 컴포넌트
    public Animator homeAnimator; //Home의 Animator 컴포넌트

    [Header("Cache")]
    private Color32 buttonActivationColor = new Color32(255, 28, 86, 255); //버튼 활성화 색상
    private Color32 buttonDeactivationColor = new Color32(100, 100, 100, 255); //버튼 비활성화 색상
    private readonly int activateMenuButtonsHash = Animator.StringToHash("ActivateMenuButtons"); //ActivateMenuButtons 애니메이션의 Hash
    private readonly int deactivateMenuButtonsHash = Animator.StringToHash("DeactivateMenuButtons"); //DeactivateMenuButtons 애니메이션의 Hash
    private readonly int activateHomeHash = Animator.StringToHash("ActivateHome"); //ActivateHome 애니메이션의 Hash
    private readonly int deactivateHomeHash = Animator.StringToHash("DeactivateHome"); //DeactivateHome 애니메이션의 Hash

    private void Awake()
    {
        home = this;
    }

    private IEnumerator Start()
    {
        Screen.fullScreen = true; //전체 화면
        if(Application.platform == RuntimePlatform.Android) Screen.orientation = ScreenOrientation.LandscapeLeft; //가로로 전환

        yield return null;

        yield return StartCoroutine(Field.field.LoadField("quattro canti")); //시작 배경 로드
        LocalDBController.localDBController.RequestUserJson(); //UserJson 요청
    }

    #region Interface
    /* MenuButton을 클릭했을 때 실행되는 함수 */
    public void OnClickMenuButton()
    {
        ActivateMenuButtons(!activateMenuButtons);
    }

    /* MenuButton들을 활성화/비활성화 하는 함수 */
    public void ActivateMenuButtons(bool state)
    {
        activateMenuButtons = state; //MenuButton들의 활성화 여부 변경
        homeAnimator.Play(state ? activateMenuButtonsHash : deactivateMenuButtonsHash); //애니메이션 실행
        menuButtonImage.color = state ? buttonActivationColor : buttonDeactivationColor; //MenuButton 색상 변경
    }

    /* Home을 활성화/비활성화 하는 함수 */
    public void ActivateHome(bool state)
    {
        homeAnimator.Play(state ? activateHomeHash : deactivateHomeHash); //애니메이션 실행
    }
    #endregion

    #region Quit
    /* QuitButton을 클릭했을 때 실행되는 함수 */
    public void OnClickQuitButton()
    {
        Notification.notification.EnableLayout(Notification.Layout.QuitApplicationAction); //레이아웃 활성화
    }
    #endregion

    #region PartsReset
    /* PartsReset 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickPartsResetButton()
    {
        StartCoroutine(ResetParts());
    }

    /* 파츠를 초기화하는 코루틴 함수 */
    private IEnumerator ResetParts()
    {
        if(Static.selectedCarName != null && Static.selectedCarName.Length != 0) //선택된 차량이 존재하면
        {
            Notification.notification.EnableLayout(Notification.Layout.ResetParts);  //레이아웃 활성화

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

            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.FrontBumper); //움직일 FrontBumper 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.RearBumper); //움직일 RearBumper 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Grill); //움직일 Grill 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Bonnet); //움직일 Bonnet 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.FrontFender); //움직일 FrontFender 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.RearFender); //움직일 RearFender 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Wheel); //움직일 Wheel 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Tire); //움직일 Tire 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Brake); //움직일 Brake 캐싱
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Spoiler); //움직일 Spoiler 캐싱

            SuspensionController.suspensionController.SyncMovePartsTransforms(); //움직일 파츠의 Transform들을 동기화

            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Entire); //차량 Entire Material 컴포넌트들 캐싱
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Bonnet); //차량 Bonnet Material 컴포넌트들 캐싱
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.SideMirror); //차량 SideMirror Material 컴포넌트들 캐싱
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Roof); //차량 Roof Material 컴포넌트들 캐싱
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.ChromeDelete); //차량 ChromeDelete Material 컴포넌트들 캐싱
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Wheel); //차량 Wheel Material 컴포넌트들 캐싱
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.BrakeCaliper); //차량 BrakeCaliper Material 컴포넌트들 캐싱
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Window); //차량 Window Material 컴포넌트들 캐싱

            CarPaint.carPaint.carEntireSurface = CarPaint.carPaint.carEntireSurface; //Entire 재질 변경
            CarPaint.carPaint.carBonnetSurface = CarPaint.carPaint.carBonnetSurface; //Bonnet 재질 변경
            CarPaint.carPaint.carSideMirrorSurface = CarPaint.carPaint.carSideMirrorSurface; //SideMirror 재질 변경
            CarPaint.carPaint.carRoofSurface = CarPaint.carPaint.carRoofSurface; //Roof 재질 변경
            CarPaint.carPaint.carChromeDeleteSurface = CarPaint.carPaint.carChromeDeleteSurface; //ChromeDelete 재질 변경
            CarPaint.carPaint.carWheelSurface = CarPaint.carPaint.carWheelSurface; //Wheel 재질 변경
            CarPaint.carPaint.carBrakeCaliperSurface = CarPaint.carPaint.carBrakeCaliperSurface; //BrakeCaliper 재질 변경

            Resources.UnloadUnusedAssets(); //메모리 누수 방지

            Notification.notification.DisableLayout(Notification.Layout.ResetParts); //레이아웃 비활성화
        }
    }
    #endregion
}