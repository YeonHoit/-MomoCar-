using UnityEngine;

public class BonnetButton : MonoBehaviour
{
    [Header("Parts")]
    public string bonnetName; //���� �̸�

    /* BonnetButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickBonnetButton()
    {
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Bonnet, bonnetName, true, true));
    }
}