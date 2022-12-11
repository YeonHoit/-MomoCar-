using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using TMPro;

public class PartsSelect : MonoBehaviour
{
    [Header("Static")]
    public static PartsSelect partsSelect; //���� ���� ����

    [Header("Parts Select Panels")]
    public Canvas partsSelectPanelsCanvas; //PartsSelectPanels�� Canvas ������Ʈ

    [Header("Content Select")]
    public RectTransform[] minimizedMainScrollViewContentRectTransforms; //�ּ�ȭ�� MainScrollView Content�� RectTransform ������Ʈ��
    public TextMeshProUGUI[] minimizedContentSelectButtonTMP; //�ּ�ȭ�� ContentSelectButton�� TMP ������Ʈ��

    public RectTransform[] maximizedMainScrollViewContentRectTransforms; //�ִ�ȭ�� MainScrollView Content�� RectTransform ������Ʈ��
    public TextMeshProUGUI[] maximizedContentSelectButtonTMP; //�ִ�ȭ�� ContentSelectButton�� TMP ������Ʈ��

    private Color32 contentSelectButtonActivationColor = new Color32(255, 28, 86, 255); //ContentSelectButton�� Ȱ��ȭ ����
    private Color32 contentSelectButtonDeactivationColor = Color.white; //ContentSelectButton�� ��Ȱ��ȭ ����

    public ScrollRect minimizedMainScrollViewScrollRect; //�ּ�ȭ�� MainScrollView�� ScrollRect ������Ʈ
    public ScrollRect maximizedMainScrollViewScrollRect; //�ִ�ȭ�� MainScrollView�� ScrollRect ������Ʈ

    [Header("Minimize/Maximize")]
    public GameObject partsSelectMinimizedPanel; //PartsSelectMinimizedPanel ������Ʈ
    public GameObject partsSelectMaximizedPanel; //PartsSelectMaximizedPanel ������Ʈ

    [Header("Create Button")]
    public GameObject[] minimizedMainScrollViewSampleButtons; //�ּ�ȭ�� MainScrollView�� SampleButton ������Ʈ��
    public GameObject[] maximizedMainScrollViewSampleButtons; //�ִ�ȭ�� MainScrollView�� SampleButton ������Ʈ��

    private void Awake()
    {
        partsSelect = this; //���� ���� ���� ����
    }

    private void Start()
    {
        OnClickContentSelectButton(0);
        MinimizePartsSelectPanel(true);
    }

    #region Interface
    /* PartsSelectPanels�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �Լ� */
    public void ActivatePartsSelectPanels(bool state)
    {
        Home.home.ActivateHome(!state); //Ȩ Ȱ��ȭ/��Ȱ��ȭ
        partsSelectPanelsCanvas.enabled = state; //PartsSelectPanels�� Canvas ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
        MainCamera.mainCamera.SetCurrentPos(CameraPosition.Center); //ī�޶� ��ġ ����
    }

    /* PartsSelectPanel�� �ּ�ȭ/�ִ�ȭ �ϴ� �Լ� */
    public void MinimizePartsSelectPanel(bool state)
    {
        partsSelectMinimizedPanel.SetActive(state); //PartsSelectMinimizedPanel Ȱ��ȭ/��Ȱ��ȭ
        partsSelectMaximizedPanel.SetActive(!state); //PartsSelectMaximizedPanel Ȱ��ȭ/��Ȱ��ȭ
    }

