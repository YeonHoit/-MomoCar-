using UnityEngine;

public class RearBumperButton : MonoBehaviour
{
    [Header("Parts")]
    public string rearBumperName; //리어 범퍼 이름

    /* RearBumperButton을 클릭했을 때 실행되는 함수 */
    public void OnClickRearBumperButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.RearBumper, rearBumperName, true, true));
    }
}