using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7 : Level
{

    private void Start()
    {
        setUp();
        gitSystem.buildRepository();

        fileSystem.NewFile("file", "");
        gitSystem.trackFile("file", "init");
        gitSystem.Commit("init commit");

        gitSystem.createBranch("test");

        gitSystem.checkout("test");
        fileSystem.NewFile("file2", "");
        gitSystem.trackFile("file2", "init");
        gitSystem.Commit("test commit");

        gitSystem.checkout("master");
        fileSystem.NewFile("file3", "");
        gitSystem.trackFile("file3", "init");
        gitSystem.Commit("update");

        gitSystem.trackFile("file2", "init");
        gitSystem.Commit("file2 update");
        gitSystem.trackFile("file", "init");
        gitSystem.Commit("file update");

        gitSystem.checkout("test");
        fileSystem.NewFile("file4", "");
        gitSystem.trackFile("file4", "init");
        gitSystem.Commit("add file4");

        gitSystem.trackFile("file", "init");
        gitSystem.Commit("fixed issue#001");

        gitSystem.checkout("master");
    }
    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] &&  gitSystem.nowCommit.transform.GetChild(2).gameObject.activeInHierarchy )
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        if (!targetSystem.targetStatus[1] && gitSystem.localRepository.branchCounts == 1 )
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        updateTarget();
        levelCostCount();
    }
}