    /* Content�� �����ϴ� ��ư�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickContentSelectButton(int index)
    {
        for (int i = 0; i < Utility.enumTypeOfPartsLength; ++i) //���� ������ŭ �ݺ�
        {
            minimizedContentSelectButtonTMP[i].color = contentSelectButtonDeactivationColor; //��� �ּ�ȭ�� Content Select Button�� ��Ȱ��ȭ �������� ����
            minimizedMainScrollViewContentRectTransforms[i].gameObject.SetActive(false); //��� �ּ�ȭ�� MainScrollView Content�� ��Ȱ��ȭ

            maximizedContentSelectButtonTMP[i].color = contentSelectButtonDeactivationColor; //��� �ִ�ȭ�� Content Select Button�� ��Ȱ��ȭ �������� ����
            maximizedMainScrollViewContentRectTransforms[i].gameObject.SetActive(false); //��� �ִ�ȭ�� MainScrollView Content�� ��Ȱ��ȭ
        }

        minimizedContentSelectButtonTMP[index].color = contentSelectButtonActivationColor; //���� �ּ�ȭ�� Content Select Button�� Ȱ��ȭ �������� ����
        minimizedMainScrollViewContentRectTransforms[index].gameObject.SetActive(true); //���� �ּ�ȭ�� MainScrollView Content�� Ȱ��ȭ
        minimizedMainScrollViewScrollRect.content = minimizedMainScrollViewContentRectTransforms[index]; //���� �ּ�ȭ�� MainScrollView Content�� Content�� ����

        maximizedContentSelectButtonTMP[index].color = contentSelectButtonActivationColor; //���� �ִ�ȭ�� Content Select Button�� Ȱ��ȭ �������� ����
        maximizedMainScrollViewContentRectTransforms[index].gameObject.SetActive(true); //���� �ִ�ȭ�� MainScrollView Content�� Ȱ��ȭ
        maximizedMainScrollViewScrollRect.content = maximizedMainScrollViewContentRectTransforms[index]; //���� �ִ�ȭ�� MainScrollView Content�� Content�� ����

        if (partsSelectPanelsCanvas.enabled) MainCamera.mainCamera.SetCurrentPos((CameraPosition)index); //ī�޶� ��ġ ����
    }
    #endregion

    #region CreatePartsButton
    /* ����Ʈ ���� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateFrontBumperButtons()
    {
        int frontBumperIndex = (int)TypeOfParts.FrontBumper; //�ε��� ����

        /* �ּ�ȭ�� FrontBumperButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[frontBumperIndex].childCount; ++i) //FrontBumperContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[frontBumperIndex].GetChild(i).gameObject; //FrontBumperContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� FrontBumperButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[frontBumperIndex].childCount; ++i) //FrontBumperContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[frontBumperIndex].GetChild(i).gameObject; //FrontBumperContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableFrontBumper.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform frontBumperButtonTransform = null; //FrontBumperButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            frontBumperButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[frontBumperIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/frontbumper/" + DBController.dbController.mountableFrontBumper[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            frontBumperButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            frontBumperButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableFrontBumper[i].first; //SampleButton�� Text ����

            frontBumperButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[frontBumperIndex]); //�θ� ����
            frontBumperButtonTransform.localScale = Vector3.one; //ũ�� ������
            frontBumperButtonTransform.gameObject.name = DBController.dbController.mountableFrontBumper[i].second; //�̸� ����
            frontBumperButtonTransform.GetComponent<FrontBumperButton>().frontBumperName = DBController.dbController.mountableFrontBumper[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            frontBumperButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            frontBumperButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[frontBumperIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/frontbumper/" + DBController.dbController.mountableFrontBumper[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            frontBumperButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            frontBumperButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableFrontBumper[i].first; //SampleButton�� Text ����

            frontBumperButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[frontBumperIndex]); //�θ� ����
            frontBumperButtonTransform.localScale = Vector3.one; //ũ�� ������
            frontBumperButtonTransform.gameObject.name = DBController.dbController.mountableFrontBumper[i].second; //�̸� ����
            frontBumperButtonTransform.GetComponent<FrontBumperButton>().frontBumperName = DBController.dbController.mountableFrontBumper[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            frontBumperButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }

    /* ���� ���� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateRearBumperButtons()
    {
        int rearBumperIndex = (int)TypeOfParts.RearBumper; //�ε��� ����

        /* �ּ�ȭ�� RearBumperButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[rearBumperIndex].childCount; ++i) //RearBumperContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[rearBumperIndex].GetChild(i).gameObject; //RearBumperContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� RearBumperButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[rearBumperIndex].childCount; ++i) //RearBumperContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[rearBumperIndex].GetChild(i).gameObject; //RearBumperContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableRearBumper.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform rearBumperButtonTransform = null; //RearBumperButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            rearBumperButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[rearBumperIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/rearbumper/" + DBController.dbController.mountableRearBumper[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            rearBumperButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            rearBumperButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableRearBumper[i].first; //SampleButton�� Text ����

            rearBumperButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[rearBumperIndex]); //�θ� ����
            rearBumperButtonTransform.localScale = Vector3.one; //ũ�� ������
            rearBumperButtonTransform.gameObject.name = DBController.dbController.mountableRearBumper[i].second; //�̸� ����
            rearBumperButtonTransform.GetComponent<RearBumperButton>().rearBumperName = DBController.dbController.mountableRearBumper[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            rearBumperButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            rearBumperButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[rearBumperIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/rearbumper/" + DBController.dbController.mountableRearBumper[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            rearBumperButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            rearBumperButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableRearBumper[i].first; //SampleButton�� Text ����

            rearBumperButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[rearBumperIndex]); //�θ� ����
            rearBumperButtonTransform.localScale = Vector3.one; //ũ�� ������
            rearBumperButtonTransform.gameObject.name = DBController.dbController.mountableRearBumper[i].second; //�̸� ����
            rearBumperButtonTransform.GetComponent<RearBumperButton>().rearBumperName = DBController.dbController.mountableRearBumper[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            rearBumperButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }

    /* �׸� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateGrillButtons()
    {
        int grillIndex = (int)TypeOfParts.Grill; //�ε��� ����

        /* �ּ�ȭ�� GrillButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[grillIndex].childCount; ++i) //GrillContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[grillIndex].GetChild(i).gameObject; //GrillContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� GrillButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[grillIndex].childCount; ++i) //GrillContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[grillIndex].GetChild(i).gameObject; //GrillContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableGrill.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform grillButtonTransform = null; //GrillButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            grillButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[grillIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/grill/" + DBController.dbController.mountableGrill[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            grillButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            grillButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableGrill[i].first; //SampleButton�� Text ����

            grillButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[grillIndex]); //�θ� ����
            grillButtonTransform.localScale = Vector3.one; //ũ�� ������
            grillButtonTransform.gameObject.name = DBController.dbController.mountableGrill[i].second; //�̸� ����
            grillButtonTransform.GetComponent<GrillButton>().grillName = DBController.dbController.mountableGrill[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            grillButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            grillButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[grillIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/grill/" + DBController.dbController.mountableGrill[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            grillButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            grillButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableGrill[i].first; //SampleButton�� Text ����

            grillButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[grillIndex]); //�θ� ����
            grillButtonTransform.localScale = Vector3.one; //ũ�� ������
            grillButtonTransform.gameObject.name = DBController.dbController.mountableGrill[i].second; //�̸� ����
            grillButtonTransform.GetComponent<GrillButton>().grillName = DBController.dbController.mountableGrill[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            grillButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }

    /* ���� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateBonnetButtons()
    {
        int bonnetIndex = (int)TypeOfParts.Bonnet; //�ε��� ����

        /* �ּ�ȭ�� BonnetButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[bonnetIndex].childCount; ++i) //BonnetContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[bonnetIndex].GetChild(i).gameObject; //BonnetContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� BonnetButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[bonnetIndex].childCount; ++i) //BonnetContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[bonnetIndex].GetChild(i).gameObject; //BonnetContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableBonnet.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform bonnetButtonTransform = null; //BonnetButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            bonnetButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[bonnetIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/bonnet/" + DBController.dbController.mountableBonnet[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            bonnetButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            bonnetButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableBonnet[i].first; //SampleButton�� Text ����

            bonnetButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[bonnetIndex]); //�θ� ����
            bonnetButtonTransform.localScale = Vector3.one; //ũ�� ������
            bonnetButtonTransform.gameObject.name = DBController.dbController.mountableBonnet[i].second; //�̸� ����
            bonnetButtonTransform.GetComponent<BonnetButton>().bonnetName = DBController.dbController.mountableBonnet[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            bonnetButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            bonnetButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[bonnetIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/bonnet/" + DBController.dbController.mountableBonnet[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            bonnetButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            bonnetButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableBonnet[i].first; //SampleButton�� Text ����

            bonnetButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[bonnetIndex]); //�θ� ����
            bonnetButtonTransform.localScale = Vector3.one; //ũ�� ������
            bonnetButtonTransform.gameObject.name = DBController.dbController.mountableBonnet[i].second; //�̸� ����
            bonnetButtonTransform.GetComponent<BonnetButton>().bonnetName = DBController.dbController.mountableBonnet[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            bonnetButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }

    /* ����Ʈ �Ӵ� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateFrontFenderButtons()
    {
        int frontFenderIndex = (int)TypeOfParts.FrontFender; //�ε��� ����

        /* �ּ�ȭ�� FrontFenderButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[frontFenderIndex].childCount; ++i) //FrontFenderContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[frontFenderIndex].GetChild(i).gameObject; //FrontFenderContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� FrontFenderButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[frontFenderIndex].childCount; ++i) //FrontFenderContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[frontFenderIndex].GetChild(i).gameObject; //FrontFenderContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableFrontFender.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform frontFenderButtonTransform = null; //FrontFenderButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            frontFenderButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[frontFenderIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/frontfender/" + DBController.dbController.mountableFrontFender[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            frontFenderButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            frontFenderButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableFrontFender[i].first; //SampleButton�� Text ����

            frontFenderButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[frontFenderIndex]); //�θ� ����
            frontFenderButtonTransform.localScale = Vector3.one; //ũ�� ������
            frontFenderButtonTransform.gameObject.name = DBController.dbController.mountableFrontFender[i].second; //�̸� ����
            frontFenderButtonTransform.GetComponent<FrontFenderButton>().frontFenderName = DBController.dbController.mountableFrontFender[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            frontFenderButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            frontFenderButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[frontFenderIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/frontfender/" + DBController.dbController.mountableFrontFender[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            frontFenderButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            frontFenderButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableFrontFender[i].first; //SampleButton�� Text ����

            frontFenderButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[frontFenderIndex]); //�θ� ����
            frontFenderButtonTransform.localScale = Vector3.one; //ũ�� ������
            frontFenderButtonTransform.gameObject.name = DBController.dbController.mountableFrontFender[i].second; //�̸� ����
            frontFenderButtonTransform.GetComponent<FrontFenderButton>().frontFenderName = DBController.dbController.mountableFrontFender[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            frontFenderButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }

    /* ���� �Ӵ� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateRearFenderButtons()
    {
        int rearFenderIndex = (int)TypeOfParts.RearFender; //�ε��� ����

        /* �ּ�ȭ�� RearFenderButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[rearFenderIndex].childCount; ++i) //RearFenderContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[rearFenderIndex].GetChild(i).gameObject; //RearFenderContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� RearFenderButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[rearFenderIndex].childCount; ++i) //RearFenderContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[rearFenderIndex].GetChild(i).gameObject; //RearFenderContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableRearFender.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform rearFenderButtonTransform = null; //RearFenderButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            rearFenderButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[rearFenderIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/rearfender/" + DBController.dbController.mountableRearFender[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            rearFenderButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            rearFenderButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableRearFender[i].first; //SampleButton�� Text ����

            rearFenderButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[rearFenderIndex]); //�θ� ����
            rearFenderButtonTransform.localScale = Vector3.one; //ũ�� ������
            rearFenderButtonTransform.gameObject.name = DBController.dbController.mountableRearFender[i].second; //�̸� ����
            rearFenderButtonTransform.GetComponent<RearFenderButton>().rearFenderName = DBController.dbController.mountableRearFender[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            rearFenderButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            rearFenderButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[rearFenderIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/rearfender/" + DBController.dbController.mountableRearFender[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            rearFenderButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            rearFenderButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableRearFender[i].first; //SampleButton�� Text ����

            rearFenderButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[rearFenderIndex]); //�θ� ����
            rearFenderButtonTransform.localScale = Vector3.one; //ũ�� ������
            rearFenderButtonTransform.gameObject.name = DBController.dbController.mountableRearFender[i].second; //�̸� ����
            rearFenderButtonTransform.GetComponent<RearFenderButton>().rearFenderName = DBController.dbController.mountableRearFender[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            rearFenderButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }

    /* �� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateWheelButtons()
    {
        yield return StartCoroutine(DBController.dbController.GetMountableWheel(DBController.dbController.mountableWheelInch, DBController.dbController.mountableWheelWidthLimit));

        int wheelIndex = (int)TypeOfParts.Wheel; //�ε��� ����

        /* �ּ�ȭ�� WheelButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[wheelIndex].childCount; ++i) //WheelContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[wheelIndex].GetChild(i).gameObject; //WheelContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� WheelButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[wheelIndex].childCount; ++i) //WheelContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[wheelIndex].GetChild(i).gameObject; //WheelContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        yield return null; //���� �����ӱ��� ���

        for (int i = 0; i < DBController.dbController.mountableWheel.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            string[] wheelNameKo = DBController.dbController.mountableWheel[i].first.Split('-'); //�� �̸� Ko
            string[] wheelNameEng = DBController.dbController.mountableWheel[i].second.Split('-'); //�� �̸� Eng

            Transform wheelButtonTransform = null; //WheelButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite
            WheelButton wheelButton = null; //WheelButton ��ũ��Ʈ

            /* MinimizedButton */
            wheelButtonTransform = minimizedMainScrollViewContentRectTransforms[wheelIndex].Find(wheelNameEng[0]); //���� ������ ��ư���� Ž��
            if (wheelButtonTransform == null) //���� ��ư�� �������� ������
            {
                wheelButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[wheelIndex]).transform; //SampleButton ����

