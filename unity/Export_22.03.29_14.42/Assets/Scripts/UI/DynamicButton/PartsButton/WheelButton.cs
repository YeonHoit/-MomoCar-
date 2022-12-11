using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WheelButton : MonoBehaviour
{
    [Header("Variable")]
    public string wheelName; //휠 이름

    private List<Pair<string, List<string>>> wheelList = new List<Pair<string, List<string>>>(); //휠 리스트
    private string selectedWidth; //선택된 폭
    private string selectedInch; //선택된 인치

    [Header("Width")]
    public TextMeshProUGUI previousWidthTMP; //폭 Previous TMP 정보
    public TextMeshProUGUI currentWidthTMP; //폭 Current TMP 정보
    public TextMeshProUGUI nextWidthTMP; //폭 Next TMP 정보

    [Header("Inch")]
    public TextMeshProUGUI previousInchTMP; //인치 Previous TMP 정보
    public TextMeshProUGUI currentInchTMP; //인치 Current TMP 정보
    public TextMeshProUGUI nextInchTMP; //인치 Next TMP 정보

    [Header("LoadParts")]
    public Image timerImage; //Timer의 Image 정보
    private Coroutine loadWheelWithTimer; //LoadWheelWithTimer 코루틴 함수 담는 변수

    /* 휠을 추가하는 함수 */
    public void AddWheel(string width, string inch)
    {
        #region AddWidth
        int widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
        {
            return item.first.Equals(width);
        }); //폭의 인덱스 저장

        if (widthIndex == -1) //폭이 존재하지 않으면
        {
            wheelList.Add(new Pair<string, List<string>>(width, new List<string>())); //리스트에 폭 추가
            wheelList.Sort((Pair<string, List<string>> first, Pair<string, List<string>> second) =>
            {
                int firstWidth = int.Parse(first.first);
                int secondWidth = int.Parse(second.first);

                if (firstWidth < secondWidth) return -1;
                else if (firstWidth > secondWidth) return 1;
                else return 0;
            }); //오름차순 정렬
            widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
            {
                return item.first.Equals(width);
            }); //폭의 인덱스 저장
        }
        #endregion

        #region AddInch
        wheelList[widthIndex].second.Add(inch); //인치 추가
        wheelList[widthIndex].second.Sort((string first, string second) =>
        {
            int firstInch = int.Parse(first);
            int secondInch = int.Parse(second);

            if (firstInch < secondInch) return -1;
            else if (firstInch > secondInch) return 1;
            else return 0;
        }); //오름차순 정렬
        #endregion

        if (selectedWidth == null || selectedWidth.Length == 0) selectedWidth = width; //아직 선택된 폭이 없으면 지정
        if (selectedInch == null || selectedInch.Length == 0) selectedInch = inch; //아직 선택된 인치가 없으면 지정

        SyncWheelButton(); //휠 버튼 동기화
    }

    /* 휠 버튼을 동기화하는 함수 */
    private void SyncWheelButton()
    {
        int widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
        {
            return item.first.Equals(selectedWidth);
        }); //선택된 휠 폭의 인덱스 저장
        int inchIndex = wheelList[widthIndex].second.FindIndex((string item) =>
        {
            return item.Equals(selectedInch);
        }); //선택된 휠 인치의 인덱스 저장

        previousWidthTMP.text = widthIndex == 0 ? null : wheelList[widthIndex - 1].first;
        currentWidthTMP.text = wheelList[widthIndex].first;
        nextWidthTMP.text = widthIndex + 1 == wheelList.Count ? null : wheelList[widthIndex + 1].first;

        previousInchTMP.text = inchIndex == 0 ? null : wheelList[widthIndex].second[inchIndex - 1];
        currentInchTMP.text = wheelList[widthIndex].second[inchIndex];
        nextInchTMP.text = inchIndex + 1 == wheelList[widthIndex].second.Count ? null : wheelList[widthIndex].second[inchIndex + 1];
    }

    /* WheelButton을 클릭하면 실행되는 함수 */
    public void OnClickWheelButton(float timer)
    {
        if (loadWheelWithTimer != null) StopCoroutine(loadWheelWithTimer); //LoadWheelWithTimer 기존 코루틴 함수가 실행 중이면 종료
        loadWheelWithTimer = StartCoroutine(LoadWheelWithTimer(timer)); //LoadWheelWithTimer 코루틴 함수 실행
    }

    /* 일정 시간 뒤 휠을 로드하는 코루틴 함수 */
    private IEnumerator LoadWheelWithTimer(float timer)
    {
        float initialTimer = timer; //초기 Timer 값
        while (timer > 0f)
        {
            timerImage.fillAmount = timer / initialTimer; //Timer 이미지 조절
            timer -= Time.deltaTime; //타이머 감소

            yield return null;
        }

        timerImage.fillAmount = 0f; //Timer 이미지 조절

        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Wheel, wheelName + '-' + selectedWidth + '-' + selectedInch, true, false)); //휠 로드
        string[] tireName = Static.selectedTireName.Split('-'); //타이어 이름 분리 후 저장
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Tire, tireName[0] + '-' + selectedWidth + '-' + tireName[2] + '-' + selectedInch, true, true)); //조건에 맞춰 타이어 로드
    }

    /* 폭 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickWidthButton(bool isNext)
    {
        int widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
        {
            return item.first.Equals(selectedWidth);
        }); //선택된 휠 폭의 인덱스 저장

        if (isNext) //다음이면
        {
            if (widthIndex + 1 == wheelList.Count) return; //폭이 없으면 종료

            selectedWidth = wheelList[widthIndex + 1].first; //선택된 폭 지정
            selectedInch = wheelList[widthIndex + 1].second[0]; //선택된 인치 지정
        }
        else //이전이면
        {
            if (widthIndex == 0) return; //폭이 없으면 종료

            selectedWidth = wheelList[widthIndex - 1].first; //선택된 폭 지정
            selectedInch = wheelList[widthIndex - 1].second[0]; //선택된 인치 지정
        }

        SyncWheelButton(); //휠 버튼 동기화

        OnClickWheelButton(2f); //휠 로드
    }

    /* 인치 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickInchButton(bool isNext)
    {
        int widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
        {
            return item.first.Equals(selectedWidth);
        }); //선택된 휠 폭의 인덱스 저장
        int inchIndex = wheelList[widthIndex].second.FindIndex((string item) =>
        {
            return item.Equals(selectedInch);
        }); //선택된 휠 인치의 인덱스 저장

        if (isNext) //다음이면
        {
            if (inchIndex + 1 == wheelList[widthIndex].second.Count) return; //인치가 없으면 종료

            selectedInch = wheelList[widthIndex].second[inchIndex + 1]; //선택된 인치 지정
        }
        else //이전이면
        {
            if (inchIndex == 0) return; //인치가 없으면 종료

            selectedInch = wheelList[widthIndex].second[inchIndex - 1]; //선택된 인치 지정
        }

        SyncWheelButton(); //휠 버튼 동기화

        OnClickWheelButton(2f); //휠 로드
    }
}