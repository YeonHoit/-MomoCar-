using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class BundleController : MonoBehaviour
{
    #region BuildBundle
#if UNITY_EDITOR
    [MenuItem("Bundle/BuildBundles_PC")] //메뉴로써 제작
    /* 번들 제작하는 함수 - PC */
    public static void BuildBundlesForPC()
    {
        string assetBundleDirectory = "Assets/Bundles_PC"; //번들을 저장할 폴더의 위치
        if (!Directory.Exists(assetBundleDirectory)) //폴더가 존재하지 않으면
        {
            Directory.CreateDirectory(assetBundleDirectory); //폴더 생성
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64); //번들 제작

        /* Texture 복사 */
        string textureBundlesDirectory = "Assets/Temps/Server/Bundles"; //Texture가 포함된 번들 디렉터리 경로
        string partsDirectory; //파츠가 포함된 디렉터리 경로
        DirectoryInfo directoryInfo;

        //Field
        partsDirectory = "field";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //FrontBumper
        partsDirectory = "frontbumper";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //RearBumper
        partsDirectory = "rearbumper";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Grill
        partsDirectory = "grill";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Bonnet
        partsDirectory = "bonnet";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //FrontFender
        partsDirectory = "frontfender";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //RearFender
        partsDirectory = "rearfender";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Wheel
        partsDirectory = "wheel";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Tire
        partsDirectory = "tire";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Brake
        partsDirectory = "brake";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Spoiler
        partsDirectory = "spoiler";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //ExhaustPipe
        partsDirectory = "exhaustpipe";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        AssetDatabase.Refresh(); //에셋 초기화
    }
#endif

#if UNITY_EDITOR
    [MenuItem("Bundle/BuildBundles_Android")] //메뉴로써 제작
    /* 번들 제작하는 함수 - Android */
    public static void BuildBundlesForAndroid()
    {
        string assetBundleDirectory = "Assets/Bundles_Android"; //번들을 저장할 폴더의 위치
        if (!Directory.Exists(assetBundleDirectory)) //폴더가 존재하지 않으면
        {
            Directory.CreateDirectory(assetBundleDirectory); //폴더 생성
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android); //번들 제작

        /* Texture 복사 */
        string textureBundlesDirectory = "Assets/Temps/Server/Bundles"; //Texture가 포함된 번들 디렉터리 경로
        string partsDirectory; //파츠가 포함된 디렉터리 경로
        DirectoryInfo directoryInfo;

        //Field
        partsDirectory = "field";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //FrontBumper
        partsDirectory = "frontbumper";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //RearBumper
        partsDirectory = "rearbumper";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Grill
        partsDirectory = "grill";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Bonnet
        partsDirectory = "bonnet";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //FrontFender
        partsDirectory = "frontfender";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //RearFender
        partsDirectory = "rearfender";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Wheel
        partsDirectory = "wheel";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Tire
        partsDirectory = "tire";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Brake
        partsDirectory = "brake";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Spoiler
        partsDirectory = "spoiler";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //ExhaustPipe
        partsDirectory = "exhaustpipe";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        AssetDatabase.Refresh(); //에셋 초기화
    }
#endif

#if UNITY_EDITOR
    [MenuItem("Bundle/BuildBundles_IOS")] //메뉴로써 제작
    /* 번들 제작하는 함수 - IOS */
    public static void BuildBundlesForIOS()
    {
        string assetBundleDirectory = "Assets/Bundles_IOS"; //번들을 저장할 폴더의 위치
        if (!Directory.Exists(assetBundleDirectory)) //폴더가 존재하지 않으면
        {
            Directory.CreateDirectory(assetBundleDirectory); //폴더 생성
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS); //번들 제작

        /* Texture 복사 */
        string textureBundlesDirectory = "Assets/Temps/Server/Bundles"; //Texture가 포함된 번들 디렉터리 경로
        string partsDirectory; //파츠가 포함된 디렉터리 경로
        DirectoryInfo directoryInfo;

        //Field
        partsDirectory = "field";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //FrontBumper
        partsDirectory = "frontbumper";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //RearBumper
        partsDirectory = "rearbumper";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Grill
        partsDirectory = "grill";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Bonnet
        partsDirectory = "bonnet";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //FrontFender
        partsDirectory = "frontfender";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //RearFender
        partsDirectory = "rearfender";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Wheel
        partsDirectory = "wheel";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Tire
        partsDirectory = "tire";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Brake
        partsDirectory = "brake";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //Spoiler
        partsDirectory = "spoiler";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        //ExhaustPipe
        partsDirectory = "exhaustpipe";
        directoryInfo = new DirectoryInfo(textureBundlesDirectory + '/' + partsDirectory); //디렉터리 정보 지정
        foreach (FileInfo fileInfo in directoryInfo.GetFiles()) //디렉터리를 탐색하면서
        {
            if (fileInfo.Extension.Equals(".png")) //확장자가 .png이면
            {
                File.Copy(textureBundlesDirectory + '/' + partsDirectory + '/' + fileInfo.Name, assetBundleDirectory + '/' + partsDirectory + '/' + fileInfo.Name); //텍스처 복사
            }
        }

        AssetDatabase.Refresh(); //에셋 초기화
    }
#endif
    #endregion

    [Header("Static")]
    public static BundleController bundleController; //전역 참조 변수
    [HideInInspector] public string serverPath; //서버 경로
    [HideInInspector] public readonly string bundleVersion = "1.2.0"; //번들 버전

    [Header("Caching")]
    public GameObject[] loadedBundles; //불러온 번들들

    private void Awake()
    {
        bundleController = this; //전역 참조 변수 지정
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) //PC이면
        {
            serverPath = Static.serverIP + "/Unity/Tuning_PC/Bundles_" + bundleVersion; //서버 경로
        }
        else if (Application.platform == RuntimePlatform.Android) //Android이면
        {
            serverPath = Static.serverIP + "/Unity/Tuning_Android/Bundles_" + bundleVersion; //서버 경로
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer) //IOS이면
        {
            serverPath = Static.serverIP + "/Unity/Tuning_IOS/Bundles_" + bundleVersion; //서버 경로
        }
    }

    /* 번들을 서버로부터 로드하는 함수 */
    public IEnumerator LoadBundle(Pair<string, string> bundleInfo, Vector3 pos, Vector3 rot, Transform parent, int bundleIndex)
    {
        using UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(serverPath + '/' + bundleInfo.first + '/' + bundleInfo.second); //번들 요청 생성

        yield return unityWebRequest.SendWebRequest(); //요청이 끝날 때까지 대기
        try
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest); //요청으로부터 번들 생성

            loadedBundles[bundleIndex] = (GameObject)Instantiate(bundle.LoadAsset(bundleInfo.second), pos, Quaternion.Euler(rot), parent); //번들에서 에셋 로드
            loadedBundles[bundleIndex].name = bundleInfo.second; //이름 정의

            bundle.Unload(false); //true이면 에셋번들 언로드
        }
        catch (Exception)
        {
            loadedBundles[bundleIndex] = null;
        }
    }
}