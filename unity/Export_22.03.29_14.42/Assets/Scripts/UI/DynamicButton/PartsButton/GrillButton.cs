using UnityEngine;

public class GrillButton : MonoBehaviour
{
    [Header("Parts")]
    public string grillName; //�׸� �̸�

    /* GrillButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickGrillButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Grill, grillName, true, true));
    }
}