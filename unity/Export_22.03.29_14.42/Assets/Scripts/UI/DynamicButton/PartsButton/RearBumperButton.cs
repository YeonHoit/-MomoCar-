using UnityEngine;

public class RearBumperButton : MonoBehaviour
{
    [Header("Parts")]
    public string rearBumperName; //���� ���� �̸�

    /* RearBumperButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickRearBumperButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.RearBumper, rearBumperName, true, true));
    }
}