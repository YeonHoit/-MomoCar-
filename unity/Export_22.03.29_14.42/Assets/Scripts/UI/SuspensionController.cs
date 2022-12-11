using UnityEngine;

public class SuspensionController : MonoBehaviour
{
    [Header("Static")]
    public static SuspensionController suspensionController; //���� ���� ����

    [Header("SuspensionControllerPanel")]
    public Canvas suspensionControllerPanelCanvas; //SuspensionControllerPanel�� Canvas ������Ʈ

    [Header("Variable")]
    public float suspensionHeight; //������� ����

    [Header("Cache")]
    public PartsTransforms carBodyPartsTransforms; //��ü�� ���� Transform ������Ʈ��
    public Transform carBodyWheelHouseTransform; //���� WheelHouse�� Transform ������Ʈ
    public Transform[] carWheelWheelCapPosTransforms; //�� WheelCapPos�� Transform ������Ʈ��

    public Transform moveBodyTransform; //������ ��ü Transform ������Ʈ
    public PartsTransforms movePartsTransforms; //������ ���� Transform ������Ʈ��
    public Transform moveBrakeDiscRotorTransform; //������ BrakeDiscRotor Transform ������Ʈ
    public Transform[] moveWheelCapTransforms; //������ WheelCap Transform ������Ʈ��

    private void Awake()
    {
        carWheelWheelCapPosTransforms = new Transform[4];
        moveWheelCapTransforms = new Transform[4];

        suspensionController = this;
    }

    /* SuspensionConrollerPanel�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �Լ� */
    public void ActivateSuspensionControllerPanel(bool state)
    {
        Home.home.ActivateHome(!state); //Home Ȱ��ȭ/��Ȱ��ȭ
        suspensionControllerPanelCanvas.enabled = state; //SuspensionControllerPanel�� Canvas ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
    }

