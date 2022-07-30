using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : Level
{

    private void Start()
    {
        setUp();
        gitSystem.buildRepository();
    }
    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] &&  gitSystem.hasRemote())
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        if (!targetSystem.targetStatus[1] && gitSystem.nowCommit != null)
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        if (!targetSystem.targetStatus[2] &&  gitSystem.hasPush)
        {
            targetSystem.targetStatus[2] = true;
            targetSystem.AccomplishTarget(2);
        }
        updateTarget();
        levelCostCount();
    }
}
