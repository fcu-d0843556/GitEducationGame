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

            levelPassedEvent[] studentEvents = JsonHelper.FromJson<levelPassedEvent>(jsonString);
            // Debug.Log(jsonString);
            // Debug.Log(JsonHelper.FromJson<levelPassedEvent>(jsonString));
            for (int i = 0 ;i< chapterButtons.Count;i++){
                chapterButtons[i].interactable = false;
            }
            chapterButtons[0].interactable = true;
            for (int i=0; i< studentEvents.Length; i++)
            {
                Level.levelScene myStatus;
                // Debug.Log(studentEvents[i].username);
                // Debug.Log(studentEvents[i].eventContent);
                Enum.TryParse(studentEvents[i].eventContent.level, out myStatus);
                if (myStatus != Level.levelScene.Level0)
                {
                    chapterButtons[(int)myStatus + 1].interactable = true;
                    if (chapterButtons[(int)myStatus+1])
                    {
                        chapterButtons[(int)myStatus+1].interactable = true;
                    }
                }
            }
            // Debug.Log("jsonString: " + jsonString);
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
