using UnityEngine;

namespace DynamicButton
{
    public class YearTypeButton : MonoBehaviour
    {
        public string yearType; //연식
        public GameObject tint; //Tint 오브젝트

        /* 연식 버튼을 클릭했을 때 실행되는 함수 */
        public void OnClickYearTypeButton()
        {
            if (CarSelect.carSelect.selectedYearTypeButtonTint != null) CarSelect.carSelect.selectedYearTypeButtonTint.SetActive(false); //기존 Tint 비활성화
            CarSelect.carSelect.selectedYearTypeButtonTint = tint; //선택된 연식 버튼 Tint로 설정
            tint.SetActive(true); //Tint 활성화

            Notification.notification.EnableLayout(Notification.Layout.LoadCarAction, () => Utility.utility.LoadStartCoroutine(CarSelect.carSelect.LoadCar(yearType))); //차량 불러오기
        }
    }
}