using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;

public class Field : MonoBehaviour
{
    [Header("Static")]
    public static Field field; //���� ���� ����

    [Header("Field Panel")]
    public Canvas fieldPanelCanvas; //FieldPanel�� Canvas ������Ʈ

    [Header("Field Buttons")]
    public Transform fieldTransform; //Field�� Transform ������Ʈ
    public RectTransform contentTransform; //Content�� Transform ������Ʈ
    public GameObject sampleButton; //SampleButton ������Ʈ

    private void Awake()
    {
        field = this;
    }

    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) //���ͳ� ������ �Ǿ����� ������
        {
            Notification.notification.EnableLayout(Notification.Layout.InternetDisconnection);  //���̾ƿ� Ȱ��ȭ
        }
        else //���ͳ� ������ �Ǿ�������
        {
            StartCoroutine(CreateFieldButtons());
        }
    }

    /* FieldPanel�� Ȱ��ȭ/��Ȱ��ȭ �ϴ� �Լ� */
    public void ActivateFieldPanel(bool state)
    {
        Home.home.ActivateHome(!state);  //Home Ȱ��ȭ/��Ȱ��ȭ
        fieldPanelCanvas.enabled = state; //FieldPanel�� Canvas ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
    }

    /* Field ��ư���� �����ϴ� �ڷ�ƾ �Լ� */
    private IEnumerator CreateFieldButtons()
    {
        yield return StartCoroutine(DBController.dbController.GetFieldInfo()); //Field �������� �ε�

        for (int i = 0; i < DBController.dbController.fieldInfos.Length; ++i) //Field ������ŭ �ݺ�
        {
            using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/field/" + DBController.dbController.fieldInfos[i] + ".png"); //�����κ��� �ؽ�ó ��û
            yield return unityWebRequest.SendWebRequest(); //��û�� ���� ������ ���

            try
            {
                Transform sampleButtonTransform = Instantiate(sampleButton).transform; //SampleButton ����

                Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //��û���κ��� �ؽ�ó ����
                sampleButtonTransform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //�ؽ�ó�� Sprite ����

                sampleButtonTransform.SetParent(contentTransform); //�θ� Content�� ����
                sampleButtonTransform.localScale = Vector3.one; //ũ�� ������
                sampleButtonTransform.gameObject.name = DBController.dbController.fieldInfos[i]; //�̸� ����
                sampleButtonTransform.GetChild(0).GetComponent<DynamicButton.FieldButton>().fieldInfo = DBController.dbController.fieldInfos[i]; //Ŭ������ �� �ҷ��� Field�� �̸��� ����
                sampleButtonTransform.gameObject.SetActive(true); //������Ʈ Ȱ��ȭ
            }
            catch (Exception) { }
        }
    }

    /* Field�� �ҷ����� �Լ� */
    public IEnumerator LoadField(string fieldInfo)
    {
        Notification.notification.EnableLayout(Notification.Layout.LoadField); //���̾ƿ� Ȱ��ȭ

        if (BundleController.bundleController.loadedBundles[12] != null) Destroy(BundleController.bundleController.loadedBundles[12]); //���� Field ����

        yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("field", fieldInfo), Vector3.zero, Vector3.zero, fieldTransform, 12)); //���� �ε�

        if (BundleController.bundleController.loadedBundles[12]) //������ �ε�Ǹ�
        {
            Resources.UnloadUnusedAssets(); //�޸� ���� ����
        }
        else //������ �ε���� ������
        {
            Notification.notification.EnableLayout(Notification.Layout.FailureLoadField); //���̾ƿ� Ȱ��ȭ
        }

        Notification.notification.DisableLayout(Notification.Layout.LoadField); //���̾ƿ� ��Ȱ��ȭ
    }
}