using UnityEngine;

public class FrontFenderButton : MonoBehaviour
{
    [Header("Parts")]
    public string frontFenderName; //프론트 휀다 이름

    /* FrontFenderButton을 클릭했을 때 실행되는 함수 */
    public void OnClickFrontFenderButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.FrontFender, frontFenderName, true, true));
    }
}