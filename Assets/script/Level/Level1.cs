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
            string[] command = GameSystemManager.GetSystem<Console.DeveloperConsole>().inputCommands;
            if (command != null && command[1].Equals("note1"))
            {
                targetSystem.targetStatus[0] = true;
                targetSystem.AccomplishTarget(0);
            }
            
        }
        else if (targetSystem.targetStatus[0] && !fileSystem.getFilesName().Contains("note1_copy"))
        {
            //Debug.Log("bad");
            targetSystem.targetStatus[0] = false;
            targetSystem.UndoTarget(0);
        }

        if (!targetSystem.targetStatus[1] && fileSystem.getFilesName().Contains("note2_copy"))
        {
            string[] command = GameSystemManager.GetSystem<Console.DeveloperConsole>().inputCommands;
            if (command != null && command[1].Equals("note2"))
            {
                targetSystem.targetStatus[1] = true;
                targetSystem.AccomplishTarget(1);
            }
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
