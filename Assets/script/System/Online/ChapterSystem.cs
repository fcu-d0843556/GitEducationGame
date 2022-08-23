using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChapterSystem : MonoBehaviour
{
    // string getCollectionApi;
    string getLevelPassedApi;
    [SerializeField]
    List<Button> chapterButtons;


    public void Awake()
    {
        // getCollectionApi = GameSystemManager.GetSystem<ApiManager>().getApiUrl("getCollection");
        getLevelPassedApi = GameSystemManager.GetSystem<ApiManager>().getApiUrl("getLevelPassed");
    }

    public IEnumerator getUserEventsFilterLevelPassed(string username)
    {
        Debug.Log("FilterLevelPassed");

        using (UnityWebRequest www = UnityWebRequest.Get(getLevelPassedApi + "?username=" + username))
        {   

            www.SetRequestHeader("Authorization", "Bearer " + GameSystemManager.GetSystem<StudentEventManager>().getJwtToken());
            yield return www.SendWebRequest();
            string jsonString = JsonHelper.fixJson(www.downloadHandler.text);

            chapterButtons[0].interactable = true;
            try
            {
                for (int i = 1; i < chapterButtons.Count; i++)
                {
                    chapterButtons[i].interactable = false;
                }

                levelPassedEvent[] studentEvents = JsonHelper.FromJson<levelPassedEvent>(jsonString);
                Debug.Log(studentEvents.Length);

                for (int i = 0; i < studentEvents.Length; i++)
                {
                    int clearLevel = Int32.Parse(studentEvents[i].eventContent.level.Split("Level")[1]);
                    chapterButtons[clearLevel + 1].interactable = true;
                }
            }
            catch
            {

            }

        }

    }

    public void initialChapterButtons(string username)
    {
        StartCoroutine(getUserEventsFilterLevelPassed(username));
    }

    private void OnEnable()
    {
        initialChapterButtons(GameSystemManager.GetSystem<StudentEventManager>().username);
    }

    [System.Serializable]
    public class levelPassedEvent
    {
        public string username;
        public LevelRecord eventContent;
    }
    [System.Serializable]
    public class LevelRecord
    {
        public string level;
    }

}
