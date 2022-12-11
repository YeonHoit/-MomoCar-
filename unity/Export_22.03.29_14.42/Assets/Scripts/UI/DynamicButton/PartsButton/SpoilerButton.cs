using UnityEngine;

public class SpoilerButton : MonoBehaviour
{
    [Header("Parts")]
    public string spoilerName; //스포일러 이름

    /* SpoilerButton을 클릭했을 때 실행되는 함수 */
    public void OnClickSpoilerButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Spoiler, spoilerName, true, true));
    }
}