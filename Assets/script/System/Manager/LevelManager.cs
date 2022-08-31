using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    int latestLevel;
    int totalLevels;
    private void Start()
    {
        totalLevels = 14;
    }
    public void latestLevelcleared(){
        latestLevel += 1;
    }
    public int getLatestLevel(){
        return latestLevel;
    }
    public void setLatestLevel(int level){
        latestLevel = level;
    }

    public int getTotalLevels(){
        return totalLevels;
    }
}
