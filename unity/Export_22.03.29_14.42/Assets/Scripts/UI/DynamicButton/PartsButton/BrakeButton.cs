using UnityEngine;

public class BrakeButton : MonoBehaviour
{
    [Header("Parts")]
    public string brakeName; //브레이크 이름

    /* BrakeButton을 클릭했을 때 실행되는 함수 */
    public void OnClickBrakeButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Brake, brakeName, true, true));
    }
}