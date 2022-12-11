using UnityEngine;
using System.Collections;
using TAA;

public class MainCamera : MonoBehaviour
{
    [Header("Static")]
    public static MainCamera mainCamera; //전역 참조 변수

    [Header("Component")]
    public Camera mainCameraCamera; //메인 카메라 Camera 컴포넌트
    public Transform mainCameraTransform; //메인 카메라 Transform 컴포넌트
    public TAAComponent mainCameraTAA; //메인 카메라 TAA 컴포넌트

    [Header("CameraPos")]
    public CameraPosition cameraPosition; //현재 카메라 위치
    public Transform currentPosTransform; //현재 추적하는 카메라 위치 Transform 컴포넌트
    public CameraPos[] cameraPositionCameraPoses; //카메라 위치의 CameraPos 컴포넌트들
    private Coroutine traceCurrentPos; //CameraPos 추적하는 코루틴 함수
    private Coroutine moveToCurrentPos; //CurrentPos로 이동하는 코루틴 함수

    [Header("Zoom")]
    private Vector3 offset; //오프셋
    private float _zoom;
    public float zoom //줌
    {
        get
        {
            return _zoom;
        }
        set
        {
            _zoom = Mathf.Clamp(value, 0f, zoomMinFOV + zoomMaxFOV * zoomFOVRatio); //범위 제한

            offset = new Vector3(0f, 0f, _zoom > zoomMinFOV ? zoomMinFOV : _zoom); //오프셋 설정
            mainCameraCamera.fieldOfView = _zoom - zoomMinFOV < 0f ? 60f : 60f - (_zoom - zoomMinFOV) / zoomFOVRatio; //Field of View 설정
        }
    }
    public float zoomFOVRatio; //줌 Field of View의 비율
    public float zoomMinFOV; //줌 최소 Field of View
    public float zoomMaxFOV; //줌 최대 Field of View

    private void Awake()
    {
        mainCamera = this; //전역 참조 변수 지정
    }

    private void Start()
    {
        SetCurrentPos(CameraPosition.Center);
        StartCoroutine(ControlTAAWithMovement());
    }

    #region CameraPos
    /* 추적할 위치로 카메라 이동하는 함수 */
    private IEnumerator TraceCurrentPos()
    {
        while (true)
        {
            if (currentPosTransform != null) //현재 위치 Transform이 존재하면
            {
                mainCameraTransform.SetPositionAndRotation(currentPosTransform.TransformPoint(offset), currentPosTransform.rotation); //Transform 갱신
            }
            yield return null;
        }
    }

    /* CurrentPos로 이동하는 함수 - 애니메이션 */
    private IEnumerator MoveToCurrentPos()
    {
        while (currentPosTransform != null && (Vector3.Distance(mainCameraTransform.position, currentPosTransform.TransformPoint(offset)) > 0.01f || Quaternion.Angle(mainCameraTransform.rotation, currentPosTransform.rotation) > 0.01f))
        {
            mainCameraTransform.position += (currentPosTransform.TransformPoint(offset) - mainCameraTransform.position) * Time.deltaTime * 15f; //카메라 위치 이동
            mainCameraTransform.rotation = Quaternion.Slerp(mainCameraTransform.rotation, currentPosTransform.rotation, Time.deltaTime * 20f); //각도 변경

            yield return null;
        }
        traceCurrentPos = StartCoroutine(TraceCurrentPos()); //TraceCurrentPos 코루틴 함수 실행
    }

    /* CurrentPos를 설정하는 함수 */
    public void SetCurrentPos(CameraPosition cameraPosition)
    {
        if (moveToCurrentPos != null) StopCoroutine(moveToCurrentPos); //기존에 설정되어 있던 코루틴 함수 중지
        if (traceCurrentPos != null) StopCoroutine(traceCurrentPos);

        currentPosTransform = cameraPositionCameraPoses[(int)cameraPosition].cameraPosTransform; //현재 카메라 위치 변경
        mainCamera.zoom = 0f; //줌 초기화
        moveToCurrentPos = StartCoroutine(MoveToCurrentPos()); //MoveToCurrentPos 코루틴 함수로 실행

        this.cameraPosition = cameraPosition; //현재 카메라 위치 변경
    }

    /* CameraPos의 Transform 컴포넌트들을 캐싱하는 함수 */
    public void CachingCameraPosTransforms()
    {
        Transform carBodyCameraPos = BundleController.bundleController.loadedBundles[11].transform.Find("CameraPos");
        cameraPositionCameraPoses[(int)CameraPosition.FrontBumper].cameraPosTransform = carBodyCameraPos.Find("FrontBumper");
        cameraPositionCameraPoses[(int)CameraPosition.RearBumper].cameraPosTransform = carBodyCameraPos.Find("RearBumper");
        cameraPositionCameraPoses[(int)CameraPosition.Grill].cameraPosTransform = carBodyCameraPos.Find("Grill");
        cameraPositionCameraPoses[(int)CameraPosition.Bonnet].cameraPosTransform = carBodyCameraPos.Find("Bonnet");
        cameraPositionCameraPoses[(int)CameraPosition.FrontFender].cameraPosTransform = carBodyCameraPos.Find("FrontFender");
        cameraPositionCameraPoses[(int)CameraPosition.RearFender].cameraPosTransform = carBodyCameraPos.Find("RearFender");
        cameraPositionCameraPoses[(int)CameraPosition.Wheel].cameraPosTransform = carBodyCameraPos.Find("Wheel");
        cameraPositionCameraPoses[(int)CameraPosition.Tire].cameraPosTransform = carBodyCameraPos.Find("Tire");
        cameraPositionCameraPoses[(int)CameraPosition.Brake].cameraPosTransform = carBodyCameraPos.Find("Brake");
        cameraPositionCameraPoses[(int)CameraPosition.Spoiler].cameraPosTransform = carBodyCameraPos.Find("Spoiler");
        cameraPositionCameraPoses[(int)CameraPosition.ExhaustPipe].cameraPosTransform = carBodyCameraPos.Find("ExhaustPipe");
    }
    #endregion

    /* 움직임에 따라 TAA를 조절하는 코루틴 함수 */
    private IEnumerator ControlTAAWithMovement()
    {
        Vector3 prePosition = Vector3.zero; //이전 위치
        float maxDistance = 0.01f; //최대 거리

        while (true)
        {
            mainCameraTAA.Blend = Mathf.Clamp(-Vector3.Distance(mainCameraTransform.position, prePosition) / (2.2857f * maxDistance) + 0.9375f, 0.5f, 0.9375f); //거리에 따라 혼합 조절
            prePosition = mainCameraTransform.position; //이전 위치 갱신

            yield return null;
        }
    }
}