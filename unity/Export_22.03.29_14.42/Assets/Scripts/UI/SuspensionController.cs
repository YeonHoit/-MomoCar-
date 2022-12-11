using UnityEngine;

public class SuspensionController : MonoBehaviour
{
    [Header("Static")]
    public static SuspensionController suspensionController; //전역 참조 변수

    [Header("SuspensionControllerPanel")]
    public Canvas suspensionControllerPanelCanvas; //SuspensionControllerPanel의 Canvas 컴포넌트

    [Header("Variable")]
    public float suspensionHeight; //서스펜션 높이

    [Header("Cache")]
    public PartsTransforms carBodyPartsTransforms; //본체의 파츠 Transform 컴포넌트들
    public Transform carBodyWheelHouseTransform; //차량 WheelHouse의 Transform 컴포넌트
    public Transform[] carWheelWheelCapPosTransforms; //휠 WheelCapPos의 Transform 컴포넌트들

    public Transform moveBodyTransform; //움직일 본체 Transform 컴포넌트
    public PartsTransforms movePartsTransforms; //움직일 파츠 Transform 컴포넌트들
    public Transform moveBrakeDiscRotorTransform; //움직일 BrakeDiscRotor Transform 컴포넌트
    public Transform[] moveWheelCapTransforms; //움직일 WheelCap Transform 컴포넌트들

    private void Awake()
    {
        carWheelWheelCapPosTransforms = new Transform[4];
        moveWheelCapTransforms = new Transform[4];

        suspensionController = this;
    }

    /* SuspensionConrollerPanel을 활성화/비활성화 하는 함수 */
    public void ActivateSuspensionControllerPanel(bool state)
    {
        Home.home.ActivateHome(!state); //Home 활성화/비활성화
        suspensionControllerPanelCanvas.enabled = state; //SuspensionControllerPanel의 Canvas 컴포넌트 활성화/비활성화
    }

    /* 움직일 파츠의 Transform들을  동기화하는 함수 */
    public void SyncMovePartsTransforms()
    {
        if (Static.selectedTireName == null) return; //선택된 타이어가 없으면 종료

        string[] tireName = Static.selectedTireName.Split('-'); //타이어 이름 분리
        int aspectRatio = int.Parse(tireName[2]); //편평비 저장
        int inch = int.Parse(tireName[3]); //인치 저장
        float wheelTireRadius = (((aspectRatio * 0.2f) * 2.5f) + (inch * 2.54f)) * 0.5f; //휠 타이어 반지름 (cm)

        /* 움직일 파츠 Transform 높이 변경 */
        if (movePartsTransforms.wheel_Front_Left)  //휠
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

        if (movePartsTransforms.tire_Front_Left)  //타이어
            movePartsTransforms.tire_Front_Left.position = new Vector3(carBodyPartsTransforms.tire_Front_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.tire_Front_Left.position.z);
        if (movePartsTransforms.tire_Front_Right)
            movePartsTransforms.tire_Front_Right.position = new Vector3(carBodyPartsTransforms.tire_Front_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.tire_Front_Right.position.z);
        if (movePartsTransforms.tire_Rear_Left)
            movePartsTransforms.tire_Rear_Left.position = new Vector3(carBodyPartsTransforms.tire_Rear_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.tire_Rear_Left.position.z);
        if (movePartsTransforms.tire_Rear_Right)
            movePartsTransforms.tire_Rear_Right.position = new Vector3(carBodyPartsTransforms.tire_Rear_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.tire_Rear_Right.position.z);

        if (movePartsTransforms.brake_Front_Left)  //브레이크
            movePartsTransforms.brake_Front_Left.position = new Vector3(carBodyPartsTransforms.brake_Front_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.brake_Front_Left.position.z);
        if (movePartsTransforms.brake_Front_Right)
            movePartsTransforms.brake_Front_Right.position = new Vector3(carBodyPartsTransforms.brake_Front_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.brake_Front_Right.position.z);
        if (movePartsTransforms.brake_Rear_Left)
            movePartsTransforms.brake_Rear_Left.position = new Vector3(carBodyPartsTransforms.brake_Rear_Left.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.brake_Rear_Left.position.z);
        if (movePartsTransforms.brake_Rear_Right)
            movePartsTransforms.brake_Rear_Right.position = new Vector3(carBodyPartsTransforms.brake_Rear_Right.position.x, wheelTireRadius * 0.01f, carBodyPartsTransforms.brake_Rear_Right.position.z);

        if (moveBrakeDiscRotorTransform) //BrakeDiscRotor
            moveBrakeDiscRotorTransform.position = new Vector3(moveBrakeDiscRotorTransform.position.x, (wheelTireRadius * 0.01f), moveBrakeDiscRotorTransform.position.z);

        if (moveBodyTransform) //본체
            moveBodyTransform.position = new Vector3(moveBodyTransform.position.x, (wheelTireRadius * 0.01f) - (carBodyWheelHouseTransform.position.y - moveBodyTransform.position.y) + (suspensionHeight * 0.01f), moveBodyTransform.position.z);

        if (movePartsTransforms.frontBumper) //프론트 범퍼
            movePartsTransforms.frontBumper.position = carBodyPartsTransforms.frontBumper.position;

        if (movePartsTransforms.rearBumper) //리어 범퍼
            movePartsTransforms.rearBumper.position = carBodyPartsTransforms.rearBumper.position;

        if (movePartsTransforms.grill) //그릴
            movePartsTransforms.grill.position = carBodyPartsTransforms.grill.position;

        if (movePartsTransforms.bonnet) //본넷
            movePartsTransforms.bonnet.position = carBodyPartsTransforms.bonnet.position;

        if (movePartsTransforms.frontFender_Left) //프론트 휀다
            movePartsTransforms.frontFender_Left.position = carBodyPartsTransforms.frontFender_Left.position;
        if (movePartsTransforms.frontFender_Right)
            movePartsTransforms.frontFender_Right.position = carBodyPartsTransforms.frontFender_Right.position;

        if (movePartsTransforms.rearFender_Left) //리어 휀다
            movePartsTransforms.rearFender_Left.position = carBodyPartsTransforms.rearFender_Left.position;
        if (movePartsTransforms.rearFender_Right)
            movePartsTransforms.rearFender_Right.position = carBodyPartsTransforms.rearFender_Right.position;

        if (movePartsTransforms.spoiler) //스포일러
            movePartsTransforms.spoiler.position = carBodyPartsTransforms.spoiler.position;
    }

    /* Slider의 값이 변경되었을 때 실행되는 함수 */
    public void OnValueChangedSlider(float suspensionHeight)
    {
        this.suspensionHeight = suspensionHeight;
        SyncMovePartsTransforms();
    }
}