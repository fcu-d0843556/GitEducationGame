
using UnityEngine;
using System.Collections;

using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class StudentEventManager : MonoBehaviour
{
    string logEventApi;
    
    string jwtToken;
    public string username {  get;  private set; }

    public bool isLogin { get; private set; } = false;


    private void Awake()
    {
        logEventApi = GameSystemManager.GetSystem<ApiManager>().getApiUrl("logEvent");
    }

    public void logStudentEvent(string eventName, string eventContent)
    {
        StartCoroutine(logEvent(eventName,eventContent));
    }

    IEnumerator logEvent(string eventName, string eventContent)
    {
        WWWForm form = new WWWForm();
       // Debug.Log("logEvent");
        form.AddField("username", username);

        form.AddField("eventName", eventName);

        form.AddField("eventContent", eventContent);


        using (UnityWebRequest www = UnityWebRequest.Post(logEventApi, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + GameSystemManager.GetSystem<StudentEventManager>().getJwtToken());
            yield return www.SendWebRequest();

            //Debug.Log("event passed : " + eventName + "\n" + eventContent);
        }

    }

    public void setJwtToken(string jwtToken)
    {
        this.jwtToken = jwtToken;
    }

    public string getJwtToken()
    {
        return this.jwtToken;
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
