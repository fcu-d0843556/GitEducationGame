using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class Achievement
{
    public int id;
    public string name;
    public string description;
    public Sprite icon;

}

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    AchievementAnimation achievementPopupObject;

    [SerializeField]
    List<Achievement> achievements;
    [SerializeField]
    GameObject achievementUnlockedObject;
    [SerializeField]
    GameObject achievementlockedObject;
    [SerializeField]
    GameObject achievementReader;

    List<GameObject> achievementObjects;

    [SerializeField]
    string logAchievementApi;
    [SerializeField]
    string getOneUserAchievementsApi;

    public int testInput;
    public bool testSwitch;
    [SerializeField]
    List<string> monthList;

    private void Start()
    {
        int index = 0;
        achievementObjects = new List<GameObject>();
        foreach (Achievement achievement in achievements)
        {
            GameObject achievementObject = Instantiate(achievementlockedObject, achievementlockedObject.transform.parent);
            achievementObject.transform.GetChild(4).GetComponent<Text>().text = achievement.description;
            achievement.id = index + 1;
            achievementObject.SetActive(true);
            achievementObject.name = "achievement" + achievement.id + "locked";
            achievementObjects.Add(achievementObject);

            achievementObject = Instantiate(achievementUnlockedObject, achievementUnlockedObject.transform.parent);
            achievementObject.transform.GetChild(1).GetComponent<Image>().sprite = achievement.icon;
            achievementObject.transform.GetChild(2).GetComponent<Text>().text = achievement.name;
            achievementObject.transform.GetChild(3).GetComponent<Text>().text = achievement.description;
            //achievementObject.transform.GetChild(4).GetComponent<Text>().text = achievement.description;
            achievementObject.name = "achievement" + achievement.id + "unlocked";
            achievementObjects.Add(achievementObject);
            index++;
        }
    }

    public void achieve(int achievementId,string time ,bool animation = true)
    {
        if (achievements[achievementId-1] != null)
        {
            if (animation)
            {
                achievementPopupObject.popup(achievements[achievementId-1].icon, achievements[achievementId-1].name);
            }

            achievementObjects.Find(x => x.name == "achievement" + (achievementId) + "locked").SetActive(false);
            GameObject ach = achievementObjects.Find(x => x.name == "achievement" + (achievementId) + "unlocked");
            DateTime dateTime = DateTime.Parse(time);
            ach.SetActive(true);
            ach.transform.GetChild(4).GetComponent<Text>().text = monthList[dateTime.Month-1]+ " " + dateTime.Day + ", " + dateTime.Year;
        }
    }

    private void Update()
    {
        if (testSwitch)
        {
            achieve(testInput, DateTime.Now.ToString());
            testSwitch = false;
        }
    }

    public IEnumerator logAchievement(int achievementId)
    {
        WWWForm form = new WWWForm();

        form.AddField("username", GameSystemManager.GetSystem<StudentEventManager>().username);

        string achievement = "{ id:" + (achievementId) + "}";
        form.AddField("achievement", achievement);


        using (UnityWebRequest www = UnityWebRequest.Post(logAchievementApi, form))
        {
            yield return www.SendWebRequest();

            string result = www.downloadHandler.text;
            if (!result.Equals("already have achievement"))
            {
                achieve(achievementId,DateTime.Now.ToString());
            }

        }

    }

    public void logAchievementByManager(int achievementId)
    {
        StartCoroutine(logAchievement(achievementId));
    }

    public IEnumerator getUserAchievements()
    {



        using (UnityWebRequest www = UnityWebRequest.Get(getOneUserAchievementsApi + "?username=" + GameSystemManager.GetSystem<StudentEventManager>().username))
        {
            yield return www.SendWebRequest();

            string jsonString = JsonHelper.fixJson(www.downloadHandler.text);
            AchievementRecord[] achievementRecords = JsonHelper.FromJson<AchievementRecord>(jsonString);
            for (int i = 0; i < achievementRecords.Length; i++) {
                //Debug.Log(achievementRecords[i].achievement.id);
                achieve(achievementRecords[i].achievement.id, achievementRecords[i].time, false);

            }

        }

    }

    public void openReader()
    {
        StartCoroutine(getUserAchievements());
        achievementReader.SetActive(true);
    }

    [Serializable]
    public class AchievementRecord
    {
        public string time;
        public Achievement achievement;
    }
}
