using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrontFenderCameraPos : CameraPos
{
    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) //PC
        {
            StartCoroutine(BackToCenterCameraPosForPC());
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) //Mobile
        {
            StartCoroutine(BackToCenterCameraPosForMobile());
        }
    }

    /* 클릭했을 때 CenterCameraPos로 돌아가는 함수 - PC 용 */
    private IEnumerator BackToCenterCameraPosForPC()
    {
        while (true)
        {
            if (MainCamera.mainCamera.cameraPosition == CameraPosition.FrontFender) //CameraPosition이 선택되면
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //클릭하면서 UI에 겹치지 않으면
                {
                    MainCamera.mainCamera.SetCurrentPos(CameraPosition.Center); //현재 카메라 위치 재설정
                }
            }
            yield return null;
        }
    }

    /* 클릭했을 때 CenterCameraPos로 돌아가는 함수 - 모바일 용 */
    private IEnumerator BackToCenterCameraPosForMobile()
    {
        while (true)
        {
            if (MainCamera.mainCamera.cameraPosition == CameraPosition.FrontFender) //CameraPosition이 선택되면
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) //클릭하면서 UI에 겹치지 않으면
                {
                    MainCamera.mainCamera.SetCurrentPos(CameraPosition.Center); //현재 카메라 위치 재설정
                }
            }
            yield return null;
        }
    }
}