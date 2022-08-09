using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level13 : Level
{

    private void Start()
    {
        setUp();
        gitSystem.buildRepository();
        gitSystem.Commit("init commit");
        gitSystem.Commit("add ignore");
        gitSystem.Commit("front update");

        gitSystem.createBranch("dev");
        gitSystem.checkout("dev");
        gitSystem.Commit("backend update");
        gitSystem.Commit("add new fun");

        gitSystem.createBranch("John");
        gitSystem.checkout("John");
        gitSystem.Commit("add new fun");

        gitSystem.checkout("master");

        GameSystemManager.GetSystem<TimerManager>().Add(new Timer(0.05f, push, null));
    }

    public void push(object obj)
    {
        gitSystem.Push();
        gitSystem.localObjects.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (!targetSystem.targetStatus[0] && gitSystem.cloned)
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        if (!targetSystem.targetStatus[1] && targetSystem.targetStatus[0] &&  gitSystem.localRepository.nowBranch.branchName.Equals("issue#016") )
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        if (!targetSystem.targetStatus[2] && targetSystem.targetStatus[1] &&  gitSystem.hasPush && gitSystem.sync)
        {
            targetSystem.targetStatus[2] = true;
            targetSystem.AccomplishTarget(2);
        }
        if (!targetSystem.targetStatus[3] && targetSystem.targetStatus[2] && gitSystem.localRepository.nowBranch.branchName.Equals("dev_fun"))
        {
            targetSystem.targetStatus[3] = true;
            targetSystem.AccomplishTarget(3);
        }
        if (!targetSystem.targetStatus[4] && targetSystem.targetStatus[3] && gitSystem.localRepository.nowBranch.branchName == "John")
        {
            Console.ConsoleCommand command = GameSystemManager.GetSystem<Console.DeveloperConsole>().lastExecuteCommand;
            Console.CommandGitInit gitCommand = (Console.CommandGitInit)command;
            if (gitCommand != null && gitCommand.type.Equals("rebase"))
            {
                targetSystem.targetStatus[4] = true;
                targetSystem.AccomplishTarget(4);
            }
        }
        updateTarget();
        levelCostCount();
    }
}
