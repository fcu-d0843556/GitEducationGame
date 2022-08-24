using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{

    public string leaderBoardApi;
    public string getAllUsersPointsApi;
    [SerializeField]
    Text head;
    [SerializeField]
    Text index;
    [SerializeField]
    Text username;
    [SerializeField]
    Text timeCost;
    [SerializeField]
    Text lineCost;
    [SerializeField]
    Text time;

    bool showAll = false;
    PointsRecord[] pointsLeaderboardRecords;
    LevelRecord[] levelLeaderboardRecords;

    RectTransform contentTableTrans;
    VerticalLayoutGroup contentTableGroup;


    public void Awake()
    {
        leaderBoardApi = GameSystemManager.GetSystem<ApiManager>().getApiUrl("getLevelLeaderboard");
        getAllUsersPointsApi = GameSystemManager.GetSystem<ApiManager>().getApiUrl("getAllUsersPoints");
    }

    public void getLevelLeaderboard()
    {
        gameObject.SetActive(true);
        Level.levelScene nowLevel = GameObject.Find("MainScreenObject").GetComponent<Level>().nowLevel;
        int levelIndex = Array.IndexOf(Level.levelScene.GetValues(nowLevel.GetType()), nowLevel);
        index.text = "";
        username.text = "";
        timeCost.text = "";
        lineCost.text = "";
        time.text = "";
        StartCoroutine(getLevelLeaderboard(levelIndex + 1));
    }

    IEnumerator getLevelLeaderboard(int level)
    {
        head.text = "排行榜 - Level " + level;
        if (contentTableTrans == null)
        {
            contentTableTrans = username.transform.parent.parent.GetComponent<RectTransform>();
        }
        if (contentTableGroup == null)
        {
            contentTableGroup = username.transform.parent.parent.GetComponent<VerticalLayoutGroup>();
        }

        using (UnityWebRequest www = UnityWebRequest.Get(leaderBoardApi + "?level=" + level))
        {
            www.SetRequestHeader("Authorization", "Bearer " + GameSystemManager.GetSystem<StudentEventManager>().getJwtToken());
            yield return www.SendWebRequest();
            
            string jsonString = JsonHelper.fixJson(www.downloadHandler.text);
            //Debug.Log("jsonString :  "  + jsonString);
            levelLeaderboardRecords = JsonHelper.FromJson<LevelRecord>(jsonString);

            updateLevelLeaderboard();
        }

    }


    public IEnumerator logLevelRecord(int timeCost , int lineCost , int level)
    {
        //Debug.Log("logLevelRecord");
        WWWForm form = new WWWForm();

        form.AddField("username", GameSystemManager.GetSystem<StudentEventManager>().username);
        form.AddField("timeCost", timeCost);
        form.AddField("lineCost", lineCost);
        form.AddField("level", level);

        string logLevelRecordApi = GameSystemManager.GetSystem<ApiManager>().getApiUrl("logLevelRecord");
        using (UnityWebRequest www = UnityWebRequest.Post(logLevelRecordApi, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + GameSystemManager.GetSystem<StudentEventManager>().getJwtToken());
            yield return www.SendWebRequest();
        }

    }

    public void showPointsLeaderboard()
    {
        gameObject.SetActive(true);
        StartCoroutine(getPointsLeaderboard());
    }

    IEnumerator getPointsLeaderboard()
    {
        if (contentTableTrans == null)
        {
            contentTableTrans = username.transform.parent.parent.GetComponent<RectTransform>();
        }
        if (contentTableGroup == null)
        {
            contentTableGroup = username.transform.parent.parent.GetComponent<VerticalLayoutGroup>();
        }
        using (UnityWebRequest www = UnityWebRequest.Get(getAllUsersPointsApi))
        {
            www.SetRequestHeader("Authorization", "Bearer " + GameSystemManager.GetSystem<StudentEventManager>().getJwtToken());
            yield return www.SendWebRequest();
            string jsonString = JsonHelper.fixJson(www.downloadHandler.text);
            pointsLeaderboardRecords = JsonHelper.FromJson<PointsRecord>(jsonString);
            //Debug.Log("jsonString" + jsonString);
            updatePointsPointsLeaderboard();
        }

    }

    void updateLevelLeaderboard()
    {
        if (levelLeaderboardRecords == null)
            return;
        index.text = "";
        username.text = "";
        timeCost.text = "";
        lineCost.text = "";
        time.text = "";
        int showCounts = 10;
        if (showAll)
        {
            showCounts = levelLeaderboardRecords.Length;
        }
        for (int i = 0; i < levelLeaderboardRecords.Length && (i < showCounts); i++)
        {
            index.text = index.text + (i + 1) + "\n";
            username.text = username.text + levelLeaderboardRecords[i].username + "\n";
            timeCost.text = timeCost.text + levelLeaderboardRecords[i].timeCost + "\n";
            lineCost.text = lineCost.text + levelLeaderboardRecords[i].lineCost + "\n";
            time.text = time.text + levelLeaderboardRecords[i].time + "\n";
        }
        contentTableTrans.sizeDelta = new Vector2(0, showCounts * 100);
        contentTableGroup.padding.bottom = showCounts * 100;

        index.GetComponent<RectTransform>().sizeDelta = new Vector2(index.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
        username.GetComponent<RectTransform>().sizeDelta = new Vector2(username.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
        timeCost.GetComponent<RectTransform>().sizeDelta = new Vector2(timeCost.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
        lineCost.GetComponent<RectTransform>().sizeDelta = new Vector2(lineCost.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
        time.GetComponent<RectTransform>().sizeDelta = new Vector2(time.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
    }

    void updatePointsPointsLeaderboard()
    {
        if (pointsLeaderboardRecords == null)
            return;
        index.text = "";
        username.text = "";
        timeCost.text = "";
        lineCost.text = "";
        int showCounts = 10;
        if (showAll)
        {
            showCounts = pointsLeaderboardRecords.Length;
        }

        for (int i = 0; i < pointsLeaderboardRecords.Length && (i < showCounts); i++)
        {
            index.text = index.text + (i + 1)  + "\n";
            username.text = username.text + pointsLeaderboardRecords[i].user + "\n";
            timeCost.text = timeCost.text + pointsLeaderboardRecords[i].gamePoints + "\n";
            lineCost.text = lineCost.text + pointsLeaderboardRecords[i].achievementCounts + "/10\n";
        }
        contentTableTrans.sizeDelta = new Vector2(0,   showCounts * 100);
        contentTableGroup.padding.bottom = showCounts * 100;

        index.GetComponent<RectTransform>().sizeDelta = new Vector2(index.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
        username.GetComponent<RectTransform>().sizeDelta = new Vector2(username.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
        timeCost.GetComponent<RectTransform>().sizeDelta = new Vector2(timeCost.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
        lineCost.GetComponent<RectTransform>().sizeDelta = new Vector2(lineCost.GetComponent<RectTransform>().sizeDelta.x, index.text.Split('\n').Length * 88);
    }

    public void switchShow(bool showAll)
    {
        this.showAll = showAll;
        updateLevelLeaderboard();
        updatePointsPointsLeaderboard();
    }

    [System.Serializable]
    public class LevelRecord
    {
        public string username;
        public string time;
        public string timeCost;
        public int lineCost;
    }

    [System.Serializable]
    public class PointsRecord
    {
        public string user;
        public int gamePoints;
        public int achievementCounts;
    }

}