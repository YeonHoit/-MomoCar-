using UnityEngine;

public class BonnetButton : MonoBehaviour
{
    [Header("Parts")]
    public string bonnetName; //본넷 이름

    /* BonnetButton을 클릭했을 때 실행되는 함수 */
    public void OnClickBonnetButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Bonnet, bonnetName, true, true));
    }
}