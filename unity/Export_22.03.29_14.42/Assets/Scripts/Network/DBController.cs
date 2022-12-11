using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using LitJson;

public class DBController : MonoBehaviour
{
    [Header("Static")]
    public static DBController dbController; //전역 참조 변수

    [Header("Network")]
    private string serverPath = Static.serverIP + "/Unity/PHP"; //서버 경로

    [Header("Cache")]
    [HideInInspector] public string yearTypeEng; //차량 연식 영문
    public CarInfo[] carInfos; //차량 정보들
    public Pair<string, string> carBundleInfo; //차량 번들 정보
    public string[] selectedPartsInfos; //선택된 파츠 정보들

    public Material[] carEntireSurfaceProperties; //Entire 재질 속성들
    public Material[] carBonnetSurfaceProperties; //Bonnet 재질 속성들
    public Material[] carSideMirrorSurfaceProperties; //SideMirror 재질 속성들
    public Material[] carRoofSurfaceProperties; //Roof 재질 속성들
    public Material[] carChromeDeleteSurfaceProperties; //ChromeDelete 재질 속성들
    public Material[] carWheelSurfaceProperties; //Wheel 재질 속성들
    public Material[] carBrakeCaliperSurfaceProperties; //BrakeCaliper 재질 속성들

    public Pair<string, string>[] mountableFrontBumper; //장착 가능한 FrontBumper
    public Pair<string, string>[] mountableRearBumper; //장착 가능한 RearBumper
    public Pair<string, string>[] mountableGrill; //장착 가능한 Grill
    public Pair<string, string>[] mountableBonnet; //장착 가능한 Bonnet
    public Pair<string, string>[] mountableFrontFender; //장착 가능한 FrontFender
    public Pair<string, string>[] mountableRearFender; //장착 가능한 RearFender
    [HideInInspector] public string mountableWheelInch; //장착 가능한 Wheel 인치
    [HideInInspector] public string mountableWheelWidthLimit; //장착 가능한 Wheel 한계 폭
    public Pair<string, string>[] mountableBrake; //장착 가능한 Brake
    public Pair<string, string>[] mountableSpoiler; //장착 가능한 Spoiler
    public Pair<string, string>[] mountableExhaustPipe; //장착 가능한 ExhaustPipe

    public Pair<string, string>[] mountableWheel; //장착 가능한 Wheel
    public Pair<string, string>[] mountableTire; //장착 가능한 Tire

    public string[] fieldInfos; //Field 정보들

    private void Awake()
    {
        dbController = this; //전역 참조 변수 지정
    }

