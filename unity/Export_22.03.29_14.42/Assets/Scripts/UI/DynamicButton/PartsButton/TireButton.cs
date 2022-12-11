using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TireButton : MonoBehaviour
{
    [Header("Variable")]
    public string tireName; //Ÿ�̾� �̸�
    private List<string> aspectRatioList = new List<string>(); //����� ����Ʈ
    private string selectedAspectRatio; //���õ� �����

    [Header("Aspect Ratio")]
    public TextMeshProUGUI previousAspectRatioTMP; //����� Previous TMP ����
    public TextMeshProUGUI currentAspectRatioTMP; //����� Current TMP ����
    public TextMeshProUGUI nextAspectRatioTMP; //����� Next TMP ����

    [Header("LoadParts")]
    public Image timerImage; //Timer�� Image ����
    private Coroutine loadTireWithTimer; //LoadTireWithTimer �ڷ�ƾ �Լ� ��� ����

    /* ����� �߰��ϴ� �Լ� */
    public void AddAspectRatio(string aspectRatio)
    {
        aspectRatioList.Add(aspectRatio); //����� �߰�
        aspectRatioList.Sort((string first, string second) =>
        {
            int firstInch = int.Parse(first);
            int secondInch = int.Parse(second);

            if (firstInch < secondInch) return -1;
            else if (firstInch > secondInch) return 1;
            else return 0;
        }); //�������� ����

        if (selectedAspectRatio == null || selectedAspectRatio.Length == 0) selectedAspectRatio = aspectRatio; //���� ���õ� ����� ������ ����

        SyncTireButton(); //Ÿ�̾� ��ư ����ȭ
    }

    /* Ÿ�̾� ��ư�� ����ȭ�ϴ� �Լ� */
    private void SyncTireButton()
    {
        int aspectRatioIndex = aspectRatioList.FindIndex((string item) =>
        {
            return item.Equals(selectedAspectRatio);
        }); //���õ� Ÿ�̾� ������� �ε��� ����

        previousAspectRatioTMP.text = aspectRatioIndex == 0 ? null : aspectRatioList[aspectRatioIndex - 1];
        currentAspectRatioTMP.text = aspectRatioList[aspectRatioIndex];
        nextAspectRatioTMP.text = aspectRatioIndex + 1 == aspectRatioList.Count ? null : aspectRatioList[aspectRatioIndex + 1];
    }

    /* TireButton�� Ŭ���ϸ� ����Ǵ� �Լ� */
    public void OnClickTireButton(float timer)
    {
        if (loadTireWithTimer != null) StopCoroutine(loadTireWithTimer); //LoadTireWithTimer ���� �ڷ�ƾ �Լ��� ���� ���̸� ����
        loadTireWithTimer = StartCoroutine(LoadTireWithTimer(timer)); //LoadTireWithTimer �ڷ�ƾ �Լ� ����
    }

    /* ���� �ð� �� Ÿ�̾ �ε��ϴ� �ڷ�ƾ �Լ� */
    private IEnumerator LoadTireWithTimer(float timer)
    {
        float initialTimer = timer; //�ʱ� Timer ��
        while (timer > 0f)
        {
            timerImage.fillAmount = timer / initialTimer; //Timer �̹��� ����
            timer -= Time.deltaTime; //Ÿ�̸� ����

            yield return null;
        }

        timerImage.fillAmount = 0f; //Timer �̹��� ����

        string[] wheelName = Static.selectedWheelName.Split('-'); //�� �̸� �и� �� ����
        Utility.utility.LoadStartCoroutine(PartsSelect.partsSelect.LoadParts(TypeOfParts.Tire, tireName + '-' + wheelName[1] + '-' + selectedAspectRatio + '-' + wheelName[2], true, true)); //�� ������ �������� Ÿ�̾� �ε�
    }

    /* ����� ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickAspectRatioButton(bool isNext)
    {
        int aspectRatioIndex = aspectRatioList.FindIndex((string item) =>
        {
            return item.Equals(selectedAspectRatio);
        }); //���õ� Ÿ�̾� ������� �ε��� ����

        if (isNext) //�����̸�
        {
            if (aspectRatioIndex + 1 == aspectRatioList.Count) return; //����� ������ ����

            selectedAspectRatio = aspectRatioList[aspectRatioIndex + 1]; //���õ� ����� ����
        }
        else //�����̸�
        {
            if (aspectRatioIndex == 0) return; //����� ������ ����

            selectedAspectRatio = aspectRatioList[aspectRatioIndex - 1]; //���õ� ����� ����
        }

        SyncTireButton(); //Ÿ�̾� ��ư ����ȭ

        OnClickTireButton(2f); //Ÿ�̾� �ε�
    }
}