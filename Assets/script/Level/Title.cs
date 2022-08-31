using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title: MonoBehaviour
{
    [SerializeField]
    GameObject titleObjects;
    [SerializeField]
    GameObject chapterSelector;

    public void StartGame()
    {
        
        int level = GameSystemManager.GetSystem<LevelManager>().getLatestLevel();
        int totalLevels = GameSystemManager.GetSystem<LevelManager>().getTotalLevels();
        if ( level != totalLevels){
            GameSystemManager.GetSystem<SceneStateManager>().LoadSceneState( new LoadSceneState("MainSceneState",  "Level" + (level + 1) + "Scene") ,true);
        } else {
            GameSystemManager.GetSystem<SceneStateManager>().LoadSceneState( new LoadSceneState("MainSceneState",  "Level" + totalLevels + "Scene") ,true);
        }
        
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
