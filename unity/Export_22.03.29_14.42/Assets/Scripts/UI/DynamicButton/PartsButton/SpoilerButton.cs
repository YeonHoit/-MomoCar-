using UnityEngine;

public class SpoilerButton : MonoBehaviour
{
    [Header("Parts")]
    public string spoilerName; //�����Ϸ� �̸�

    /* SpoilerButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickSpoilerButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Spoiler, spoilerName, true, true));
    }
}