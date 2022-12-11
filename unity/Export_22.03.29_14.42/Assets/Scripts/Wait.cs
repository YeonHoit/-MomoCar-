using UnityEngine;

public class Wait : MonoBehaviour
{
    private void Awake()
    {
        UnityMessageManager.Instance.OnMessage += OnMessage;
    }

    private void Start()
    {
        Screen.fullScreen = false; //???? ???? ????
        if (Application.platform == RuntimePlatform.Android) Screen.orientation = ScreenOrientation.Portrait; //?????? ????
        UnityMessageManager.Instance.SendMessageToRN("exit|");
    }

    private void OnMessage(string data)
    {
        string[] splitedData = data.Split('|'); //???????? ????
        if (splitedData[0].Equals("start"))
        {
            UnityMessageManager.Instance.OnMessage -= OnMessage;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); //0?? ?? ????????
        }
    }
}