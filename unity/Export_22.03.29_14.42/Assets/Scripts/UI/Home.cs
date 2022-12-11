using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Home : MonoBehaviour
{
    [Header("Static")]
    public static Home home; //���� ���� ����

    [Header("Variable")]
    private bool activateMenuButtons = true; //MenuButton���� Ȱ��ȭ ����

    [Header("Component")]
    public Image menuButtonImage; //MenuButton�� Image ������Ʈ
    public Animator homeAnimator; //Home�� Animator ������Ʈ

    [Header("Cache")]
    private Color32 buttonActivationColor = new Color32(255, 28, 86, 255); //��ư Ȱ��ȭ ����
    private Color32 buttonDeactivationColor = new Color32(100, 100, 100, 255); //��ư ��Ȱ��ȭ ����
    private readonly int activateMenuButtonsHash = Animator.StringToHash("ActivateMenuButtons"); //ActivateMenuButtons �ִϸ��̼��� Hash
    private readonly int deactivateMenuButtonsHash = Animator.StringToHash("DeactivateMenuButtons"); //DeactivateMenuButtons �ִϸ��̼��� Hash
    private readonly int activateHomeHash = Animator.StringToHash("ActivateHome"); //ActivateHome �ִϸ��̼��� Hash
    private readonly int deactivateHomeHash = Animator.StringToHash("DeactivateHome"); //DeactivateHome �ִϸ��̼��� Hash

    private void Awake()
    {
        home = this;
    }

    private IEnumerator Start()
    {
        Screen.fullScreen = true; //��ü ȭ��
        if(Application.platform == RuntimePlatform.Android) Screen.orientation = ScreenOrientation.LandscapeLeft; //���η� ��ȯ

        yield return null;

        yield return StartCoroutine(Field.field.LoadField("quattro canti")); //���� ��� �ε�
        LocalDBController.localDBController.RequestUserJson(); //UserJson ��û
    }

    #region Interface
    /* MenuButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickMenuButton()
    {
        ActivateMenuButtons(!activateMenuButtons);
    }

    /* MenuButton���� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �Լ� */
    public void ActivateMenuButtons(bool state)
    {
        activateMenuButtons = state; //MenuButton���� Ȱ��ȭ ���� ����
        homeAnimator.Play(state ? activateMenuButtonsHash : deactivateMenuButtonsHash); //�ִϸ��̼� ����
        menuButtonImage.color = state ? buttonActivationColor : buttonDeactivationColor; //MenuButton ���� ����
    }

    /* Home�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �Լ� */
    public void ActivateHome(bool state)
    {
        homeAnimator.Play(state ? activateHomeHash : deactivateHomeHash); //�ִϸ��̼� ����
    }
    #endregion

    #region Quit
    /* QuitButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickQuitButton()
    {
        Notification.notification.EnableLayout(Notification.Layout.QuitApplicationAction); //���̾ƿ� Ȱ��ȭ
    }
    #endregion

    #region PartsReset
    /* PartsReset ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickPartsResetButton()
    {
        StartCoroutine(ResetParts());
    }

    /* ������ �ʱ�ȭ�ϴ� �ڷ�ƾ �Լ� */
    private IEnumerator ResetParts()
    {
        if(Static.selectedCarName != null && Static.selectedCarName.Length != 0) //���õ� ������ �����ϸ�
        {
            Notification.notification.EnableLayout(Notification.Layout.ResetParts);  //���̾ƿ� Ȱ��ȭ

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

            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.FrontBumper); //������ FrontBumper ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.RearBumper); //������ RearBumper ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Grill); //������ Grill ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Bonnet); //������ Bonnet ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.FrontFender); //������ FrontFender ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.RearFender); //������ RearFender ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Wheel); //������ Wheel ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Tire); //������ Tire ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Brake); //������ Brake ĳ��
            CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Spoiler); //������ Spoiler ĳ��

            SuspensionController.suspensionController.SyncMovePartsTransforms(); //������ ������ Transform���� ����ȭ

            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Entire); //���� Entire Material ������Ʈ�� ĳ��
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Bonnet); //���� Bonnet Material ������Ʈ�� ĳ��
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.SideMirror); //���� SideMirror Material ������Ʈ�� ĳ��
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Roof); //���� Roof Material ������Ʈ�� ĳ��
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.ChromeDelete); //���� ChromeDelete Material ������Ʈ�� ĳ��
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Wheel); //���� Wheel Material ������Ʈ�� ĳ��
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.BrakeCaliper); //���� BrakeCaliper Material ������Ʈ�� ĳ��
            CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Window); //���� Window Material ������Ʈ�� ĳ��

            CarPaint.carPaint.carEntireSurface = CarPaint.carPaint.carEntireSurface; //Entire ���� ����
            CarPaint.carPaint.carBonnetSurface = CarPaint.carPaint.carBonnetSurface; //Bonnet ���� ����
            CarPaint.carPaint.carSideMirrorSurface = CarPaint.carPaint.carSideMirrorSurface; //SideMirror ���� ����
            CarPaint.carPaint.carRoofSurface = CarPaint.carPaint.carRoofSurface; //Roof ���� ����
            CarPaint.carPaint.carChromeDeleteSurface = CarPaint.carPaint.carChromeDeleteSurface; //ChromeDelete ���� ����
            CarPaint.carPaint.carWheelSurface = CarPaint.carPaint.carWheelSurface; //Wheel ���� ����
            CarPaint.carPaint.carBrakeCaliperSurface = CarPaint.carPaint.carBrakeCaliperSurface; //BrakeCaliper ���� ����

            Resources.UnloadUnusedAssets(); //�޸� ���� ����

            Notification.notification.DisableLayout(Notification.Layout.ResetParts); //���̾ƿ� ��Ȱ��ȭ
        }
    }
    #endregion
}