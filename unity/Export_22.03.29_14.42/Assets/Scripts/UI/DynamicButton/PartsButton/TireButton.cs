using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TireButton : MonoBehaviour
{
    [Header("Variable")]
    public string tireName; //타이어 이름
    private List<string> aspectRatioList = new List<string>(); //편평비 리스트
    private string selectedAspectRatio; //선택된 편평비

    [Header("Aspect Ratio")]
    public TextMeshProUGUI previousAspectRatioTMP; //편평비 Previous TMP 정보
    public TextMeshProUGUI currentAspectRatioTMP; //편평비 Current TMP 정보
    public TextMeshProUGUI nextAspectRatioTMP; //편평비 Next TMP 정보

    [Header("LoadParts")]
    public Image timerImage; //Timer의 Image 정보
    private Coroutine loadTireWithTimer; //LoadTireWithTimer 코루틴 함수 담는 변수

    /* 편평비를 추가하는 함수 */
    public void AddAspectRatio(string aspectRatio)
    {
        aspectRatioList.Add(aspectRatio); //편평비 추가
        aspectRatioList.Sort((string first, string second) =>
        {
            int firstInch = int.Parse(first);
            int secondInch = int.Parse(second);

            if (firstInch < secondInch) return -1;
            else if (firstInch > secondInch) return 1;
            else return 0;
        }); //오름차순 정렬

        if (selectedAspectRatio == null || selectedAspectRatio.Length == 0) selectedAspectRatio = aspectRatio; //아직 선택된 편평비가 없으면 지정

        SyncTireButton(); //타이어 버튼 동기화
    }

    /* 타이어 버튼을 동기화하는 함수 */
    private void SyncTireButton()
    {
        int aspectRatioIndex = aspectRatioList.FindIndex((string item) =>
        {
            return item.Equals(selectedAspectRatio);
        }); //선택된 타이어 편평비의 인덱스 저장

        previousAspectRatioTMP.text = aspectRatioIndex == 0 ? null : aspectRatioList[aspectRatioIndex - 1];
        currentAspectRatioTMP.text = aspectRatioList[aspectRatioIndex];
        nextAspectRatioTMP.text = aspectRatioIndex + 1 == aspectRatioList.Count ? null : aspectRatioList[aspectRatioIndex + 1];
    }

    /* TireButton을 클릭하면 실행되는 함수 */
    public void OnClickTireButton(float timer)
    {
        if (loadTireWithTimer != null) StopCoroutine(loadTireWithTimer); //LoadTireWithTimer 기존 코루틴 함수가 실행 중이면 종료
        loadTireWithTimer = StartCoroutine(LoadTireWithTimer(timer)); //LoadTireWithTimer 코루틴 함수 실행
    }

    /* 일정 시간 뒤 타이어를 로드하는 코루틴 함수 */
    private IEnumerator LoadTireWithTimer(float timer)
    {
        float initialTimer = timer; //초기 Timer 값
        while (timer > 0f)
        {
            timerImage.fillAmount = timer / initialTimer; //Timer 이미지 조절
            timer -= Time.deltaTime; //타이머 감소

            yield return null;
        }

        timerImage.fillAmount = 0f; //Timer 이미지 조절

        string[] wheelName = Static.selectedWheelName.Split('-'); //휠 이름 분리 후 저장
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Tire, tireName + '-' + wheelName[1] + '-' + selectedAspectRatio + '-' + wheelName[2], true, true)); //휠 조건을 바탕으로 타이어 로드
    }

    /* 편평비 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickAspectRatioButton(bool isNext)
    {
        int aspectRatioIndex = aspectRatioList.FindIndex((string item) =>
        {
            return item.Equals(selectedAspectRatio);
        }); //선택된 타이어 편평비의 인덱스 저장

        if (isNext) //다음이면
        {
            if (aspectRatioIndex + 1 == aspectRatioList.Count) return; //편평비가 없으면 종료

            selectedAspectRatio = aspectRatioList[aspectRatioIndex + 1]; //선택된 편평비 지정
        }
        else //이전이면
        {
            if (aspectRatioIndex == 0) return; //편평비가 없으면 종료

            selectedAspectRatio = aspectRatioList[aspectRatioIndex - 1]; //선택된 편평비 지정
        }

        SyncTireButton(); //타이어 버튼 동기화

        OnClickTireButton(2f); //타이어 로드
    }
}