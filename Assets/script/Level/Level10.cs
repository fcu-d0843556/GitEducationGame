using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level10 : Level
{

    private void Start()
    {
        setUp();

        gitSystem.buildRepository();

        fileSystem.NewFile("index", "");
        gitSystem.trackFile("index", "init");
        gitSystem.Commit("init commit");

        fileSystem.NewFile("page1", "");
        gitSystem.trackFile("page1", "init");
        gitSystem.Commit("Add page1");

        fileSystem.NewFile("page2", "");
        gitSystem.trackFile("page2", "init");
        gitSystem.Commit("add page2");


        //gitSystem.Push();

         gitSystem.createBranch("teamA");
         gitSystem.checkout("teamA");
         gitSystem.trackFile("page2", "test update page2");
         gitSystem.Commit("update page2");

         gitSystem.checkout("master");
         gitSystem.trackFile("page2", "master update page2");
         gitSystem.Commit("update page2");

        GameSystemManager.GetSystem<TimerManager>().Add(new Timer(0.05f, push , null));



    }

    public void push(object obj)
    {
        gitSystem.Push();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] &&  gitSystem.hasStash && gitSystem.localRepository.nowBranch.branchName == "teamA")
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        if (!targetSystem.targetStatus[1] && targetSystem.targetStatus[0] &&  gitSystem.localRepository.nowBranch.branchName == "teamA" && gitSystem.localRepository.nowBranch.commitCounts >= 3 && gitSystem.hasPush)
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        if (!targetSystem.targetStatus[2] && targetSystem.targetStatus[1] && !gitSystem.hasStash &&  gitSystem.localRepository.nowBranch.branchName == "master")
        {
            targetSystem.targetStatus[2] = true;
            targetSystem.AccomplishTarget(2);
        }
        updateTarget();
        levelCostCount();
    }
}
