using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level11 : Level
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

        gitSystem.trackFile("index", "update index");
        gitSystem.Commit("update index");

        gitSystem.createBranch("branchA");

        GameSystemManager.GetSystem<TimerManager>().Add(new Timer(0.05f, push , null));



    }

    public void push(object obj)
    {
        gitSystem.Push();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] && gitSystem.localRepository.nowBranch.branchName == "branchA")
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        if (!targetSystem.targetStatus[1] && targetSystem.targetStatus[0] &&  gitSystem.localRepository.nowBranch.branchName == "branchA" && gitSystem.localRepository.nowBranch.commitCounts >= 4 )
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        if (!targetSystem.targetStatus[2] && targetSystem.targetStatus[1] &&  gitSystem.localRepository.nowBranch.branchName == "master" )
        {
            Console.ConsoleCommand command = GameSystemManager.GetSystem<Console.DeveloperConsole>().lastExecuteCommand;
            Console.CommandGitInit gitCommand = (Console.CommandGitInit)command;
            if (gitCommand != null && gitCommand.type.Equals("rebase"))
            {
                targetSystem.targetStatus[2] = true;
                targetSystem.AccomplishTarget(2);
            }
        }
        if (!targetSystem.targetStatus[3] && targetSystem.targetStatus[2] && gitSystem.hasPush  && gitSystem.sync == true)
        {
            targetSystem.targetStatus[2] = true;
            targetSystem.AccomplishTarget(2);
        }
        updateTarget();
        levelCostCount();
    }
}
