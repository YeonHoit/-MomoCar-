using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CenterCameraPos : CameraPos
{
    [Header("Static")]
    public static CenterCameraPos centerCameraPos; //전역 참조 변수

    [Header("Component")]
    public Transform centerCameraPosTransform; //CenterCameraPos의 Transform 컴포넌트

    [Header("Rotation")]
    public float tracingSpeed; //추적 속도
    public float degreeX; //X축 각도
    public float degreeY; //Y축 각도
    public EventSystem currentEventSystem; //현재 EventSystem

    private void Awake()
    {
        centerCameraPos = this; //전역 참조 변수 지정
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) //PC
        {
            StartCoroutine(RotateMainPosForPC());
            StartCoroutine(PinchZoomForPC());
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) //Mobile
        {
            StartCoroutine(RotateMainPosForMobile());
            StartCoroutine(PinchZoomForMobile());
        }
    }

    /* Main 카메라 위치를 회전하는 함수 - PC 용 */
    private IEnumerator RotateMainPosForPC()
    {
        Vector3 cursorPoint = Vector3.zero; //커서 지점
        Vector3 tracingPoint = Vector3.zero; //추적 지점
        Vector3 preTracingPoint = Vector3.zero; //이전 추적 지점
        bool cursorOnUI = false; //UI 위에 커서 올라가 있는지 판단하는 변수

        while (true)
        {
            if (Input.GetMouseButtonDown(0)) //클릭하는 순간
            {
                cursorOnUI = currentEventSystem.IsPointerOverGameObject(); //UI 오버 지정
                if (!cursorOnUI) //UI가 클릭되지 않으면
                {
                    cursorPoint = Input.mousePosition; //커서 지점 갱신
                    tracingPoint = Input.mousePosition; //추적 지점 갱신
                    preTracingPoint = Input.mousePosition; //이전 추적 지점 갱신
                }
            }

            if (MainCamera.mainCamera.cameraPosition == CameraPosition.Center || MainCamera.mainCamera.cameraPosition == CameraPosition.CarPaint) //CameraPosition이나 CarPaint가 선택되면
            {
                if (Input.GetMouseButton(0) && !cursorOnUI) //클릭되는 동안
                {
                    cursorPoint = Input.mousePosition; //커서 지점 갱신
                }

                tracingPoint += (cursorPoint - tracingPoint) * Time.deltaTime * tracingSpeed; //추적 지점 갱신

                degreeY += (tracingPoint.x - preTracingPoint.x) * Static.cameraRotationSensitivity; //좌우
                degreeY %= 360f;

                degreeX += (tracingPoint.y - preTracingPoint.y) * Static.cameraRotationSensitivity; //상하
                degreeX = Mathf.Clamp(degreeX, -90f, 0f);

                centerCameraPosTransform.eulerAngles = new Vector3(degreeX, degreeY, centerCameraPosTransform.eulerAngles.z); //각도 지정
                preTracingPoint = tracingPoint; //이전 추적 지점 갱신
            }
            yield return null;
        }
    }

    /* Main 카메라 위치를 회전하는 함수 - 모바일 용 */
    private IEnumerator RotateMainPosForMobile()
    {
        Vector3 cursorPoint = Vector3.zero; //커서 지점
        Vector3 tracingPoint = Vector3.zero; //추적 지점
        Vector3 preTracingPoint = Vector3.zero; //이전 추적 지점
        bool cursorOnUI = false; //UI 위에 커서 올라가 있는지 판단하는 변수
        int preTouchCount = 0; //이전 터치 개수

        while (true)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touches.Length == 1 && preTouchCount > 1)) //터치가 발생하거나 하나의 터치로 변경되면
            {
                cursorOnUI = currentEventSystem.IsPointerOverGameObject(Input.touches[0].fingerId); //UI 오버 지정
                if (!cursorOnUI) //UI가 클릭되지 않으면
                {
                    cursorPoint = Input.touches[0].position; //커서 지점 갱신
                    tracingPoint = Input.touches[0].position; //추적 지점 갱신
                    preTracingPoint = Input.touches[0].position; //이전 추적 지점 갱신
                }
            }

            if (MainCamera.mainCamera.cameraPosition == CameraPosition.Center || MainCamera.mainCamera.cameraPosition == CameraPosition.CarPaint) //CameraPosition이나 CarPaint가 선택되면
            {
                if (Input.touches.Length == 1 && !cursorOnUI) //클릭되는 동안 기준점 각도 변경
                {
                    cursorPoint = Input.touches[0].position; //커서 지점 갱신
                }

                tracingPoint += (cursorPoint - tracingPoint) * Time.deltaTime * tracingSpeed; //추적 지점 갱신

                degreeY += (tracingPoint.x - preTracingPoint.x) * Static.cameraRotationSensitivity; //좌우
                degreeY %= 360f;

                degreeX += (tracingPoint.y - preTracingPoint.y) * Static.cameraRotationSensitivity; //상하
                degreeX = Mathf.Clamp(degreeX, -90f, 0f);

                centerCameraPosTransform.eulerAngles = new Vector3(degreeX, degreeY, centerCameraPosTransform.eulerAngles.z); //각도 지정
                preTracingPoint = tracingPoint; //이전 추적 지점 갱신
            }
            preTouchCount = Input.touches.Length; //이전 터치 개수 갱신

            yield return null;
        }
    }

    /* 핀치 줌을 담당하는 함수 - PC 용 */
    private IEnumerator PinchZoomForPC()
    {
        while (true)
        {
            MainCamera.mainCamera.zoom += Input.mouseScrollDelta.y * MainCamera.mainCamera.zoomMaxFOV * Static.cameraZoomSensitivity;
            yield return null;
        }
    }

    /* 핀치 줌을 담당하는 함수 - 모바일 용 */
    private IEnumerator PinchZoomForMobile()
    {
        while (true)
        {
            if (MainCamera.mainCamera.cameraPosition == CameraPosition.Center) //CameraPosition이 선택되면
            {
                if (Input.touches.Length == 2) //두 개의 손가락이 터치가 되면
                {
                    Touch firstTouch = Input.touches[0]; //첫 번째 손가락 터치
                    Touch secondTouch = Input.touches[1]; //두 번째 손가락 터치

                    Vector2 preFirstTouchPos = firstTouch.position - firstTouch.deltaPosition; //첫 번째 손가락 이전 위치
                    Vector2 preSecondTouchPos = secondTouch.position - secondTouch.deltaPosition; //두 번째 손가락 이전 위치

                    float axis = Vector3.Magnitude(firstTouch.position - secondTouch.position) - Vector3.Magnitude(preFirstTouchPos - preSecondTouchPos); //줌 가속도 계산
                    float moveDistance = axis * Static.cameraZoomSensitivity; //이동해야하는 거리

                    MainCamera.mainCamera.zoom += moveDistance; //거리 적용
                }
            }
            yield return null;
        }
    }
}