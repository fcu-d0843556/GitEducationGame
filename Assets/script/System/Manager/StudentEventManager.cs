
using UnityEngine;
using System.Collections;

using UnityEngine.Networking;
using UnityEngine.UI;

public class StudentEventManager : MonoBehaviour
{
    [SerializeField]
    string logEventApi;

    string jwtToken;
    public string username {  get;  private set; }

    public bool isLogin { get; private set; } = false;


    public void logStudentEvent(string eventName, string eventContent)
    {
        StartCoroutine(logEvent(eventName,eventContent));
    }

    IEnumerator logEvent(string eventName, string eventContent)
    {
        WWWForm form = new WWWForm();

        form.AddField("username", username);

        form.AddField("eventName", eventName);

        form.AddField("eventContent", eventContent);


        using (UnityWebRequest www = UnityWebRequest.Post(logEventApi, form))
        {
            yield return www.SendWebRequest();

            //Debug.Log("event passed : " + eventName + "\n" + eventContent);
        }

    }

    public void setJwtToken(string jwtToken)
    {
        this.jwtToken = jwtToken;
    }

    public void setUsername(string username)
    {
        this.username = username;
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "目前使用者：" + username;
        isLogin = true;
    }

    public void logout()
    {
        jwtToken = "";
        username = "";
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
        isLogin = false;
    }




}
