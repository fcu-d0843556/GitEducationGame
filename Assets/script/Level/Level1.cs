using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : Level
{
    private void Start()
    {
        setUp();
        gitSystem.buildRepository();  
    }
    // Update is called once per frame
    void Update()
    {
        if (!targetSystem.targetStatus[0] && fileSystem.getFilesName().Contains("note1_copy"))
        {
            //Debug.Log("g1");
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        else if (targetSystem.targetStatus[0] && !fileSystem.getFilesName().Contains("note1_copy"))
        {
            //Debug.Log("bad");
            targetSystem.targetStatus[0] = false;
            targetSystem.UndoTarget(0);
        }

        if (!targetSystem.targetStatus[1] && fileSystem.getFilesName().Contains("note2_copy"))
        {
            //Debug.Log("g2");
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        else if (targetSystem.targetStatus[1] && !fileSystem.getFilesName().Contains("note2_copy"))
        {
            //Debug.Log("bad2");
            targetSystem.targetStatus[1] = false;
            targetSystem.UndoTarget(1);
        }
        updateTarget();
        levelCostCount();
    }
}
