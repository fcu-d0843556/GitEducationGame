using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChapterSystem : MonoBehaviour
{
    [SerializeField]
    string getCollectionApi;
    [SerializeField]
    List<Button> chapterButtons;


    IEnumerator getLevelPassed(string username)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(getCollectionApi + "collection="
            + username + "&filterKey=event_name" + "&filterValue=level_passed"))
        {
            yield return www.SendWebRequest();
            // Debug.Log(www.downloadHandler.text);
            string jsonString = JsonHelper.fixJson(www.downloadHandler.text);
            levelPassedEvent[] studentEvents = JsonHelper.FromJson<levelPassedEvent>(jsonString);
            chapterButtons[0].interactable = true;
            for (int i=0; i< studentEvents.Length; i++)
            {
                Level.levelScene myStatus;
                Enum.TryParse(studentEvents[i].event_content.level, out myStatus);
                if (myStatus != Level.levelScene.Level0)
                {
                    chapterButtons[(int)myStatus + 1].interactable = true;
                    if (chapterButtons[(int)myStatus+1])
                    {
                        chapterButtons[(int)myStatus+1].interactable = true;
                    }
                }
                // Debug.Log(studentEvents[i].event_content.level + " : " + myStatus);
                /*if(studentEvents[i].event_content.level == ((Level.levelScene)i).ToString())
                {
                    chapterButtons[(int)myStatus].interactable = true;
                    if (chapterButtons[i + 1])
                    {
                        chapterButtons[i + 1].interactable = true;
                    }
                }*/
            }
        }
    }

    public void initialChapterButtons(string username)
    {
        StartCoroutine(getLevelPassed(username));
    }

    private void OnEnable()
    {
        initialChapterButtons(GameSystemManager.GetSystem<StudentEventManager>().username);
    }

    [System.Serializable]
    public class levelPassedEvent
    {
        public string username;
        public LevelRecord event_content;

    }
    [System.Serializable]
    public class LevelRecord
    {
        public string level;
    }

}