    /* ������ ������ Transform����  ����ȭ�ϴ� �Լ� */
    public void SyncMovePartsTransforms()
    {
        if (Static.selectedTireName == null) return; //���õ� Ÿ�̾ ������ ����

        string[] tireName = Static.selectedTireName.Split('-'); //Ÿ�̾� �̸� �и�
        int aspectRatio = int.Parse(tireName[2]); //����� ����
        int inch = int.Parse(tireName[3]); //��ġ ����
        float wheelTireRadius = (((aspectRatio * 0.2f) * 2.5f) + (inch * 2.54f)) * 0.5f; //�� Ÿ�̾� ������ (cm)

        /* ������ ���� Transform ���� ���� */
        if (movePartsTransforms.wheel_Front_Left)  //��
            movePartsTransforms.wheel_Front_Left.position = new Vector3(carBodyPartsTransforms.wheel_Front_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.wheel_Front_Left.position.z);
        if (movePartsTransforms.wheel_Front_Right)
            movePartsTransforms.wheel_Front_Right.position = new Vector3(carBodyPartsTransforms.wheel_Front_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.wheel_Front_Right.position.z);
        if (movePartsTransforms.wheel_Rear_Left)
            movePartsTransforms.wheel_Rear_Left.position = new Vector3(carBodyPartsTransforms.wheel_Rear_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.wheel_Rear_Left.position.z);
        if (movePartsTransforms.wheel_Rear_Right)
            movePartsTransforms.wheel_Rear_Right.position = new Vector3(carBodyPartsTransforms.wheel_Rear_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.wheel_Rear_Right.position.z);

        if (moveWheelCapTransforms[0] && carWheelWheelCapPosTransforms[0]) //WheelCap
            moveWheelCapTransforms[0].position = carWheelWheelCapPosTransforms[0].position;
        if (moveWheelCapTransforms[1] && carWheelWheelCapPosTransforms[1])
            moveWheelCapTransforms[1].position = carWheelWheelCapPosTransforms[1].position;
        if (moveWheelCapTransforms[2] && carWheelWheelCapPosTransforms[2])
            moveWheelCapTransforms[2].position = carWheelWheelCapPosTransforms[2].position;
        if (moveWheelCapTransforms[3] && carWheelWheelCapPosTransforms[3])
            moveWheelCapTransforms[3].position = carWheelWheelCapPosTransforms[3].position;

        if (movePartsTransforms.tire_Front_Left)  //Ÿ�̾�
            movePartsTransforms.tire_Front_Left.position = new Vector3(carBodyPartsTransforms.tire_Front_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.tire_Front_Left.position.z);
        if (movePartsTransforms.tire_Front_Right)
            movePartsTransforms.tire_Front_Right.position = new Vector3(carBodyPartsTransforms.tire_Front_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.tire_Front_Right.position.z);
        if (movePartsTransforms.tire_Rear_Left)
            movePartsTransforms.tire_Rear_Left.position = new Vector3(carBodyPartsTransforms.tire_Rear_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.tire_Rear_Left.position.z);
        if (movePartsTransforms.tire_Rear_Right)
            movePartsTransforms.tire_Rear_Right.position = new Vector3(carBodyPartsTransforms.tire_Rear_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.tire_Rear_Right.position.z);

        if (movePartsTransforms.brake_Front_Left)  //�극��ũ
            movePartsTransforms.brake_Front_Left.position = new Vector3(carBodyPartsTransforms.brake_Front_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.brake_Front_Left.position.z);
        if (movePartsTransforms.brake_Front_Right)
            movePartsTransforms.brake_Front_Right.position = new Vector3(carBodyPartsTransforms.brake_Front_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.brake_Front_Right.position.z);
        if (movePartsTransforms.brake_Rear_Left)
            movePartsTransforms.brake_Rear_Left.position = new Vector3(carBodyPartsTransforms.brake_Rear_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.brake_Rear_Left.position.z);
        if (movePartsTransforms.brake_Rear_Right)
            movePartsTransforms.brake_Rear_Right.position = new Vector3(carBodyPartsTransforms.brake_Rear_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.brake_Rear_Right.position.z);

        if (moveBrakeDiscRotorTransform) //BrakeDiscRotor
            moveBrakeDiscRotorTransform.position = new Vector3(moveBrakeDiscRotorTransform.position.x, (wheelTireRadius * 0.01f), moveBrakeDiscRotorTransform.position.z);

        if (moveBodyTransform) //��ü
            moveBodyTransform.position = new Vector3(moveBodyTransform.position.x, (wheelTireRadius * 0.01f) - (carBodyWheelHouseTransform.position.y - moveBodyTransform.position.y) + (suspensionHeight * 0.01f), moveBodyTransform.position.z);

        if (movePartsTransforms.frontBumper) //����Ʈ ����
            movePartsTransforms.frontBumper.position = carBodyPartsTransforms.frontBumper.position;

        if (movePartsTransforms.rearBumper) //���� ����
            movePartsTransforms.rearBumper.position = carBodyPartsTransforms.rearBumper.position;

        if (movePartsTransforms.grill) //�׸�
            movePartsTransforms.grill.position = carBodyPartsTransforms.grill.position;

        if (movePartsTransforms.bonnet) //����
            movePartsTransforms.bonnet.position = carBodyPartsTransforms.bonnet.position;

        if (movePartsTransforms.frontFender_Left) //����Ʈ �Ӵ�
            movePartsTransforms.frontFender_Left.position = carBodyPartsTransforms.frontFender_Left.position;
        if (movePartsTransforms.frontFender_Right)
            movePartsTransforms.frontFender_Right.position = carBodyPartsTransforms.frontFender_Right.position;

        if (movePartsTransforms.rearFender_Left) //���� �Ӵ�
            movePartsTransforms.rearFender_Left.position = carBodyPartsTransforms.rearFender_Left.position;
        if (movePartsTransforms.rearFender_Right)
            movePartsTransforms.rearFender_Right.position = carBodyPartsTransforms.rearFender_Right.position;

        if (movePartsTransforms.spoiler) //�����Ϸ�
            movePartsTransforms.spoiler.position = carBodyPartsTransforms.spoiler.position;
    }

    /* Slider�� ���� ����Ǿ��� �� ����Ǵ� �Լ� */
    public void OnValueChangedSlider(float suspensionHeight)
    {
        this.suspensionHeight = suspensionHeight;
        SyncMovePartsTransforms();
    }
}