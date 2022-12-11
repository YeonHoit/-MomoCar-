using UnityEngine;

public class GrillButton : MonoBehaviour
{
    [Header("Parts")]
    public string grillName; //그릴 이름

    /* GrillButton을 클릭했을 때 실행되는 함수 */
    public void OnClickGrillButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Grill, grillName, true, true));
    }
}