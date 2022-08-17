using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{

    string loginApi;
    string registerApi;
    [SerializeField]
    GameObject loginDialogue;
    [SerializeField]
    Button startButton;
    [SerializeField]
    Button chapterButton;
    [SerializeField]
    Button loginButton;
    [SerializeField]
    Button achievementButton;

    [SerializeField]
    InputField username;
    [SerializeField]
    InputField password;
    [SerializeField]
    Text loginStatus;
    [SerializeField]
    Text token;

    private void Start()
    {
        loginButton.onClick.AddListener(loginDialogueShow);

        if (GameSystemManager.GetSystem<StudentEventManager>().isLogin)
        {
            loginStatus.text = "登入狀態：登入成功";
            loginButton.GetComponentInChildren<Text>().text = "登出帳號";
            startButton.interactable = true;
            chapterButton.interactable = true;
            achievementButton.interactable = true;
            loginButton.onClick.RemoveAllListeners();
            loginButton.onClick.AddListener(logout);
        }
        loginApi = GameSystemManager.GetSystem<ApiManager>().getApiUrl("login");
        registerApi = GameSystemManager.GetSystem<ApiManager>().getApiUrl("register");
    }

    public void loginDialogueShow()
    {
        loginDialogue.SetActive(true);
    }

    public void login()
    {
        StartCoroutine(loginAuth());
    }

    public void register()
    {
        StartCoroutine(registerAuth());
    }

    IEnumerator loginAuth()
    {

        WWWForm form = new WWWForm();
        if (username.text.Equals("") || password.text.Equals(""))
        {
            loginStatus.text = "登入狀態：密碼錯誤或帳號不存在";
            yield return null;
        }
        else
        {
            form.AddField("username", username.text);
            form.AddField("password", password.text);

            using (UnityWebRequest www = UnityWebRequest.Post(loginApi, form))
            {
                yield return www.SendWebRequest();

                //Debug.Log(www.result.ToString());
                loginJson loginResult = JsonUtility.FromJson<loginJson>(www.downloadHandler.text);
                if (loginResult.status)
                {
                    Debug.Log("loginAuth loginResult.token");
                    Debug.Log(loginResult.token);
                    loginStatus.text = "登入狀態：登入成功";
                    loginButton.GetComponentInChildren<Text>().text = "登出帳號";
                    startButton.interactable = true;
                    chapterButton.interactable = true;
                    loginDialogue.SetActive(false);
                    GameSystemManager.GetSystem<StudentEventManager>().setJwtToken(loginResult.token);
                    GameSystemManager.GetSystem<StudentEventManager>().setUsername(username.text);
                    loginButton.onClick.RemoveAllListeners();
                    loginButton.onClick.AddListener(logout);
                    achievementButton.interactable = true;
                }
                else
                {
                    loginStatus.text = "登入狀態：密碼錯誤或帳號不存在";
                }
                //token.text = www.downloadHandler.text;
                //Debug.Log(token.text);
            }
        }
    }

    IEnumerator registerAuth()
    {
        WWWForm form = new WWWForm();
        if (username.text.Equals("") || password.text.Equals(""))
        {
            loginStatus.text = "登入狀態：帳號或密碼不能為空";
            yield return null;
        }
        else
        {
            form.AddField("username", username.text);
            form.AddField("name", username.text);
            form.AddField("password", password.text);

            using (UnityWebRequest www = UnityWebRequest.Post(registerApi, form))
            {
                yield return www.SendWebRequest();
                //Debug.Log(www.result.ToString());
                string result = www.downloadHandler.text;
                if (result.Equals("Username had already been registered"))
                {
                    loginStatus.text = "登入狀態：註冊失敗，帳號已被註冊";
                }
                else
                {
                    loginStatus.text = "登入狀態：註冊成功，請按下登入鈕";
                }
            }
        }
    }


    class loginJson
    {
        public bool status;
        public string token;
    }

    public void logout()
    {
        GameSystemManager.GetSystem<StudentEventManager>().logout();
        loginStatus.text = "登入狀態：已登出";
        startButton.interactable = false;
        chapterButton.interactable = false;
        achievementButton.interactable = false;
        loginButton.GetComponentInChildren<Text>().text = "登入帳號";
        loginButton.onClick.RemoveAllListeners();
        loginButton.onClick.AddListener(loginDialogueShow);
        username.text = "";
        password.text = "";
    }
}
