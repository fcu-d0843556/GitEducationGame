using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : Level
{

    private void Start()
    {
        setUp();
        gitSystem.buildRepository();
    }
    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[1] && targetSystem.targetStatus[0] &&  gitSystem.nowCommit != null )
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        if (!targetSystem.targetStatus[0] && fileSystem.fileIsTracked("note1") && fileSystem.fileIsTracked("note2"))
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        else if (!targetSystem.targetStatus[1] && (!fileSystem.fileIsTracked("note1") || !fileSystem.fileIsTracked("note2")))
        {
            targetSystem.targetStatus[0] = false;
            targetSystem.UndoTarget(0);
        }
        updateTarget();
        levelCostCount();
    }
}
