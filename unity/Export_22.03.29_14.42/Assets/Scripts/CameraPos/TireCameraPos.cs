using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TireCameraPos : CameraPos
{
    [Header("CameraOffset")]
    public Vector3 centerCameraPosRotation; //CenterCameraPos�� ȸ�� ����

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

    /* Ŭ������ �� CenterCameraPos�� ���ư��� �Լ� - PC �� */
    private IEnumerator BackToCenterCameraPosForPC()
    {
        while (true)
        {
            if (MainCamera.mainCamera.cameraPosition == CameraPosition.Tire) //CameraPosition�� ���õǸ�
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //Ŭ���ϸ鼭 UI�� ��ġ�� ������
                {
                    CenterCameraPos.centerCameraPos.centerCameraPosTransform.localEulerAngles = centerCameraPosRotation; //CenterCameraPos�� ȸ�� ���� ����
                    CenterCameraPos.centerCameraPos.degreeX = centerCameraPosRotation.x; //x�� ���� ����
                    CenterCameraPos.centerCameraPos.degreeY = centerCameraPosRotation.y; //y�� ���� ����
                    MainCamera.mainCamera.SetCurrentPos(CameraPosition.Center); //���� ī�޶� ��ġ �缳��
                }
            }
            yield return null;
        }
    }

    /* Ŭ������ �� CenterCameraPos�� ���ư��� �Լ� - ����� �� */
    private IEnumerator BackToCenterCameraPosForMobile()
    {
        while (true)
        {
            if (MainCamera.mainCamera.cameraPosition == CameraPosition.Tire) //CameraPosition�� ���õǸ�
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) //Ŭ���ϸ鼭 UI�� ��ġ�� ������
                {
                    CenterCameraPos.centerCameraPos.centerCameraPosTransform.localEulerAngles = centerCameraPosRotation; //CenterCameraPos�� ȸ�� ���� ����
                    CenterCameraPos.centerCameraPos.degreeX = centerCameraPosRotation.x; //x�� ���� ����
                    CenterCameraPos.centerCameraPos.degreeY = centerCameraPosRotation.y; //y�� ���� ����
                    MainCamera.mainCamera.SetCurrentPos(CameraPosition.Center); //���� ī�޶� ��ġ �缳��
                }
            }
            yield return null;
        }
    }
}