using UnityEngine;

public class RearFenderButton : MonoBehaviour
{
    [Header("Parts")]
    public string rearFenderName; //���� �Ӵ� �̸�

    /* RearFenderButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickRearFenderButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.RearFender, rearFenderName, true, true));
    }
}