using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level12 : Level
{

    private void Start()
    {
        setUp();

        gitSystem.buildRepository();

        fileSystem.NewFile("index", "");
        gitSystem.trackFile("index", "init");
        gitSystem.Commit("init commit");

        GameSystemManager.GetSystem<TimerManager>().Add(new Timer(0.05f, push , null));



    }

    public void push(object obj)
    {
        gitSystem.Push();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] && gitSystem.localRepository.nowBranch.branchName.Equals("master") && gitSystem.localRepository.nowBranch.commitCounts >= 2)
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        if (!targetSystem.targetStatus[1] && targetSystem.targetStatus[0] &&  !gitSystem.localRepository.nowBranch.branchName.Equals("master") && gitSystem.localRepository.nowBranch.commitCounts >= 2 )
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        if (!targetSystem.targetStatus[2] && targetSystem.targetStatus[1] &&  gitSystem.tagCounts >=2 )
        {
            targetSystem.targetStatus[2] = true;
            targetSystem.AccomplishTarget(2);
        }
        if (!targetSystem.targetStatus[3] && targetSystem.targetStatus[2] && gitSystem.hasPush  && gitSystem.sync == true)
        {
            targetSystem.targetStatus[3] = true;
            targetSystem.AccomplishTarget(3);
        }
        updateTarget();
        levelCostCount();
    }
}
