using UnityEngine;

namespace DynamicButton
{
    public class CarTypeButton : MonoBehaviour
    {
        public GameObject yearType; //YearType ������Ʈ
        public GameObject tint; //Tint ������Ʈ

        /* ���� ���� ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
        public void OnClickCarTypeButton()
        {
            if (yearType.activeSelf) //YearType ������Ʈ�� Ȱ��ȭ �Ǿ�������
            {
                yearType.SetActive(false); //YearType ������Ʈ ��Ȱ��ȭ
                tint.SetActive(false); //Tint ������Ʈ ��Ȱ��ȭ

                CarSelect.carSelect.selectedCarTypeButtonScript = null; //���õ� ���� ���� ��ư Script ����
            }
            else //YearType ������Ʈ�� ��Ȱ��ȭ �Ǿ�������
            {
                yearType.SetActive(true); //YearType ������Ʈ Ȱ��ȭ
                tint.SetActive(true); //Tint ������Ʈ Ȱ��ȭ

                if (CarSelect.carSelect.selectedCarTypeButtonScript != null) //���õ� ���� ���� ��ư Script�� �����ϸ�
                {
                    CarSelect.carSelect.selectedCarTypeButtonScript.tint.SetActive(false); //���õ� ���� ���� ��ư�� Tint ��Ȱ��ȭ
                    CarSelect.carSelect.selectedCarTypeButtonScript.yearType.SetActive(false); //���õ� ���� ���� ��ư�� YearType ��Ȱ��ȭ
                }
                CarSelect.carSelect.selectedCarTypeButtonScript = this; //���õ� ���� ���� ��ư Script�� ����
            }
        }
    }
}