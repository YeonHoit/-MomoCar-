using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using TMPro;

public class PartsSelect : MonoBehaviour
{
    [Header("Static")]
    public static PartsSelect partsSelect; //전역 참조 변수

    [Header("Parts Select Panels")]
    public Canvas partsSelectPanelsCanvas; //PartsSelectPanels의 Canvas 컴포넌트

    [Header("Content Select")]
    public RectTransform[] minimizedMainScrollViewContentRectTransforms; //최소화된 MainScrollView Content의 RectTransform 컴포넌트들
    public TextMeshProUGUI[] minimizedContentSelectButtonTMP; //최소화된 ContentSelectButton의 TMP 컴포넌트들

    public RectTransform[] maximizedMainScrollViewContentRectTransforms; //최대화된 MainScrollView Content의 RectTransform 컴포넌트들
    public TextMeshProUGUI[] maximizedContentSelectButtonTMP; //최대화된 ContentSelectButton의 TMP 컴포넌트들

    private Color32 contentSelectButtonActivationColor = new Color32(255, 28, 86, 255); //ContentSelectButton의 활성화 색상
    private Color32 contentSelectButtonDeactivationColor = Color.white; //ContentSelectButton의 비활성화 색상

    public ScrollRect minimizedMainScrollViewScrollRect; //최소화된 MainScrollView의 ScrollRect 컴포넌트
    public ScrollRect maximizedMainScrollViewScrollRect; //최대화된 MainScrollView의 ScrollRect 컴포넌트

    [Header("Minimize/Maximize")]
    public GameObject partsSelectMinimizedPanel; //PartsSelectMinimizedPanel 오브젝트
    public GameObject partsSelectMaximizedPanel; //PartsSelectMaximizedPanel 오브젝트

    [Header("Create Button")]
    public GameObject[] minimizedMainScrollViewSampleButtons; //최소화된 MainScrollView의 SampleButton 오브젝트들
    public GameObject[] maximizedMainScrollViewSampleButtons; //최대화된 MainScrollView의 SampleButton 오브젝트들

    private void Awake()
    {
        partsSelect = this; //전역 참조 변수 지정
    }

    private void Start()
    {
        OnClickContentSelectButton(0);
        MinimizePartsSelectPanel(true);
    }

    #region Interface
    /* PartsSelectPanels를 활성화/비활성화 하는 함수 */
    public void ActivatePartsSelectPanels(bool state)
    {
        Home.home.ActivateHome(!state); //홈 활성화/비활성화
        partsSelectPanelsCanvas.enabled = state; //PartsSelectPanels의 Canvas 컴포넌트 활성화/비활성화
        MainCamera.mainCamera.SetCurrentPos(CameraPosition.Center); //카메라 위치 변경
    }

    /* PartsSelectPanel을 최소화/최대화 하는 함수 */
    public void MinimizePartsSelectPanel(bool state)
    {
        partsSelectMinimizedPanel.SetActive(state); //PartsSelectMinimizedPanel 활성화/비활성화
        partsSelectMaximizedPanel.SetActive(!state); //PartsSelectMaximizedPanel 활성화/비활성화
    }

    /* Content를 선택하는 버튼을 클릭했을 때 실행되는 함수 */
    public void OnClickContentSelectButton(int index)
    {
        for (int i = 0; i < Utility.enumTypeOfPartsLength; ++i) //파츠 개수만큼 반복
        {
            minimizedContentSelectButtonTMP[i].color = contentSelectButtonDeactivationColor; //모든 최소화된 Content Select Button을 비활성화 색상으로 변경
            minimizedMainScrollViewContentRectTransforms[i].gameObject.SetActive(false); //모든 최소화된 MainScrollView Content를 비활성화

            maximizedContentSelectButtonTMP[i].color = contentSelectButtonDeactivationColor; //모든 최대화된 Content Select Button을 비활성화 색상으로 변경
            maximizedMainScrollViewContentRectTransforms[i].gameObject.SetActive(false); //모든 최대화된 MainScrollView Content를 비활성화
        }

        minimizedContentSelectButtonTMP[index].color = contentSelectButtonActivationColor; //선택 최소화된 Content Select Button을 활성화 색상으로 변경
        minimizedMainScrollViewContentRectTransforms[index].gameObject.SetActive(true); //선택 최소화된 MainScrollView Content를 활성화
        minimizedMainScrollViewScrollRect.content = minimizedMainScrollViewContentRectTransforms[index]; //선택 최소화된 MainScrollView Content를 Content로 지정

        maximizedContentSelectButtonTMP[index].color = contentSelectButtonActivationColor; //선택 최대화된 Content Select Button을 활성화 색상으로 변경
        maximizedMainScrollViewContentRectTransforms[index].gameObject.SetActive(true); //선택 최대화된 MainScrollView Content를 활성화
        maximizedMainScrollViewScrollRect.content = maximizedMainScrollViewContentRectTransforms[index]; //선택 최대화된 MainScrollView Content를 Content로 지정

        if (partsSelectPanelsCanvas.enabled) MainCamera.mainCamera.SetCurrentPos((CameraPosition)index); //카메라 위치 변경
    }
    #endregion

