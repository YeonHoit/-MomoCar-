using UnityEngine;

public class FrontFenderButton : MonoBehaviour
{
    [Header("Parts")]
    public string frontFenderName; //����Ʈ �Ӵ� �̸�

    /* FrontFenderButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickFrontFenderButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.FrontFender, frontFenderName, true, true));
    }
}