using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour
{
    [Header("Static")]
    public static Utility utility; //전역 접근 변수

    [Header("Variable")]
    public static readonly int enumTypeOfPartsLength = System.Enum.GetValues(typeof(TypeOfParts)).Length; //enum TypeOfParts 길이

    private void Awake()
    {
        utility = this;
    }

    /* StartCoroutine 함수를 불러오는 함수 - SetActive 방지용 */
    public void LoadStartCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    /* 두 변수 값을 교환하는 함수 */
    public static void Swap<T>(ref T first, ref T second)
    {
        T temp = first;
        first = second;
        second = temp;
    }

    /* Material들 중에서 이름과 일치하는 Material을 반환하는 함수 */
    public static Material FindMaterialWithName(ref Material[] materials, string name)
    {
        if (materials == null) return null; //Materials가 존재하면

        for (int i = 0; i < materials.Length; ++i) //Material 개수만큼 반복
        {
            if (materials[i].name.Equals(name)) //이름이 일치하면
            {
                return materials[i]; //반환
            }
        }

        return null; //일치하는 것이 없으면 null 반환
    }

    /* Material들의 속성을 일괄 변경하는 함수 */
    public static void SetCarMaterialsProperty(ref Material[] materials, string property, Color32 value)
    {
        if (materials == null) return; //Materials가 존재하지 않으면

        for (int i = 0; i < materials.Length; ++i) //Material의 개수만큼 반복
        {
            if (materials[i] != null) //Material이 존재하면
            {
                materials[i].SetColor(property, new Color32(value.r, value.g, value.b, ((Color32)materials[i].GetColor(property)).a)); //속성 적용
            }
        }
    }

    /* Material들의 속성을 일괄 변경하는 함수 */
    public static void SetCarMaterialsProperty(ref Material[] materials, string property, int value)
    {
        if (materials == null) return; //Materials가 존재하지 않으면

        for (int i = 0; i < materials.Length; ++i) //Material의 개수만큼 반복
        {
            if (materials[i] != null) //Material이 존재하면
            {
                materials[i].SetInt(property, value); //속성 적용
            }
        }
    }
}

#region Struct
public struct Pair<T1, T2> //한 쌍의 자료를 담는 구조체
{
    public T1 first;
    public T2 second;

    public Pair(T1 first, T2 second)
    {
        this.first = first;
        this.second = second;
    }
}

public struct CarInfo //차량 정보
{
    public string manufacturer_eng; //제조업체 영어
    public string manufacturer_ko; //제조업체 한글
    public string carType_eng; //차종 영어
    public string carType_ko; //차종 한글
    public string yearType_eng; //연식 영어
    public string yearType_ko; //연식 한글
};

public struct PartsTransforms //파츠 Transform 컴포넌트들
{
    public Transform frontBumper;
    public Transform rearBumper;
    public Transform grill;
    public Transform bonnet;
    public Transform frontFender_Left;
    public Transform frontFender_Right;
    public Transform rearFender_Left;
    public Transform rearFender_Right;
    public Transform wheel_Front_Left;
    public Transform wheel_Front_Right;
    public Transform wheel_Rear_Left;
    public Transform wheel_Rear_Right;
    public Transform tire_Front_Left;
    public Transform tire_Front_Right;
    public Transform tire_Rear_Left;
    public Transform tire_Rear_Right;
    public Transform brake_Front_Left;
    public Transform brake_Front_Right;
    public Transform brake_Rear_Left;
    public Transform brake_Rear_Right;
    public Transform spoiler;
};

public enum TypeOfParts //파츠의 종류
{
    FrontBumper,
    RearBumper,
    Grill,
    Bonnet,
    FrontFender,
    RearFender,
    Wheel,
    Tire,
    Brake,
    Spoiler,
    ExhaustPipe,
};

public enum CameraPosition //카메라 위치
{
    FrontBumper,
    RearBumper,
    Grill,
    Bonnet,
    FrontFender,
    RearFender,
    Wheel,
    Tire,
    Brake,
    Spoiler,
    ExhaustPipe,
    Center,
    CarPaint
};
#endregion