using UnityEngine;

namespace DynamicButton
{
    public class CarTypeButton : MonoBehaviour
    {
        public GameObject yearType; //YearType 오브젝트
        public GameObject tint; //Tint 오브젝트

        /* 차량 유형 버튼을 클릭했을 때 실행되는 함수 */
        public void OnClickCarTypeButton()
        {
            if (yearType.activeSelf) //YearType 오브젝트가 활성화 되어있으면
            {
                yearType.SetActive(false); //YearType 오브젝트 비활성화
                tint.SetActive(false); //Tint 오브젝트 비활성화

                CarSelect.carSelect.selectedCarTypeButtonScript = null; //선택된 차량 유형 버튼 Script 제거
            }
            else //YearType 오브젝트가 비활성화 되어있으면
            {
                yearType.SetActive(true); //YearType 오브젝트 활성화
                tint.SetActive(true); //Tint 오브젝트 활성화

                if (CarSelect.carSelect.selectedCarTypeButtonScript != null) //선택된 차량 유형 버튼 Script가 존재하면
                {
                    CarSelect.carSelect.selectedCarTypeButtonScript.tint.SetActive(false); //선택된 차량 유형 버튼의 Tint 비활성화
                    CarSelect.carSelect.selectedCarTypeButtonScript.yearType.SetActive(false); //선택된 차량 유형 버튼의 YearType 비활성화
                }
                CarSelect.carSelect.selectedCarTypeButtonScript = this; //선택된 차량 유형 버튼 Script로 지정
            }
        }
    }
}