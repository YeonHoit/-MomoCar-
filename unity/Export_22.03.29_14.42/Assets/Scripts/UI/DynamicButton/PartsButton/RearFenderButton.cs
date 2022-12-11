using UnityEngine;

public class RearFenderButton : MonoBehaviour
{
    [Header("Parts")]
    public string rearFenderName; //리어 휀다 이름

    /* RearFenderButton을 클릭했을 때 실행되는 함수 */
    public void OnClickRearFenderButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.RearFender, rearFenderName, true, true));
    }
}