                if (partsSprite == null) //���� Sprite�� �������� ������
                {
                    using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/wheel/" + wheelNameEng[0] + ".png"); //�����κ��� �ؽ�ó ��û
                    yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                    try
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                        partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                    }
                    catch (Exception)
                    {
                        partsSprite = null;
                    }
                }

                wheelButtonTransform.GetChild(0).GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
                wheelButtonTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = wheelNameKo[0]; //SampleButton�� Text ����

                wheelButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[wheelIndex]); //�θ� ����
                wheelButtonTransform.localScale = Vector3.one; //ũ�� ������
                wheelButtonTransform.gameObject.name = wheelNameEng[0]; //�̸� ����
                wheelButtonTransform.GetComponent<WheelButton>().wheelName = wheelNameEng[0]; //Ŭ������ �� �ҷ��� ���� �̸��� ����
                wheelButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
            }

            wheelButton = wheelButtonTransform.GetComponent<WheelButton>(); //WheelButton ������Ʈ ����
            wheelButton.AddWheel(wheelNameEng[1], wheelNameEng[2]); //�� �߰�

            /* MaximizedButton */
            wheelButtonTransform = maximizedMainScrollViewContentRectTransforms[wheelIndex].Find(wheelNameEng[0]); //���� ������ ��ư���� Ž��
            if (wheelButtonTransform == null) //���� ��ư�� �������� ������
            {
                wheelButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[wheelIndex]).transform; //SampleButton ����

                if (partsSprite == null) //���� Sprite�� �������� ������
                {
                    using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/wheel/" + wheelNameEng[0] + ".png"); //�����κ��� �ؽ�ó ��û
                    yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                    try
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                        partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                    }
                    catch (Exception)
                    {
                        partsSprite = null;
                    }
                }

                wheelButtonTransform.GetChild(0).GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
                wheelButtonTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = wheelNameKo[0]; //SampleButton�� Text ����

                wheelButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[wheelIndex]); //�θ� ����
                wheelButtonTransform.localScale = Vector3.one; //ũ�� ������
                wheelButtonTransform.gameObject.name = wheelNameEng[0]; //�̸� ����
                wheelButtonTransform.GetComponent<WheelButton>().wheelName = wheelNameEng[0]; //Ŭ������ �� �ҷ��� ���� �̸��� ����
                wheelButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
            }

            wheelButton = wheelButtonTransform.GetComponent<WheelButton>(); //WheelButton ������Ʈ ����
            wheelButton.AddWheel(wheelNameEng[1], wheelNameEng[2]); //�� �߰�
        }
    }

    /* Ÿ�̾� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateTireButtons()
    {
        string[] wheelName = Static.selectedWheelName.Split('-'); //�� �̸� �и� �� ����
        yield return StartCoroutine(DBController.dbController.GetMountableTire(wheelName[2], wheelName[1]));

        int tireIndex = (int)TypeOfParts.Tire; //�ε��� ����

        /* �ּ�ȭ�� TireButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[tireIndex].childCount; ++i) //TireContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[tireIndex].GetChild(i).gameObject; //TireContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� TireButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[tireIndex].childCount; ++i) //TireContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[tireIndex].GetChild(i).gameObject; //TireContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        yield return null; //���� �����ӱ��� ���

        for (int i = 0; i < DBController.dbController.mountableTire.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            string[] tireNameKo = DBController.dbController.mountableTire[i].first.Split('-'); //Ÿ�̾� �̸� Ko
            string[] tireNameEng = DBController.dbController.mountableTire[i].second.Split('-'); //Ÿ�̾� �̸� Eng

            Transform tireButtonTransform = null; //TireButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite
            TireButton tireButton = null; //TireButton ��ũ��Ʈ

            /* MinimizedButton */
            tireButtonTransform = minimizedMainScrollViewContentRectTransforms[tireIndex].Find(tireNameEng[0]); //���� ������ ��ư���� Ž��
            if (tireButtonTransform == null) //���� ��ư�� �������� ������
            {
                tireButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[tireIndex]).transform; //SampleButton ����

                if (partsSprite == null) //���� Sprite�� �������� ������
                {
                    using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/tire/" + tireNameEng[0] + ".png"); //�����κ��� �ؽ�ó ��û
                    yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                    try
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                        partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                    }
                    catch (Exception)
                    {
                        partsSprite = null;
                    }
                }

                tireButtonTransform.GetChild(0).GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
                tireButtonTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tireNameKo[0]; //SampleButton�� Text ����

                tireButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[tireIndex]); //�θ� ����
                tireButtonTransform.localScale = Vector3.one; //ũ�� ������
                tireButtonTransform.gameObject.name = tireNameEng[0]; //�̸� ����
                tireButtonTransform.GetComponent<TireButton>().tireName = tireNameEng[0]; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
                tireButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
            }

            tireButton = tireButtonTransform.GetComponent<TireButton>(); //TireButton ������Ʈ ����
            tireButton.AddAspectRatio(tireNameEng[2]); //����� �߰�

            /* MaximizedButton */
            tireButtonTransform = maximizedMainScrollViewContentRectTransforms[tireIndex].Find(tireNameEng[0]); //���� ������ ��ư���� Ž��
            if (tireButtonTransform == null) //���� ��ư�� �������� ������
            {
                tireButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[tireIndex]).transform; //SampleButton ����

                if (partsSprite == null) //���� Sprite�� �������� ������
                {
                    using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/tire/" + tireNameEng[0] + ".png"); //�����κ��� �ؽ�ó ��û
                    yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                    try
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                        partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                    }
                    catch (Exception)
                    {
                        partsSprite = null;
                    }
                }

                tireButtonTransform.GetChild(0).GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
                tireButtonTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tireNameKo[0]; //SampleButton�� Text ����

                tireButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[tireIndex]); //�θ� ����
                tireButtonTransform.localScale = Vector3.one; //ũ�� ������
                tireButtonTransform.gameObject.name = tireNameEng[0]; //�̸� ����
                tireButtonTransform.GetComponent<TireButton>().tireName = tireNameEng[0]; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
                tireButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
            }

            tireButton = tireButtonTransform.GetComponent<TireButton>(); //TireButton ������Ʈ ����
            tireButton.AddAspectRatio(tireNameEng[2]); //����� �߰�
        }

        Resources.UnloadUnusedAssets(); //�޸� ���� ����
    }

    /* �극��ũ ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateBrakeButtons()
    {
        int brakeIndex = (int)TypeOfParts.Brake; //�ε��� ����

        /* �ּ�ȭ�� BrakeButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[brakeIndex].childCount; ++i) //BrakeContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[brakeIndex].GetChild(i).gameObject; //BrakeContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� BrakeButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[brakeIndex].childCount; ++i) //BrakeContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[brakeIndex].GetChild(i).gameObject; //BrakeContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableBrake.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform brakeButtonTransform = null; //BrakeButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            brakeButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[brakeIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/brake/" + DBController.dbController.mountableBrake[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            brakeButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            brakeButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableBrake[i].first; //SampleButton�� Text ����

            brakeButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[brakeIndex]); //�θ� ����
            brakeButtonTransform.localScale = Vector3.one; //ũ�� ������
            brakeButtonTransform.gameObject.name = DBController.dbController.mountableBrake[i].second; //�̸� ����
            brakeButtonTransform.GetComponent<BrakeButton>().brakeName = DBController.dbController.mountableBrake[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            brakeButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            brakeButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[brakeIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/brake/" + DBController.dbController.mountableBrake[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            brakeButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            brakeButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableBrake[i].first; //SampleButton�� Text ����

            brakeButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[brakeIndex]); //�θ� ����
            brakeButtonTransform.localScale = Vector3.one; //ũ�� ������
            brakeButtonTransform.gameObject.name = DBController.dbController.mountableBrake[i].second; //�̸� ����
            brakeButtonTransform.GetComponent<BrakeButton>().brakeName = DBController.dbController.mountableBrake[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            brakeButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }

    /* �����Ϸ� ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateSpoilerButtons()
    {
        int spoilerIndex = (int)TypeOfParts.Spoiler; //�ε��� ����

        /* �ּ�ȭ�� SpoilerButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[spoilerIndex].childCount; ++i) //SpoilerContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[spoilerIndex].GetChild(i).gameObject; //SpoilerContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� SpoilerButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[spoilerIndex].childCount; ++i) //SpoilerContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[spoilerIndex].GetChild(i).gameObject; //SpoilerContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableSpoiler.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform spoilerButtonTransform = null; //SpoilerButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            spoilerButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[spoilerIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/spoiler/" + DBController.dbController.mountableSpoiler[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            spoilerButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            spoilerButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableSpoiler[i].first; //SampleButton�� Text ����

            spoilerButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[spoilerIndex]); //�θ� ����
            spoilerButtonTransform.localScale = Vector3.one; //ũ�� ������
            spoilerButtonTransform.gameObject.name = DBController.dbController.mountableSpoiler[i].second; //�̸� ����
            spoilerButtonTransform.GetComponent<SpoilerButton>().spoilerName = DBController.dbController.mountableSpoiler[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            spoilerButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            spoilerButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[spoilerIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/spoiler/" + DBController.dbController.mountableSpoiler[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            spoilerButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            spoilerButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableSpoiler[i].first; //SampleButton�� Text ����

            spoilerButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[spoilerIndex]); //�θ� ����
            spoilerButtonTransform.localScale = Vector3.one; //ũ�� ������
            spoilerButtonTransform.gameObject.name = DBController.dbController.mountableSpoiler[i].second; //�̸� ����
            spoilerButtonTransform.GetComponent<SpoilerButton>().spoilerName = DBController.dbController.mountableSpoiler[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            spoilerButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }

    /* ��ⱸ ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    public IEnumerator CreateExhaustPipeButtons()
    {
        int exhaustPipeIndex = (int)TypeOfParts.ExhaustPipe; //�ε��� ����

        /* �ּ�ȭ�� ExhaustPipeButton ��� ���� */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[exhaustPipeIndex].childCount; ++i) //ExhaustPipeContent�� ��� �ڽ� Ž��
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[exhaustPipeIndex].GetChild(i).gameObject; //ExhaustPipeContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        /* �ִ�ȭ�� ExhaustPipeButton ��� ���� */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[exhaustPipeIndex].childCount; ++i) //ExhaustPipeContent�� ��� �ڽ� Ž��
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[exhaustPipeIndex].GetChild(i).gameObject; //ExhaustPipeContent�� ��ư�� ����
            if (button.activeSelf) Destroy(button); //��ư�� Ȱ��ȭ �Ǿ������� ����
        }

        for (int i = 0; i < DBController.dbController.mountableExhaustPipe.Length; ++i) //���� ���� ������ŭ �ݺ�
        {
            Transform exhaustPipeButtonTransform = null; //ExhaustPipeButton�� Transform ������Ʈ
            Sprite partsSprite = null; //���� Sprite

            /* MinimizedButton */
            exhaustPipeButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[exhaustPipeIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/exhaustpipe/" + DBController.dbController.mountableExhaustPipe[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            exhaustPipeButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            exhaustPipeButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableExhaustPipe[i].first; //SampleButton�� Text ����

            exhaustPipeButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[exhaustPipeIndex]); //�θ� ����
            exhaustPipeButtonTransform.localScale = Vector3.one; //ũ�� ������
            exhaustPipeButtonTransform.gameObject.name = DBController.dbController.mountableExhaustPipe[i].second; //�̸� ����
            exhaustPipeButtonTransform.GetComponent<ExhaustPipeButton>().exhaustPipeName = DBController.dbController.mountableExhaustPipe[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            exhaustPipeButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ

            /* MaximizedButton */
            exhaustPipeButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[exhaustPipeIndex]).transform; //SampleButton ����

            if (partsSprite == null) //���� Sprite�� �������� ������
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/exhaustpipe/" + DBController.dbController.mountableExhaustPipe[i].second + ".png"); //�����κ��� �ؽ�ó ��û
                yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            exhaustPipeButtonTransform.GetComponent<Image>().sprite = partsSprite; //���� Sprite ����
            exhaustPipeButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableExhaustPipe[i].first; //SampleButton�� Text ����

            exhaustPipeButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[exhaustPipeIndex]); //�θ� ����
            exhaustPipeButtonTransform.localScale = Vector3.one; //ũ�� ������
            exhaustPipeButtonTransform.gameObject.name = DBController.dbController.mountableExhaustPipe[i].second; //�̸� ����
            exhaustPipeButtonTransform.GetComponent<ExhaustPipeButton>().exhaustPipeName = DBController.dbController.mountableExhaustPipe[i].second; //Ŭ������ �� �ҷ��� Ÿ�̾��� �̸��� ����
            exhaustPipeButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
        }
    }
    #endregion

    #region LoadParts
    /* ������ �ҷ����� �ڷ�ƾ �Լ� */
    public IEnumerator LoadParts(TypeOfParts loadedTypeOfParts, string partsName, bool caching, bool unloadUnusedAssets)
    {
        if (caching) Notification.notification.EnableLayout(Notification.Layout.LoadParts); //ĳ���̸� ���̾ƿ� Ȱ��ȭ

        if (TypeOfParts.FrontBumper == loadedTypeOfParts) //����Ʈ ����
        {
            if (SuspensionController.suspensionController.movePartsTransforms.frontBumper != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.frontBumper.gameObject); //���� ���� ����
            CarPaint.carPaint.carEntireMaterials[1] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[9] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[10] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[11] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("frontbumper", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.frontBumper.position, SuspensionController.suspensionController.carBodyPartsTransforms.frontBumper.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.FrontBumper], (int)TypeOfParts.FrontBumper)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.FrontBumper]) //������ �ε�Ǹ�
            {
                Static.selectedFrontBumperName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.FrontBumper); //������ ���� Transform ĳ��
                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Entire); //���� ���� ĳ��
                    CarPaint.carPaint.carEntireSurface = CarPaint.carPaint.carEntireSurface; //���� ���� ���� ����
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedFrontBumperName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.RearBumper == loadedTypeOfParts) //���� ����
        {
            if (SuspensionController.suspensionController.movePartsTransforms.rearBumper != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.rearBumper.gameObject); //���� ���� ����
            CarPaint.carPaint.carEntireMaterials[2] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("rearbumper", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.rearBumper.position, SuspensionController.suspensionController.carBodyPartsTransforms.rearBumper.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.RearBumper], (int)TypeOfParts.RearBumper)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.RearBumper]) //������ �ε�Ǹ�
            {
                Static.selectedRearBumperName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.RearBumper); //������ ���� Transform ĳ��
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedRearBumperName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.Grill == loadedTypeOfParts) //�׸�
        {
            if (SuspensionController.suspensionController.movePartsTransforms.grill != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.grill.gameObject); //���� ���� ����
            CarPaint.carPaint.carChromeDeleteMaterials[3] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[4] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[5] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("grill", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.grill.position, SuspensionController.suspensionController.carBodyPartsTransforms.grill.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Grill], (int)TypeOfParts.Grill)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Grill]) //������ �ε�Ǹ�
            {
                Static.selectedGrillName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Grill); //������ ���� Transform ĳ��
                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.ChromeDelete); //���� ���� ĳ��
                    CarPaint.carPaint.carChromeDeleteSurface = CarPaint.carPaint.carChromeDeleteSurface; //���� ���� ���� ����
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedGrillName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.Bonnet == loadedTypeOfParts) //����
        {
            if (SuspensionController.suspensionController.movePartsTransforms.bonnet != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.bonnet.gameObject); //���� ���� ����
            CarPaint.carPaint.carBonnetMaterials[0] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[6] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[7] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carChromeDeleteMaterials[8] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("bonnet", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.bonnet.position, SuspensionController.suspensionController.carBodyPartsTransforms.bonnet.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Bonnet], (int)TypeOfParts.Bonnet)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Bonnet]) //������ �ε�Ǹ�
            {
                Static.selectedBonnetName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Bonnet); //������ ���� Transform ĳ��

                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Bonnet); //���� ���� ĳ��
                    CarPaint.carPaint.carBonnetSurface = CarPaint.carPaint.carBonnetSurface; //���� ���� ���� ����

                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.ChromeDelete); //���� ���� ĳ��
                    CarPaint.carPaint.carChromeDeleteSurface = CarPaint.carPaint.carChromeDeleteSurface; //���� ���� ���� ����
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedBonnetName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.FrontFender == loadedTypeOfParts) //����Ʈ �Ӵ�
        {
            if (SuspensionController.suspensionController.movePartsTransforms.frontFender_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.frontFender_Left.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.frontFender_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.frontFender_Right.gameObject); //���� ���� ����

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("frontfender", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Left.position, SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Left.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.FrontFender], (int)TypeOfParts.FrontFender)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.FrontFender]) //������ �ε�Ǹ�
            {
                GameObject parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.FrontFender],
                SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Right.position,
                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Right.localEulerAngles),
                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.FrontFender]); //���� ����
                parts.transform.localScale = new Vector3(-1f, parts.transform.localScale.y, parts.transform.localScale.z); //�¿� ����
                parts.name = partsName; //���� �̸� ����

                Static.selectedFrontFenderName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.FrontFender); //������ ���� Transform ĳ��
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedFrontFenderName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.RearFender == loadedTypeOfParts) //���� �Ӵ�
        {
            if (SuspensionController.suspensionController.movePartsTransforms.rearFender_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.rearFender_Left.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.rearFender_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.rearFender_Right.gameObject); //���� ���� ����

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("rearfender", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Left.position, SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Left.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.RearFender], (int)TypeOfParts.RearFender)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.RearFender]) //������ �ε�Ǹ�
            {
                GameObject parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.RearFender],
                SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Right.position,
                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Right.localEulerAngles),
                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.RearFender]); //���� ����
                parts.transform.localScale = new Vector3(-1f, parts.transform.localScale.y, parts.transform.localScale.z); //�¿� ����
                parts.name = partsName; //���� �̸� ����

                Static.selectedRearFenderName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.RearFender); //������ ���� Transform ĳ��
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedRearFenderName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.Wheel == loadedTypeOfParts) //��
        {
            if (SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right.gameObject); //���� ���� ����
            CarPaint.carPaint.carWheelMaterials[0] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carWheelMaterials[1] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carWheelMaterials[2] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carWheelMaterials[3] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carWheelMaterials[4] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carWheelMaterials[5] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carWheelMaterials[6] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carWheelMaterials[7] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("wheel", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Left.position, SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Left.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Wheel], (int)TypeOfParts.Wheel)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Wheel]) //������ �ε�Ǹ�
            {
                GameObject parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Wheel],
                SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Right.position,
                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Right.localEulerAngles),
                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Wheel]); //���� ����
                parts.name = partsName; //���� �̸� ����

                parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Wheel],
                                SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Left.position,
                                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Left.localEulerAngles),
                                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Wheel]); //���� ����
                parts.name = partsName; //���� �̸� ����

                parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Wheel],
                                SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Right.position,
                                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Right.localEulerAngles),
                                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Wheel]); //���� ����
                parts.name = partsName; //���� �̸� ����

                Static.selectedWheelName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Wheel); //������ ���� Transform ĳ��
                    SuspensionController.suspensionController.SyncMovePartsTransforms(); //���� ��ġ ����ȭ
                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Wheel); //���� ���� ĳ��
                    CarPaint.carPaint.carWheelSurface = CarPaint.carPaint.carWheelSurface; //���� ���� ���� ����
                }

                yield return StartCoroutine(CreateTireButtons()); //Ÿ�̾� ��ư ����
            }
            else //������ �ε���� ������
            {
                Static.selectedWheelName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.Tire == loadedTypeOfParts) //Ÿ�̾�
        {
            if (SuspensionController.suspensionController.movePartsTransforms.tire_Front_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.tire_Front_Left.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.tire_Front_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.tire_Front_Right.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Left.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Right.gameObject); //���� ���� ����

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("tire", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Left.position, SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Left.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Tire], (int)TypeOfParts.Tire)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Tire]) //������ �ε�Ǹ�
            {
                GameObject parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Tire],
                SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Right.position,
                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Right.localEulerAngles),
                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Tire]); //���� ����
                parts.name = partsName; //���� �̸� ����

                parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Tire],
                                SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Left.position,
                                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Left.localEulerAngles),
                                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Tire]); //���� ����
                parts.name = partsName; //���� �̸� ����

                parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Tire],
                                SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Right.position,
                                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Right.localEulerAngles),
                                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Tire]); //���� ����
                parts.name = partsName; //���� �̸� ����

                Static.selectedTireName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Tire); //������ ���� Transform ĳ��
                    SuspensionController.suspensionController.SyncMovePartsTransforms(); //���� ��ġ ����ȭ
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedTireName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.Brake == loadedTypeOfParts) //�극��ũ
        {
            if (SuspensionController.suspensionController.movePartsTransforms.brake_Front_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.brake_Front_Left.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.brake_Front_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.brake_Front_Right.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Left.gameObject); //���� ���� ����
            if (SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Right.gameObject); //���� ���� ����
            CarPaint.carPaint.carBrakeCaliperMaterials[0] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carBrakeCaliperMaterials[1] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carBrakeCaliperMaterials[2] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)
            CarPaint.carPaint.carBrakeCaliperMaterials[3] = null; //���� ���� ĳ�� ����(CarPaint ��ũ��Ʈ���� �����ϴ� �κ�)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("brake", partsName), Vector3.zero, Vector3.zero, null, (int)TypeOfParts.Brake)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Brake]) //������ �ε�Ǹ�
            {
                Transform brakeFrontLeft = BundleController.bundleController.loadedBundles[(int)TypeOfParts.Brake].transform.GetChild(0); //BrakeFrontLeft�� Transform ������Ʈ
                Transform brakeFrontRight = Instantiate(brakeFrontLeft).transform; //BrakeFrontRight�� Transform ������Ʈ
                Transform brakeRearLeft = BundleController.bundleController.loadedBundles[(int)TypeOfParts.Brake].transform.GetChild(1); //BrakeRearLeft�� Transform ������Ʈ
                Transform brakeRearRight = Instantiate(brakeRearLeft).transform; //BrakeRearRight�� Transform ������Ʈ

                brakeFrontLeft.SetPositionAndRotation(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Left.position, Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Left.localEulerAngles)); //��ġ �� ȸ�� ����
                brakeFrontLeft.SetParent(CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Brake]); //�θ� ����
                brakeFrontLeft.name = partsName; //���� �̸� ����

                brakeFrontRight.SetPositionAndRotation(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Right.position, Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Right.localEulerAngles)); //��ġ �� ȸ�� ����
                brakeFrontRight.SetParent(CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Brake]); //�θ� ����
                brakeFrontRight.name = partsName; //���� �̸� ����

                brakeRearLeft.SetPositionAndRotation(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Left.position, Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Left.localEulerAngles)); //��ġ �� ȸ�� ����
                brakeRearLeft.SetParent(CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Brake]); //�θ� ����
                brakeRearLeft.name = partsName; //���� �̸� ����

                brakeRearRight.SetPositionAndRotation(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Right.position, Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Right.localEulerAngles)); //��ġ �� ȸ�� ����
                brakeRearRight.SetParent(CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Brake]); //�θ� ����
                brakeRearRight.name = partsName; //���� �̸� ����

                Static.selectedBrakeName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Brake); //������ ���� Transform ĳ��
                    SuspensionController.suspensionController.SyncMovePartsTransforms(); //���� ��ġ ����ȭ
                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.BrakeCaliper); //���� ���� ĳ��
                    CarPaint.carPaint.carBrakeCaliperSurface = CarPaint.carPaint.carBrakeCaliperSurface; //���� ���� ���� ����
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedBrakeName = null; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }
        else if (TypeOfParts.Spoiler == loadedTypeOfParts) //�����Ϸ�
        {
            if (SuspensionController.suspensionController.movePartsTransforms.spoiler != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.spoiler.gameObject); //���� ���� ����

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("spoiler", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.spoiler.position, SuspensionController.suspensionController.carBodyPartsTransforms.spoiler.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Spoiler], (int)TypeOfParts.Spoiler)); //���� ���� �ε�

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Spoiler]) //������ �ε�Ǹ�
            {
                Static.selectedSpoilerName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Spoiler); //������ ���� Transform ĳ��
                }
            }
            else //������ �ε���� ������
            {
                Static.selectedSpoilerName = partsName; //���õ� ���� �̸� ����

                if (caching) //ĳ���̸�
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //���̾ƿ� Ȱ��ȭ
                }
            }
        }

        if (unloadUnusedAssets) Resources.UnloadUnusedAssets(); //�޸� ���� ����

        if (caching) Notification.notification.DisableLayout(Notification.Layout.LoadParts); //ĳ���̸� ���̾ƿ� ��Ȱ��ȭ
    }
    #endregion
}