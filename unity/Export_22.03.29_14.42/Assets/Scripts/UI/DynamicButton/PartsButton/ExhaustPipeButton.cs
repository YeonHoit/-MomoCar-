using UnityEngine;

public class ExhaustPipeButton : MonoBehaviour
{
    [Header("Parts")]
    public string exhaustPipeName; //��ⱸ �̸�

    /* ExhaustPipeButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickExhaustPipeButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.ExhaustPipe, exhaustPipeName, true, true));
    }
}