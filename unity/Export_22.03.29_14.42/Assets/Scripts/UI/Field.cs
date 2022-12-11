using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;

public class Field : MonoBehaviour
{
    [Header("Static")]
    public static Field field; //전역 참조 변수

    [Header("Field Panel")]
    public Canvas fieldPanelCanvas; //FieldPanel의 Canvas 컴포넌트

    [Header("Field Buttons")]
    public Transform fieldTransform; //Field의 Transform 컴포넌트
    public RectTransform contentTransform; //Content의 Transform 컴포넌트
    public GameObject sampleButton; //SampleButton 오브젝트

    private void Awake()
    {
        field = this;
    }

    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) //인터넷 연결이 되어있지 않으면
        {
            Notification.notification.EnableLayout(Notification.Layout.InternetDisconnection);  //레이아웃 활성화
        }
        else //인터넷 연결이 되어있으면
        {
            StartCoroutine(CreateFieldButtons());
        }
    }

    /* FieldPanel을 활성화/비활성화 하는 함수 */
    public void ActivateFieldPanel(bool state)
    {
        Home.home.ActivateHome(!state);  //Home 활성화/비활성화
        fieldPanelCanvas.enabled = state; //FieldPanel의 Canvas 컴포넌트 활성화/비활성화
    }

    /* Field 버튼들을 생성하는 코루틴 함수 */
    private IEnumerator CreateFieldButtons()
    {
        yield return StartCoroutine(DBController.dbController.GetFieldInfo()); //Field 정보들을 로드

        for (int i = 0; i < DBController.dbController.fieldInfos.Length; ++i) //Field 개수만큼 반복
        {
            using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/field/" + DBController.dbController.fieldInfos[i] + ".png"); //서버로부터 텍스처 요청
            yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기

            try
            {
                Transform sampleButtonTransform = Instantiate(sampleButton).transform; //SampleButton 생성

                Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                sampleButtonTransform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용

                sampleButtonTransform.SetParent(contentTransform); //부모를 Content로 변경
                sampleButtonTransform.localScale = Vector3.one; //크기 재조절
                sampleButtonTransform.gameObject.name = DBController.dbController.fieldInfos[i]; //이름 변경
                sampleButtonTransform.GetChild(0).GetComponent<DynamicButton.FieldButton>().fieldInfo = DBController.dbController.fieldInfos[i]; //클릭했을 때 불러올 Field의 이름을 지정
                sampleButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
            }
            catch (Exception) { }
        }
    }

    /* Field를 불러오는 함수 */
    public IEnumerator LoadField(string fieldInfo)
    {
        Notification.notification.EnableLayout(Notification.Layout.LoadField); //레이아웃 활성화

        if (BundleController.bundleController.loadedBundles[12] != null) Destroy(BundleController.bundleController.loadedBundles[12]); //기존 Field 제거

        yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("field", fieldInfo), Vector3.zero, Vector3.zero, fieldTransform, 12)); //번들 로드

        if (BundleController.bundleController.loadedBundles[12]) //번들이 로드되면
        {
            Resources.UnloadUnusedAssets(); //메모리 누수 방지
        }
        else //번들이 로드되지 않으면
        {
            Notification.notification.EnableLayout(Notification.Layout.FailureLoadField); //레이아웃 활성화
        }

        Notification.notification.DisableLayout(Notification.Layout.LoadField); //레이아웃 비활성화
    }
}