using UnityEngine;

public class BrakeButton : MonoBehaviour
{
    [Header("Parts")]
    public string brakeName; //�극��ũ �̸�

    /* BrakeButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickBrakeButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Brake, brakeName, true, true));
    }
}