    #region CreatePartsButton
    /* 프론트 범퍼 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateFrontBumperButtons()
    {
        int frontBumperIndex = (int)TypeOfParts.FrontBumper; //인덱스 저장

        /* 최소화된 FrontBumperButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[frontBumperIndex].childCount; ++i) //FrontBumperContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[frontBumperIndex].GetChild(i).gameObject; //FrontBumperContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 FrontBumperButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[frontBumperIndex].childCount; ++i) //FrontBumperContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[frontBumperIndex].GetChild(i).gameObject; //FrontBumperContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableFrontBumper.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform frontBumperButtonTransform = null; //FrontBumperButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            frontBumperButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[frontBumperIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/frontbumper/" + DBController.dbController.mountableFrontBumper[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            frontBumperButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            frontBumperButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableFrontBumper[i].first; //SampleButton의 Text 지정

            frontBumperButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[frontBumperIndex]); //부모 변경
            frontBumperButtonTransform.localScale = Vector3.one; //크기 재조절
            frontBumperButtonTransform.gameObject.name = DBController.dbController.mountableFrontBumper[i].second; //이름 변경
            frontBumperButtonTransform.GetComponent<FrontBumperButton>().frontBumperName = DBController.dbController.mountableFrontBumper[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            frontBumperButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            frontBumperButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[frontBumperIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/frontbumper/" + DBController.dbController.mountableFrontBumper[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            frontBumperButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            frontBumperButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableFrontBumper[i].first; //SampleButton의 Text 지정

            frontBumperButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[frontBumperIndex]); //부모 변경
            frontBumperButtonTransform.localScale = Vector3.one; //크기 재조절
            frontBumperButtonTransform.gameObject.name = DBController.dbController.mountableFrontBumper[i].second; //이름 변경
            frontBumperButtonTransform.GetComponent<FrontBumperButton>().frontBumperName = DBController.dbController.mountableFrontBumper[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            frontBumperButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }

    /* 리어 범퍼 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateRearBumperButtons()
    {
        int rearBumperIndex = (int)TypeOfParts.RearBumper; //인덱스 저장

        /* 최소화된 RearBumperButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[rearBumperIndex].childCount; ++i) //RearBumperContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[rearBumperIndex].GetChild(i).gameObject; //RearBumperContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 RearBumperButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[rearBumperIndex].childCount; ++i) //RearBumperContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[rearBumperIndex].GetChild(i).gameObject; //RearBumperContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableRearBumper.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform rearBumperButtonTransform = null; //RearBumperButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            rearBumperButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[rearBumperIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/rearbumper/" + DBController.dbController.mountableRearBumper[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            rearBumperButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            rearBumperButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableRearBumper[i].first; //SampleButton의 Text 지정

            rearBumperButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[rearBumperIndex]); //부모 변경
            rearBumperButtonTransform.localScale = Vector3.one; //크기 재조절
            rearBumperButtonTransform.gameObject.name = DBController.dbController.mountableRearBumper[i].second; //이름 변경
            rearBumperButtonTransform.GetComponent<RearBumperButton>().rearBumperName = DBController.dbController.mountableRearBumper[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            rearBumperButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            rearBumperButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[rearBumperIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/rearbumper/" + DBController.dbController.mountableRearBumper[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            rearBumperButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            rearBumperButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableRearBumper[i].first; //SampleButton의 Text 지정

            rearBumperButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[rearBumperIndex]); //부모 변경
            rearBumperButtonTransform.localScale = Vector3.one; //크기 재조절
            rearBumperButtonTransform.gameObject.name = DBController.dbController.mountableRearBumper[i].second; //이름 변경
            rearBumperButtonTransform.GetComponent<RearBumperButton>().rearBumperName = DBController.dbController.mountableRearBumper[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            rearBumperButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }

    /* 그릴 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateGrillButtons()
    {
        int grillIndex = (int)TypeOfParts.Grill; //인덱스 저장

        /* 최소화된 GrillButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[grillIndex].childCount; ++i) //GrillContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[grillIndex].GetChild(i).gameObject; //GrillContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 GrillButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[grillIndex].childCount; ++i) //GrillContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[grillIndex].GetChild(i).gameObject; //GrillContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableGrill.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform grillButtonTransform = null; //GrillButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            grillButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[grillIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/grill/" + DBController.dbController.mountableGrill[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            grillButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            grillButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableGrill[i].first; //SampleButton의 Text 지정

            grillButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[grillIndex]); //부모 변경
            grillButtonTransform.localScale = Vector3.one; //크기 재조절
            grillButtonTransform.gameObject.name = DBController.dbController.mountableGrill[i].second; //이름 변경
            grillButtonTransform.GetComponent<GrillButton>().grillName = DBController.dbController.mountableGrill[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            grillButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            grillButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[grillIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/grill/" + DBController.dbController.mountableGrill[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            grillButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            grillButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableGrill[i].first; //SampleButton의 Text 지정

            grillButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[grillIndex]); //부모 변경
            grillButtonTransform.localScale = Vector3.one; //크기 재조절
            grillButtonTransform.gameObject.name = DBController.dbController.mountableGrill[i].second; //이름 변경
            grillButtonTransform.GetComponent<GrillButton>().grillName = DBController.dbController.mountableGrill[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            grillButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }

    /* 본넷 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateBonnetButtons()
    {
        int bonnetIndex = (int)TypeOfParts.Bonnet; //인덱스 저장

        /* 최소화된 BonnetButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[bonnetIndex].childCount; ++i) //BonnetContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[bonnetIndex].GetChild(i).gameObject; //BonnetContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 BonnetButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[bonnetIndex].childCount; ++i) //BonnetContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[bonnetIndex].GetChild(i).gameObject; //BonnetContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableBonnet.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform bonnetButtonTransform = null; //BonnetButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            bonnetButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[bonnetIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/bonnet/" + DBController.dbController.mountableBonnet[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            bonnetButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            bonnetButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableBonnet[i].first; //SampleButton의 Text 지정

            bonnetButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[bonnetIndex]); //부모 변경
            bonnetButtonTransform.localScale = Vector3.one; //크기 재조절
            bonnetButtonTransform.gameObject.name = DBController.dbController.mountableBonnet[i].second; //이름 변경
            bonnetButtonTransform.GetComponent<BonnetButton>().bonnetName = DBController.dbController.mountableBonnet[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            bonnetButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            bonnetButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[bonnetIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/bonnet/" + DBController.dbController.mountableBonnet[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            bonnetButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            bonnetButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableBonnet[i].first; //SampleButton의 Text 지정

            bonnetButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[bonnetIndex]); //부모 변경
            bonnetButtonTransform.localScale = Vector3.one; //크기 재조절
            bonnetButtonTransform.gameObject.name = DBController.dbController.mountableBonnet[i].second; //이름 변경
            bonnetButtonTransform.GetComponent<BonnetButton>().bonnetName = DBController.dbController.mountableBonnet[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            bonnetButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }

    /* 프론트 휀다 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateFrontFenderButtons()
    {
        int frontFenderIndex = (int)TypeOfParts.FrontFender; //인덱스 저장

        /* 최소화된 FrontFenderButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[frontFenderIndex].childCount; ++i) //FrontFenderContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[frontFenderIndex].GetChild(i).gameObject; //FrontFenderContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 FrontFenderButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[frontFenderIndex].childCount; ++i) //FrontFenderContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[frontFenderIndex].GetChild(i).gameObject; //FrontFenderContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableFrontFender.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform frontFenderButtonTransform = null; //FrontFenderButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            frontFenderButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[frontFenderIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/frontfender/" + DBController.dbController.mountableFrontFender[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            frontFenderButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            frontFenderButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableFrontFender[i].first; //SampleButton의 Text 지정

            frontFenderButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[frontFenderIndex]); //부모 변경
            frontFenderButtonTransform.localScale = Vector3.one; //크기 재조절
            frontFenderButtonTransform.gameObject.name = DBController.dbController.mountableFrontFender[i].second; //이름 변경
            frontFenderButtonTransform.GetComponent<FrontFenderButton>().frontFenderName = DBController.dbController.mountableFrontFender[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            frontFenderButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            frontFenderButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[frontFenderIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/frontfender/" + DBController.dbController.mountableFrontFender[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            frontFenderButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            frontFenderButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableFrontFender[i].first; //SampleButton의 Text 지정

            frontFenderButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[frontFenderIndex]); //부모 변경
            frontFenderButtonTransform.localScale = Vector3.one; //크기 재조절
            frontFenderButtonTransform.gameObject.name = DBController.dbController.mountableFrontFender[i].second; //이름 변경
            frontFenderButtonTransform.GetComponent<FrontFenderButton>().frontFenderName = DBController.dbController.mountableFrontFender[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            frontFenderButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }

    /* 리어 휀다 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateRearFenderButtons()
    {
        int rearFenderIndex = (int)TypeOfParts.RearFender; //인덱스 저장

        /* 최소화된 RearFenderButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[rearFenderIndex].childCount; ++i) //RearFenderContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[rearFenderIndex].GetChild(i).gameObject; //RearFenderContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 RearFenderButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[rearFenderIndex].childCount; ++i) //RearFenderContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[rearFenderIndex].GetChild(i).gameObject; //RearFenderContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableRearFender.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform rearFenderButtonTransform = null; //RearFenderButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            rearFenderButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[rearFenderIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/rearfender/" + DBController.dbController.mountableRearFender[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            rearFenderButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            rearFenderButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableRearFender[i].first; //SampleButton의 Text 지정

            rearFenderButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[rearFenderIndex]); //부모 변경
            rearFenderButtonTransform.localScale = Vector3.one; //크기 재조절
            rearFenderButtonTransform.gameObject.name = DBController.dbController.mountableRearFender[i].second; //이름 변경
            rearFenderButtonTransform.GetComponent<RearFenderButton>().rearFenderName = DBController.dbController.mountableRearFender[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            rearFenderButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            rearFenderButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[rearFenderIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/rearfender/" + DBController.dbController.mountableRearFender[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            rearFenderButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            rearFenderButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableRearFender[i].first; //SampleButton의 Text 지정

            rearFenderButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[rearFenderIndex]); //부모 변경
            rearFenderButtonTransform.localScale = Vector3.one; //크기 재조절
            rearFenderButtonTransform.gameObject.name = DBController.dbController.mountableRearFender[i].second; //이름 변경
            rearFenderButtonTransform.GetComponent<RearFenderButton>().rearFenderName = DBController.dbController.mountableRearFender[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            rearFenderButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }

    /* 휠 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateWheelButtons()
    {
        yield return StartCoroutine(DBController.dbController.GetMountableWheel(DBController.dbController.mountableWheelInch, DBController.dbController.mountableWheelWidthLimit));

        int wheelIndex = (int)TypeOfParts.Wheel; //인덱스 저장

        /* 최소화된 WheelButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[wheelIndex].childCount; ++i) //WheelContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[wheelIndex].GetChild(i).gameObject; //WheelContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 WheelButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[wheelIndex].childCount; ++i) //WheelContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[wheelIndex].GetChild(i).gameObject; //WheelContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        yield return null; //다음 프레임까지 대기

        for (int i = 0; i < DBController.dbController.mountableWheel.Length; ++i) //장착 가능 개수만큼 반복
        {
            string[] wheelNameKo = DBController.dbController.mountableWheel[i].first.Split('-'); //휠 이름 Ko
            string[] wheelNameEng = DBController.dbController.mountableWheel[i].second.Split('-'); //휠 이름 Eng

            Transform wheelButtonTransform = null; //WheelButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite
            WheelButton wheelButton = null; //WheelButton 스크립트

            /* MinimizedButton */
            wheelButtonTransform = minimizedMainScrollViewContentRectTransforms[wheelIndex].Find(wheelNameEng[0]); //현재 생성된 버튼들을 탐색
            if (wheelButtonTransform == null) //기존 버튼이 존재하지 않으면
            {
                wheelButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[wheelIndex]).transform; //SampleButton 생성

