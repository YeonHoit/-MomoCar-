using UnityEngine;

public class FrontBumperButton : MonoBehaviour
{
    [Header("Parts")]
    public string frontBumperName; //프론트 범퍼 이름

    /* FrontBumperButton을 클릭했을 때 실행되는 함수 */
    public void OnClickFrontBumperButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.FrontBumper, frontBumperName, true, true));
    }
}