using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6 : Level
{

    private void Start()
    {
        setUp();
        gitSystem.buildRepository();
        fileSystem.NewFile("index", "<h1>Hello World!</h1>");
        fileSystem.NewFile("test", "<p>Web Test</p>");
        gitSystem.trackFile("index", "<h1>Hello World!</h1>");
        gitSystem.trackFile("test", "<p>Web Test</p>");
        gitSystem.Commit("init commit");
    }
    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] &&  gitSystem.localRepository.branchCounts > 1 )
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        if (!targetSystem.targetStatus[1] && gitSystem.localRepository.nowBranch.branchName != "master" && !gitSystem.localRepository.nowBranch.branchStart )
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        if (!targetSystem.targetStatus[2] && targetSystem.targetStatus[1] && gitSystem.localRepository.nowBranch.branchName == "master" )
        {
            targetSystem.targetStatus[2] = true;
            targetSystem.AccomplishTarget(2);
        }
        updateTarget();
        levelCostCount();
    }
}
