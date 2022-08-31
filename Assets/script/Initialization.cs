using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour {
    [SerializeField]
    List<GameObject> _prefab;

    // Use this for initialization
    void Awake () {
        if (!GameSystemManager.GetSystem<ApiManager>())
            GameSystemManager.AddSystem<ApiManager>(Instantiate(_prefab[4]));
        if (!GameSystemManager.GetSystem<ScreenEffect>())
            GameSystemManager.AddSystem<ScreenEffect>(Instantiate(_prefab[0]));
        if (!GameSystemManager.GetSystem<StudentEventManager>())
            GameSystemManager.AddSystem<StudentEventManager>(Instantiate(_prefab[1]));
        if (!GameSystemManager.GetSystem<LeaderBoard>())
            GameSystemManager.AddSystem<LeaderBoard>(Instantiate(_prefab[2]));
        if (!GameSystemManager.GetSystem<AchievementManager>())
            GameSystemManager.AddSystem<AchievementManager>(Instantiate(_prefab[3]));
        if (!GameSystemManager.GetSystem<LevelManager>())
            GameSystemManager.AddSystem<LevelManager>(Instantiate(_prefab[5]));
    }



}