                if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
                {
                    using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/wheel/" + wheelNameEng[0] + ".png"); //서버로부터 텍스처 요청
                    yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                    try
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                        partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                    }
                    catch (Exception)
                    {
                        partsSprite = null;
                    }
                }

                wheelButtonTransform.GetChild(0).GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
                wheelButtonTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = wheelNameKo[0]; //SampleButton의 Text 지정

                wheelButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[wheelIndex]); //부모 변경
                wheelButtonTransform.localScale = Vector3.one; //크기 재조절
                wheelButtonTransform.gameObject.name = wheelNameEng[0]; //이름 변경
                wheelButtonTransform.GetComponent<WheelButton>().wheelName = wheelNameEng[0]; //클릭했을 때 불러올 휠의 이름을 지정
                wheelButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
            }

            wheelButton = wheelButtonTransform.GetComponent<WheelButton>(); //WheelButton 컴포넌트 저장
            wheelButton.AddWheel(wheelNameEng[1], wheelNameEng[2]); //휠 추가

            /* MaximizedButton */
            wheelButtonTransform = maximizedMainScrollViewContentRectTransforms[wheelIndex].Find(wheelNameEng[0]); //현재 생성된 버튼들을 탐색
            if (wheelButtonTransform == null) //기존 버튼이 존재하지 않으면
            {
                wheelButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[wheelIndex]).transform; //SampleButton 생성

                if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
                {
                    using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/wheel/" + wheelNameEng[0] + ".png"); //서버로부터 텍스처 요청
                    yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                    try
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                        partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                    }
                    catch (Exception)
                    {
                        partsSprite = null;
                    }
                }

                wheelButtonTransform.GetChild(0).GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
                wheelButtonTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = wheelNameKo[0]; //SampleButton의 Text 지정

                wheelButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[wheelIndex]); //부모 변경
                wheelButtonTransform.localScale = Vector3.one; //크기 재조절
                wheelButtonTransform.gameObject.name = wheelNameEng[0]; //이름 변경
                wheelButtonTransform.GetComponent<WheelButton>().wheelName = wheelNameEng[0]; //클릭했을 때 불러올 휠의 이름을 지정
                wheelButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
            }

            wheelButton = wheelButtonTransform.GetComponent<WheelButton>(); //WheelButton 컴포넌트 저장
            wheelButton.AddWheel(wheelNameEng[1], wheelNameEng[2]); //휠 추가
        }
    }

    /* 타이어 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateTireButtons()
    {
        string[] wheelName = Static.selectedWheelName.Split('-'); //휠 이름 분리 후 저장
        yield return StartCoroutine(DBController.dbController.GetMountableTire(wheelName[2], wheelName[1]));

        int tireIndex = (int)TypeOfParts.Tire; //인덱스 저장

        /* 최소화된 TireButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[tireIndex].childCount; ++i) //TireContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[tireIndex].GetChild(i).gameObject; //TireContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 TireButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[tireIndex].childCount; ++i) //TireContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[tireIndex].GetChild(i).gameObject; //TireContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        yield return null; //다음 프레임까지 대기

        for (int i = 0; i < DBController.dbController.mountableTire.Length; ++i) //장착 가능 개수만큼 반복
        {
            string[] tireNameKo = DBController.dbController.mountableTire[i].first.Split('-'); //타이어 이름 Ko
            string[] tireNameEng = DBController.dbController.mountableTire[i].second.Split('-'); //타이어 이름 Eng

            Transform tireButtonTransform = null; //TireButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite
            TireButton tireButton = null; //TireButton 스크립트

            /* MinimizedButton */
            tireButtonTransform = minimizedMainScrollViewContentRectTransforms[tireIndex].Find(tireNameEng[0]); //현재 생성된 버튼들을 탐색
            if (tireButtonTransform == null) //기존 버튼이 존재하지 않으면
            {
                tireButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[tireIndex]).transform; //SampleButton 생성

                if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
                {
                    using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/tire/" + tireNameEng[0] + ".png"); //서버로부터 텍스처 요청
                    yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                    try
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                        partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                    }
                    catch (Exception)
                    {
                        partsSprite = null;
                    }
                }

                tireButtonTransform.GetChild(0).GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
                tireButtonTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tireNameKo[0]; //SampleButton의 Text 지정

                tireButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[tireIndex]); //부모 변경
                tireButtonTransform.localScale = Vector3.one; //크기 재조절
                tireButtonTransform.gameObject.name = tireNameEng[0]; //이름 변경
                tireButtonTransform.GetComponent<TireButton>().tireName = tireNameEng[0]; //클릭했을 때 불러올 타이어의 이름을 지정
                tireButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
            }

            tireButton = tireButtonTransform.GetComponent<TireButton>(); //TireButton 컴포넌트 저장
            tireButton.AddAspectRatio(tireNameEng[2]); //편평비 추가

            /* MaximizedButton */
            tireButtonTransform = maximizedMainScrollViewContentRectTransforms[tireIndex].Find(tireNameEng[0]); //현재 생성된 버튼들을 탐색
            if (tireButtonTransform == null) //기존 버튼이 존재하지 않으면
            {
                tireButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[tireIndex]).transform; //SampleButton 생성

                if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
                {
                    using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/tire/" + tireNameEng[0] + ".png"); //서버로부터 텍스처 요청
                    yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                    try
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                        partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                    }
                    catch (Exception)
                    {
                        partsSprite = null;
                    }
                }

                tireButtonTransform.GetChild(0).GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
                tireButtonTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tireNameKo[0]; //SampleButton의 Text 지정

                tireButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[tireIndex]); //부모 변경
                tireButtonTransform.localScale = Vector3.one; //크기 재조절
                tireButtonTransform.gameObject.name = tireNameEng[0]; //이름 변경
                tireButtonTransform.GetComponent<TireButton>().tireName = tireNameEng[0]; //클릭했을 때 불러올 타이어의 이름을 지정
                tireButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
            }

            tireButton = tireButtonTransform.GetComponent<TireButton>(); //TireButton 컴포넌트 저장
            tireButton.AddAspectRatio(tireNameEng[2]); //편평비 추가
        }

        Resources.UnloadUnusedAssets(); //메모리 누수 방지
    }

    /* 브레이크 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateBrakeButtons()
    {
        int brakeIndex = (int)TypeOfParts.Brake; //인덱스 저장

        /* 최소화된 BrakeButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[brakeIndex].childCount; ++i) //BrakeContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[brakeIndex].GetChild(i).gameObject; //BrakeContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 BrakeButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[brakeIndex].childCount; ++i) //BrakeContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[brakeIndex].GetChild(i).gameObject; //BrakeContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableBrake.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform brakeButtonTransform = null; //BrakeButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            brakeButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[brakeIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/brake/" + DBController.dbController.mountableBrake[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            brakeButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            brakeButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableBrake[i].first; //SampleButton의 Text 지정

            brakeButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[brakeIndex]); //부모 변경
            brakeButtonTransform.localScale = Vector3.one; //크기 재조절
            brakeButtonTransform.gameObject.name = DBController.dbController.mountableBrake[i].second; //이름 변경
            brakeButtonTransform.GetComponent<BrakeButton>().brakeName = DBController.dbController.mountableBrake[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            brakeButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            brakeButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[brakeIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/brake/" + DBController.dbController.mountableBrake[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            brakeButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            brakeButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableBrake[i].first; //SampleButton의 Text 지정

            brakeButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[brakeIndex]); //부모 변경
            brakeButtonTransform.localScale = Vector3.one; //크기 재조절
            brakeButtonTransform.gameObject.name = DBController.dbController.mountableBrake[i].second; //이름 변경
            brakeButtonTransform.GetComponent<BrakeButton>().brakeName = DBController.dbController.mountableBrake[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            brakeButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }

    /* 스포일러 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateSpoilerButtons()
    {
        int spoilerIndex = (int)TypeOfParts.Spoiler; //인덱스 저장

        /* 최소화된 SpoilerButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[spoilerIndex].childCount; ++i) //SpoilerContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[spoilerIndex].GetChild(i).gameObject; //SpoilerContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 SpoilerButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[spoilerIndex].childCount; ++i) //SpoilerContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[spoilerIndex].GetChild(i).gameObject; //SpoilerContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableSpoiler.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform spoilerButtonTransform = null; //SpoilerButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            spoilerButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[spoilerIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/spoiler/" + DBController.dbController.mountableSpoiler[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            spoilerButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            spoilerButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableSpoiler[i].first; //SampleButton의 Text 지정

            spoilerButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[spoilerIndex]); //부모 변경
            spoilerButtonTransform.localScale = Vector3.one; //크기 재조절
            spoilerButtonTransform.gameObject.name = DBController.dbController.mountableSpoiler[i].second; //이름 변경
            spoilerButtonTransform.GetComponent<SpoilerButton>().spoilerName = DBController.dbController.mountableSpoiler[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            spoilerButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            spoilerButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[spoilerIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/spoiler/" + DBController.dbController.mountableSpoiler[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            spoilerButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            spoilerButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableSpoiler[i].first; //SampleButton의 Text 지정

            spoilerButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[spoilerIndex]); //부모 변경
            spoilerButtonTransform.localScale = Vector3.one; //크기 재조절
            spoilerButtonTransform.gameObject.name = DBController.dbController.mountableSpoiler[i].second; //이름 변경
            spoilerButtonTransform.GetComponent<SpoilerButton>().spoilerName = DBController.dbController.mountableSpoiler[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            spoilerButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }

    /* 배기구 버튼들을 생성하는 코루틴 함수 */
    public IEnumerator CreateExhaustPipeButtons()
    {
        int exhaustPipeIndex = (int)TypeOfParts.ExhaustPipe; //인덱스 저장

        /* 최소화된 ExhaustPipeButton 모두 제거 */
        for (int i = 0; i < minimizedMainScrollViewContentRectTransforms[exhaustPipeIndex].childCount; ++i) //ExhaustPipeContent의 모든 자식 탐색
        {
            GameObject button = minimizedMainScrollViewContentRectTransforms[exhaustPipeIndex].GetChild(i).gameObject; //ExhaustPipeContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        /* 최대화된 ExhaustPipeButton 모두 제거 */
        for (int i = 0; i < maximizedMainScrollViewContentRectTransforms[exhaustPipeIndex].childCount; ++i) //ExhaustPipeContent의 모든 자식 탐색
        {
            GameObject button = maximizedMainScrollViewContentRectTransforms[exhaustPipeIndex].GetChild(i).gameObject; //ExhaustPipeContent의 버튼을 저장
            if (button.activeSelf) Destroy(button); //버튼이 활성화 되어있으면 제거
        }

        for (int i = 0; i < DBController.dbController.mountableExhaustPipe.Length; ++i) //장착 가능 개수만큼 반복
        {
            Transform exhaustPipeButtonTransform = null; //ExhaustPipeButton의 Transform 컴포넌트
            Sprite partsSprite = null; //파츠 Sprite

            /* MinimizedButton */
            exhaustPipeButtonTransform = Instantiate(minimizedMainScrollViewSampleButtons[exhaustPipeIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/exhaustpipe/" + DBController.dbController.mountableExhaustPipe[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            exhaustPipeButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            exhaustPipeButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableExhaustPipe[i].first; //SampleButton의 Text 지정

            exhaustPipeButtonTransform.SetParent(minimizedMainScrollViewContentRectTransforms[exhaustPipeIndex]); //부모 변경
            exhaustPipeButtonTransform.localScale = Vector3.one; //크기 재조절
            exhaustPipeButtonTransform.gameObject.name = DBController.dbController.mountableExhaustPipe[i].second; //이름 변경
            exhaustPipeButtonTransform.GetComponent<ExhaustPipeButton>().exhaustPipeName = DBController.dbController.mountableExhaustPipe[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            exhaustPipeButtonTransform.gameObject.SetActive(true); //오브젝트 활성화

            /* MaximizedButton */
            exhaustPipeButtonTransform = Instantiate(maximizedMainScrollViewSampleButtons[exhaustPipeIndex]).transform; //SampleButton 생성

            if (partsSprite == null) //파츠 Sprite가 존재하지 않으면
            {
                using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(BundleController.bundleController.serverPath + "/exhaustpipe/" + DBController.dbController.mountableExhaustPipe[i].second + ".png"); //서버로부터 텍스처 요청
                yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
                try
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(unityWebRequest); //요청으로부터 텍스처 생성
                    partsSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); //텍스처로 Sprite 적용
                }
                catch (Exception)
                {
                    partsSprite = null;
                }
            }

            exhaustPipeButtonTransform.GetComponent<Image>().sprite = partsSprite; //파츠 Sprite 적용
            exhaustPipeButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DBController.dbController.mountableExhaustPipe[i].first; //SampleButton의 Text 지정

            exhaustPipeButtonTransform.SetParent(maximizedMainScrollViewContentRectTransforms[exhaustPipeIndex]); //부모 변경
            exhaustPipeButtonTransform.localScale = Vector3.one; //크기 재조절
            exhaustPipeButtonTransform.gameObject.name = DBController.dbController.mountableExhaustPipe[i].second; //이름 변경
            exhaustPipeButtonTransform.GetComponent<ExhaustPipeButton>().exhaustPipeName = DBController.dbController.mountableExhaustPipe[i].second; //클릭했을 때 불러올 타이어의 이름을 지정
            exhaustPipeButtonTransform.gameObject.SetActive(true); //오브젝트 활성화
        }
    }
    #endregion

    #region LoadParts
    /* 파츠를 불러오는 코루틴 함수 */
    public IEnumerator LoadParts(TypeOfParts loadedTypeOfParts, string partsName, bool caching, bool unloadUnusedAssets)
    {
        if (caching) Notification.notification.EnableLayout(Notification.Layout.LoadParts); //캐싱이면 레이아웃 활성화

        if (TypeOfParts.FrontBumper == loadedTypeOfParts) //프론트 범퍼
        {
            if (SuspensionController.suspensionController.movePartsTransforms.frontBumper != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.frontBumper.gameObject); //기존 파츠 제거
            CarPaint.carPaint.carEntireMaterials[1] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[9] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[10] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[11] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("frontbumper", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.frontBumper.position, SuspensionController.suspensionController.carBodyPartsTransforms.frontBumper.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.FrontBumper], (int)TypeOfParts.FrontBumper)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.FrontBumper]) //번들이 로드되면
            {
                Static.selectedFrontBumperName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.FrontBumper); //움직일 파츠 Transform 캐싱
                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Entire); //도색 부위 캐싱
                    CarPaint.carPaint.carEntireSurface = CarPaint.carPaint.carEntireSurface; //도색 부위 재질 변경
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedFrontBumperName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.RearBumper == loadedTypeOfParts) //리어 범퍼
        {
            if (SuspensionController.suspensionController.movePartsTransforms.rearBumper != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.rearBumper.gameObject); //기존 파츠 제거
            CarPaint.carPaint.carEntireMaterials[2] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("rearbumper", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.rearBumper.position, SuspensionController.suspensionController.carBodyPartsTransforms.rearBumper.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.RearBumper], (int)TypeOfParts.RearBumper)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.RearBumper]) //번들이 로드되면
            {
                Static.selectedRearBumperName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.RearBumper); //움직일 파츠 Transform 캐싱
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedRearBumperName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.Grill == loadedTypeOfParts) //그릴
        {
            if (SuspensionController.suspensionController.movePartsTransforms.grill != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.grill.gameObject); //기존 파츠 제거
            CarPaint.carPaint.carChromeDeleteMaterials[3] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[4] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[5] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("grill", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.grill.position, SuspensionController.suspensionController.carBodyPartsTransforms.grill.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Grill], (int)TypeOfParts.Grill)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Grill]) //번들이 로드되면
            {
                Static.selectedGrillName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Grill); //움직일 파츠 Transform 캐싱
                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.ChromeDelete); //도색 부위 캐싱
                    CarPaint.carPaint.carChromeDeleteSurface = CarPaint.carPaint.carChromeDeleteSurface; //도색 부위 재질 변경
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedGrillName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.Bonnet == loadedTypeOfParts) //본넷
        {
            if (SuspensionController.suspensionController.movePartsTransforms.bonnet != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.bonnet.gameObject); //기존 파츠 제거
            CarPaint.carPaint.carBonnetMaterials[0] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[6] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[7] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carChromeDeleteMaterials[8] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("bonnet", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.bonnet.position, SuspensionController.suspensionController.carBodyPartsTransforms.bonnet.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Bonnet], (int)TypeOfParts.Bonnet)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Bonnet]) //번들이 로드되면
            {
                Static.selectedBonnetName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Bonnet); //움직일 파츠 Transform 캐싱

                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Bonnet); //도색 부위 캐싱
                    CarPaint.carPaint.carBonnetSurface = CarPaint.carPaint.carBonnetSurface; //도색 부위 재질 변경

                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.ChromeDelete); //도색 부위 캐싱
                    CarPaint.carPaint.carChromeDeleteSurface = CarPaint.carPaint.carChromeDeleteSurface; //도색 부위 재질 변경
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedBonnetName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.FrontFender == loadedTypeOfParts) //프론트 휀다
        {
            if (SuspensionController.suspensionController.movePartsTransforms.frontFender_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.frontFender_Left.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.frontFender_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.frontFender_Right.gameObject); //기존 파츠 제거

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("frontfender", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Left.position, SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Left.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.FrontFender], (int)TypeOfParts.FrontFender)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.FrontFender]) //번들이 로드되면
            {
                GameObject parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.FrontFender],
                SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Right.position,
                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.frontFender_Right.localEulerAngles),
                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.FrontFender]); //파츠 복사
                parts.transform.localScale = new Vector3(-1f, parts.transform.localScale.y, parts.transform.localScale.z); //좌우 반전
                parts.name = partsName; //파츠 이름 변경

                Static.selectedFrontFenderName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.FrontFender); //움직일 파츠 Transform 캐싱
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedFrontFenderName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.RearFender == loadedTypeOfParts) //리어 휀다
        {
            if (SuspensionController.suspensionController.movePartsTransforms.rearFender_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.rearFender_Left.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.rearFender_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.rearFender_Right.gameObject); //기존 파츠 제거

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("rearfender", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Left.position, SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Left.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.RearFender], (int)TypeOfParts.RearFender)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.RearFender]) //번들이 로드되면
            {
                GameObject parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.RearFender],
                SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Right.position,
                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.rearFender_Right.localEulerAngles),
                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.RearFender]); //파츠 복사
                parts.transform.localScale = new Vector3(-1f, parts.transform.localScale.y, parts.transform.localScale.z); //좌우 반전
                parts.name = partsName; //파츠 이름 변경

                Static.selectedRearFenderName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.RearFender); //움직일 파츠 Transform 캐싱
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedRearFenderName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.Wheel == loadedTypeOfParts) //휠
        {
            if (SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Left.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.wheel_Front_Right.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Left.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.wheel_Rear_Right.gameObject); //기존 파츠 제거
            CarPaint.carPaint.carWheelMaterials[0] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carWheelMaterials[1] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carWheelMaterials[2] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carWheelMaterials[3] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carWheelMaterials[4] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carWheelMaterials[5] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carWheelMaterials[6] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carWheelMaterials[7] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("wheel", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Left.position, SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Left.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Wheel], (int)TypeOfParts.Wheel)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Wheel]) //번들이 로드되면
            {
                GameObject parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Wheel],
                SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Right.position,
                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Front_Right.localEulerAngles),
                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Wheel]); //파츠 복사
                parts.name = partsName; //파츠 이름 변경

                parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Wheel],
                                SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Left.position,
                                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Left.localEulerAngles),
                                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Wheel]); //파츠 복사
                parts.name = partsName; //파츠 이름 변경

                parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Wheel],
                                SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Right.position,
                                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.wheel_Rear_Right.localEulerAngles),
                                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Wheel]); //파츠 복사
                parts.name = partsName; //파츠 이름 변경

                Static.selectedWheelName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Wheel); //움직일 파츠 Transform 캐싱
                    SuspensionController.suspensionController.SyncMovePartsTransforms(); //파츠 위치 동기화
                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.Wheel); //도색 부위 캐싱
                    CarPaint.carPaint.carWheelSurface = CarPaint.carPaint.carWheelSurface; //도색 부위 재질 변경
                }

                yield return StartCoroutine(CreateTireButtons()); //타이어 버튼 생성
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedWheelName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.Tire == loadedTypeOfParts) //타이어
        {
            if (SuspensionController.suspensionController.movePartsTransforms.tire_Front_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.tire_Front_Left.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.tire_Front_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.tire_Front_Right.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Left.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.tire_Rear_Right.gameObject); //기존 파츠 제거

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("tire", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Left.position, SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Left.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Tire], (int)TypeOfParts.Tire)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Tire]) //번들이 로드되면
            {
                GameObject parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Tire],
                SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Right.position,
                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.tire_Front_Right.localEulerAngles),
                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Tire]); //파츠 복사
                parts.name = partsName; //파츠 이름 변경

                parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Tire],
                                SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Left.position,
                                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Left.localEulerAngles),
                                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Tire]); //파츠 복사
                parts.name = partsName; //파츠 이름 변경

                parts = Instantiate(BundleController.bundleController.loadedBundles[(int)TypeOfParts.Tire],
                                SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Right.position,
                                Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.tire_Rear_Right.localEulerAngles),
                                CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Tire]); //파츠 복사
                parts.name = partsName; //파츠 이름 변경

                Static.selectedTireName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Tire); //움직일 파츠 Transform 캐싱
                    SuspensionController.suspensionController.SyncMovePartsTransforms(); //파츠 위치 동기화
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedTireName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.Brake == loadedTypeOfParts) //브레이크
        {
            if (SuspensionController.suspensionController.movePartsTransforms.brake_Front_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.brake_Front_Left.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.brake_Front_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.brake_Front_Right.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Left != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Left.gameObject); //기존 파츠 제거
            if (SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Right != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.brake_Rear_Right.gameObject); //기존 파츠 제거
            CarPaint.carPaint.carBrakeCaliperMaterials[0] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carBrakeCaliperMaterials[1] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carBrakeCaliperMaterials[2] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)
            CarPaint.carPaint.carBrakeCaliperMaterials[3] = null; //도색 부위 캐싱 제거(CarPaint 스크립트에서 참조하는 부분)

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("brake", partsName), Vector3.zero, Vector3.zero, null, (int)TypeOfParts.Brake)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Brake]) //번들이 로드되면
            {
                Transform brakeFrontLeft = BundleController.bundleController.loadedBundles[(int)TypeOfParts.Brake].transform.GetChild(0); //BrakeFrontLeft의 Transform 컴포넌트
                Transform brakeFrontRight = Instantiate(brakeFrontLeft).transform; //BrakeFrontRight의 Transform 컴포넌트
                Transform brakeRearLeft = BundleController.bundleController.loadedBundles[(int)TypeOfParts.Brake].transform.GetChild(1); //BrakeRearLeft의 Transform 컴포넌트
                Transform brakeRearRight = Instantiate(brakeRearLeft).transform; //BrakeRearRight의 Transform 컴포넌트

                brakeFrontLeft.SetPositionAndRotation(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Left.position, Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Left.localEulerAngles)); //위치 및 회전 적용
                brakeFrontLeft.SetParent(CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Brake]); //부모 변경
                brakeFrontLeft.name = partsName; //파츠 이름 변경

                brakeFrontRight.SetPositionAndRotation(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Right.position, Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Front_Right.localEulerAngles)); //위치 및 회전 적용
                brakeFrontRight.SetParent(CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Brake]); //부모 변경
                brakeFrontRight.name = partsName; //파츠 이름 변경

                brakeRearLeft.SetPositionAndRotation(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Left.position, Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Left.localEulerAngles)); //위치 및 회전 적용
                brakeRearLeft.SetParent(CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Brake]); //부모 변경
                brakeRearLeft.name = partsName; //파츠 이름 변경

                brakeRearRight.SetPositionAndRotation(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Right.position, Quaternion.Euler(SuspensionController.suspensionController.carBodyPartsTransforms.brake_Rear_Right.localEulerAngles)); //위치 및 회전 적용
                brakeRearRight.SetParent(CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Brake]); //부모 변경
                brakeRearRight.name = partsName; //파츠 이름 변경

                Static.selectedBrakeName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Brake); //움직일 파츠 Transform 캐싱
                    SuspensionController.suspensionController.SyncMovePartsTransforms(); //파츠 위치 동기화
                    CarPaint.carPaint.CachingCarPaintPartMaterials(CarPaint.PaintPart.BrakeCaliper); //도색 부위 캐싱
                    CarPaint.carPaint.carBrakeCaliperSurface = CarPaint.carPaint.carBrakeCaliperSurface; //도색 부위 재질 변경
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedBrakeName = null; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }
        else if (TypeOfParts.Spoiler == loadedTypeOfParts) //스포일러
        {
            if (SuspensionController.suspensionController.movePartsTransforms.spoiler != null) Destroy(SuspensionController.suspensionController.movePartsTransforms.spoiler.gameObject); //기존 파츠 제거

            yield return StartCoroutine(BundleController.bundleController.LoadBundle(new Pair<string, string>("spoiler", partsName), SuspensionController.suspensionController.carBodyPartsTransforms.spoiler.position, SuspensionController.suspensionController.carBodyPartsTransforms.spoiler.localEulerAngles, CarSelect.carSelect.carPartsTransforms[(int)TypeOfParts.Spoiler], (int)TypeOfParts.Spoiler)); //파츠 번들 로드

            if (BundleController.bundleController.loadedBundles[(int)TypeOfParts.Spoiler]) //번들이 로드되면
            {
                Static.selectedSpoilerName = partsName; //선택된 파츠 이름 지정

                if (caching) //캐싱이면
                {
                    CarSelect.carSelect.CachingMoveTransforms((int)TypeOfParts.Spoiler); //움직일 파츠 Transform 캐싱
                }
            }
            else //번들이 로드되지 않으면
            {
                Static.selectedSpoilerName = partsName; //선택된 파츠 이름 제거

                if (caching) //캐싱이면
                {
                    Notification.notification.EnableLayout(Notification.Layout.FailureLoadParts); //레이아웃 활성화
                }
            }
        }

        if (unloadUnusedAssets) Resources.UnloadUnusedAssets(); //메모리 누수 방지

        if (caching) Notification.notification.DisableLayout(Notification.Layout.LoadParts); //캐싱이면 레이아웃 비활성화
    }
    #endregion
}