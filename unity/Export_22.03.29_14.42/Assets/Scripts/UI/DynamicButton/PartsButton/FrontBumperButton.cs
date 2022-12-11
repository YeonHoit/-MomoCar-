using UnityEngine;

public class FrontBumperButton : MonoBehaviour
{
    [Header("Parts")]
    public string frontBumperName; //����Ʈ ���� �̸�

    /* FrontBumperButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickFrontBumperButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.FrontBumper, frontBumperName, true, true));
    }
}