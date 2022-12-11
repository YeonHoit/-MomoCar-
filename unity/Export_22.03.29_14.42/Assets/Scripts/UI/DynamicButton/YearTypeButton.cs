using UnityEngine;

namespace DynamicButton
{
    public class YearTypeButton : MonoBehaviour
    {
        public string yearType; //����
        public GameObject tint; //Tint ������Ʈ

        /* ���� ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
        public void OnClickYearTypeButton()
        {
            if (CarSelect.carSelect.selectedYearTypeButtonTint != null) CarSelect.carSelect.selectedYearTypeButtonTint.SetActive(false); //���� Tint ��Ȱ��ȭ
            CarSelect.carSelect.selectedYearTypeButtonTint = tint; //���õ� ���� ��ư Tint�� ����
            tint.SetActive(true); //Tint Ȱ��ȭ

            Notification.notification.EnableLayout(Notification.Layout.LoadCarAction, () => Utility.utility.LoadStartCoroutine(CarSelect.carSelect.LoadCar(yearType))); //���� �ҷ�����
        }
    }
}