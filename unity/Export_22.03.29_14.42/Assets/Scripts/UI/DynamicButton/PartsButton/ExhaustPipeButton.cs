using UnityEngine;

public class ExhaustPipeButton : MonoBehaviour
{
    [Header("Parts")]
    public string exhaustPipeName; //배기구 이름

    /* ExhaustPipeButton을 클릭했을 때 실행되는 함수 */
    public void OnClickExhaustPipeButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.ExhaustPipe, exhaustPipeName, true, true));
    }
}