using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level14 : Level
{

    private void Start()
    {
        setUp();
        gitSystem.buildRepository();

        nextLevelButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.AddListener(delegate
        {
            GameSystemManager.GetSystem<SceneStateManager>().LoadSceneState(new LoadSceneState("MainSceneState","TitleScene"), true);
        });
    }



    // Update is called once per frame
    void Update()
    {
        if (!targetSystem.targetStatus[0] && gitSystem.localRepository.nowBranch.commits.Count == 4)
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }

        if (targetSystem.targetStatus[0] && !targetSystem.targetStatus[1] && gitSystem.localRepository.nowBranch.commits.Count == 2)
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        updateTarget();
        levelCostCount();
    }
}
