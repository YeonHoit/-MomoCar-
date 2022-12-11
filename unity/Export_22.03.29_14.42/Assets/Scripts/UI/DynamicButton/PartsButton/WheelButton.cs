using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WheelButton : MonoBehaviour
{
    [Header("Variable")]
    public string wheelName; //�� �̸�

    private List<Pair<string, List<string>>> wheelList = new List<Pair<string, List<string>>>(); //�� ����Ʈ
    private string selectedWidth; //���õ� ��
    private string selectedInch; //���õ� ��ġ

    [Header("Width")]
    public TextMeshProUGUI previousWidthTMP; //�� Previous TMP ����
    public TextMeshProUGUI currentWidthTMP; //�� Current TMP ����
    public TextMeshProUGUI nextWidthTMP; //�� Next TMP ����

    [Header("Inch")]
    public TextMeshProUGUI previousInchTMP; //��ġ Previous TMP ����
    public TextMeshProUGUI currentInchTMP; //��ġ Current TMP ����
    public TextMeshProUGUI nextInchTMP; //��ġ Next TMP ����

    [Header("LoadParts")]
    public Image timerImage; //Timer�� Image ����
    private Coroutine loadWheelWithTimer; //LoadWheelWithTimer �ڷ�ƾ �Լ� ��� ����

    /* ���� �߰��ϴ� �Լ� */
    public void AddWheel(string width, string inch)
    {
        #region AddWidth
        int widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
        {
            return item.first.Equals(width);
        }); //���� �ε��� ����

        if (widthIndex == -1) //���� �������� ������
        {
            wheelList.Add(new Pair<string, List<string>>(width, new List<string>())); //����Ʈ�� �� �߰�
            wheelList.Sort((Pair<string, List<string>> first, Pair<string, List<string>> second) =>
            {
                int firstWidth = int.Parse(first.first);
                int secondWidth = int.Parse(second.first);

                if (firstWidth < secondWidth) return -1;
                else if (firstWidth > secondWidth) return 1;
                else return 0;
            }); //�������� ����
            widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
            {
                return item.first.Equals(width);
            }); //���� �ε��� ����
        }
        #endregion

        #region AddInch
        wheelList[widthIndex].second.Add(inch); //��ġ �߰�
        wheelList[widthIndex].second.Sort((string first, string second) =>
        {
            int firstInch = int.Parse(first);
            int secondInch = int.Parse(second);

            if (firstInch < secondInch) return -1;
            else if (firstInch > secondInch) return 1;
            else return 0;
        }); //�������� ����
        #endregion

        if (selectedWidth == null || selectedWidth.Length == 0) selectedWidth = width; //���� ���õ� ���� ������ ����
        if (selectedInch == null || selectedInch.Length == 0) selectedInch = inch; //���� ���õ� ��ġ�� ������ ����

        SyncWheelButton(); //�� ��ư ����ȭ
    }

    /* �� ��ư�� ����ȭ�ϴ� �Լ� */
    private void SyncWheelButton()
    {
        int widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
        {
            return item.first.Equals(selectedWidth);
        }); //���õ� �� ���� �ε��� ����
        int inchIndex = wheelList[widthIndex].second.FindIndex((string item) =>
        {
            return item.Equals(selectedInch);
        }); //���õ� �� ��ġ�� �ε��� ����

        previousWidthTMP.text = widthIndex == 0 ? null : wheelList[widthIndex - 1].first;
        currentWidthTMP.text = wheelList[widthIndex].first;
        nextWidthTMP.text = widthIndex + 1 == wheelList.Count ? null : wheelList[widthIndex + 1].first;

        previousInchTMP.text = inchIndex == 0 ? null : wheelList[widthIndex].second[inchIndex - 1];
        currentInchTMP.text = wheelList[widthIndex].second[inchIndex];
        nextInchTMP.text = inchIndex + 1 == wheelList[widthIndex].second.Count ? null : wheelList[widthIndex].second[inchIndex + 1];
    }

    /* WheelButton�� Ŭ���ϸ� ����Ǵ� �Լ� */
    public void OnClickWheelButton(float timer)
    {
        if (loadWheelWithTimer != null) StopCoroutine(loadWheelWithTimer); //LoadWheelWithTimer ���� �ڷ�ƾ �Լ��� ���� ���̸� ����
        loadWheelWithTimer = StartCoroutine(LoadWheelWithTimer(timer)); //LoadWheelWithTimer �ڷ�ƾ �Լ� ����
    }

    /* ���� �ð� �� ���� �ε��ϴ� �ڷ�ƾ �Լ� */
    private IEnumerator LoadWheelWithTimer(float timer)
    {
        float initialTimer = timer; //�ʱ� Timer ��
        while (timer > 0f)
        {
            timerImage.fillAmount = timer / initialTimer; //Timer �̹��� ����
            timer -= Time.deltaTime; //Ÿ�̸� ����

            yield return null;
        }

        timerImage.fillAmount = 0f; //Timer �̹��� ����

        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Wheel, wheelName + '-' + selectedWidth + '-' + selectedInch, true, false)); //�� �ε�
        string[] tireName = Static.selectedTireName.Split('-'); //Ÿ�̾� �̸� �и� �� ����
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Tire, tireName[0] + '-' + selectedWidth + '-' + tireName[2] + '-' + selectedInch, true, true)); //���ǿ� ���� Ÿ�̾� �ε�
    }

    /* �� ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickWidthButton(bool isNext)
    {
        int widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
        {
            return item.first.Equals(selectedWidth);
        }); //���õ� �� ���� �ε��� ����

        if (isNext) //�����̸�
        {
            if (widthIndex + 1 == wheelList.Count) return; //���� ������ ����

            selectedWidth = wheelList[widthIndex + 1].first; //���õ� �� ����
            selectedInch = wheelList[widthIndex + 1].second[0]; //���õ� ��ġ ����
        }
        else //�����̸�
        {
            if (widthIndex == 0) return; //���� ������ ����

            selectedWidth = wheelList[widthIndex - 1].first; //���õ� �� ����
            selectedInch = wheelList[widthIndex - 1].second[0]; //���õ� ��ġ ����
        }

        SyncWheelButton(); //�� ��ư ����ȭ

        OnClickWheelButton(2f); //�� �ε�
    }

    /* ��ġ ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickInchButton(bool isNext)
    {
        int widthIndex = wheelList.FindIndex((Pair<string, List<string>> item) =>
        {
            return item.first.Equals(selectedWidth);
        }); //���õ� �� ���� �ε��� ����
        int inchIndex = wheelList[widthIndex].second.FindIndex((string item) =>
        {
            return item.Equals(selectedInch);
        }); //���õ� �� ��ġ�� �ε��� ����

        if (isNext) //�����̸�
        {
            if (inchIndex + 1 == wheelList[widthIndex].second.Count) return; //��ġ�� ������ ����

            selectedInch = wheelList[widthIndex].second[inchIndex + 1]; //���õ� ��ġ ����
        }
        else //�����̸�
        {
            if (inchIndex == 0) return; //��ġ�� ������ ����

            selectedInch = wheelList[widthIndex].second[inchIndex - 1]; //���õ� ��ġ ����
        }

        SyncWheelButton(); //�� ��ư ����ȭ

        OnClickWheelButton(2f); //�� �ε�
    }
}