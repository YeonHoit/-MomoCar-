using UnityEngine;
using LitJson;

public class LocalDBController : MonoBehaviour
{
    [Header("Static")]
    public static LocalDBController localDBController; //???? ???? ????

    private void Awake()
    {
        localDBController = this;

        UnityMessageManager.Instance.OnMessage += OnMessage;
    }

    /* Message?? ???? ???? */
    public void OnMessage(string data)
    {
        string[] splitedData = data.Split('|'); //???????? ????

        if (splitedData[0].Equals("response_user_json")) //UserJson ????
        {
            JsonData jsonData = JsonMapper.ToObject(splitedData[1]); //Json Data?? ????
            string userCar = jsonData["UserCar"].ToString(); //?????? ???? ????

            Utility.utility.LoadStartCoroutine(CarSelect.carSelect.LoadCarFromKo(userCar)); //???? ????????

            string quality = jsonData["quality"].ToString(); //???? ???? ????
            if (quality.Equals("L")) Option.option.quality = 0;
            else if (quality.Equals("M")) Option.option.quality = 1;
            else if (quality.Equals("H")) Option.option.quality = 2;
            else Option.option.quality = 3;

            string frame = jsonData["frame"].ToString(); //?????? ???? ????
            if (frame.Equals("L")) Option.option.frame = 0;
            else if (frame.Equals("M")) Option.option.frame = 1;
            else Option.option.frame = 2;
        }
    }

    /* UserJson?? ???????? ????  */
    public void RequestUserJson()
    {
        UnityMessageManager.Instance.SendMessageToRN("request_user_json|"); //UserJson ????
    }

    /* UserJson?? ???????? ???? */
    public void SaveUserJson()
    {
        string quality = Option.option.quality == 0 ? "L" : Option.option.quality == 1 ? "M" : Option.option.quality == 2 ? "H" : "U"; //???? ????
        string frame = Option.option.frame == 0 ? "L" : Option.option.frame == 1 ? "M" : "H"; //?????? ????

        string parse = "{\"quality\":\"" + quality + "\",\"frame\":\"" + frame + "\"}"; //????
        UnityMessageManager.Instance.SendMessageToRN("save_user_json|" + parse); //UserJson ????
    }
}