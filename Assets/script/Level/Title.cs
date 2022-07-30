using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title: MonoBehaviour
{
    [SerializeField]
    GameObject titleObjects;
    [SerializeField]
    GameObject chapterSelector;

    public void StartGame()
    {
        GameSystemManager.GetSystem<SceneStateManager>().LoadSceneState( new LoadSceneState("MainSceneState", "Level0Scene") ,true);
    }
    
    public void openChapterSelector()
    {
        titleObjects.SetActive(false);
        chapterSelector.SetActive(true);
    }

    public void closeChapterSelector()
    {
        titleObjects.SetActive(true);
        chapterSelector.SetActive(false);
    }

    public void levelStart(string level)
    {
        GameSystemManager.GetSystem<SceneStateManager>().LoadSceneState(new LoadSceneState("MainSceneState", "Level" + level + "Scene"), true);
    }

    public void openAchievementReader()
    {
        GameSystemManager.GetSystem<AchievementManager>().openReader();
    }
}