    #region LocalDBController
    /* 한글 연식 정보를 존재하는 영문 연식 정보로 반환하는 코루틴 함수 */
    public IEnumerator GetYearTypeEngFromKo(string yearTypeKo)
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetYearTypeEngFromKo.php";  //서버 경로 지정
        WWWForm form = new WWWForm(); //Post 방식으로 넘겨줄 데이터
        form.AddField("yearTypeKo", yearTypeKo); //연식 정보 전달

        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            yearTypeEng = webRequest.downloadHandler.text; //차량 연식 저장
        }
        catch (Exception) //예외 발생하면 차량 정보 모두 제거
        {
            yearTypeEng = null;
        }
    }
    #endregion

    #region CarSelectTab
    /* 차량 정보를 반환하는 코루틴 함수 */
    public IEnumerator GetCarInfo()
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetCarInfo.php";  //서버 경로 지정
        WWWForm form = new WWWForm(); //Post 방식으로 넘겨줄 데이터

        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            JsonData jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text); //Json 데이터로 변경
            carInfos = new CarInfo[jsonData.Count]; //차량 정보를 담는 배열 생성
            for (int i = 0; i < jsonData.Count; ++i) //차량 개수만큼 반복
            {
                carInfos[i].manufacturer_eng = jsonData[i]["manufacturer_eng"].ToString(); //제조 업체
                carInfos[i].manufacturer_ko = jsonData[i]["manufacturer_ko"].ToString();

                carInfos[i].carType_eng = jsonData[i]["carType_eng"].ToString(); //차량 유형
                carInfos[i].carType_ko = jsonData[i]["carType_ko"].ToString();

                carInfos[i].yearType_eng = jsonData[i]["yearType_eng"].ToString(); //연식
                carInfos[i].yearType_ko = jsonData[i]["yearType_ko"].ToString();
            }
        }
        catch (Exception) //예외 발생하면 차량 정보 모두 제거
        {
            Notification.notification.EnableLayout(Notification.Layout.FailureGetCarInfo); //레이아웃 활성화
            carInfos = new CarInfo[0]; //차량 정보를 담는 배열 생성
        }
    }

    /* 차량 번들 정보를 반환하는 코루틴 함수 */
    public IEnumerator GetCarBundleInfo(string carInfo)
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetCarBundleInfo.php";  //서버 경로 지정
        WWWForm form = new WWWForm(); //Post 방식으로 넘겨줄 데이터
        form.AddField("carInfo", carInfo); //차량 정보 전달

        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            JsonData jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text); //Json 데이터로 변경
            carBundleInfo = new Pair<string, string>(); //차량 번들 정보를 담는 배열 생성

            carBundleInfo.first = jsonData["BundlePath"].ToString(); //번들 경로
            carBundleInfo.second = jsonData["BundleName"].ToString(); //번들 이름
        }
        catch (Exception) //예외 발생하면 차량 정보 모두 제거
        {
            carBundleInfo = new Pair<string, string>(); //차량 번들 정보를 담는 배열 생성
        }
    }

    /* 선택된 부품들의 번들 정보를 반환하는 코루틴 함수 */
    public IEnumerator GetSelectedPartsName(string carInfo)
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetSelectedPartsName.php";  //서버 경로 지정
        WWWForm form = new WWWForm(); //Post 방식으로 넘겨줄 데이터
        form.AddField("carInfo", carInfo); //차량 정보 전달

        int partsSize = Enum.GetValues(typeof(TypeOfParts)).Length; //파츠의 개수

        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            JsonData jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text); //Json 데이터로 변경

            selectedPartsInfos = new string[partsSize]; //파츠 정보를 담는 배열 생성

            selectedPartsInfos[(int)TypeOfParts.FrontBumper] = jsonData["FrontBumper"].ToString();
            selectedPartsInfos[(int)TypeOfParts.RearBumper] = jsonData["RearBumper"].ToString();
            selectedPartsInfos[(int)TypeOfParts.Grill] = jsonData["Grill"].ToString();
            selectedPartsInfos[(int)TypeOfParts.Bonnet] = jsonData["Bonnet"].ToString();
            selectedPartsInfos[(int)TypeOfParts.FrontFender] = jsonData["FrontFender"].ToString();
            selectedPartsInfos[(int)TypeOfParts.RearFender] = jsonData["RearFender"].ToString();
            selectedPartsInfos[(int)TypeOfParts.Wheel] = jsonData["Wheel"].ToString();
            selectedPartsInfos[(int)TypeOfParts.Tire] = jsonData["Tire"].ToString();
            selectedPartsInfos[(int)TypeOfParts.Brake] = jsonData["Brake"].ToString();
            selectedPartsInfos[(int)TypeOfParts.Spoiler] = jsonData["Spoiler"].ToString();
            selectedPartsInfos[(int)TypeOfParts.ExhaustPipe] = jsonData["ExhaustPipe"].ToString();
        }
        catch (Exception) //예외 발생하면 차량 정보 모두 제거
        {
            selectedPartsInfos = new string[0]; //파츠 정보를 담는 배열 생성
        }
    }
    #endregion

    /* 도색 부위의 재질 정보를 가져오는 코루틴 함수 */
    public IEnumerator GetPaintPartSurfaceInfo(string carName)
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetPaintPartSurfaceInfo.php";  //서버 경로 지정
        WWWForm form;

        form = new WWWForm(); //Post 방식으로 넘겨줄 데이터
        form.AddField("carName", carName); //차량 이름 전달

        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            JsonData jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text); //Json 데이터로 변경

            /* Entire */
            JsonData entireJson = JsonMapper.ToObject(jsonData["Entire"].ToString());
            carEntireSurfaceProperties = new Material[entireJson.Count]; //재질 개수만큼 할당
            for (int i = 0; i < entireJson.Count; ++i) //재질 개수만큼 반복
            {
                carEntireSurfaceProperties[i] = new Material(CarPaint.carPaint.carEntireSurfaces[i]); //셰이더 지정

                JsonData surfaceJson = JsonMapper.ToObject(entireJson[i].ToJson());
                if (surfaceJson.Count > 0) //Property가 존재하면
                {
                    foreach (string value in surfaceJson.Keys) //Property 개수만큼 반복
                    {
                        if (surfaceJson[value].IsInt) //Value가 정수이면
                        {
                            carEntireSurfaceProperties[i].SetInt(value, (int)surfaceJson[value]);
                        }
                        else if (surfaceJson[value].IsString) //Value가 문자열이면
                        {
                            string[] float3 = surfaceJson[value].ToString().Split(',');
                            carEntireSurfaceProperties[i].SetVector(value, new Vector3(int.Parse(float3[0]), int.Parse(float3[1]), int.Parse(float3[2])));
                        }
                    }
                }
            }

            /* Bonnet */
            JsonData bonnetJson = JsonMapper.ToObject(jsonData["Bonnet"].ToString());
            carBonnetSurfaceProperties = new Material[bonnetJson.Count]; //재질 개수만큼 할당
            for (int i = 0; i < bonnetJson.Count; ++i) //재질 개수만큼 반복
            {
                carBonnetSurfaceProperties[i] = new Material(CarPaint.carPaint.carBonnetSurfaces[i]); //셰이더 지정

                JsonData surfaceJson = JsonMapper.ToObject(bonnetJson[i].ToJson());
                if (surfaceJson.Count > 0) //Property가 존재하면
                {
                    foreach (string value in surfaceJson.Keys) //Property 개수만큼 반복
                    {
                        if (surfaceJson[value].IsInt) //Value가 정수이면
                        {
                            carBonnetSurfaceProperties[i].SetInt(value, (int)surfaceJson[value]);
                        }
                        else if (surfaceJson[value].IsString) //Value가 문자열이면
                        {
                            string[] float3 = surfaceJson[value].ToString().Split(',');
                            carBonnetSurfaceProperties[i].SetVector(value, new Vector3(int.Parse(float3[0]), int.Parse(float3[1]), int.Parse(float3[2])));
                        }
                    }
                }
            }

            /* SideMirror */
            JsonData sideMirrorJson = JsonMapper.ToObject(jsonData["SideMirror"].ToString());
            carSideMirrorSurfaceProperties = new Material[sideMirrorJson.Count]; //재질 개수만큼 할당
            for (int i = 0; i < sideMirrorJson.Count; ++i) //재질 개수만큼 반복
            {
                carSideMirrorSurfaceProperties[i] = new Material(CarPaint.carPaint.carSideMirrorSurfaces[i]); //셰이더 지정

                JsonData surfaceJson = JsonMapper.ToObject(sideMirrorJson[i].ToJson());
                if (surfaceJson.Count > 0) //Property가 존재하면
                {
                    foreach (string value in surfaceJson.Keys) //Property 개수만큼 반복
                    {
                        if (surfaceJson[value].IsInt) //Value가 정수이면
                        {
                            carSideMirrorSurfaceProperties[i].SetInt(value, (int)surfaceJson[value]);
                        }
                        else if (surfaceJson[value].IsString) //Value가 문자열이면
                        {
                            string[] float3 = surfaceJson[value].ToString().Split(',');
                            carSideMirrorSurfaceProperties[i].SetVector(value, new Vector3(int.Parse(float3[0]), int.Parse(float3[1]), int.Parse(float3[2])));
                        }
                    }
                }
            }

            /* Roof */
            JsonData roofJson = JsonMapper.ToObject(jsonData["Roof"].ToString());
            carRoofSurfaceProperties = new Material[roofJson.Count]; //재질 개수만큼 할당
            for (int i = 0; i < roofJson.Count; ++i) //재질 개수만큼 반복
            {
                carRoofSurfaceProperties[i] = new Material(CarPaint.carPaint.carRoofSurfaces[i]); //셰이더 지정

                JsonData surfaceJson = JsonMapper.ToObject(roofJson[i].ToJson());
                if (surfaceJson.Count > 0) //Property가 존재하면
                {
                    foreach (string value in surfaceJson.Keys) //Property 개수만큼 반복
                    {
                        if (surfaceJson[value].IsInt) //Value가 정수이면
                        {
                            carRoofSurfaceProperties[i].SetInt(value, (int)surfaceJson[value]);
                        }
                        else if (surfaceJson[value].IsString) //Value가 문자열이면
                        {
                            string[] float3 = surfaceJson[value].ToString().Split(',');
                            carRoofSurfaceProperties[i].SetVector(value, new Vector3(int.Parse(float3[0]), int.Parse(float3[1]), int.Parse(float3[2])));
                        }
                    }
                }
            }

            /* ChromeDelete */
            JsonData chromeDeleteJson = JsonMapper.ToObject(jsonData["ChromeDelete"].ToString());
            carChromeDeleteSurfaceProperties = new Material[chromeDeleteJson.Count]; //재질 개수만큼 할당
            for (int i = 0; i < chromeDeleteJson.Count; ++i) //재질 개수만큼 반복
            {
                carChromeDeleteSurfaceProperties[i] = new Material(CarPaint.carPaint.carChromeDeleteSurfaces[i]); //셰이더 지정

                JsonData surfaceJson = JsonMapper.ToObject(chromeDeleteJson[i].ToJson());
                if (surfaceJson.Count > 0) //Property가 존재하면
                {
                    foreach (string value in surfaceJson.Keys) //Property 개수만큼 반복
                    {
                        if (surfaceJson[value].IsInt) //Value가 정수이면
                        {
                            carChromeDeleteSurfaceProperties[i].SetInt(value, (int)surfaceJson[value]);
                        }
                        else if (surfaceJson[value].IsString) //Value가 문자열이면
                        {
                            string[] float3 = surfaceJson[value].ToString().Split(',');
                            carChromeDeleteSurfaceProperties[i].SetVector(value, new Vector3(int.Parse(float3[0]), int.Parse(float3[1]), int.Parse(float3[2])));
                        }
                    }
                }
            }

            /* Wheel */
            JsonData wheelJson = JsonMapper.ToObject(jsonData["Wheel"].ToString());
            carWheelSurfaceProperties = new Material[wheelJson.Count]; //재질 개수만큼 할당
            for (int i = 0; i < wheelJson.Count; ++i) //재질 개수만큼 반복
            {
                carWheelSurfaceProperties[i] = new Material(CarPaint.carPaint.carWheelSurfaces[i]); //셰이더 지정

                JsonData surfaceJson = JsonMapper.ToObject(wheelJson[i].ToJson());
                if (surfaceJson.Count > 0) //Property가 존재하면
                {
                    foreach (string value in surfaceJson.Keys) //Property 개수만큼 반복
                    {
                        if (surfaceJson[value].IsInt) //Value가 정수이면
                        {
                            carWheelSurfaceProperties[i].SetInt(value, (int)surfaceJson[value]);
                        }
                        else if (surfaceJson[value].IsString) //Value가 문자열이면
                        {
                            string[] float3 = surfaceJson[value].ToString().Split(',');
                            carWheelSurfaceProperties[i].SetVector(value, new Vector3(int.Parse(float3[0]), int.Parse(float3[1]), int.Parse(float3[2])));
                        }
                    }
                }
            }

            /* BrakeCaliper */
            JsonData brakeCaliperJson = JsonMapper.ToObject(jsonData["BrakeCaliper"].ToString());
            carBrakeCaliperSurfaceProperties = new Material[brakeCaliperJson.Count]; //재질 개수만큼 할당
            for (int i = 0; i < brakeCaliperJson.Count; ++i) //재질 개수만큼 반복
            {
                carBrakeCaliperSurfaceProperties[i] = new Material(CarPaint.carPaint.carBrakeCaliperSurfaces[i]); //셰이더 지정

                JsonData surfaceJson = JsonMapper.ToObject(brakeCaliperJson[i].ToJson());
                if (surfaceJson.Count > 0) //Property가 존재하면
                {
                    foreach (string value in surfaceJson.Keys) //Property 개수만큼 반복
                    {
                        if (surfaceJson[value].IsInt) //Value가 정수이면
                        {
                            carBrakeCaliperSurfaceProperties[i].SetInt(value, (int)surfaceJson[value]);
                        }
                        else if (surfaceJson[value].IsString) //Value가 문자열이면
                        {
                            string[] float3 = surfaceJson[value].ToString().Split(',');
                            carBrakeCaliperSurfaceProperties[i].SetVector(value, new Vector3(int.Parse(float3[0]), int.Parse(float3[1]), int.Parse(float3[2])));
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            carEntireSurfaceProperties = null;
            carBonnetSurfaceProperties = null;
            carSideMirrorSurfaceProperties = null;
            carRoofSurfaceProperties = null;
            carChromeDeleteSurfaceProperties = null;
            carWheelSurfaceProperties = null;
            carBrakeCaliperSurfaceProperties = null;
        }
    }

    #region GetMountableParts
    /* 장착 가능한 파츠들의 정보를 가져오는 코루틴 함수 */
    public IEnumerator GetMountableParts(string carInfo)
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetMountableParts.php";  //서버 경로 지정
        WWWForm form;

        form = new WWWForm(); //Post 방식으로 넘겨줄 데이터
        form.AddField("carInfo", carInfo); //차량 정보 전달

        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            JsonData jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text); //Json 데이터로 변경

            /* FrontBumper */
            JsonData frontBumperJson = JsonMapper.ToObject(jsonData["FrontBumper"].ToString());
            mountableFrontBumper = new Pair<string, string>[frontBumperJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < frontBumperJson.Count; ++i)
            {
                mountableFrontBumper[i].first = frontBumperJson[i]["Ko"].ToString();
                mountableFrontBumper[i].second = frontBumperJson[i]["Eng"].ToString();
            }

            /* RearBumper */
            JsonData rearBumperJson = JsonMapper.ToObject(jsonData["RearBumper"].ToString());
            mountableRearBumper = new Pair<string, string>[rearBumperJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < rearBumperJson.Count; ++i)
            {
                mountableRearBumper[i].first = rearBumperJson[i]["Ko"].ToString();
                mountableRearBumper[i].second = rearBumperJson[i]["Eng"].ToString();
            }

            /* Grill */
            JsonData grillJson = JsonMapper.ToObject(jsonData["Grill"].ToString());
            mountableGrill = new Pair<string, string>[grillJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < grillJson.Count; ++i)
            {
                mountableGrill[i].first = grillJson[i]["Ko"].ToString();
                mountableGrill[i].second = grillJson[i]["Eng"].ToString();
            }

            /* Bonnet */
            JsonData bonnetJson = JsonMapper.ToObject(jsonData["Bonnet"].ToString());
            mountableBonnet = new Pair<string, string>[bonnetJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < bonnetJson.Count; ++i)
            {
                mountableBonnet[i].first = bonnetJson[i]["Ko"].ToString();
                mountableBonnet[i].second = bonnetJson[i]["Eng"].ToString();
            }

            /* FrontFender */
            JsonData frontFenderJson = JsonMapper.ToObject(jsonData["FrontFender"].ToString());
            mountableFrontFender = new Pair<string, string>[frontFenderJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < frontFenderJson.Count; ++i)
            {
                mountableFrontFender[i].first = frontFenderJson[i]["Ko"].ToString();
                mountableFrontFender[i].second = frontFenderJson[i]["Eng"].ToString();
            }

            /* RearFender */
            JsonData rearFenderJson = JsonMapper.ToObject(jsonData["RearFender"].ToString());
            mountableRearFender = new Pair<string, string>[rearFenderJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < rearFenderJson.Count; ++i)
            {
                mountableRearFender[i].first = rearFenderJson[i]["Ko"].ToString();
                mountableRearFender[i].second = rearFenderJson[i]["Eng"].ToString();
            }

            /* WheelInch */
            mountableWheelInch = jsonData["WheelInch"].ToString(); //장착 가능한 인치 지정

            /* WheelLimitWidth */
            mountableWheelWidthLimit = jsonData["WheelWidthLimit"].ToString(); //장착 가능한 한계 폭 지정

            /* Brake */
            JsonData brakeJson = JsonMapper.ToObject(jsonData["Brake"].ToString());
            mountableBrake = new Pair<string, string>[brakeJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < brakeJson.Count; ++i)
            {
                mountableBrake[i].first = brakeJson[i]["Ko"].ToString();
                mountableBrake[i].second = brakeJson[i]["Eng"].ToString();
            }

            /* Spoiler */
            JsonData spoilerJson = JsonMapper.ToObject(jsonData["Spoiler"].ToString());
            mountableSpoiler = new Pair<string, string>[spoilerJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < spoilerJson.Count; ++i)
            {
                mountableSpoiler[i].first = spoilerJson[i]["Ko"].ToString();
                mountableSpoiler[i].second = spoilerJson[i]["Eng"].ToString();
            }

            /* ExhaustPipe */
            JsonData exhaustPipeJson = JsonMapper.ToObject(jsonData["ExhaustPipe"].ToString());
            mountableExhaustPipe = new Pair<string, string>[exhaustPipeJson.Count]; //장착 가능한 파츠 개수만큼 초기화
            for (int i = 0; i < exhaustPipeJson.Count; ++i)
            {
                mountableExhaustPipe[i].first = exhaustPipeJson[i]["Ko"].ToString();
                mountableExhaustPipe[i].second = exhaustPipeJson[i]["Eng"].ToString();
            }
        }
        catch (Exception)
        {
            mountableFrontBumper = new Pair<string, string>[0];
            mountableRearBumper = new Pair<string, string>[0];
            mountableGrill = new Pair<string, string>[0];
            mountableBonnet = new Pair<string, string>[0];
            mountableFrontFender = new Pair<string, string>[0];
            mountableRearFender = new Pair<string, string>[0];
            mountableWheelInch = null;
            mountableWheelWidthLimit = null;
            mountableBrake = new Pair<string, string>[0];
            mountableSpoiler = new Pair<string, string>[0];
            mountableExhaustPipe = new Pair<string, string>[0];
        }
    }

    /* 치수를 바탕으로 장착 가능한 Wheel 정보를 가져오는 코루틴 함수 */
    public IEnumerator GetMountableWheel(string wheelInch, string wheelWidthLimit)
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetMountableWheel.php";  //서버 경로 지정

        WWWForm form = new WWWForm(); //Post 방식으로 넘겨줄 데이터
        form.AddField("wheelInch", wheelInch);
        form.AddField("wheelWidthLimit", wheelWidthLimit);

        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            JsonData jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text); //Json 데이터로 변경
            mountableWheel = new Pair<string, string>[jsonData.Count]; //장착 가능한 Wheel 정보를 담는 배열 초기화
            for (int i = 0; i < jsonData.Count; ++i) //Json 데이터 개수만큼 반복
            {
                mountableWheel[i].first = jsonData[i]["Ko"].ToString();
                mountableWheel[i].second = jsonData[i]["Eng"].ToString();
            }
        }
        catch (Exception)
        {
            mountableWheel = new Pair<string, string>[0]; //장착 가능한 Wheel 정보를 담는 배열 초기화
        }
    }

    /* 치수를 바탕으로 장착 가능한 Tire 정보를 가져오는 코루틴 함수 */
    public IEnumerator GetMountableTire(string tireInch, string tireWidth)
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetMountableTire.php";  //서버 경로 지정

        WWWForm form = new WWWForm(); //Post 방식으로 넘겨줄 데이터
        form.AddField("tireInch", tireInch);
        form.AddField("tireWidth", tireWidth);

        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            JsonData jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text); //Json 데이터로 변경
            mountableTire = new Pair<string, string>[jsonData.Count]; //장착 가능한 Tire 정보를 담는 배열 초기화
            for (int i = 0; i < jsonData.Count; ++i) //Json 데이터 개수만큼 반복
            {
                mountableTire[i].first = jsonData[i]["Ko"].ToString();
                mountableTire[i].second = jsonData[i]["Eng"].ToString();
            }
        }
        catch (Exception)
        {
            mountableTire = new Pair<string, string>[0]; //장착 가능한 Tire 정보를 담는 배열 초기화
        }
    }
    #endregion

    #region Field
    /* Field 정보들을 가져오는 코루틴 함수 */
    public IEnumerator GetFieldInfo()
    {
        /* PHP 호출 */
        string serverPath = this.serverPath + "/GetFieldInfo.php";  //서버 경로 지정
        WWWForm form;

        form = new WWWForm(); //Post 방식으로 넘겨줄 데이터
        using UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form); //웹 서버에 요청
        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

        try
        {
            JsonData jsonData = JsonMapper.ToObject(webRequest.downloadHandler.text); //Json 데이터로 변경
            fieldInfos = new string[jsonData.Count]; //Field 정보를 담는 배열 초기화

            for (int i = 0; i < jsonData.Count; ++i) //레이블 개수만큼 반복
            {
                fieldInfos[i] = jsonData[i]["Name"].ToString(); //Field 이름 저장
            }
        }
        catch (Exception) //예외 발생하면 Field 정보 모두 제거
        {
            Notification.notification.EnableLayout(Notification.Layout.FailureGetFieldInfo); //레이아웃 활성화
            fieldInfos = new string[0]; //Field 정보를 담는 배열 초기화
        }
    }
    #endregion
}